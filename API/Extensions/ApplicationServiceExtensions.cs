using API.Services.ConcreteService;
using Infrastructure.Imts;
using Infrastructure.Imts.ConnectionService;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence;

namespace API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection ApplicationServices(this IServiceCollection services, IConfiguration config)
        {

            services.AddDbContext<ImtsContext>(opt =>
             {
                 opt.UseSqlServer(config.GetConnectionString("ImtsContext"));
             });
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
            services.AddScoped<IConnectionService, ConnectionService>();
            services.AddScoped<ConcreteService>();
            services.Configure<ImtsSettings>(config.GetSection("Imts"));
            

            services.AddMvc().AddNewtonsoftJson();
            return services;
        }
    }
}