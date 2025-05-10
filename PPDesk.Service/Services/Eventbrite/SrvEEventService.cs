using AutoMapper;
using PPDesk.Abstraction.DTO.Response.Eventbride.Event;
using PPDesk.Abstraction.DTO.Service.Eventbrite;
using PPDesk.Abstraction.Helper;
using PPDesk.Service.Storages.Eventbride;
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
    public interface ISrvEEventService : IForServiceCollectionExtension
    {
        Task<IEnumerable<SrvEEvent>> GetListEventsByOrganizationIdAsync();
    }

    public class SrvEEventService : ISrvEEventService
    {
        private readonly IMapper _mapper;

        public SrvEEventService(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<IEnumerable<SrvEEvent>> GetListEventsByOrganizationIdAsync()
        {
            using (var listener = new HttpListener())
            {
                string bearer = SrvETokenStorage.Bearer;
                long organizationId = SrvAppConfigurationStorage.EOrganization.Id;
                int pageNumber = 1;
                bool hasMoreRecord = true;

                List<SrvEEvent> srvEEvents = new List<SrvEEvent>();

                var client = new RestClient("https://www.eventbriteapi.com/v3");

                while (hasMoreRecord)
                {
                    var request = new RestRequest($"organizations/{organizationId}/events/?page={pageNumber}&page_size=200");
                    request.AddHeader("Authorization", $"Bearer {bearer}");

                    try
                    {
                        var response = await client.ExecuteGetAsync(request);
                        var eventResponse = JsonSerializer.Deserialize<EEventsResponse>(response.Content);

                        srvEEvents.AddRange(_mapper.Map<IEnumerable<SrvEEvent>>(eventResponse.Events));

                        pageNumber++;

                        if (!eventResponse.Pagination.HasMoreItems)
                        {
                            hasMoreRecord = false;
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }


                return srvEEvents;
            }
        }
    }
}
