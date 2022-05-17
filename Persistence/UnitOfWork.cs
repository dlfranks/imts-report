using System;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppContext _context;
        private readonly ILogger _logger;
        private readonly UserManager<AppUser> _userManager;
        private readonly ImtsContext _imtsContext;
        public UnitOfWork(AppContext context, ImtsContext imtsContext, ILoggerFactory loggerFactory, UserManager<AppUser> userManager)
        {
            _imtsContext = imtsContext;
            _context = context;
            _logger = loggerFactory.CreateLogger("logs");
            _userManager = userManager;
        }

        public IUserRepository _Users = null;
        public IRepository<OfficeRole> _OfficeRoles = null;

        public IUserRepository Users
        {
            get
            {
                if (_Users == null)
                    _Users = new UserRepository(_context, _imtsContext, _logger, _userManager); ;
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


    }
}