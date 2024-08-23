using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFARM.Helpers
{
    public class UserPlantList
    {
        public static readonly string GETDATE_QUERY = @"SELECT Facilities_id
                                                          FROM [dbo].[PlaceInfo]
                                                         GROUP BY Facilities_id";
        public static int USER_NUM {  get; set; }
        public static int PLANT_IDX { get; set; }
        public static int PLANT_NUM { get; set; }
        public static string PLANT_NAME { get; set; }
    }
}
