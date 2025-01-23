using PoemTown.Repository.Entities;
using PoemTown.Repository.Enums;
using PoemTown.Repository.Enums.Accounts;

namespace PoemTown.Repository.DataSeedings;

public class AccountDataSeeding
{
    public static IList<User> DefaultUsers => new List<User>
    {
        new User()
        {
            Id = new Guid("094de1df-60b1-4a58-878c-dc6909f7350b"),
            FullName = "admin",
            UserName = "admin@gmail.com",
            NormalizedUserName = "admin",
            Email = "admin@gmail.com",
            NormalizedEmail = "ADMIN@GMAIL.COM",
            EmailConfirmed = true,
            Status = AccountStatus.Active,
            Salt = "UQquiGRiRIG1g/4gdm/sfMY7Kk0qqcV8iAYaY8eRmAo=",
            SecurityStamp = "A6WZZDMSOY6XEPH4VJRSRVTAXICX34US",
            PasswordHash = "AQAAAAIAAYagAAAAEKlMNvvuvDkRs2XwysLan5iHCJP9ImDgi6iw39nygXtE1ant3Kv5n2oi6hZCqwDybA==",
        },
        new User
        {
            Id = new Guid("a3ee2988-67b2-4017-b63b-a0dae4708359"),
            FullName = "user",
            UserName = "user@gmail.com",
            NormalizedUserName = "user",
            Email = "user@gmail.com",
            NormalizedEmail = "USER@GMAIL.COM",
            Status = AccountStatus.Active,
            EmailConfirmed = true,
            Salt = "UQquiGRiRIG1g/4gdm/sfMY7Kk0qqcV8iAYaY8eRmAo=",
            SecurityStamp = "A6WZZDMSOY6XEPH4VJRSRVTAXICX34US",
            PasswordHash = "AQAAAAIAAYagAAAAEKlMNvvuvDkRs2XwysLan5iHCJP9ImDgi6iw39nygXtE1ant3Kv5n2oi6hZCqwDybA==",
        }
    };
}