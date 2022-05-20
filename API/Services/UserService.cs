using System;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Application.Interfaces;
using Application.User;
using Domain;
using Domain.imts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace API.Services
{
    public class UserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<UserService> _logger;

        private readonly UserManager<AppUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        public UserService(IHttpContextAccessor httpContextAccessor,
        UserManager<AppUser> userManager, IUnitOfWork unitOfWork, ILogger<UserService> logger)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;
            _userManager = userManager;


        }
        public async Task<UserSetting> CreateUserSettings()
        {
            var currentUserSetting = (UserSetting)_httpContextAccessor.HttpContext.Items["CurrentUserSettings"];
            if (currentUserSetting != null) return currentUserSetting;
            var us = new UserSetting();
            var userId = _httpContextAccessor.HttpContext.Items["UserId"].ToString();
            if (userId == null) return null;
            var officeId = int.Parse(_httpContextAccessor.HttpContext.Items["OfficeId"].ToString());
            if (officeId <= 0) return null;
            var appUser = await _userManager.Users.FirstOrDefaultAsync(q => q.Id == userId);
            var appUserOfficeRoles = await _unitOfWork.Users.getAppUsersOfficeRoleByUserId(userId).ToListAsync();
            us.currentOfficeId = officeId;
            us.imtsEmployeeId = appUser.ImtsEmployeeId;
            us.userName = appUser.UserName;
            var currentOfficeRole = appUserOfficeRoles.Where(q => q.ImtsOfficeId == us.currentOfficeId).FirstOrDefault();
            us.appUserId = userId;
            us.email = appUser.Email;
            us.fullName = appUser.FirstName + " " + appUser.LastName;
            us.roleName = currentOfficeRole.Role.RoleName;
            us._isSuperUser = appUser.IsImtsUser;
            
            if(us.isSuperUser)
            {
                us._memberOffices = await _unitOfWork.Users.getImtsAllOffices();

            }else{
                us._memberOffices = appUserOfficeRoles.Select(q => new IDValuePair
                {
                    id = q.ImtsOfficeId,
                    name = q.ImtsOffice.name
                }).ToList();
            }
            
            us.imtsEmployeeId = appUser.ImtsEmployeeId;
            us.IsImtsUser = appUser.IsImtsUser;
            return us;

        }

        public async Task<bool> permissionForEmployee(PermissionAction action, string userId = null, bool autoThrow = false)
        {
            //Only Imts users can create users of this app;
            var currentUser = await CreateUserSettings();
            if (currentUser == null) return false;


            //Can't delete yourself no matter who you are
            if (currentUser.appUserId == userId && action == PermissionAction.Delete)
            {
                if (autoThrow) throw new InvalidOperationException("User " + currentUser.userName + " does not have permission to " + action.ToString() + " on employee " + userId);
                return (false);
            }

            //Can do anything else to yourself
            if (currentUser.appUserId == userId) return (true);
            //Otherwise only these guys have permission to do anything else
            if (currentUser.isSuperUser || currentUser.roleName == OfficeRoleEnum.Administrator.ToString().ToLower())
                return true;

            if (autoThrow) throw new InvalidOperationException("User " + currentUser.userName + " does not have permission to " + action.ToString() + " on employee " + userId);
            return (false);
        }
    }
}