using CommunityToolkit.Mvvm.ComponentModel;
using PPDesk.Abstraction.DTO.Service.Eventbrite;
using PPDesk.Abstraction.DTO.Service.PP;
using PPDesk.Abstraction.Helper;
using PPDesk.Service.Services.PP;
using PPDesk.Service.Storages.Eventbride;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PPDesk.ViewModels
{
    public class SettingViewModel : ObservableObject, IForServiceCollectionExtension
    {
        private readonly ISrvDatabaseService _databaseService;
        private readonly ISrvHelperService _helperService;

        private string _apiKey;
        private string _clientSecret;
        private string _privateToken;
        private string _publicToken;
        private string _redirectUri;

        public string? ApiKey
        {
            get => _apiKey;
            set
            {
                SetProperty(ref _apiKey!, value);
            }
        }
        public string? ClientSecret
        {
            get => _clientSecret;
            set
            {
                SetProperty(ref _clientSecret!, value);
            }
        }
        public string? PrivateToken
        {
            get => _privateToken;
            set
            {
                SetProperty(ref _privateToken!, value);
            }
        }
        public string? PublicToken
        {
            get => _publicToken;
            set
            {
                SetProperty(ref _publicToken!, value);
            }
        }
        public string? RedirectUri
        {
            get => _redirectUri;
            set
            {
                SetProperty(ref _redirectUri!, value);
            }
        }

        public SettingViewModel(ISrvDatabaseService databaseService, ISrvHelperService helperService)
        {
            _databaseService = databaseService;
            _helperService = helperService;
        }

        public async Task CreateDatabaseAsync()
        {
            await _databaseService.CreateTablesAsync();
        }

        public void LoadApiKey()
        {
            var apiKey = SrvEApiKeyStorage.Configuration;
            if(apiKey != null)
            {
                ApiKey = apiKey.ApiKey;
                ClientSecret = apiKey.ClientSecret;
                PrivateToken = apiKey.PrivateToken;
                PublicToken = apiKey.PublicToken;
                RedirectUri = apiKey.RedirectUri;
            }
        }

        public async Task SaveApiKeyAsync()
        {
            if(PrivateToken != null)
            {
                SrvEApiKey apiKey = new SrvEApiKey
                {
                    ApiKey = ApiKey,
                    ClientSecret = ClientSecret,
                    PrivateToken = PrivateToken,
                    PublicToken = PublicToken,
                    RedirectUri = RedirectUri
                };

                var helper = new SrvHelper
                {
                    Key = "EventbrideApiKey",
                    Json = JsonSerializer.Serialize(apiKey)
                };

                if(SrvEApiKeyStorage.Configuration == null)
                {
                    await _helperService.InsertHelperAsync(helper);
                }
                else
                {
                    await _helperService.UpdateHelperAsync(helper);
                }

                SrvEApiKeyStorage.SetpiKeyStorage(apiKey);
            }
        }
    }
}
