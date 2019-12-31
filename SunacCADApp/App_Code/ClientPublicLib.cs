using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SunacCADApp
{
    public class ClientPublicLib
    {
        /// <summary>
        /// 获取远程访问用户的Ip地址
        /// </summary>
        /// <returns>返回Ip地址</returns>
        public static string GetLoginIp()
        {
            string loginip = "";
            //Request.ServerVariables[""]--获取服务变量集合 
            if (HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"] != null) //判断发出请求的远程主机的ip地址是否为空
            {
                //获取发出请求的远程主机的Ip地址
                loginip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
            }
            //判断登记用户是否使用设置代理
            else if (HttpContext.Current.Request.ServerVariables["HTTP_VIA"] != null)
            {
                if (HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
                {
                    //获取代理的服务器Ip地址
                    loginip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
                }
                else
                {
                    //获取客户端IP
                    loginip = HttpContext.Current.Request.UserHostAddress;
                }
            }
            else
            {
                //获取客户端IP
                loginip = HttpContext.Current.Request.UserHostAddress;
            }

            loginip = HttpContext.Current.Request.UserHostAddress;
            return loginip;
        }

    }
}