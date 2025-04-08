using Microsoft.AspNetCore.Mvc;
using PoemTown.Repository.Enums.Announcements;

namespace PoemTown.Service.QueryOptions.FilterOptions.AnnouncementFilters;

public class GetAnnouncementFilterOption
{
    [FromQuery(Name = "isRead")]
    public bool? IsRead { get; set; }
    [FromQuery(Name = "type")]
    public AnnouncementType? Type { get; set; }
}