using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Domain;

namespace Persistence
{
    public interface IRepository<T> where T : class, IEntity
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
        IQueryable<AppUserOfficeRole> getUsersRoles(string appUserId);
        Task addRoleToUser(string appUserId, int officeId, string roleName);
        Task<bool> addRoleToUser(AppUser appUser, int officeId, string roleName);
        Task removeRoleFromUser(string appUserId, int officeId, string roleName);
        Task<OfficeRole> getOfficeRole(string roleName);
    }

    public interface IUnitOfWork
    {
        IUserRepository Users { get; }
        IRepository<OfficeRole> OfficeRoles { get; }
        

        Task Commit();
        Task<bool> TryCommit();

    }
}