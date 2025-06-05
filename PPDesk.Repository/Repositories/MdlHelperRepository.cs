﻿using Dapper;
using PPDesk.Abstraction.DTO.Repository;
using PPDesk.Abstraction.Helper;
using PPDesk.Repository.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z.Dapper.Plus;

namespace PPDesk.Repository.Repositories
{
    public interface IMdlHelperRepository : IForServiceCollectionExtension
    {
        Task CreateTableHelpersAsync();
        Task DeleteAllHelpersAsync();
        Task<MdlHelper> GetHelperByKeyAsync(string key);
        Task InsertHelperAsync(IEnumerable<MdlHelper> helpers);
        Task UpdateHelperAsync(IEnumerable<MdlHelper> helpers);
    }

    public class MdlHelperRepository : BaseRepository<MdlHelper>, IMdlHelperRepository
    {
        public MdlHelperRepository(IDatabaseConnectionFactory connectionFactory) : base(connectionFactory)
        {
        }

        public async Task CreateTableHelpersAsync()
        {
            var connection = await _connectionFactory.CreateConnectionAsync();
            await connection.QueryAsync($"CREATE TABLE HELPERS (" +
                $"Id INTEGER PRIMARY KEY AUTOINCREMENT," +
                $"Key VARCHAR(255) NOT NULL," +
                $"Json VARCHAR(255) NOT NULL)");
        }

        public async Task<MdlHelper> GetHelperByKeyAsync(string key)
        {
            string sql = "SELECT * FROM HELPERS WHERE Key = @key";

            var connection = await _connectionFactory.CreateConnectionAsync();
            return await connection.QueryFirstAsync<MdlHelper>(sql, new
            {
                key
            });
        }

        public async Task InsertHelperAsync(IEnumerable<MdlHelper> helpers)
        {
            var connection = await _connectionFactory.CreateConnectionAsync();
            await connection.BulkInsertAsync(helpers);
        }

        public async Task UpdateHelperAsync(IEnumerable<MdlHelper> helpers)
        {
            var connection = await _connectionFactory.CreateConnectionAsync();

            const string updateSql = @"
            UPDATE HELPERS 
            SET Json = @Json 
            WHERE Key = @Key";

            await connection.ExecuteAsync(updateSql, helpers);
        }

        public async Task DeleteAllHelpersAsync()
        {
            var sql = "DELETE FROM HELPERS";
            
            var connection = await _connectionFactory.CreateConnectionAsync();
            await connection.QueryAsync(sql);
        }
    }
}
