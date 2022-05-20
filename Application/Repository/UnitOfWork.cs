using System;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Persistence;
using Application.Interfaces;

namespace Application.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly Persistence.AppContext _context;
        private readonly ILogger _logger;
        private readonly ImtsContext _imtsContext;
        public UnitOfWork(Persistence.AppContext context, ImtsContext imtsContext, ILoggerFactory loggerFactory, UserManager<AppUser> userManager)
        {
            _imtsContext = imtsContext;
            _context = context;
            _logger = loggerFactory.CreateLogger("logs");
        }

        public IUserRepository _Users = null;
        public IRepository<OfficeRole> _OfficeRoles = null;

        public IUserRepository AppUsers
        {
            get
            {
                if (_Users == null)
                    _Users = new UserRepository(_context, _imtsContext, _logger); ;
                return _Users;
            }
        }
        public IRepository<OfficeRole> OfficeRoles
        {
            get
            {
                if (_OfficeRoles == null)
                    _OfficeRoles = new BaseRepository<OfficeRole>(_context, _logger); ;
                return _OfficeRoles;
            }
        }

        public async Task Commit()
        {
            await _context.SaveChangesAsync();
        }
        public async Task<bool> TryCommit()
        {
            try
            {
                await Commit();
            }
            catch (Exception ex)
            {
                //Elmah
                return false;
            }
            return true;
        }
        public void Dispose()
        {
            _context.Dispose();
            _imtsContext.Dispose();
        }

    }
}