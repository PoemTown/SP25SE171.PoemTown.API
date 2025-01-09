using PoemTown.Repository.Entities;

namespace PoemTown.Repository.DataSeedings;

public class UserRoleDataSeeding
{
    public static IList<UserRole> DefaultUserRoles => new List<UserRole>
    {
        new UserRole()
        {
            UserId = new Guid("094de1df-60b1-4a58-878c-dc6909f7350b"),
            RoleId = new Guid("B74C0A77-A451-4F16-DE61-08DCDFCDB851")
        },
        new UserRole()
        {
            UserId = new Guid("a3ee2988-67b2-4017-b63b-a0dae4708359"),
            RoleId = new Guid("89FCA251-F021-425B-DE62-08DCDFCDB851")
        },
    };
}