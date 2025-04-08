using PoemTown.Service.BusinessModels.ResponseModels.AchievementRespponses;
using PoemTown.Service.BusinessModels.ResponseModels.LeaderBoardResponses;
using PoemTown.Service.BusinessModels.ResponseModels.StatisticsResponse;
using PoemTown.Service.BusinessModels.ResponseModels.TemplateResponses;

namespace PoemTown.Service.BusinessModels.ResponseModels.UserResponses;

public class GetUserOnlineProfileResponse
{
    public Guid Id { get; set; }
    public string UserName { get; set; }
    public string DisplayName { get; set; }
    public string Avatar { get; set; }
    public string Email { get; set; }
    public string Gender { get; set; }
    public DateOnly? DateOfBirth { get; set; }
    public int TotalFollowers { get; set; }
    public int TotalFollowings { get; set; }
    public bool IsMine { get; set; }
    public string Bio { get; set; }
    public bool? IsFollowed { get; set; } = false;
    public IList<GetUserTemplateDetailInUserThemeResponse>? UserTemplateDetails { get; set; }
    public IList<GetAchievementResponse>? Achievements { get; set; }
    public StatisticResponse? UserStatistic { get; set; }
}