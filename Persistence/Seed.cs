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
                        IsWoodEmployee = true,
                    },
                    new AppUser
                    {
                        FirstName = "Jane",
                        LastName = "Jane",
                        Email = "jane@test.com",
                        UserName = "jane@test.com",
                        IsWoodEmployee = false,
                    },

                };

                foreach (var user in users)
                {
                    await userManager.CreateAsync(user, "Pa$$w0rd");
                }
            }
        }
    }
}