using Microsoft.Extensions.DependencyInjection;
using PPDesk.Abstraction.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PPDesk.Helper.Collection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSharedLibrary(this IServiceCollection services)
        {
            services.Scan(scan => scan
            .FromAssemblies(Assembly.GetExecutingAssembly())
            .AddClasses(classes => classes.AssignableTo<IForServiceCollectionExtension>())
            .AsSelf()
            .AsImplementedInterfaces()
            .WithTransientLifetime());

            return services;
        }
    }
}
