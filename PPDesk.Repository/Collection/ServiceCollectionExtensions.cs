using Microsoft.Extensions.DependencyInjection;
using PPDesk.Abstraction.DTO.Repository;
using PPDesk.Abstraction.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Z.Dapper.Plus;

namespace PPDesk.Repository.Collection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSharedLibraryRepositories(this IServiceCollection services)
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

    public static class RepositoryCollectionExtension
    {
        public static void ConfigurationDatabase()
        {
            ConfigureInserts();
        }

        private static void ConfigureInserts()
        {
            DapperPlusManager.Entity<MdlTable>()
            .Table("TABLES")
            .UseBulkOptions(options => {
                options.IgnoreOnInsertExpression = x => new { x.Id };
            });
            DapperPlusManager.Entity<MdlTableUser>()
            .Table("TABLEUSERS")
            .UseBulkOptions(options => {
                options.IgnoreOnInsertExpression = x => new { x.Id };
            });
            DapperPlusManager.Entity<MdlUser>()
            .Table("USERS")
            .UseBulkOptions(options => {
                options.IgnoreOnInsertExpression = x => new { x.Id };
            });
            DapperPlusManager.Entity<MdlEvent>()
            .Table("EVENTS")
            .UseBulkOptions(options => {
                options.IgnoreOnInsertExpression = x => new { x.Id };
            });
        }
    }
}
