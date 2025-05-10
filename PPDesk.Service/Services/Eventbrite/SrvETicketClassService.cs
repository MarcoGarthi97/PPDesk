using AutoMapper;
using PPDesk.Abstraction.DTO.Response.Eventbride.TicketClass;
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
    public interface ISrvETicketClassService : IForServiceCollectionExtension
    {
        Task<IEnumerable<SrvETicketClass>> GetListTicketClassesByEventIdsAsync(IEnumerable<long> eventIds);
    }

    public class SrvETicketClassService : ISrvETicketClassService
    {
        private readonly IMapper _mapper;

        public SrvETicketClassService(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<IEnumerable<SrvETicketClass>> GetListTicketClassesByEventIdsAsync(IEnumerable<long> eventIds)
        {
            using (var listener = new HttpListener())
            {
                string bearer = SrvETokenStorage.Bearer;
                long organizationId = SrvAppConfigurationStorage.EOrganization.Id;
                int pageNumber = 1;
                bool hasMoreRecord = true;

                List<SrvETicketClass> srvETicketClasses = new List<SrvETicketClass>();

                var client = new RestClient("https://www.eventbriteapi.com/v3");

                foreach(long idEvent in eventIds)
                {
                    while (hasMoreRecord)
                    {
                        var request = new RestRequest($"events/{idEvent}/ticket_classes/?page={pageNumber}");
                        request.AddHeader("Authorization", $"Bearer {bearer}");

                        try
                        {
                            var response = await client.ExecuteGetAsync(request);
                            var eventResponse = JsonSerializer.Deserialize<ETicketClassesResponse>(response.Content);

                            srvETicketClasses.AddRange(_mapper.Map<IEnumerable<SrvETicketClass>>(eventResponse.TicketClasses));

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

                    hasMoreRecord = true;
                    pageNumber = 1;
                }


                return srvETicketClasses;
            }
        }
    }
}
