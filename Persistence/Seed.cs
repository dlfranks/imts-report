using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public static class Seed
    {
        public static async Task SeedData(AppContext context, UserManager<AppUser> userManager)
        {

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
                if (!userManager.Users.Any())
                {
                    var officeList = new List<AppUserOfficeRole>();
                    var users = new List<AppUser>();
                    users.Add(new AppUser
                    {
                        FirstName = "Deana",
                        LastName = "Franks",
                        Email = "deana.franks@woodplc.com",
                        UserName = "deana.franks@woodplc.com",
                        IsImtsUser = true,
                        MainOfficeId = 1,
                        IsSuperUser = true,
                        CreateDate = System.DateTime.Now,
                        UpdatedDate = System.DateTime.Now
                    });

                    officeList.Add(new AppUserOfficeRole
                    {
                        AppUserId = userManager.Users.Where(q => q.Email == "deana.franks@woodplc.com")
                                .Select(q => q.Id).First(),
                        RoleId = context.OfficeRoles.Where(q => q.Id == (int)OfficeRoleEnum.Administrator).Select(q => q.Id).First(),
                        ImtsOfficeId = 26
                    });

                    officeList.Add(new AppUserOfficeRole
                    {
                        AppUserId = userManager.Users.Where(q => q.Email == "deana.franks@woodplc.com")
                                .Select(q => q.Id).First(),
                        RoleId = context.OfficeRoles.Where(q => q.Id == (int)OfficeRoleEnum.User).Select(q => q.Id).First(),
                        ImtsOfficeId = 11
                    });

                    officeList.Add(new AppUserOfficeRole
                    {
                        AppUserId = userManager.Users.Where(q => q.Email == "deana.franks@woodplc.com")
                                 .Select(q => q.Id).First(),
                        RoleId = context.OfficeRoles.Where(q => q.Id == (int)OfficeRoleEnum.User).Select(q => q.Id).First(),
                        ImtsOfficeId = 2
                    });

                    users.Add(new AppUser
                    {
                        FirstName = "Jane",
                        LastName = "Jane",
                        Email = "jane@test.com",
                        UserName = "jane@test.com",
                        IsImtsUser = false,
                        CreateDate = System.DateTime.Now,
                        UpdatedDate = System.DateTime.Now
                    });

                    officeList.Add(new AppUserOfficeRole
                    {
                        AppUserId = userManager.Users.Where(q => q.Email == "jane@test.com")
                                .Select(q => q.Id).First(),
                        RoleId = context.OfficeRoles.Where(q => q.Id == (int)OfficeRoleEnum.User).Select(q => q.Id).First(),
                        ImtsOfficeId = 26
                    });

                    foreach (var user in users)
                    {
                        await userManager.CreateAsync(user, "Pa$$w0rd");
                    }

                    await context.AddRangeAsync(officeList);
                    await context.SaveChangesAsync();
                }

        }








    }
}