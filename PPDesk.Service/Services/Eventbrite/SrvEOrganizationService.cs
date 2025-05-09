using AutoMapper;
using PPDesk.Abstraction.DTO.Response.Eventbride;
using PPDesk.Abstraction.DTO.Service.Eventbrite;
using PPDesk.Abstraction.Helper;
using PPDesk.Service.Storages.Eventbride;
using RestSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace PPDesk.Service.Services.Eventbrite
{
    public interface ISrvEOrganizationService : IForServiceCollectionExtension
    {
        Task<IEnumerable<SrvEOrganization>> GetOrganizationsAsync();
        Task LoadOrganizationsAsync();
    }

    public class SrvEOrganizationService : ISrvEOrganizationService
    {
        private readonly IMapper _mapper;
        public SrvEOrganizationService(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task LoadOrganizationsAsync()
        {
            var organizations = await GetOrganizationsAsync();

            SrvEOrganizationStorage.SetOrganizations(organizations);
        }

        public async Task<IEnumerable<SrvEOrganization>> GetOrganizationsAsync()
        {
            var tcs = new TaskCompletionSource<HttpListenerContext>();

            IEnumerable<SrvEOrganization> srvEOrganizations = new List<SrvEOrganization>();

            await Task.Run(async () =>
            {
                using (var listener = new HttpListener())
                {
                    var bearer = SrvETokenStorage.Bearer;

                    var client = new RestClient("https://www.eventbriteapi.com/v3");
                    var request = new RestRequest($"users/me/organizations/");
                    request.AddHeader("Authorization", $"Bearer {bearer}");

                    try
                    {
                        //TODO: Manca di fare l'autenticazione prima e salvare i dati della key del json
                        var response = await client.ExecuteGetAsync(request);
                        var eOrganization = JsonSerializer.Deserialize<EOrganizationsResponse>(response.Content);

                        srvEOrganizations = _mapper.Map<IEnumerable<SrvEOrganization>>(eOrganization);
                    }
                    catch (Exception ex)
                    {

                    }
                }
            });


            return srvEOrganizations;
        }  
    }
}
