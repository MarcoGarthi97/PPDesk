using PPDesk.Abstraction.DTO.Repository;
using PPDesk.Abstraction.DTO.Repository.Table;
using PPDesk.Abstraction.DTO.Repository.User;
using Z.Dapper.Plus;

namespace PPDesk.Repository.Collection
{
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
                options.BatchSize = 2000;
            });
            DapperPlusManager.Entity<MdlOrder>()
            .Table("ORDERS")
            .UseBulkOptions(options => {
                options.IgnoreOnInsertExpression = x => new { x.Id };
                options.BatchSize = 2000;
            });
            DapperPlusManager.Entity<MdlUser>()
            .Table("USERS")
            .UseBulkOptions(options => {
                options.IgnoreOnInsertExpression = x => new { x.Id };
                options.BatchSize = 2000;
            });
            DapperPlusManager.Entity<MdlEvent>()
            .Table("EVENTS")
            .UseBulkOptions(options => {
                options.IgnoreOnInsertExpression = x => new { x.Id };
                options.BatchSize = 2000;
            });
        }
    }
}
