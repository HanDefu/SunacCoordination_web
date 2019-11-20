using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AFrame.DBUtility;
using Common.Utility;
using Common.Utility.Extender;

namespace SunacCADApp.Data
{
    public  class Test_Helper_Db
    {
        public static void AddHelper()
        {
            string sql=string.Format(@"INSERT INTO dbo.Test_Log (ExecDateTime) VALUES (GETDATE())");
            MsSqlHelperEx.Execute(sql);
        }
    }
}
