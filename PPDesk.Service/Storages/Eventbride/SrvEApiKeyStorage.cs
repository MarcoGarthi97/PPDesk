using PPDesk.Abstraction.DTO.Service.Eventbrite;

namespace PPDesk.Service.Storages.Eventbride
{
    public static class SrvEApiKeyStorage
    {
        public static SrvEApiKey Configuration { get; private set; }
        
        public static void SetpiKeyStorage(SrvEApiKey srvEApiKey)
        {
            Configuration = srvEApiKey;
        }
    }
}
