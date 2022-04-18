using System.Configuration;

namespace dsdProjectTemplate.Utility
{
    public static class SQLConnectionString
    {
        public static string dbConnection = ConfigurationManager.ConnectionStrings["dbConn"].ToString();
    }
}
