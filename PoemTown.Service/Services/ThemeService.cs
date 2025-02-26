using AutoMapper;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PoemTown.Repository.Base;
using PoemTown.Repository.CustomException;
using PoemTown.Repository.Entities;
using PoemTown.Repository.Enums.TemplateDetails;
using PoemTown.Repository.Enums.Templates;
using PoemTown.Repository.Interfaces;
using PoemTown.Service.BusinessModels.RequestModels.ThemeRequests;
using PoemTown.Service.BusinessModels.ResponseModels.TemplateResponses;
using PoemTown.Service.BusinessModels.ResponseModels.ThemeResponses;
using PoemTown.Service.Events.ThemeEvents;
using PoemTown.Service.Interfaces;
using PoemTown.Service.QueryOptions.FilterOptions.ThemeFilters;
using PoemTown.Service.QueryOptions.RequestOptions;
using PoemTown.Service.QueryOptions.SortOptions.ThemeSorts;

namespace PoemTown.Service.Services;

public class ThemeService : IThemeService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IPublishEndpoint _publishEndpoint;

    public ThemeService(IUnitOfWork unitOfWork,
        IMapper mapper,
        IPublishEndpoint publishEndpoint)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _publishEndpoint = publishEndpoint;
    }

    public async Task CreateUserTheme(Guid userId, CreateUserThemeRequest request)
    {
        // If create theme as in use, then set all other user themes into not in use
        if (request.IsInUse == true)
        {
            await _unitOfWork.GetRepository<Theme>()
                .AsQueryable()
                .Where(p => p.UserId == userId)
                .ExecuteUpdateAsync(setters => setters.SetProperty(p => p.IsInUse, false));
        }

        /*// If create theme as default, then set all other user themes into not default
        if(request.IsDefault == true)
        {
            await _unitOfWork.GetRepository<Theme>()
                .AsQueryable()
                .Where(p => p.UserId == userId)
                .ExecuteUpdateAsync(setters => setters.SetProperty(p => p.IsDefault, false));
        }*/

        var userTheme = _mapper.Map<Theme>(request);
        userTheme.UserId = userId;

        await _unitOfWork.GetRepository<Theme>().InsertAsync(userTheme);
        await _unitOfWork.SaveChangesAsync();

        // Check if default theme not exist, then create default theme
        Theme? existDefaultTheme = await _unitOfWork.GetRepository<Theme>()
            .FindAsync(p => p.UserId == userId && p.IsDefault == true);
        if (existDefaultTheme == null)
        {
            var message = new CreateDefaultUserThemeEvent()
            {
                UserId = userId,
                IsInUse = !request.IsInUse ?? true
            };
            await _publishEndpoint.Publish(message);
        }
    }

    public async Task<PaginationResponse<GetThemeResponse>>
        GetUserTheme(Guid userId, RequestOptionsBase<GetUserThemeFilterOption, GetUserThemeSortOption> request)
    {
        // Check if default theme not exist, then create default theme
        var defaultTheme = await _unitOfWork.GetRepository<Theme>()
            .FindAsync(p => p.UserId == userId && p.IsDefault == true);

        // If any theme is in use, create default theme with status IsInUse = false and vice versa
        var existUsingTheme = await _unitOfWork.GetRepository<Theme>()
            .AsQueryable()
            .AnyAsync(p => p.UserId == userId && p.IsInUse == true);
        if (defaultTheme == null)
        {
            await CreateDefaultThemeAndUserTemplate(userId, !existUsingTheme);
        }


        var themeQuery = _unitOfWork.GetRepository<Theme>().AsQueryable();

        themeQuery = themeQuery.Where(p => p.UserId == userId);

        // IsDelete
        if (request.IsDelete == true)
        {
            themeQuery = themeQuery.Where(p => p.DeletedTime != null);
        }
        else
        {
            themeQuery = themeQuery.Where(p => p.DeletedTime == null);
        }

        // Filter
        if (request.FilterOptions != null)
        {
            if (request.FilterOptions.IsInUse != null)
            {
                themeQuery = themeQuery.Where(p => p.IsInUse == request.FilterOptions.IsInUse);
            }

            if (request.FilterOptions.IsDefault != null)
            {
                themeQuery = themeQuery.Where(p => p.IsDefault == request.FilterOptions.IsDefault);
            }
        }

        // Sort
        themeQuery = request.SortOptions switch
        {
            GetUserThemeSortOption.CreatedTimeAscending => themeQuery.OrderBy(p => p.CreatedTime),
            GetUserThemeSortOption.CreatedTimeDescending => themeQuery.OrderByDescending(p => p.CreatedTime),
            _ => themeQuery.OrderByDescending(p => p.CreatedTime)
        };

        // Pagination
        var queryPaging = await _unitOfWork.GetRepository<Theme>()
            .GetPagination(themeQuery, request.PageNumber, request.PageSize);

        //var themes = _mapper.Map<List<GetThemeResponse>>(queryPaging.Data);

        IList<GetThemeResponse> themes = new List<GetThemeResponse>();
        foreach (var theme in queryPaging.Data)
        {
            var themeEntity = await _unitOfWork.GetRepository<Theme>().FindAsync(p => p.Id == theme.Id);
            if (themeEntity == null)
            {
                throw new CoreException(StatusCodes.Status400BadRequest, "Theme not found");
            }

            themes.Add(_mapper.Map<GetThemeResponse>(themeEntity));
            // Assign author to poem by adding into the last element of the list
            themes.Last().UserTemplateDetails = _mapper.Map<IList<GetUserTemplateDetailResponse>>
            (themeEntity.ThemeUserTemplateDetails.Where(p => p.ThemeId == themeEntity.Id)
                .Select(p => p.UserTemplateDetail).ToList());
        }

        return new PaginationResponse<GetThemeResponse>(themes, queryPaging.PageNumber, queryPaging.PageSize,
            queryPaging.TotalRecords, queryPaging.CurrentPageRecords);
    }

    public async Task UpdateUserTheme(Guid userId, UpdateUserThemeRequest request)
    {
        var theme = await _unitOfWork.GetRepository<Theme>()
            .FindAsync(p => p.UserId == userId && p.Id == request.Id);

        // Check theme exist
        if (theme == null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "Theme not found");
        }

        // If update this theme to in use, then set all other user themes into not in use
        if (request.IsInUse == true)
        {
            await _unitOfWork.GetRepository<Theme>()
                .AsQueryable()
                .Where(p => p.UserId == userId)
                .ExecuteUpdateAsync(setters => setters.SetProperty(p => p.IsInUse, false));
        }

        /*// If update this theme to default, then set all other user themes into not default
        if(request.IsDefault == true)
        {
            await _unitOfWork.GetRepository<Theme>()
                .AsQueryable()
                .Where(p => p.UserId == userId)
                .ExecuteUpdateAsync(setters => setters.SetProperty(p => p.IsDefault, false));
        }*/

        _mapper.Map(request, theme);
        _unitOfWork.GetRepository<Theme>().Update(theme);
        await _unitOfWork.SaveChangesAsync();

        // Check if default theme not exist, then create default theme
        Theme? existDefaultTheme = await _unitOfWork.GetRepository<Theme>()
            .FindAsync(p => p.UserId == userId && p.IsDefault == true);
        if (existDefaultTheme == null)
        {
            var message = new CreateDefaultUserThemeEvent()
            {
                UserId = userId,
                IsInUse = !request.IsInUse ?? !theme.IsInUse
            };
            await _publishEndpoint.Publish(message);
        }
    }

    public async Task DeleteUserTheme(Guid userId, Guid themeId)
    {
        var theme = await _unitOfWork.GetRepository<Theme>()
            .FindAsync(p => p.UserId == userId && p.Id == themeId);

        // Check theme exist
        if (theme == null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "Theme not found");
        }

        // Check is default theme, then cannot delete
        if (theme.IsDefault == true)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "Cannot delete default theme");
        }

        // Check is in use, then set default theme in use, set current theme not in use and finally soft delete theme
        if (theme.IsInUse)
        {
            var themeDefault = await _unitOfWork.GetRepository<Theme>()
                .FindAsync(p => p.UserId == userId && p.IsDefault == true);
            if (themeDefault == null)
            {
                await CreateDefaultThemeAndUserTemplate(userId, true);
            }

            // Set default theme in use
            themeDefault.IsInUse = true;
            _unitOfWork.GetRepository<Theme>().Update(themeDefault);

            // Set current theme not in use
            theme.IsInUse = false;
            _unitOfWork.GetRepository<Theme>().Update(theme);
        }

        _unitOfWork.GetRepository<Theme>().Delete(theme);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteUserThemePermanent(Guid userId, Guid themeId)
    {
        var theme = await _unitOfWork.GetRepository<Theme>()
            .FindAsync(p => p.UserId == userId && p.Id == themeId);

        // Check theme exist
        if (theme == null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "Theme not found");
        }

        // Check if theme has not been soft deleted, then cannot delete permanently
        if (theme.DeletedTime == null)
        {
            throw new CoreException(StatusCodes.Status400BadRequest, "Theme has not been soft deleted");
        }

        _unitOfWork.GetRepository<Theme>().DeletePermanent(theme);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task CreateDefaultThemeAndUserTemplate(Guid userId, bool isInUse)
    {
        var defaultTheme = await _unitOfWork.GetRepository<Theme>()
            .FindAsync(p => p.UserId == userId && p.IsDefault == true);

        // Check default theme exist
        if (defaultTheme != null)
        {
            return;
        }

        // Create default theme
        var theme = new Theme
        {
            Name = "Theme mặc định",
            IsDefault = true,
            IsInUse = isInUse,
            UserId = userId
        };

        await _unitOfWork.GetRepository<Theme>().InsertAsync(theme);

        // Create default user template exist
        UserTemplate? userTemplate = await _unitOfWork.GetRepository<UserTemplate>()
            .FindAsync(p => p.UserId == userId && p.TagName == "Default");

        if (userTemplate == null)
        {
            MasterTemplate? masterTemplate =
                await _unitOfWork.GetRepository<MasterTemplate>().FindAsync(p => p.TagName == "Default");

            IList<UserTemplateDetail> userTemplateDetails;
            // Create master template and master template detail if not exist
            if (masterTemplate == null)
            {
                masterTemplate = new MasterTemplate()
                {
                    TemplateName = "Template mặc định",
                    Status = TemplateStatus.Active,
                    Price = 0,
                    TagName = "Default",
                    Type = TemplateType.Bundle,
                };
                await _unitOfWork.GetRepository<MasterTemplate>().InsertAsync(masterTemplate);

                // Create master template details
                var masterTemplateDetails = GetDefaultMasterTemplateDetailData(masterTemplate.Id);
                await _unitOfWork.GetRepository<MasterTemplateDetail>().InsertRangeAsync(masterTemplateDetails);
                await _unitOfWork.SaveChangesAsync();
            }

            // Then assign MasterTemplate and MasterTemplateDetail to UserTemplate and UserTemplateDetail 
            // Create user template
            userTemplate = new UserTemplate()
            {
                UserId = userId,
                MasterTemplateId = masterTemplate.Id,
                TemplateName = masterTemplate.TemplateName,
                Type = masterTemplate.Type,
                Status = masterTemplate.Status,
                TagName = masterTemplate.TagName
            };
            await _unitOfWork.GetRepository<UserTemplate>().InsertAsync(userTemplate);

            // Create user template details
            userTemplateDetails = await _unitOfWork.GetRepository<MasterTemplateDetail>()
                .AsQueryable()
                .Where(p => p.MasterTemplateId == masterTemplate.Id)
                .Select(p => new UserTemplateDetail()
                {
                    UserTemplateId = userTemplate.Id,
                    ColorCode = p.ColorCode,
                    Type = p.Type,
                    Image = p.Image
                })
                .ToListAsync();
            await _unitOfWork.GetRepository<UserTemplateDetail>().InsertRangeAsync(userTemplateDetails);

            // Finally assign UserTemplateDetail ThemeUserTemplateDetail
            // Create theme user template
            var themeUserTemplateDetails = userTemplateDetails.Select(p => new ThemeUserTemplateDetail()
            {
                ThemeId = theme.Id,
                UserTemplateDetailId = p.Id
            }).ToList();
            await _unitOfWork.GetRepository<ThemeUserTemplateDetail>().InsertRangeAsync(themeUserTemplateDetails);
        }


        await _unitOfWork.SaveChangesAsync();
    }

    public static IList<MasterTemplateDetail> GetDefaultMasterTemplateDetailData(Guid masterTemplateId)
    {
        IList<MasterTemplateDetail> userTemplateDetails = new List<MasterTemplateDetail>()
        {
            new()
            {
                MasterTemplateId = masterTemplateId,
                Type = TemplateDetailType.Header,
                Image =
                    "https://s3-hcm5-r1.longvan.net/poemtown.staging/templates/default header-1740549085.jpg",
            },
            new()
            {
                MasterTemplateId = masterTemplateId,
                Type = TemplateDetailType.NavBackground
            },
            new()
            {
                MasterTemplateId = masterTemplateId,
                Type = TemplateDetailType.NavBorder,
                ColorCode = "#cccccc"
            },
            new()
            {
                MasterTemplateId = masterTemplateId,
                Type = TemplateDetailType.MainBackground,
            },
            new()
            {
                MasterTemplateId = masterTemplateId,
                Type = TemplateDetailType.AchievementBackground,
            },
            new()
            {
                MasterTemplateId = masterTemplateId,
                Type = TemplateDetailType.AchievementBorder,
                ColorCode = "#E4EF00"
            },
            new()
            {
                MasterTemplateId = masterTemplateId,
                Type = TemplateDetailType.StatisticBackground,
            },
            new()
            {
                MasterTemplateId = masterTemplateId,
                Type = TemplateDetailType.StatisticBorder,
                ColorCode = "#cccccc"
            },
        };
        return userTemplateDetails;
    }
}