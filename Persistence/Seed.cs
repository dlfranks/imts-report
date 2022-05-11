using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Identity;

namespace Persistence
{
    public static class Seed
    {
        public static async Task SeedData(AppContext context, UserManager<AppUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var users = new List<AppUser>
                {
                    new AppUser
                    {
                        FirstName = "Deana",
                        LastName = "Franks",
                        Email = "deana.franks@woodplc.com",
                        UserName = "deana.franks@woodplc.com",
                        IsImtsUser = true,
                        MainOfficeId = 1,

                    },
                    new AppUser
                    {
                        FirstName = "Jane",
                        LastName = "Jane",
                        Email = "jane@test.com",
                        UserName = "jane@test.com",
                        IsImtsUser = false,
                    },

                };

                foreach (var user in users)
                {
                    await userManager.CreateAsync(user, "Pa$$w0rd");
                }
            }
            if (!context.OfficeRoles.Any())
            {
                var roles = new List<OfficeRole>
                {
                    new OfficeRole
                    {
                        RoleName="super"
                    },
                    new OfficeRole
                    {
                        RoleName="administrator"
                    },
                    new OfficeRole
                    {
                        RoleName="user"
                    },
                };

                await context.AddRangeAsync(roles);
                await context.SaveChangesAsync();

            }

        }
    }
}