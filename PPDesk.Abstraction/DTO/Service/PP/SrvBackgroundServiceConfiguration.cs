namespace PPDesk.Abstraction.DTO.Service.PP
{
    public class SrvBackgroundServiceConfiguration
    {
        public int MinutesInterval { get; set; }

        public SrvBackgroundServiceConfiguration() { }

        public SrvBackgroundServiceConfiguration(int minutesInterval)
        {
            MinutesInterval = minutesInterval;
        }
    }
}
