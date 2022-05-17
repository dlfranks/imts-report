using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Persistence
{
    public class BaseRepository<T> : IRepository<T> where T : class, IEntity
    {
        protected AppContext _context;
        internal DbSet<T> _set;
        public readonly ILogger _logger;

        public BaseRepository(
            AppContext context,
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
        protected readonly DbSet<OfficeRole> _OfficeRole;
        protected readonly DbSet<AppUserOfficeRole> _AppUserOfficeRole;
        private readonly AppContext _context;
        private readonly ImtsContext _imtsContext;
        private readonly ILogger _logger;
        protected readonly UserManager<AppUser> _UserManager;
        public UserRepository(
            AppContext context,
            ImtsContext imtsContext,
            ILogger logger,
            UserManager<AppUser> userManager)
        {
            _OfficeRole = context.OfficeRoles;
            _AppUserOfficeRole = context.AppUserOfficeRoles;
            _context = context;
            _imtsContext = imtsContext;
            _logger = logger;
            _UserManager = userManager;
        }

        public async Task addRoleToUser(string appUserId, int officeId, string roleName)
        {

            var _roleId = await _OfficeRole.Where(q => q.RoleName == roleName).Select(q => q.Id).FirstAsync();
            _AppUserOfficeRole.Add(new AppUserOfficeRole
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
                var _roleId = await _OfficeRole.Where(q => q.RoleName == roleName).Select(q => q.Id).FirstAsync();
                var appUserOfficeRole = new AppUserOfficeRole
                {
                    AppUser = appUser,
                    RoleId = _roleId,
                    ImtsOfficeId = officeId
                };
                _AppUserOfficeRole.Add(appUserOfficeRole);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} addRoleToUser function error", typeof(UserRepository));
                return false;
            }
        }
        public IQueryable<AppUserOfficeRole> getUsersRoles(string appUserId)
        {
            return _AppUserOfficeRole.Where(q => q.AppUserId == appUserId).Include(q => q.Role);
        }

        public async Task<OfficeRole> getOfficeRole(string roleName)
        {
            return await _OfficeRole.Where(q => q.RoleName == roleName).FirstAsync();
        }

        public async Task removeRoleFromUser(string appUserId, int officeId, string roleName)
        {
            var role = await getOfficeRole(roleName);
            var appUserOfficeRole = await _context.AppUserOfficeRoles.Where(q => q.AppUserId == appUserId && q.RoleId == role.Id).FirstOrDefaultAsync();
            _AppUserOfficeRole.Remove(appUserOfficeRole);
        }


    }
}