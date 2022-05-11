using System.Text;
using API.Services;
using Domain;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Persistence;

namespace API.Extensions
{
    public static class IdentityServiceExtensions
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddIdentityCore<AppUser>(opts =>
            {
                opts.Password.RequiredLength = 8;
                //opts.Password.RequireNonAlphanumeric = true;
                //opts.Password.RequireLowercase = false;
                //opts.Password.RequireUppercase = true;
                //opts.Password.RequireDigit = true;

            })
            .AddEntityFrameworkStores<AppContext>()
            .AddSignInManager<SignInManager<AppUser>>();



            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(opt =>
            {
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
                
            });
            
            services.AddScoped<TokenService>();
            services.AddScoped<UserService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }

    }
}