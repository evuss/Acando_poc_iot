using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using AzureWebAPI1.Models;
using System.Data.SqlClient;

namespace AzureWebAPI1.Models
{
    public class SpotStateContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public SpotStateContext() : base("name=SpotStateContext")
        {
        }

        public IEnumerable<SpotState> GetSpotStateChangesSinceTS(string SinceTS) {
            return this.SpotStates.SqlQuery("dbo.pGetSpotStatusChanges @SinceTS", new SqlParameter("SinceTS",SinceTS)).ToList();
            //return this.SpotStates.SqlQuery("exec dbo.pGetSpotStatusChanges").ToList();
        }

        public IEnumerable<SpotState> GetSpotStateChangesSinceTS(string SinceTS,string TimeToDie) {
            return this.SpotStates.SqlQuery("dbo.pGetSpotStatusChanges @SinceTS @TimeToDie", new SqlParameter("SinceTS", SinceTS), new SqlParameter("TimeToDie", TimeToDie)).ToList();
        }

        /*public DateTime GetDbUtcTimestamp() {
            String sTemp;

            sTemp = Database.SqlQuery<string>("select CURRENT_TIMESTAMP",null).First<string>();
            return DateTime.ParseExact(sTemp, "yyyy-MM-dd HH:mm:ss,fff",
                                       System.Globalization.CultureInfo.InvariantCulture);
        }*/

        public System.Data.Entity.DbSet<AzureWebAPI1.Models.SpotState> SpotStates { get; set; }
    }
}
