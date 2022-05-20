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
        IQueryable<AppUserOfficeRole> getAppUsersOfficeRoleByOffice(int officeId);
        IQueryable<AppUserOfficeRole> getAppUsersOfficeRoleByUserId(string appUserId);
        Task<AppUserOfficeRole> getAppUsersOfficeRoleByUserIdAndOfficeId(string appUserId, int officeId);
        Task addRoleToUser(string appUserId, int officeId, string roleName);
        Task<bool> addRoleToUser(AppUser appUser, int officeId, string roleName);
        Task removeRoleFromUser(string appUserId, int officeId, string roleName);
        Task<OfficeRole> getOfficeRole(string roleName);
        DbSet<AppUser> AppUsers { get; }
        Task<List<IDValuePair>> getImtsAllOffices();


    }

    public interface IUnitOfWork
    {
        IUserRepository Users { get; }
        IRepository<OfficeRole> OfficeRoles { get; }
        

        Task Commit();
        Task<bool> TryCommit();

    }
}