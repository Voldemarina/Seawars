using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seawars.Infrastructure.Data
{
    public static class ConnectionStrings
    {

        public const string ApiPath = "https://sea-wars.azurewebsites.net/";

        public const string MsSqlConnectionString = "Server=tcp:seawars-db-server.database.windows.net,1433;Initial Catalog=Seawars_db;User Id=Anton@seawars-db-server;Password=Vatazh0k";

    }
}
