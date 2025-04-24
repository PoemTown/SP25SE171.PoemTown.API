using PoemTown.Repository.Base;
using PoemTown.Repository.CustomException;
using PoemTown.Repository.Enums.Accounts;
using PoemTown.Service.BusinessModels.RequestModels.AccountRequests;
using PoemTown.Service.BusinessModels.ResponseModels.AccountResponses;
using PoemTown.Service.QueryOptions.FilterOptions.AccountFilters;
using PoemTown.Service.QueryOptions.RequestOptions;
using PoemTown.Service.QueryOptions.SortOptions.AccountSorts;

namespace PoemTown.Service.Interfaces;

public interface IAccountService
    {
        /// <summary>
        /// Confirms a user's email by validating the provided OTP and updating the user's email confirmation status.
        /// </summary>
        /// <param name="request">The email confirmation request containing the email and OTP.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        /// <exception cref="CoreException">Thrown if the user is not found, email is already confirmed, OTP is invalid, or OTP is expired.</exception>
        Task ConfirmEmail(ConfirmEmailRequest request);

        /// <summary>
        /// Sends a new email OTP to the user for email confirmation purposes.
        /// </summary>
        /// <param name="request">The request containing the user's email address.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        /// <exception cref="CoreException">Thrown if the user is not found.</exception>
        Task SendEmailOtp(ResendEmailConfirmationRequest request);

        /// <summary>
        /// Changes a user's password by validating the current password and replacing it with a new one.
        /// </summary>
        /// <param name="email">The email address of the user.</param>
        /// <param name="request">The change password request containing the current and new passwords.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        /// <exception cref="CoreException">Thrown if the user is not found, email is not confirmed, or the current password is incorrect.</exception>
        Task ChangePassword(Guid userId, ChangePasswordRequest request);

        /// <summary>
        /// Initiates the password reset process by generating a reset password token and publishing it to an email event.
        /// </summary>
        /// <param name="request">The request containing the user's email address.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        /// <exception cref="CoreException">Thrown if the user is not found or email is not confirmed.</exception>
        Task ForgotPassword(ForgotPasswordRequest request);

        /// <summary>
        /// Resets a user's password using a reset token and timestamp, and updates the user's password.
        /// </summary>
        /// <param name="request">The request containing the reset token, new password, and expiration timestamp.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        /// <exception cref="CoreException">Thrown if the user is not found, email is not confirmed, or the reset token is expired or invalid.</exception>
        Task NewPasswordForgot(NewPasswordForgotRequest request);

        Task<PaginationResponse<GetAccountResponse>>
            GetAccounts(RequestOptionsBase<GetAccountFilterOption, GetAccountSortOption> request);

        Task<GetAccountDetailResponse> GetAccountDetail(Guid userId);
        Task UpdateAccountStatus(Guid userId, AccountStatus status);
        Task AddAccountRole(Guid userId, Guid roleId);
        Task RemoveAccountRole(Guid userId, Guid roleId);
        Task UpdateAccountRole(Guid userId, Guid roleId);
        Task CreateModeratorAccount(CreateModeratorAccountRequest request);
        Task DeleteAccount(Guid accountId);

        Task DeleteModeratorAccount(Guid accountId);
        /*
        Task DeleteAccountPermanent(Guid accountId);
    */
    }