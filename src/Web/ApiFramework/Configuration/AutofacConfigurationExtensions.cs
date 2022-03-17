using Autofac;
using MvpVendingMachineApp.Common;
using MvpVendingMachineApp.Common.General;
using MvpVendingMachineApp.Domain.Entities;
using MvpVendingMachineApp.Domain.IRepositories;
using MvpVendingMachineApp.Persistance.Db;
using MvpVendingMachineApp.Persistance.Jwt;
using MvpVendingMachineApp.Persistance.Repositories;

namespace MvpVendingMachineApp.ApiFramework.Configuration
{
    public static class AutofacConfigurationExtensions
    {
        public static void RegisterServices(this ContainerBuilder containerBuilder)
        {
            //RegisterType > As > Liftetime
            containerBuilder.RegisterGeneric(typeof(Repository<>)).As(typeof(IRepository<>)).InstancePerLifetimeScope();

            containerBuilder.RegisterGeneric(typeof(EfReadOnlyRepository<>)).As(typeof(IReadOnlyRepository<>)).InstancePerLifetimeScope();

            var commonAssembly = typeof(SiteSettings).Assembly;
            var entitiesAssembly = typeof(IEntity).Assembly;
            var dataAssembly = typeof(AppDbContext).Assembly;
            var servicesAssembly = typeof(JwtService).Assembly;

            containerBuilder.RegisterAssemblyTypes(commonAssembly, entitiesAssembly, dataAssembly, servicesAssembly)
                .AssignableTo<IScopedDependency>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            containerBuilder.RegisterAssemblyTypes(commonAssembly, entitiesAssembly, dataAssembly, servicesAssembly)
                .AssignableTo<ITransientDependency>()
                .AsImplementedInterfaces()
                .InstancePerDependency();

            containerBuilder.RegisterAssemblyTypes(commonAssembly, entitiesAssembly, dataAssembly, servicesAssembly)
                .AssignableTo<ISingletonDependency>()
                .AsImplementedInterfaces()
                .SingleInstance();
        }
    }
}
