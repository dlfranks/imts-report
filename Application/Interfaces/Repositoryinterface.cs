using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Domain;
using Domain.imts;
using Microsoft.EntityFrameworkCore;

namespace Application.Interfaces
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> get();
        IQueryable<T> get(int id);
        Task add(T newEntity);
        Task<T> update (T entity);
        Task remove(int id);
        void remove(T entity);
        
        
    }

    public interface IUserRepository
    {
        DbSet<AppUser> AppUsers { get; }
        void updateAppUserOfficeRole(AppUserOfficeRole appUserOfficeRole);
        Task addRoleToUser(string appUserId, int officeId, string roleName);
        Task<bool> addRoleToUser(AppUser appUser, int officeId, string roleName);
        IQueryable<AppUserOfficeRole> getAppUsersOfficeRolesByOffice(int officeId);
        Task<List<AppUserOfficeRole>> getAppUsersOfficeRolesByUser(string userId);
        Task<AppUserOfficeRole> getAppUsersOfficeRolesByUserAndOffice(string appUserId, int officeId);
        Task<OfficeRole> getOfficeRoleByUserAndOffice(string userId, int officeId);
        Task<OfficeRole> getOfficeRoleByRoleName(string roleName);
        Task removeRoleFromUser(string appUserId, int officeId, string roleName);
        Task<List<Office>> getImtsOfficesByUser(int imtsEmployeeId);
        Task<List<IDValuePair>> getImtsAllOffices();

    }

    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        IRepository<OfficeRole> OfficeRoles { get; }
        

        Task Commit();
        Task<bool> TryCommit();

    }
}