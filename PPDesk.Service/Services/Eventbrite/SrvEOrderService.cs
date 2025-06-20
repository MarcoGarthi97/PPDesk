﻿using AutoMapper;
using PPDesk.Abstraction.DTO.Response.Eventbride.Order;
using PPDesk.Abstraction.DTO.Service.Eventbrite;
using PPDesk.Abstraction.DTO.Service.Eventbrite.Order;
using PPDesk.Abstraction.Helper;
using PPDesk.Service.Storages.Eventbride;
using PPDesk.Service.Storages.PP;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PPDesk.Service.Services.Eventbrite
{
    public interface ISrvEOrderService : IForServiceCollectionExtension
    {
        Task<IEnumerable<SrvEOrder>> GetListOrdersByEventIdAsync(long eventId);
        Task<IEnumerable<SrvEOrder>> GetListOrdersByOrganizationIdAsync();
    }

    public class SrvEOrderService : ISrvEOrderService
    {
        private readonly IMapper _mapper;

        public SrvEOrderService(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<IEnumerable<SrvEOrder>> GetListOrdersByOrganizationIdAsync()
        {
            long organizationId = SrvAppConfigurationStorage.EOrganization.Id;
            return await GetListOrders($"organizations/{organizationId}");
        }

        public async Task<IEnumerable<SrvEOrder>> GetListOrdersByEventIdAsync(long eventId)
        {
            return await GetListOrders($"events/{eventId}");
        }

        private async Task<IEnumerable<SrvEOrder>> GetListOrders(string url)
        {
            using (var listener = new HttpListener())
            {
                string bearer = SrvETokenStorage.Bearer;
                int pageNumber = 1;
                bool hasMoreRecord = true;

                List<SrvEOrder> srvEEorders = new List<SrvEOrder>();

                var client = new RestClient("https://www.eventbriteapi.com/v3");

                while (hasMoreRecord)
                {
                    var request = new RestRequest($"{url}/orders/?page={pageNumber}&expand=attendees");
                    request.AddHeader("Authorization", $"Bearer {bearer}");

                    try
                    {
                        var response = await client.ExecuteGetAsync(request);
                        var orderResponse = JsonSerializer.Deserialize<EOrdersResponse>(response.Content);

                        srvEEorders.AddRange(_mapper.Map<IEnumerable<SrvEOrder>>(orderResponse.Orders));

                        pageNumber++;

                        if (!orderResponse.Pagination.HasMoreItems)
                        {
                            hasMoreRecord = false;
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }

                return srvEEorders;
            }
        }
    }
}
