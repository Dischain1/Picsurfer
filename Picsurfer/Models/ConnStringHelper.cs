using System.Configuration;

namespace Picsurfer.Models
{
    public static class ConnectionHelper
    {
        public static string connStr = ConfigurationManager.ConnectionStrings["PicsurferContext"].ConnectionString;
    }
}