using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using PPDesk.Abstraction.DTO.Service.PP.Order;
using PPDesk.Abstraction.DTO.Service.PP.Table;
using PPDesk.Abstraction.Helper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PPDesk.Service.Services.PP
{
    public interface ISrvLiveCacheService : IForServiceCollectionExtension
    {
        IEnumerable<SrvInformationOrder> GetLiveOrders();
        void SetLiveOrders(IEnumerable<SrvInformationOrder> orders);
        void UpdateLiveOrder(SrvInformationOrder order);
        void ClearLiveOrders();

        IEnumerable<SrvInformationTable> GetLiveTables();
        void SetLiveTables(IEnumerable<SrvInformationTable> tables);
        void UpdateLiveTable(SrvInformationTable table);
        void ClearLiveTables();

        void ClearAll();
    }

    public class SrvLiveCacheService : ISrvLiveCacheService
    {
        private readonly IMemoryCache _cache;
        private readonly ILogger<SrvLiveCacheService> _logger;
        private const string ORDERS_CACHE_KEY = "LiveOrders";
        private const string TABLES_CACHE_KEY = "LiveTables";
        private static readonly TimeSpan CacheExpiration = TimeSpan.FromMinutes(15);

        public SrvLiveCacheService(IMemoryCache cache, ILogger<SrvLiveCacheService> logger)
        {
            _cache = cache;
            _logger = logger;
        }

        public IEnumerable<SrvInformationOrder> GetLiveOrders()
        {
            if (_cache.TryGetValue(ORDERS_CACHE_KEY, out List<SrvInformationOrder> orders))
            {
                _logger.LogInformation($"Retrieved {orders?.Count ?? 0} live orders from cache");
                return orders ?? Enumerable.Empty<SrvInformationOrder>();
            }

            _logger.LogInformation("No live orders found in cache");
            return null;
        }

        public void SetLiveOrders(IEnumerable<SrvInformationOrder> orders)
        {
            if (orders == null)
            {
                _logger.LogWarning("Attempted to cache null orders");
                return;
            }

            var ordersList = orders.ToList();
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(CacheExpiration)
                .SetPriority(CacheItemPriority.High);

            _cache.Set(ORDERS_CACHE_KEY, ordersList, cacheEntryOptions);
            _logger.LogInformation($"Cached {ordersList.Count} live orders");
        }

        public void UpdateLiveOrder(SrvInformationOrder order)
        {
            if (order == null)
            {
                _logger.LogWarning("Attempted to update null order in cache");
                return;
            }

            if (_cache.TryGetValue(ORDERS_CACHE_KEY, out List<SrvInformationOrder> orders))
            {
                var existingOrder = orders.FirstOrDefault(o => o.Id == order.Id);
                if (existingOrder != null)
                {
                    var index = orders.IndexOf(existingOrder);
                    orders[index] = order;
                    SetLiveOrders(orders);
                    _logger.LogInformation($"Updated order {order.Id} in cache");
                }
                else
                {
                    _logger.LogWarning($"Order {order.Id} not found in cache for update");
                }
            }
            else
            {
                _logger.LogWarning("No orders cache found for update");
            }
        }

        public void ClearLiveOrders()
        {
            _cache.Remove(ORDERS_CACHE_KEY);
            _logger.LogInformation("Cleared live orders cache");
        }

        public IEnumerable<SrvInformationTable> GetLiveTables()
        {
            if (_cache.TryGetValue(TABLES_CACHE_KEY, out List<SrvInformationTable> tables))
            {
                _logger.LogInformation($"Retrieved {tables?.Count ?? 0} live tables from cache");
                return tables ?? Enumerable.Empty<SrvInformationTable>();
            }

            _logger.LogInformation("No live tables found in cache");
            return null;
        }

        public void SetLiveTables(IEnumerable<SrvInformationTable> tables)
        {
            if (tables == null)
            {
                _logger.LogWarning("Attempted to cache null tables");
                return;
            }

            var tablesList = tables.ToList();
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(CacheExpiration)
                .SetPriority(CacheItemPriority.High);

            _cache.Set(TABLES_CACHE_KEY, tablesList, cacheEntryOptions);
            _logger.LogInformation($"Cached {tablesList.Count} live tables");
        }

        public void UpdateLiveTable(SrvInformationTable table)
        {
            if (table == null)
            {
                _logger.LogWarning("Attempted to update null table in cache");
                return;
            }

            if (_cache.TryGetValue(TABLES_CACHE_KEY, out List<SrvInformationTable> tables))
            {
                var existingTable = tables.FirstOrDefault(t => t.Id == table.Id);
                if (existingTable != null)
                {
                    var index = tables.IndexOf(existingTable);
                    tables[index] = table;
                    SetLiveTables(tables);
                    _logger.LogInformation($"Updated table {table.Id} in cache");
                }
                else
                {
                    _logger.LogWarning($"Table {table.Id} not found in cache for update");
                }
            }
            else
            {
                _logger.LogWarning("No tables cache found for update");
            }
        }

        public void ClearLiveTables()
        {
            _cache.Remove(TABLES_CACHE_KEY);
            _logger.LogInformation("Cleared live tables cache");
        }

        public void ClearAll()
        {
            ClearLiveOrders();
            ClearLiveTables();
            _logger.LogInformation("Cleared all live caches");
        }
    }
}
