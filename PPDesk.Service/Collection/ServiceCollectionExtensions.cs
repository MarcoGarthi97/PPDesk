using Microsoft.Extensions.DependencyInjection;
using PPDesk.Abstraction.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PPDesk.Service.Collection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSharedLibrarySrvServices(this IServiceCollection services)
        {
            services.Scan(scan => scan
            .FromAssemblies(Assembly.GetExecutingAssembly())
            .AddClasses()
            .AsImplementedInterfaces(typeInfo =>
                typeInfo.GetInterfaces().Any(i =>
                    i == typeof(IForServiceCollectionExtension) ||
                    typeInfo == typeof(IForServiceCollectionExtension))
            )
            .WithScopedLifetime()
            );

            return services;
        }
    }
}
