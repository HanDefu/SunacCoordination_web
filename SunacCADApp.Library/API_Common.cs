using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunacCADApp.Library
{
    public   class API_Common
    {

        /// <summary>
        /// 获取UUID
        /// </summary>
        public static string UUID 
        {
            get 
            {
                return Guid.NewGuid().ToString("N").ToUpper();
            }
        }

        public static string SEND_DATETIME
        {
            get
            {
                return DateTime.Now.ToString("yyyyMMddHHMMss");
            }
        }


    }
}
