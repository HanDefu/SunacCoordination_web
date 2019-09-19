using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AFrame.DBUtility;
using System.Data;
namespace SunacCADApp.Data
{
    public class MsSqlHelperEx
    {
        public static int Execute(string sql)
        {
            IDbHelper helper = SingletonDbHelper.GetInstance.InitDbHelper("MsSql");
            helper.SetConnectionString("Sys_Conn");
            return helper.ExecuteCommand(sql);

        }


        public static DataTable ExecuteDataTable(string sql)
        {
            IDbHelper helper = SingletonDbHelper.GetInstance.InitDbHelper("MsSql");
            helper.SetConnectionString("Sys_Conn");
            return helper.GetDataSet(sql);
        }

        public static object ExecuteScalar(string sql)
        {
            IDbHelper helper = SingletonDbHelper.GetInstance.InitDbHelper("MsSql");
            helper.SetConnectionString("Sys_Conn");
            return helper.GetScalar(sql);
        }
    }
}
