using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MvpVendingMachineApp.Domain.Entities.Users;
using MvpVendingMachineApp.Persistance.Db;
using Respawn;
using Respawn.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MvpVendingMachineApp.Api.Test
{
    [SetUpFixture]
    public class Testing
    {
        public static IConfiguration _configuration;
        public static IServiceCollection _services;
        public static IServiceScopeFactory _scopeFactory;
        private static Checkpoint _checkpoint;
        public static TingtelUser _currentUser;

        [OneTimeSetUp]
        public async Task RunBeforeAnyTests()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile("appsettings.Development.json", true, true)
                .AddEnvironmentVariables();

            _configuration = builder.Build();

            _services = new ServiceCollection();
            var startup = new Startup(_configuration);
            _services.AddSingleton(s => new Mock<IHostEnvironment>().Object);
            _services.AddSingleton(Mock.Of<IWebHostEnvironment>(w =>
                w.ApplicationName == "Tingtel.Services.Client"
                && w.EnvironmentName == "Development"));
            _services.AddScoped(_ => _configuration);
            _services.AddSingleton<PartnerManager>();
            _services.AddLogging();

            startup.ConfigureServices(_services);

            _scopeFactory = _services.BuildServiceProvider().GetService<IServiceScopeFactory>();

            _checkpoint = new Checkpoint()


        }

        public static async Task ResetDbState()
        {
            await _checkpoint.Reset(_configuration.GetConnectionString("AppOptions:WriteDatabaseConnectionString"));
        }





        public static async Task AddAsync<TEntity>(TEntity entity)
            where TEntity : class
        {
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetService<VendorWriteDbContext>() ?? throw new InvalidOperationException("Cannot use null context");

            await context.AddAsync(entity);
            await context.SaveChangesAsync();
        }

        public async static Task Remove<TEntity>(TEntity entity)
            where TEntity : class
        {
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetService<VendorWriteDbContext>();
            context.Remove(entity);
            await context.SaveChangesAsync();
        }



        public static async Task AddRangeAsync<TEntity>(List<TEntity> entities)
            where TEntity : class
        {
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetService<VendorWriteDbContext>();

            await context.AddRangeAsync(entities);
            await context.SaveChangesAsync();
        }

        public static async Task UpdateAsync<TEntity>(TEntity entity)
            where TEntity : class
        {
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetService<VendorWriteDbContext>();

            context.Update(entity);
            await context.SaveChangesAsync();
        }



        public static async Task<TEntity> FindAsync<TEntity>(long Id)
            where TEntity : class
        {
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetService<VendorReadOnlyDbContext>();

            return await context.FindAsync<TEntity>(Id);
        }

        public static async Task<User> FindByUserNameAsync(string username)
        {
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetService<VendorReadOnlyDbContext>();

            return await context.Users.FirstOrDefaultAsync(r => r.UserName == username);
        }

        public static async Task<IEnumerable<TEntity>> GetAllAsync<TEntity>(Expression<Func<TEntity, bool>> predicate = null)
            where TEntity : class
        {
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetService<VendorReadOnlyDbContext>();

            IQueryable<TEntity> query = null;
            if (predicate == null) query = context.Set<TEntity>();
            else query = context.Set<TEntity>().Where(predicate);

            return await query.ToListAsync();
        }

        public static async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
        {
            using var scope = _scopeFactory.CreateScope();
            var mediator = scope.ServiceProvider.GetService<IMediator>();

            return await mediator.Send(request);
        }


    }
}
