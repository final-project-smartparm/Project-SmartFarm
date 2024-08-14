using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFARM.Helpers
{
    public class Common
    {
        public static readonly string CONNSTRING = "Data Source=210.119.12.79;" +
                                                    "Initial Catalog=SFARM;" +
                                                    "Persist Security Info=True;" +
                                                    "User ID=sa;" +
                                                    "Encrypt=False;" +
                                                    "Password=mssql_p@ss";

        public static readonly string TODAY = DateTime.Now.ToString("yyyy-MM-dd");

    }
}
