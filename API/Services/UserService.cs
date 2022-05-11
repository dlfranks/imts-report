
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Services.Models;
using Application.Core;
using Domain;
using Domain.imts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace API.Services.Models
{
    public class UserSetting
    {
        public string[] roles { get; set; }
        public string userName { get; set; }
        public string fullName { get; set; }
        public string email { get; set; }
        public int? officeId { get; set; }
        public string officeName { get; set; }
        public UnitSystem officeUnitSystem { get; set; }
        public OfficeRegion officeRegion { get; set; }
        public int? employeeId { get; set; }
        public bool _isProjectManager { get; set; }
        public bool isProjectManager
        {
            get
            {
                return (isAuthenticated && _isProjectManager);
            }
        }

        public bool _isProjectEngineer { get; set; }
        public bool isProjectEngineer
        {
            get
            {
                return (isAuthenticated && _isProjectEngineer);
            }
        }

        public bool isAuthenticated
        {
            get
            {
                return (employeeId.HasValue);
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

        public bool isInRole(string role)
        {
            if (!isAuthenticated)
                return (false);
            if (roles == null)
                return (false);
            if (String.IsNullOrWhiteSpace(role))
                return (false);
            return roles.Contains(role.ToLower());
        }

        public List<IDValuePair> _memberOffices { get; set; }
        public List<IDValuePair> memberOffices
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
        private readonly TokenService _tokenService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private IEntityScope _Scope;
        public UserService(
            IUnitOfWork unitOfWork,
            IHttpContextAccessor httpContextAccessor,
            ImtsContext imtsContext,
            UserManager<AppUser> userManager, 
            TokenService tokenService
        )
        {
            _imtsContext = imtsContext;
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _tokenService = tokenService;
        }
        
        public IEntityScope Scope
        {
            get
            {
                var token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                if (token != null)
                {
                    var officeId = _tokenService.ValidateJwtToken(token);
                    if (officeId != null && officeId > 0)
                    {
                        _Scope = new EntityScope();
                        _Scope.officeId = officeId ?? 0;
                        return _Scope;
                    }

                }
                return null;

            }
            set
            {
                _Scope = new EntityScope();
                _Scope.officeId = value.officeId;
            }
        }


        public IEntityScope SwitchOffice(int officeId)
        {
            if(_Scope == null)
                _Scope = new EntityScope();
            _Scope.officeId = officeId;
            return _Scope;
        }

        

        public async Task<Result<AppUser>> CreateUser(RegisterDTO registerDto)
        {
            List<string> errors = new List<string>();
            Employee imtsUser = null;

            var user = new AppUser
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                Email = registerDto.Email,
                UserName = registerDto.Email,
                MainOfficeId = Scope.officeId,
                IsImtsUser = registerDto.IsImtsUser
            };

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
            await _unitOfWork.Users.addRoleToUser(user, Scope.officeId, registerDto.RoleName);


            if (await _unitOfWork.TryCommit())
            {
                return Result<AppUser>.Success(user);
            }
            else
            {
                return Result<AppUser>.Failure("Failed to create a user.");
            }
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
        public async Task<bool> IsInRoles(AppUser user, string role)
        {
            var roles = await GetUserRolesInOffice(user.ImtsEmployeeId, Scope.officeId);
            if (roles == null)
                return (false);
            if (String.IsNullOrWhiteSpace(role))
                return (false);
            return roles.Contains(role.ToLower());
        }
        public async Task<bool> permissionForEmployee(PermissionAction action, string userId = null, bool autoThrow = false)
        {
            //Only Imts users can create users of this app;
            var currentUser = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            if (currentUser == null) return false;
            Employee currentEmployee = await GetImtsUserById(currentUser.ImtsEmployeeId);
            AppUser appUser = null; Employee appUserEmployee = null;
            if (!string.IsNullOrEmpty(userId))
                appUser = await _userManager.Users.Where(q => q.Id == userId).FirstOrDefaultAsync();

            if (appUser != null && appUser.ImtsEmployeeId > 0)
                appUserEmployee = await GetImtsUserById(appUser.ImtsEmployeeId);


            //Can't delete yourself no matter who you are
            if (currentUser.Id == appUser.Id && action == PermissionAction.Delete)
            {
                if (autoThrow) throw new InvalidOperationException("User " + currentUser.UserName + " does not have permission to " + action.ToString() + " on employee " + appUser.Email.ToString());
                return (false);
            }

            //Can do anything else to yourself
            if (currentUser.Id == appUser.Id) return (true);
            //Otherwise only these guys have permission to do anything else
            if (currentEmployee.isSuperUser || await IsInRoles(currentUser, "Office Administrator")) return (true);

            if (autoThrow) throw new InvalidOperationException("User " + currentUser.Email + " does not have permission to " + action.ToString() + " on employee " + appUser.Email.ToString());
            return (false);
        }

    }
}