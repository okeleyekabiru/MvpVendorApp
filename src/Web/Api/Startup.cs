using MvpVendingMachineApp.Application;
using MvpVendingMachineApp.Common;
using MvpVendingMachineApp.Common.General;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MvpVendingMachineApp.Persistance;
using MvpVendingMachineApp.Domain.IRepositories;
using MvpVendingMachineApp.Persistance.Repositories;
using MvpVendingMachineApp.Persistance.Jwt;

namespace MvpVendingMachineApp.Api
{
    public class Startup
    {
        private readonly SiteSettings siteSetting;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            siteSetting = configuration.GetSection(nameof(SiteSettings)).Get<SiteSettings>();
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddWebApi(Configuration, siteSetting);
            services.AddPersistance(Configuration);
            services.AddCommon(Configuration);
            services.AddApplication(Configuration);
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IJwtService, JwtService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment() || env.IsStaging())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseWebApi(Configuration);
        }
    }
}