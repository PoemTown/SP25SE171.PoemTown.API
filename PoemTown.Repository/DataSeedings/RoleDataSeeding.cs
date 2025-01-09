using PoemTown.Repository.Entities;
using PoemTown.Repository.Utils;

namespace PoemTown.Repository.DataSeedings;

public class RoleDataSeeding
{
    public static IList<Role> DefaultRoles => new List<Role>
    {
        new Role()
        {
            Id = new Guid("B74C0A77-A451-4F16-DE61-08DCDFCDB851"),
            Name = "ADMIN",
            NormalizedName = "ADMIN",
            CreatedBy = "System",
            LastUpdatedBy = "System",
            CreatedTime = DateTimeHelper.SystemTimeNow,
            LastUpdatedTime = DateTimeHelper.SystemTimeNow,
            ConcurrencyStamp = "A6WZZDMSOY6XEPH4VJRSRVTAXICX34US"
        },
        new Role()
        {
            Id = new Guid("89FCA251-F021-425B-DE62-08DCDFCDB851"),
            Name = "USER",
            NormalizedName = "USER",
            CreatedBy = "System",
            LastUpdatedBy = "System",
            CreatedTime = DateTimeHelper.SystemTimeNow,
            LastUpdatedTime = DateTimeHelper.SystemTimeNow,
            ConcurrencyStamp = "A6WZZDMSOY6XEPH4VJRSRVTAXICX34US"
        },
    };
}