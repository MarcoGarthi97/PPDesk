using PPDesk.Abstraction.DTO.Service.Eventbrite;
using System;

namespace PPDesk.Service.Storages.Eventbride
{
    public static class SrvEApiKeyStorage
    {
        public static SrvEApiKey Configuration { get; private set; }
        
        public static void SetpiKeyStorage(SrvEApiKey srvEApiKey)
        {
            if(string.IsNullOrEmpty(srvEApiKey.PrivateToken))
            {
                throw new Exception("Errore: file appsettings.json non valorizzato");
            }

            Configuration = srvEApiKey;
        }
    }
}
