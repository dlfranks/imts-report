using System.Collections.Generic;
using API.Models.FieldConcreteTest;
using API.Services;
using API.Services.ConcreteService;

using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Persistence;

namespace API.Extentions
{
    public static class ApplicationServiceExtentions
    {
        public static IServiceCollection ApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            
        //    services.AddDbContext<ImtsContext>(opt =>
        //     {
        //         opt.UseSqlServer(config.GetConnectionString("ImtsContext"));
        //     });
            services.AddDbContext<AppContext>(opt =>
           {
               opt.UseSqlite(config.GetConnectionString("DefaultConnection"));
           });
            
            services.AddCors(opt =>
            {
                opt.AddPolicy("CorsPolicy", policy =>
                {
                    policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:3000");
                });
            });

            //  services.AddSwaggerGen(c =>
            // {
            //     c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
            // });

            //services.AddMediatR(typeof(ProjectList.Handler).Assembly);
            //services.AddAutoMapper(typeof(MappingProfiles).Assembly);
            services.AddHttpClient();
            services.AddScoped<IConcreteService, ConcreteService>();
            services.AddMvc().AddNewtonsoftJson();
            return services;
        }
    }
}