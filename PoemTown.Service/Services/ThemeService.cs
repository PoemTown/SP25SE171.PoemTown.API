using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PoemTown.Repository.Base;
using PoemTown.Repository.CustomException;
using PoemTown.Repository.Entities;
using PoemTown.Repository.Interfaces;
using PoemTown.Service.BusinessModels.RequestModels.ThemeRequests;
using PoemTown.Service.BusinessModels.ResponseModels.ThemeResponses;
using PoemTown.Service.Interfaces;
using PoemTown.Service.QueryOptions.FilterOptions.ThemeFilters;
using PoemTown.Service.QueryOptions.RequestOptions;
using PoemTown.Service.QueryOptions.SortOptions.ThemeSorts;

namespace PoemTown.Service.Services;

public class ThemeService : IThemeService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ThemeService(IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
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

        // If create theme as default, then set all other user themes into not default
        if(request.IsDefault == true)
        {
            await _unitOfWork.GetRepository<Theme>()
                .AsQueryable()
                .Where(p => p.UserId == userId)
                .ExecuteUpdateAsync(setters => setters.SetProperty(p => p.IsDefault, false));
        }
        
        var userTheme = _mapper.Map<Theme>(request);
        userTheme.UserId = userId;
        
        await _unitOfWork.GetRepository<Theme>().InsertAsync(userTheme);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<PaginationResponse<GetThemeResponse>>
        GetUserTheme(Guid userId, RequestOptionsBase<GetUserThemeFilterOption, GetUserThemeSortOption> request)
    {
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
        
        var themes = _mapper.Map<List<GetThemeResponse>>(queryPaging.Data);

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
        
        // If update this theme to default, then set all other user themes into not default
        if(request.IsDefault == true)
        {
            await _unitOfWork.GetRepository<Theme>()
                .AsQueryable()
                .Where(p => p.UserId == userId)
                .ExecuteUpdateAsync(setters => setters.SetProperty(p => p.IsDefault, false));
        }

        _mapper.Map(request, theme);
        _unitOfWork.GetRepository<Theme>().Update(theme);
        await _unitOfWork.SaveChangesAsync();
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
                throw new CoreException(StatusCodes.Status400BadRequest, "Default theme not found");
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
}