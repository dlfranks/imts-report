
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Middleware;
using API.Services.Interfaces;
using API.Services.Models;
using API.ViewModels;
using Application.Core;
using Domain;
using Domain.imts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Persistence;

namespace API.Services.Models
{
    public class UserSetting
    {
        public string appUserId { get; set; }
        public string currentOfficeRoleName { get; set; }
        public int currentOfficeId { get; set; }
        public string userName { get; set; }
        public string fullName { get; set; }
        public string email { get; set; }
        public int? mainOfficeId { get; set; }
        public string mainOfficeName { get; set; }
        public bool IsImtsUser { get; set; }
        public int? imtsEmployeeId { get; set; }

        public bool isAuthenticated
        {
            get
            {
                return (!string.IsNullOrEmpty(appUserId));
            }
        }

        public bool _isSuperUser { get; set; }
        public bool isSuperUser
        {
            get
            {
                return (isAuthenticated && _isSuperUser);
            }
        }

        public bool isUserInOffice(int officeId)
        {
            if (!isAuthenticated)
                return (false);
            if (officeId <= 0)
                return (false);
            if(_memberOffices == null)
                return false;
            
            return _memberOffices.Where(q => q.OfficeId == officeId).Count() > 0;
        }

        public List<AppUserOfficeRoleViewModel> _memberOffices { get; set; }
        public List<AppUserOfficeRoleViewModel> memberOffices
        {
            get
            {
                return (_memberOffices);
            }
        }
    }

}
namespace API.Services
{

    public class UserService
    {
        private readonly ImtsContext _imtsContext;
        private readonly UserManager<AppUser> _userManager;

        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public UserService(
            IUnitOfWork unitOfWork,
            IHttpContextAccessor httpContextAccessor,
            ImtsContext imtsContext,
            UserManager<AppUser> userManager

        )
        {
            _imtsContext = imtsContext;
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;

        }

        

        public async Task<UserSetting> CreateUserSettings()
        {
            var currentUserSetting =  (UserSetting)_httpContextAccessor.HttpContext.Items["CurrentUserSettings"];
            if(currentUserSetting != null) return currentUserSetting;
            var us = new UserSetting();
            var userId = _httpContextAccessor.HttpContext.Items["UserId"].ToString();
            us.currentOfficeId = int.Parse(_httpContextAccessor.HttpContext.Items["OfficeId"].ToString());
            var CurrentUserSettings = (UserSetting)_httpContextAccessor.HttpContext.Items["CurrentUserSettings"];
            var appUser = await _userManager.Users.FirstOrDefaultAsync(q => q.Id == userId);
            var appUserOfficeRoles = await getUserOfficeRoles(userId);
            var currentOfficeRole = appUserOfficeRoles.Where(q => q.ImtsOfficeId == us.currentOfficeId).FirstOrDefault();

            us.appUserId = userId;
            us.email = appUser.Email;
            us.fullName = appUser.FirstName + " " + appUser.LastName;
            us.currentOfficeRoleName = currentOfficeRole.Role.RoleName;
            var isUserInRoleInOffice = appUserOfficeRoles.Where(q => q.ImtsOfficeId == appUser.MainOfficeId
                && q.Role.RoleName == OfficeRoleEnum.Super.ToString().ToLower()).ToList();

            us._isSuperUser = isUserInRoleInOffice.Count > 0;
            us._memberOffices = appUserOfficeRoles.Select(q => new AppUserOfficeRoleViewModel
                {
                    UserId = q.AppUserId,
                    OfficeId = q.ImtsOfficeId,
                    OfficeName = q.ImtsOffice.name,
                    RoleId = q.RoleId,
                    RoleName = q.Role.RoleName 
                    }).ToList();
            us.imtsEmployeeId = appUser.ImtsEmployeeId;
            us.IsImtsUser = appUser.IsImtsUser;
            CurrentUserSettings = us;

            return us;

        }
        public void RemoveUserSetting()
        {
            _httpContextAccessor.HttpContext.Items["CurrentUserSettings"] = null;
        }
        
        public async Task<Result<AppUser>> CreateUser(RegisterDTO registerDto, int officeId)
        {
            List<string> errors = new List<string>();
            Employee imtsUser = null;
            AppUser user = new AppUser();
            user = await _userManager.FindByNameAsync(registerDto.Email);

            user.FirstName = registerDto.FirstName;
            user.LastName = registerDto.LastName;
            user.Email = registerDto.Email;
            user.UserName = registerDto.Email;
            user.IsImtsUser = registerDto.IsImtsUser;
            user.MainOfficeId = officeId;


            if (registerDto.IsImtsUser)
            {
                var employeeResult = await GetImtsUserByUserName(registerDto.Email);
                imtsUser = employeeResult.Value;
                if (employeeResult.IsSuccess)
                {
                    user.MainOfficeId = employeeResult.Value.mainOfficeId;
                    user.ImtsUserName = employeeResult.Value.userName;
                    user.IsImtsUser = true;
                    user.ImtsEmployeeId = employeeResult.Value.id;
                }
            }

            //create AppUser
            var result = await _userManager.CreateAsync(user, registerDto.Password);
            //Create a role to the user
            await _unitOfWork.Users.addRoleToUser(user, officeId, registerDto.RoleName);


            if (await _unitOfWork.TryCommit())
            {
                return Result<AppUser>.Success(user);
            }
            else
            {
                return Result<AppUser>.Failure("Failed to create a user.");
            }
        }

        public async Task<IEnumerable<AppUserOfficeRole>> getUserOfficeRoles(string appUserId)
        {
            var userOfficesRoles = await _unitOfWork.Users.getUsersRoles(appUserId).ToListAsync();
            var imtsOffices = await _imtsContext.Offices.ToListAsync();
            foreach (var uor in userOfficesRoles)
            {
                var o = imtsOffices.Where(q => q.id == uor.ImtsOfficeId).First();
                if (o != null)
                    uor.ImtsOffice = o;
            }
            return userOfficesRoles;
        }
        public async Task<AppUserOfficeRole> getUserOfficeRole(string appUserId, int officeId)
        {
            var userOfficesRole = await _unitOfWork.Users.getUsersRoles(appUserId).Where(q => q.ImtsOfficeId == officeId).FirstOrDefaultAsync();
            var o = await _imtsContext.Offices.Where(q => q.id == userOfficesRole.ImtsOfficeId).FirstAsync();
            if (o != null)
                userOfficesRole.ImtsOffice = o;

            return userOfficesRole;
        }
        public async Task<List<IDValuePair>> GetUserOffices(AppUser user)
        {

            var result = await GetImtsUserByUserName(user.UserName);
            //ImtsUser
            if (result.IsSuccess)
            {
                var offices = await GetUserOfficesFromImts(result.Value.id);
                return offices;
            }
            //NonImtsUser will get the current office
            else
            {
                var lst = new List<IDValuePair>();
                var office = await GetOfficeFromImts(user.MainOfficeId);
                if (office == null) return lst;
                lst.Add(new IDValuePair { id = office.id, name = office.name });
                return lst;
            }

        }
        public async Task<Office> GetOfficeFromImts(int officeId)
        {
            return await _imtsContext.Offices.Where(q => q.id == officeId).FirstAsync();
        }
        public async Task<List<IDValuePair>> GetUserOfficesFromImts(int employeeId)
        {
            var list = await _imtsContext.UsersInOfficeRoles.Include(q => q.office)
            .Where(q => q.employeeId == employeeId).GroupBy(q => q.officeId).Select(g => g.First().office).ToListAsync();
            var lst = list.Select(u => new IDValuePair() { id = u.id, name = u.name }).ToList();

            return lst;
        }
        public async Task<Employee> GetImtsUserById(int employeeId)
        {
            Employee employee = await _imtsContext.Employees.Where(q => q.id == employeeId && q.active == true).FirstOrDefaultAsync();
            if (employee == null) return null;
            return employee;
        }
        public async Task<bool> IsImtsUser(string userName)
        {
            var imtsUser = await GetImtsUserByUserName(userName);
            if(imtsUser != null)
                return true;
            else return false;
        }

        public async Task<Result<Employee>> GetImtsUserByUserName(string userName)
        {

            userName = userName.ToLower();

            //If an email was specified, trim it for amec/amecfw users.  All other users, leave it on there.
            var emin = userName.IndexOf('@');
            if (emin > -1)
            {
                var em = userName.Substring(emin + 1);
                if (em.ToLower() == "amec.com" || em.ToLower() == "amecfw.com" || em.ToLower() == "woodplc.com")
                {
                    userName = userName.Substring(0, emin);
                }
            }

            //If no domain was specified and it is an internal address, then
            //autopopulate the domain for situations where there is only one user
            userName = userName.Replace('/', '\\');
            if (userName.IndexOf('\\') == -1 && userName.IndexOf('@') == -1)
            {
                var e = _imtsContext.Employees.Where(q => q.userName == userName && q.active == true);
                //Straight username match
                if (e.Count() == 0)
                {
                    //Pick domain account if only one (more than one you need to specify)
                    var tmp = "\\" + userName;
                    e = _imtsContext.Employees.Where(q => q.userName.EndsWith(tmp));
                    if (e.Count() == 1)
                    {
                        var result = await e.FirstAsync();
                        userName = result.userName;
                    }
                }
            }
            Employee employee = await _imtsContext.Employees.Where(q => q.userName == userName && q.active == true).FirstOrDefaultAsync();
            if (employee != null)
                return Result<Employee>.Success(employee);
            else
                return Result<Employee>.Failure("User name cannot be found in Imts.");

        }
        public async Task<string> GetImtsUserName(string userName)
        {
            var employee = await _imtsContext.Employees.Where(q => q.userName == userName).FirstOrDefaultAsync();
            if (employee == null)
                return employee.userName;
            else
                return "";
        }

        public IQueryable<UsersInOfficeRole> UserRoles(int employeeId)
        {
            return _imtsContext.UsersInOfficeRoles
                .Include(q => q.office)
                .Include(q => q.userRole)
                .Where(q => q.employeeId == employeeId);
        }
        public IQueryable<UsersInOfficeRole> UserRolesInOffice(int employeeId, int officeId)
        {
            return UserRoles(employeeId).Where(q => q.officeId == officeId);
        }
        public async Task<string[]> GetUserRolesInOffice(int employeeId, int officeId)
        {
            return await UserRolesInOffice(employeeId, officeId).Select(q => q.userRole.roleName).ToArrayAsync();
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
            if (currentUser.isSuperUser || currentUser.currentOfficeRoleName == OfficeRoleEnum.Administrator.ToString().ToLower())
                return true;

            if (autoThrow) throw new InvalidOperationException("User " + currentUser.userName + " does not have permission to " + action.ToString() + " on employee " + userId);
            return (false);
        }

    }
}