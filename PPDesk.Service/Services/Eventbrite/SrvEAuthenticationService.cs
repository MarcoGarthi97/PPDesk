using PPDesk.Abstraction.Helper;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using PPDesk.Service.Storages.Eventbride;
using System.Text.Json;
using PPDesk.Abstraction.DTO.Response.Eventbride;

namespace PPDesk.Service.Services.Eventbrite
{
    public interface ISrvEAuthenticationService : IForServiceCollectionExtension
    {
        Task GetAuthenticationAsync();
    }

    public class SrvEAuthenticationService : ISrvEAuthenticationService
    {
        public SrvEAuthenticationService() { }

        public async Task GetAuthenticationAsync()
        {
            try
            {
                var tcs = new TaskCompletionSource<HttpListenerContext>();
                string code = string.Empty;

                await Task.Run(async () => {
                    using (var listener = new HttpListener())
                    {
                        listener.Prefixes.Add(SrvEApiKeyStorage.Configuration.RedirectUri + "/");
                        listener.Start();

                        var authUrl = $"https://www.eventbrite.com/oauth/authorize?response_type=code&client_id={SrvEApiKeyStorage.Configuration.ApiKey}&redirect_uri={Uri.EscapeDataString(SrvEApiKeyStorage.Configuration.RedirectUri)}"; 
                        Process.Start(new ProcessStartInfo { FileName = authUrl, UseShellExecute = true });  

                        var context = listener.GetContext(); 
                        tcs.SetResult(context);

                        listener.Stop(); 

                        code = context.Request.QueryString["code"];  

                        using var http = new HttpClient();
                        var values = new Dictionary<string, string>
                        {
                            ["grant_type"] = "authorization_code",
                            ["code"] = code,
                            ["client_id"] = SrvEApiKeyStorage.Configuration.ApiKey,
                            ["client_secret"] = SrvEApiKeyStorage.Configuration.ClientSecret,
                            ["redirect_uri"] = SrvEApiKeyStorage.Configuration.RedirectUri
                        };
                        var tokenResponse = await http.PostAsync("https://www.eventbrite.com/oauth/token", new FormUrlEncodedContent(values));
                        tokenResponse.EnsureSuccessStatusCode();
                        var json = await tokenResponse.Content.ReadAsStringAsync();

                        Console.WriteLine("Response token: " + json);

                        var eTokenResponse = JsonSerializer.Deserialize<ETokenResponse>(json);

                        SrvETokenStorage.SetCode(code);
                        SrvETokenStorage.SetBearer(eTokenResponse.Token);
                    }
                });

                var result = await tcs.Task;
            }
            catch (Exception ex)
            {

            }
        }
    }
}
