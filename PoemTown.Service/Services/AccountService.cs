using AutoMapper;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PoemTown.Repository.Base;
using PoemTown.Repository.CustomException;
using PoemTown.Repository.Entities;
using PoemTown.Repository.Enums;
using PoemTown.Repository.Enums.Accounts;
using PoemTown.Repository.Interfaces;
using PoemTown.Repository.Utils;
using PoemTown.Service.BusinessModels.RequestModels.AccountRequests;
using PoemTown.Service.BusinessModels.ResponseModels.AccountResponses;
using PoemTown.Service.BusinessModels.ResponseModels.RoleResponse;
using PoemTown.Service.Consumers.ThemeConsumers;
using PoemTown.Service.Events.CollectionEvents;
using PoemTown.Service.Events.EmailEvents;
using PoemTown.Service.Events.ThemeEvents;
using PoemTown.Service.Events.UserEWalletEvents;
using PoemTown.Service.Interfaces;
using PoemTown.Service.QueryOptions.FilterOptions.AccountFilters;
using PoemTown.Service.QueryOptions.RequestOptions;
using PoemTown.Service.QueryOptions.SortOptions.AccountSorts;

namespace PoemTown.Service.Services;

public class AccountService : IAccountService
{
    private readonly UserManager<User> _userManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ITokenService _tokenService;
    private readonly IMapper _mapper;
    
    public AccountService
    (
        UserManager<User> userManager, 
        IUnitOfWork unitOfWork,
        IPublishEndpoint publishEndpoint,
        IMapper mapper,
        ITokenService tokenService
        )
    {
        _userManager = userManager;
        _unitOfWork = unitOfWork;
        _publishEndpoint = publishEndpoint;
        _mapper = mapper;
        _tokenService = tokenService;
    }
    

    /// <summary>
    /// Check if user is already confirmed email or not and check if email otp is correct and not expired then set email confirmed to true
    /// </summary>
    /// <param name="request"></param>
    /// <exception cref="CoreException"></exception>
    public async Task ConfirmEmail(ConfirmEmailRequest request)
    {
        User? user = await _unitOfWork.GetRepository<User>().FindAsync(p => p.Email == request.Email);
        
        if(user == null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "User not found");
        }
        
        if(user.EmailConfirmed)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "Email is already confirmed");
        }
        
        if(user.EmailOtp != request.EmailOtp)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "Email Otp is incorrect");
        }
        
        //Check if email otp is expired in 5 minutes as unix timestamp
        if(String.CompareOrdinal(user.EmailOtpExpiration, TimeStampHelper.GenerateUnixTimeStampNow().ToString()) < 0 )
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "Email OTP is expired");
        }
        //Set email confirmed to true and remove current email otp
        user.EmailConfirmed = true;
        user.Status = AccountStatus.Active;
        user.EmailOtp = null;
        user.EmailOtpExpiration = null;
        
        await _userManager.UpdateAsync(user);
        
        //Create default collection for user
        CreateDefaultCollectionEvent message = new CreateDefaultCollectionEvent()
        {
            UserId = user.Id
        };
        await _publishEndpoint.Publish(message);

        //Create default theme for user
        CreateDefaultUserThemeEvent themeMessage = new CreateDefaultUserThemeEvent()
        {
            UserId = user.Id,
            IsInUse = true,
        };
        await _publishEndpoint.Publish(themeMessage);
        
        //Initial user e-wallet
        InitialUserEWalletEvent initialUserEWalletEvent = new InitialUserEWalletEvent()
        {
            UserId = user.Id
        };
        await _publishEndpoint.Publish(initialUserEWalletEvent);
    }

    public async Task SendEmailOtp(ResendEmailConfirmationRequest request)
    {
        User user = await _unitOfWork.GetRepository<User>().FindAsync(p => p.Email == request.Email);
        if(user == null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "User not found");
        }
        
        //generate new email otp and email otp expiration
        user.EmailOtp = OtpGenerator.GenerateOtp();
        user.EmailOtpExpiration = TimeStampHelper.GenerateUnixTimeStamp(0, 10, 0).ToString();
        await _userManager.UpdateAsync(user);
        
        //publish email otp to rabbitmq
        EmailOtpEvent message = new EmailOtpEvent
        {
            Email = user.Email,
            EmailOtp = user.EmailOtp
        };  
        await _publishEndpoint.Publish(message);
    }
    
    public async Task ChangePassword(Guid userId, ChangePasswordRequest request)
    {
        User? user = await _unitOfWork.GetRepository<User>().FindAsync(p => p.Id == userId);
        if(user == null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "User not found");
        }

        if (!user.EmailConfirmed)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "Email is not confirmed");
        }
        
        //Get salt from database and hash current password
        string? salt = await _unitOfWork.GetRepository<User>()
            .AsQueryable()
            .Where(p => p.Email == user.Email)
            .Select(p => p.Salt)
            .FirstOrDefaultAsync();
        
        string hashedPassword = PasswordHasher.HashPassword(request.CurrentPassword, salt);
        //check if hashedPassword with salt is equal to passwordHash in database by identity (hashedPassword in database is generated by identity - 2 times hash)
        bool checkPassword = await _userManager.CheckPasswordAsync(user, hashedPassword);
        
        if (!checkPassword)
        {
            throw new CoreException(StatusCodes.Status401Unauthorized, "Current password is incorrect");
        }
        
        //Generate new salt and hash new password with salt
        string newSalt = PasswordHasher.GenerateSalt();
        string newHashedPassword = PasswordHasher.HashPassword(request.NewPassword, newSalt);
        
        //Change password
        IdentityResult result = await _userManager.ChangePasswordAsync(user, hashedPassword, newHashedPassword);
        if(!result.Succeeded)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, result.Errors.First().Description);
        }
        
        //Update salt and save salt to database
        user.Salt = newSalt;
        await _userManager.UpdateAsync(user);
    }

    public async Task ForgotPassword(ForgotPasswordRequest request)
    {
        User? user = await _unitOfWork.GetRepository<User>().FindAsync(p => p.Email == request.Email);
        if(user == null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "User not found");
        }
        
        if (!user.EmailConfirmed)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "Email is not confirmed");
        }
        
        //Generate reset password token and expired timestamp
        string resetPasswordToken = await _userManager.GeneratePasswordResetTokenAsync(user);
        string timeStamp = TimeStampHelper.GenerateUnixTimeStamp(0, 10, 0).ToString();
        
        //publish email event to rabbitmq with reset password token and expired timestamp
        ForgotPasswordEvent message = new ForgotPasswordEvent()
        {
            Email = user.Email,
            ResetPasswordToken = resetPasswordToken,
            ExpiredTimeStamp = timeStamp
        };  
        await _publishEndpoint.Publish(message);
    }
    
    /// <summary>
    /// Create new password using reset password token and expired timestamp, receive from email
    /// </summary>
    /// <param name="request"></param>
    /// <exception cref="CoreException"></exception>
    public async Task NewPasswordForgot(NewPasswordForgotRequest request)
    {
        User? user = await _unitOfWork.GetRepository<User>().FindAsync(p => p.Email == request.Email);
        
        if(user == null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "User not found");
        }
        
        if (!user.EmailConfirmed)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "Email is not confirmed");
        }
        
        if(String.CompareOrdinal(request.ExpiredTimeStamp, TimeStampHelper.GenerateUnixTimeStampNow().ToString()) < 0 )
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "Reset password token is expired");
        }
        
        //Generate new salt and hash new password with salt
        string newSalt = PasswordHasher.GenerateSalt();
        string newHashedPassword = PasswordHasher.HashPassword(request.NewPassword, newSalt);
        
        //Change password using reset password token
        IdentityResult result = await _userManager.ResetPasswordAsync(user, request.ResetPasswordToken, newHashedPassword);
        if(!result.Succeeded)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, result.Errors.First().Description);
        }
        
        user.Salt = newSalt;
        await _userManager.UpdateAsync(user);
    }

    public async Task<PaginationResponse<GetAccountResponse>>
        GetAccounts(RequestOptionsBase<GetAccountFilterOption, GetAccountSortOption> request)
    {
        var accountQuery = _unitOfWork.GetRepository<User>().AsQueryable();

        // IsDeleted
        if (request.IsDelete == true)
        {
            accountQuery = accountQuery.Where(p => p.DeletedTime != null);
        }
        else
        {
            accountQuery = accountQuery.Where(p => p.DeletedTime == null);
        }
        
        // Filter
        if (request.FilterOptions != null)
        {
            if (!string.IsNullOrEmpty(request.FilterOptions.UserName))
            {
                accountQuery = accountQuery.Where(p => p.UserName!.ToLower().Trim()
                    .Contains(request.FilterOptions.UserName.ToLower().Trim()));
            }
            
            if (!string.IsNullOrEmpty(request.FilterOptions.Email))
            {
                accountQuery = accountQuery.Where(p => p.Email!.ToLower().Trim()
                    .Contains(request.FilterOptions.Email.ToLower().Trim()));
            }
            
            if (!string.IsNullOrEmpty(request.FilterOptions.PhoneNumner))
            {
                accountQuery = accountQuery.Where(p => p.PhoneNumber!.ToLower().Trim()
                    .Contains(request.FilterOptions.PhoneNumner.ToLower().Trim()));
            }
            
            if (request.FilterOptions.Status != null)
            {
                accountQuery = accountQuery.Where(p => p.Status == request.FilterOptions.Status);
            }
            
            if (request.FilterOptions.RoleId != null)
            {
                accountQuery = accountQuery.Where(p => p.UserRoles.Any(r => r.Role.Id == request.FilterOptions.RoleId));
            }
        }

        // Sort
        accountQuery = request.SortOptions switch
        {
            GetAccountSortOption.CreatedTimeAscending => accountQuery.OrderBy(p => p.CreatedTime),
            GetAccountSortOption.CreatedTimeDescending => accountQuery.OrderByDescending(p => p.CreatedTime),
            GetAccountSortOption.LastUpdatedTimeAscending => accountQuery.OrderBy(p => p.LastUpdatedTime),
            GetAccountSortOption.LastUpdatedTimeDescending => accountQuery.OrderByDescending(p => p.LastUpdatedTime),
            GetAccountSortOption.DeletedTimeAscending => accountQuery.OrderBy(p => p.DeletedTime),
            GetAccountSortOption.DeletedTimeDescending => accountQuery.OrderByDescending(p => p.DeletedTime),
            _ => accountQuery.OrderBy(p => p.CreatedTime)
        };
        
        // Pagination
        var queryPaging = await _unitOfWork.GetRepository<User>()
            .GetPagination(accountQuery, request.PageNumber, request.PageSize);

        IList<GetAccountResponse> accounts = new List<GetAccountResponse>();
        foreach (var account in queryPaging.Data)
        {
            var accountEntity = await _unitOfWork.GetRepository<User>().FindAsync(p => p.Id == account.Id);
            
            // Skip if account is null
            if(accountEntity == null)
            {
                continue;
            }
            
            accounts.Add(_mapper.Map<GetAccountResponse>(accountEntity));
            
            // Map roles
            accounts.Last().Roles = accountEntity.UserRoles.Select(p => new GetRoleResponse()
            {
                Id = p.Role.Id,
                Name = p.Role.Name!
            }).ToList();
        }

        return new PaginationResponse<GetAccountResponse>(accounts, queryPaging.PageNumber, queryPaging.PageSize,
            queryPaging.TotalRecords, queryPaging.CurrentPageRecords);
    }
    
    public async Task<GetAccountDetailResponse> GetAccountDetail(Guid userId)
    {
        User? user = await _unitOfWork.GetRepository<User>().FindAsync(p => p.Id == userId);
        
        // Check if user is null
        if(user == null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "User not found");
        }
        
        // Map user to response
        GetAccountDetailResponse account = _mapper.Map<GetAccountDetailResponse>(user);
        account.Roles = user.UserRoles.Select(p => new GetRoleResponse()
        {
            Id = p.Role.Id,
            Name = p.Role.Name!
        }).ToList();
        
        return account;
    }

    public async Task UpdateAccountStatus(Guid userId, AccountStatus status)
    {
        User? user = await _unitOfWork.GetRepository<User>().FindAsync(p => p.Id == userId);

        // Check if user is null
        if (user == null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "User not found");
        }

        /*// Check if user is admin
        if(user.UserRoles.Any(p => p.Role.Name == "ADMIN"))
        {
            throw new CoreException(StatusCodes.Status401Unauthorized, "Cannot update status of admin account");
        }*/

        if (status != AccountStatus.Active)
        {
            await _tokenService.RemoveUserRefreshToken(userId);
        }
        
        user.Status = status;
        await _userManager.UpdateAsync(user);
    }
    
    public async Task AddAccountRole(Guid userId, Guid roleId)
    {
        User? user = await _unitOfWork.GetRepository<User>().FindAsync(p => p.Id == userId);
        
        // Check if user is null
        if(user == null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "User not found");
        }
        
        Role? role = await _unitOfWork.GetRepository<Role>().FindAsync(p => p.Id == roleId);
        // Check if role is null
        if(role == null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "Role not found");
        }
        
        /*// Check if user is admin
        if(user.UserRoles.Any(p => p.Role.Name == "ADMIN"))
        {
            throw new CoreException(StatusCodes.Status401Unauthorized, "Cannot update role of admin account");
        }*/
        
        // Check if user already has this role
        if(user.UserRoles.Any(p => p.Role.Id == roleId))
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "User already has this role");
        }
        
        // Add role to user
        await _userManager.AddToRoleAsync(user, role.Name!);
    }
    
    public async Task RemoveAccountRole(Guid userId, Guid roleId)
    {
        User? user = await _unitOfWork.GetRepository<User>().FindAsync(p => p.Id == userId);
        
        // Check if user is null
        if(user == null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "User not found");
        }
        
        Role? role = await _unitOfWork.GetRepository<Role>().FindAsync(p => p.Id == roleId);
        // Check if role is null
        if(role == null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "Role not found");
        }
        
        /*// Check if user is admin
        if(user.UserRoles.Any(p => p.Role.Name == "ADMIN"))
        {
            throw new CoreException(StatusCodes.Status401Unauthorized, "Cannot update role of admin account");
        }*/
        
        // Check if user has not own this role
        if(user.UserRoles.All(p => p.Role.Id != roleId))
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "User does not have this role");
        }
        
        // Remove role from user
        await _userManager.RemoveFromRoleAsync(user, role.Name!);
    }
    
    public async Task UpdateAccountRole(Guid userId, Guid roleId)
    {
        User? user = await _unitOfWork.GetRepository<User>().FindAsync(p => p.Id == userId);
        
        // Check if user is null
        if(user == null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "User not found");
        }
        
        Role? role = await _unitOfWork.GetRepository<Role>().FindAsync(p => p.Id == roleId);
        
        // Check if role is null
        if(role == null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "Role not found");
        }

        // Check if request role is admin, cannot update role of admin account
        if (role.Name == "ADMIN")
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "Cannot update role of admin account");
        }
        
        IList<UserRole> userRoles = await _unitOfWork.GetRepository<UserRole>()
            .AsQueryable()
            .Where(p => p.UserId == userId)
            .ToListAsync();
        
        // Remove all roles of user
        _unitOfWork.GetRepository<UserRole>().DeletePermanentRange(userRoles);
        
        // Add new role to user
        UserRole userRole = new UserRole()
        {
            RoleId = role.Id,
            UserId = userId,
        };
        
        await _unitOfWork.GetRepository<UserRole>().InsertAsync(userRole);
        await _unitOfWork.SaveChangesAsync();
    }
    
    
    public async Task CreateModeratorAccount(CreateModeratorAccountRequest request)
    {
        // Check if email is already used
        User? user = await _unitOfWork.GetRepository<User>()
            .AsQueryable()
            .FirstOrDefaultAsync(p => p.Email == request.Email && p.DeletedTime == null);
        if(user != null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "Email is already used");
        }
        
        var userSalt = PasswordHasher.GenerateSalt();
        // Create new user
        user = new User()
        {
            UserName = request.Email,
            Email = request.Email,
            FullName = request.FullName,
            PhoneNumber = request.PhoneNumber,
            Status = AccountStatus.Active,
            EmailConfirmed = true,
            DisplayName = request.Email,
            Salt = userSalt,
        };
        
        var generatePassword = PasswordHasher.GenerateSecurePassword();
        
        // Hash password
        string hashedPassword = PasswordHasher.HashPassword(generatePassword, user.Salt);
        
        // Add user to database
        IdentityResult result = await _userManager.CreateAsync(user, hashedPassword);
        if(!result.Succeeded)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, result.Errors.First().Description);
        }
        
        // Add role to user
        Role? role = await _unitOfWork.GetRepository<Role>().FindAsync(p => p.Name == "MODERATOR");
        if(role == null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "Role not found");
        }
        
        await _userManager.AddToRoleAsync(user, role.Name!);
        
        // Send email to user
        SendPasswordToModeratorAccountEvent message = new SendPasswordToModeratorAccountEvent()
        {
            Email = user.Email,
            FullName = user.FullName ?? user.Email,
            Password = generatePassword
        };
        await _publishEndpoint.Publish(message);
    }

    public async Task DeleteAccount(Guid accountId)
    {
        Role? roleUser = await _unitOfWork.GetRepository<Role>()
            .AsQueryable()
            .FirstOrDefaultAsync(p => p.Name == "USER");

        // Check if role is null
        if (roleUser == null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "Role not found");
        }
        
        User? user = await _unitOfWork.GetRepository<User>().FindAsync(p => p.Id == accountId && p.UserRoles
            .Any(r => r.RoleId == roleUser.Id));
        
        // Check if user is null
        if(user == null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "User not found");
        }
        
        // Check if user is admin
        if(user.UserRoles.Any(p => p.Role.Name == "ADMIN"))
        {
            throw new CoreException(StatusCodes.Status401Unauthorized, "Cannot delete admin account");
        }

        user.UserName = null;
        user.NormalizedUserName = null;
        user.Email = null;
        user.NormalizedEmail = null;
        user.PhoneNumber = null;
        user.DeletedTime = DateTimeHelper.SystemTimeNow;
        
        _unitOfWork.GetRepository<User>().Update(user);
        
        // Remove all user tokens
        var userTokens = await _unitOfWork.GetRepository<UserToken>()
            .AsQueryable()
            .Where(p => p.UserId == accountId)
            .ToListAsync();
        
        _unitOfWork.GetRepository<UserToken>().DeletePermanentRange(userTokens);
        await _unitOfWork.SaveChangesAsync();
    }
    
    public async Task DeleteModeratorAccount(Guid accountId)
    {
        Role? roleModerator = await _unitOfWork.GetRepository<Role>()
            .AsQueryable()
            .FirstOrDefaultAsync(p => p.Name == "MODERATOR");

        // Check if role is null
        if (roleModerator == null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "Role not found");
        }
        
        User? user = await _unitOfWork.GetRepository<User>().FindAsync(p => p.Id == accountId && p.UserRoles
            .Any(r => r.RoleId == roleModerator.Id));
        
        // Check if user is null
        if(user == null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "User not found");
        }
        
        // Check if user is admin
        if(user.UserRoles.Any(p => p.Role.Name == "ADMIN"))
        {
            throw new CoreException(StatusCodes.Status401Unauthorized, "Cannot delete admin account");
        }

        user.UserName = null;
        user.NormalizedUserName = null;
        user.Email = null;
        user.NormalizedEmail = null;
        user.PhoneNumber = null;
        user.DeletedTime = DateTimeHelper.SystemTimeNow;
        
        _unitOfWork.GetRepository<User>().Update(user);
        
        // Remove all user tokens
        var userTokens = await _unitOfWork.GetRepository<UserToken>()
            .AsQueryable()
            .Where(p => p.UserId == accountId)
            .ToListAsync();
        
        _unitOfWork.GetRepository<UserToken>().DeletePermanentRange(userTokens);
        await _unitOfWork.SaveChangesAsync();
    }
    /*public async Task DeleteAccountPermanent(Guid accountId)
    {
        User? user = await _userManager.FindByIdAsync(accountId.ToString());
        
        // Check if user is null
        if(user == null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "User not found");
        }
        
        // Check if user is already deleted
        if(user.DeletedTime == null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "This account is not soft deleted yet");
        }
        
        _unitOfWork.GetRepository<User>().DeletePermanent(user);
        await _unitOfWork.SaveChangesAsync();
    }*/
}
