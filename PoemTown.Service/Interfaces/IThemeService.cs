using PoemTown.Repository.Base;
using PoemTown.Service.BusinessModels.RequestModels.ThemeRequests;
using PoemTown.Service.BusinessModels.ResponseModels.ThemeResponses;
using PoemTown.Service.QueryOptions.FilterOptions.ThemeFilters;
using PoemTown.Service.QueryOptions.RequestOptions;
using PoemTown.Service.QueryOptions.SortOptions.ThemeSorts;

namespace PoemTown.Service.Interfaces;

public interface IThemeService
{
    Task CreateUserTheme(Guid userId, CreateUserThemeRequest request);

    Task<PaginationResponse<GetThemeResponse>>
        GetUserTheme(Guid userId, RequestOptionsBase<GetUserThemeFilterOption, GetUserThemeSortOption> request);

    Task UpdateUserTheme(Guid userId, UpdateUserThemeRequest request);
    Task DeleteUserTheme(Guid userId, Guid themeId);
    Task DeleteUserThemePermanent(Guid userId, Guid themeId);
    Task CreateDefaultThemeAndUserTemplate(Guid userId, bool isInUse);
}