using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Persistence;
using Domain.imts;
using Application.Interfaces;

namespace Application.Repository
{
    public class BaseRepository<T> : IRepository<T> where T : class, IEntity
    {
        protected Persistence.AppContext _context;
        internal DbSet<T> _set;
        

        public BaseRepository(
            Persistence.AppContext context
            )
        {
            _context = context;
            _set = context.Set<T>();
            
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
                //Elmah
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
        private readonly Persistence.AppContext _context;
        private readonly ImtsContext _imtsContext;
        private readonly ILogger _logger;
        public UserRepository(
            Persistence.AppContext context,
            ImtsContext imtsContext,
            ILogger logger)
        {

            _context = context;
            _imtsContext = imtsContext;
            _logger = logger;


        }



        public async Task addRoleToUser(string appUserId, int officeId, string roleName)
        {
            var _roleId = await _context.OfficeRoles.Where(q => q.RoleName == roleName).Select(q => q.Id).FirstAsync();
            _context.AppUserOfficeRoles.Add(new AppUserOfficeRole
            {
                AppUserId = appUserId,
                RoleId = _roleId,
                ImtsOfficeId = officeId
            });
        }

        public IQueryable<AppUserOfficeRole> getAppUserOfficeRoleByOffice(int officeId)
        {
            return _context.AppUserOfficeRoles.Where(q => q.ImtsOfficeId == officeId).Include(q => q.AppUser).Include(q => q.Role);
        }
        public async Task<List<AppUserOfficeRole>> getAppUserOfficeRoleByUser(string userId)
        {
            var userOfficeRoles = await _context.AppUserOfficeRoles.Where(q => q.AppUserId == userId).Include(q => q.Role).ToListAsync();
            if (userOfficeRoles == null && userOfficeRoles.Count == 0) return null;
            var imtsOffices = await _imtsContext.Offices.ToListAsync();
            foreach (var u in userOfficeRoles)
            {
                u.ImtsOffice = imtsOffices.Where(q => q.id == u.ImtsOfficeId).FirstOrDefault();
            }
            return userOfficeRoles;
        }

        public async Task<AppUserOfficeRole> getAppUserOfficeRoleByUserAndOffice(string appUserId, int officeId)
        {
            var userOfficesRole = await getAppUserOfficeRoleByOffice(officeId).Where(q => q.AppUserId == appUserId).FirstOrDefaultAsync();
            if (userOfficesRole == null) return null;
            var o = await _imtsContext.Offices.Where(q => q.id == userOfficesRole.ImtsOfficeId).FirstAsync();
            if (o != null)
                userOfficesRole.ImtsOffice = o;

            return userOfficesRole;
        }
        public async Task<OfficeRole> getOfficeRoleByRoleName(string roleName)
        {
            return await _context.OfficeRoles.Where(q => q.RoleName == roleName).FirstAsync();
        }
        
        public async Task removeAppUserOfficeRole(string appUserId, int officeId)
        {

            var appUserOfficeRole = await getAppUserOfficeRoleByUserAndOffice(appUserId, officeId);
            await removeAppUserOfficeRole(appUserOfficeRole.AppUserId, appUserOfficeRole.ImtsOfficeId, appUserOfficeRole.Role.RoleName);
        }
        public async Task removeAppUserOfficeRole(string appUserId, int officeId, string roleName)
        {
            var role = await getOfficeRoleByRoleName(roleName);
            var appUserOfficeRole = await _context.AppUserOfficeRoles.Where(q => q.AppUserId == appUserId && q.RoleId == role.Id).FirstOrDefaultAsync();
            _context.AppUserOfficeRoles.Remove(appUserOfficeRole);
        }

        public async Task<List<Office>> getImtsOfficesByUser(int imtsEmployeeId)
        {
            return await _imtsContext.UsersInOfficeRoles.Where(q => q.employeeId == imtsEmployeeId)
                .GroupBy(q => q.officeId).Select(g => g.First().office).ToListAsync();

        }
        public async Task<List<IDValuePair>> getImtsAllOffices()
        {
            return await _imtsContext.Offices.Select(q => new IDValuePair
            {
                id = q.id,
                name = q.name
            }).ToListAsync();

        }
    }
}