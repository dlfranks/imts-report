using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Application.Interfaces;
using Persistence;
using Domain.imts;

namespace Application.Repository
{
    public class BaseRepository<T> : IRepository<T> where T : class, IEntity
    {
        protected Persistence.AppContext _context;
        internal DbSet<T> _set;
        public readonly ILogger _logger;

        public BaseRepository(
            Persistence.AppContext context,
            ILogger logger)
        {
            _context = context;
            _set = context.Set<T>();
            _logger = logger;
        }

        public virtual IQueryable<T> get()
        {
            return _set;
        }
        public virtual IQueryable<T> get(int id)
        {
            return _set.Where(q => q.Id == id);
        }

        public async Task add(T entity)
        {
            await _set.AddAsync(entity);

        }
        public async Task<T> update(T entity)
        {
            T original = await _set.FindAsync(entity.Id);
            _context.Entry(original).CurrentValues.SetValues(entity);

            try
            {
                _context.Entry(original).Property("timeStamp").OriginalValue
                = typeof(T).GetProperty("timeStamp").GetValue(entity, null);
            }
            catch (Exception e)
            {

            }
            return original;
        }
        public async Task remove(int id)
        {
            _set.Remove(await get(id).FirstOrDefaultAsync());
        }
        public void remove(T entity)
        {
            _set.Remove(entity);
        }

    }

    public class UserRepository : IUserRepository
    {
        protected readonly DbSet<OfficeRole> _OfficeRoles;
        protected readonly DbSet<AppUserOfficeRole> _AppUserOfficeRoles;
        public DbSet<AppUser> _AppUsers;
        private readonly Persistence.AppContext _context;
        private readonly ImtsContext _imtsContext;
        private readonly ILogger _logger;
        protected readonly UserManager<AppUser> _UserManager;
        public UserRepository(
            Persistence.AppContext context,
            ImtsContext imtsContext,
            ILogger logger,
            UserManager<AppUser> userManager)
        {
            _OfficeRoles = context.OfficeRoles;
            _AppUserOfficeRoles = context.AppUserOfficeRoles;
            _context = context;
            _imtsContext = imtsContext;
            _logger = logger;
            _UserManager = userManager;
            _AppUsers = context.Users;

        }

        public DbSet<AppUser> AppUsers
        {
            get
            {
                if (_AppUsers == null)
                    _AppUsers = _context.Users;
                return _AppUsers;
            }
        }

        public async Task addRoleToUser(string appUserId, int officeId, string roleName)
        {

            var _roleId = await _OfficeRoles.Where(q => q.RoleName == roleName).Select(q => q.Id).FirstAsync();
            _AppUserOfficeRoles.Add(new AppUserOfficeRole
            {
                AppUserId = appUserId,
                RoleId = _roleId,
                ImtsOfficeId = officeId
            });
        }

        public async Task<bool> addRoleToUser(AppUser appUser, int officeId, string roleName)
        {
            try
            {
                var _roleId = await _OfficeRoles.Where(q => q.RoleName == roleName).Select(q => q.Id).FirstAsync();
                var appUserOfficeRole = new AppUserOfficeRole
                {
                    AppUser = appUser,
                    RoleId = _roleId,
                    ImtsOfficeId = officeId
                };
                _AppUserOfficeRoles.Add(appUserOfficeRole);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} addRoleToUser function error", typeof(UserRepository));
                return false;
            }
        }

        public IQueryable<AppUserOfficeRole> getAppUsersOfficeRoleByOffice(int officeId)
        {
            return _AppUserOfficeRoles.Where(q => q.ImtsOfficeId == officeId).Include(q => q.Role);
        }
        public IQueryable<AppUserOfficeRole> getAppUsersOfficeRoleByUserId(string appUserId)
        {
            return _AppUserOfficeRoles.Where(q => q.AppUserId == appUserId).Include(q => q.Role);
        }
        public async Task<AppUserOfficeRole> getAppUsersOfficeRoleByUserIdAndOfficeId(string appUserId, int officeId)
        {
            var userOfficesRole = await getAppUsersOfficeRoleByUserId(appUserId).Where(q => q.ImtsOfficeId == officeId).FirstOrDefaultAsync();
            var o = await _imtsContext.Offices.Where(q => q.id == userOfficesRole.ImtsOfficeId).FirstAsync();
            if (o != null)
                userOfficesRole.ImtsOffice = o;

            return userOfficesRole;
        }

        public async Task<OfficeRole> getOfficeRole(string roleName)
        {
            return await _OfficeRoles.Where(q => q.RoleName == roleName).FirstAsync();
        }

        public async Task removeRoleFromUser(string appUserId, int officeId, string roleName)
        {
            var role = await getOfficeRole(roleName);
            var appUserOfficeRole = await _context.AppUserOfficeRoles.Where(q => q.AppUserId == appUserId && q.RoleId == role.Id).FirstOrDefaultAsync();
            _AppUserOfficeRoles.Remove(appUserOfficeRole);
        }

        public async Task<List<Office>> getImtsOfficeByUser(int imtsEmployeeId)
        {
            var offices = await _imtsContext.UsersInOfficeRoles.Where(q => q.employeeId == imtsEmployeeId)
                .GroupBy(q => q.officeId).Select(g => g.First().office).ToListAsync();
            return offices;
        }
        public async Task<List<IDValuePair>> getImtsAllOffices()
        {
            var offices = await _imtsContext.Offices.Select(q => new IDValuePair
            {
                id = q.id,
                name = q.name
            }).ToListAsync();
            return offices;
        }
    }
}