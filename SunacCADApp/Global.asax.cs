using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;



namespace SunacCADApp
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            TimeTask.Instance().ExecuteTask += new System.Timers.ElapsedEventHandler(Global_ExecuteTask);
            TimeTask.Instance().Interval = 1000 * 60;
            TimeTask.Instance().Start();
        }

        protected void Global_ExecuteTask(object sender, System.Timers.ElapsedEventArgs e)
        {
            SunacCADApp.Data.Test_Helper_Db.AddHelper();
        }
    }
}
