using System;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Persistence;

namespace API.Extensions
{
    public class AuthSignInManager<TUser> : SignInManager<AppUser> where TUser : class
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly Persistence.AppContext _db;
        private readonly IHttpContextAccessor _contextAccessor;
        public AuthSignInManager(
            UserManager<AppUser> userManager, 
            IHttpContextAccessor contextAccessor, 
            IUserClaimsPrincipalFactory<AppUser> claimsFactory, 
            IOptions<IdentityOptions> optionsAccessor, 
            ILogger<SignInManager<AppUser>> logger, 
            Persistence.AppContext dbContext,
            IAuthenticationSchemeProvider schemeProvider,
            IUserConfirmation<AppUser> userConfirmation
        ) : base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemeProvider, userConfirmation)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _contextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(contextAccessor));
            _db = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public override async Task<SignInResult> PasswordSignInAsync(string userName, string password, bool rememberMe, bool shouldLockout)
        {
            var user = UserManager.FindByEmailAsync(userName).Result;

            var result = await base.PasswordSignInAsync(userName, password, rememberMe, shouldLockout);
            if(result.Succeeded)
            {
                
            }

            return 
        }
    }
}