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
        Task addRoleToUser(string appUserId, int officeId, string roleName);
        IQueryable<AppUserOfficeRole> getAppUserOfficeRoleByOffice(int officeId);
        Task<List<AppUserOfficeRole>> getAppUserOfficeRoleByUser(string userId);
        Task<AppUserOfficeRole> getAppUserOfficeRoleByUserAndOffice(string appUserId, int officeId);
        Task<OfficeRole> getOfficeRoleByRoleName(string roleName);
        Task removeAppUserOfficeRole(string appUserId, int officeId, string roleName);
        Task removeAppUserOfficeRole(string appUserId, int officeId);
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