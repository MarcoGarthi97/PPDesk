namespace PPDesk.Abstraction.DTO.Service.PP
{
    public class SrvDatabaseConfigurationBySQL
    {
        public bool LoadFast { get; set; }

        public SrvDatabaseConfigurationBySQL() { }

        public SrvDatabaseConfigurationBySQL(bool loadFast)
        {
            LoadFast = loadFast;
        }
    }
}
