using MvpVendingMachineApp.Api.Filters;
using MvpVendingMachineApp.Domain.IRepositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using MvpVendingMachineApp.Common.General;
using FluentValidation;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MvpVendingMachineApp.Common;
using MvpVendingMachineApp.Common.Utilities;
using FluentValidation.AspNetCore;
using MvpVendingMachineApp.Common.General.Constants;
using MvpVendingMachineApp.Api.Controllers.v1.Users.Validators;
using Microsoft.Extensions.Options;

namespace MvpVendingMachineApp.Api
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddWebApi(this IServiceCollection services, IConfiguration configuration, SiteSettings siteSettings)
        {
            services.Configure<SiteSettings>(configuration.GetSection(nameof(SiteSettings)));

            services.AddApiVersioning(o =>
            {
                o.ReportApiVersions = true;
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.DefaultApiVersion = new ApiVersion(1, 0);
            });

            services.AddSwaggerOptions();
            services.AddHttpContextAccessor();
            services.AddJwtAuthentication(siteSettings.JwtSettings);
            services.AddGlobalFilterControllers();
            services.AddAutoMapperConfiguration();
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            return services;
        }

        public static IApplicationBuilder UseWebApi(this IApplicationBuilder app, IConfiguration configuration)
        {
            app.UseCors(builder =>
            {
                builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
            });

            app.UseAppSwagger(configuration);
            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            return app;
        }

        #region Swagger
        public static IServiceCollection AddSwaggerOptions(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "MvpVendingMachineApp.WebUI",
                    Description = "Vending machine using Clean Architecture implementation with ASP.NET Core Web Api",
                    Contact = new OpenApiContact
                    {
                        Name = "kabiru Okeleye",
                        Email = "kabiruOkeleye@gmail.com",
                        Url = new Uri("https://github.com/okeleyekabiru"),
                    },
                });
                options.SwaggerDoc("v2", new OpenApiInfo
                {
                    Version = "v2",
                    Title = "MvpVendingMachineApp.WebUI",
                    Description = "Vending machine using Clean Architecture implementation with ASP.NET Core Web Api",
                    Contact = new OpenApiContact
                    {
                        Name = "kabiru Okeleye",
                        Email = "kabiruOkeleye@gmail.com",
                        Url = new Uri("https://github.com/okeleyekabiru"),
                    },
                });

                #region Filters



                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme."
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                 {
                     {
                           new OpenApiSecurityScheme
                             {
                                 Reference = new OpenApiReference
                                 {
                                     Type = ReferenceType.SecurityScheme,
                                     Id = "Bearer"
                                 }
                             },
                             new string[] {}
                     }
                 });

                #endregion
            });


            return services;
        }

        public static IApplicationBuilder UseAppSwagger(this IApplicationBuilder app, IConfiguration configuration)
        {

            app.UseSwagger();

            //Swagger middleware for generate UI from swagger.json
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "MvpVendingMachineApp.WebUI v1");
                options.SwaggerEndpoint("/swagger/v2/swagger.json", "MvpVendingMachineApp.WebUI v2");

            });

            return app;
        }
        #endregion

        public static void AddJwtAuthentication(this IServiceCollection services, JwtSettings jwtSettings)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                var secretKey = Encoding.UTF8.GetBytes(jwtSettings.SecretKey);
                var encryptionKey = Encoding.UTF8.GetBytes(jwtSettings.EncryptKey);

                var validationParameters = new TokenValidationParameters
                {
                    ClockSkew = TimeSpan.Zero, // default: 5 min
                    RequireSignedTokens = true,

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(secretKey),

                    RequireExpirationTime = true,
                    ValidateLifetime = true,

                    ValidateAudience = true, //default : false
                    ValidAudience = jwtSettings.Audience,

                    ValidateIssuer = true, //default : false
                    ValidIssuer = jwtSettings.Issuer,

                    TokenDecryptionKey = new SymmetricSecurityKey(encryptionKey)
                };

                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = validationParameters;

                options.Events = new JwtBearerEvents
                {

                    OnTokenValidated = async context =>
                    {
                        var userRepository = context.HttpContext.RequestServices.GetRequiredService<IUserRepository>();
                        var siteSettings = context.HttpContext.RequestServices.GetRequiredService<IOptionsSnapshot<SiteSettings>>().Value;
                        var claimsIdentity = context.Principal.Identity as ClaimsIdentity;
                        if (claimsIdentity.Claims?.Any() != true)
                            context.Fail("This token has no claims.");

                        //Find user and token from database and perform your custom validation
                        var userId = claimsIdentity.GetUserId<int>();

                        var user = await userRepository.GetByIdAsync(context.HttpContext.RequestAborted, userId);
                        //if ((user.LastLoginAttempt.Date - DateTime.Now.AddMinutes(-siteSettings.JwtSettings.ExpirationMinutes)).TotalMinutes <= 0)
                        //{
                        //    context.Response.StatusCode = 400;
                        //    context.
                        //}

                        await userRepository.UpdateLastLoginDateAsync(user, context.HttpContext.RequestAborted);
                    }
                };
            });
            services.AddAuthorization(options =>
            {
                options.AddPolicy(Policy.RequireSeller,
                     policy => policy.RequireRole(Role.Seller));

                options.AddPolicy(Policy.RequireBuyer,
                    policy => policy.RequireRole(Role.Buyer));
            });

        }

        public static void AddGlobalFilterControllers(this IServiceCollection services)
        {
            services.AddControllers(options =>
            {
                options.Filters.Add(typeof(ValidateModelStateAttribute));
                options.Filters.Add(new ApiExceptionFilter());
            })
            .AddFluentValidation(options =>
            {
                options.RegisterValidatorsFromAssemblyContaining<Startup>();
            });

            services.AddCors();
        }

        public static void AddAutoMapperConfiguration(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(Startup));
        }
    }
}

