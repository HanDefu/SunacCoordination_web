using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Configuration.Internal;
using System.Collections.Specialized;

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

        public static string WebURL 
        {
            get 
            {
                return ConfigurationSettings.AppSettings["WebURL"];
            }
        }


        public static string GlobalSetting
        {
            get 
            {

                return ConfigurationSettings.AppSettings["ExceDateTime"];
            }
        }
        public static string SEND_DATETIME
        {
            get
            {
                return DateTime.Now.ToString("yyyyMMddHHMMss");
            }
        }

        public static string GlobalParam(string Key) 
        {
            return ConfigurationSettings.AppSettings[Key];
        }

        public static string GetSOAPReSource(string url, string datastr)
        {
            try
            {
                //request
                Uri uri = new Uri(url);
                WebRequest webRequest = WebRequest.Create(uri);
                webRequest.ContentType = "text/xml; charset=utf-8";
                webRequest.Headers.Add("Username", "POQ_CAD");
                webRequest.Headers.Add("Password", "cad@1234");
                webRequest.Method = "POST";
                using (Stream requestStream = webRequest.GetRequestStream())
                {
                    byte[] paramBytes = Encoding.UTF8.GetBytes(datastr.ToString());
                    requestStream.Write(paramBytes, 0, paramBytes.Length);
                }
                //response
                WebResponse webResponse = webRequest.GetResponse();
                using (StreamReader myStreamReader = new StreamReader(webResponse.GetResponseStream(), Encoding.UTF8))
                {
                    string result = "";
                    return result = myStreamReader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool DownloadFile(string file,string saveFile) 
        {
            try
            {
               // string pathFile = string.Format(@"\\192.168.7.209\CAD\ProjectFiles\Window_NC2_0.dwg");
                string pathFile = file;
                FileStream fs = new FileStream(pathFile, FileMode.Open);
                byte[] bytes = new byte[(int)fs.Length];
                fs.Read(bytes, 0, bytes.Length);
                fs.Close();
                string fileName = System.Web.HttpUtility.UrlEncode(saveFile, System.Text.Encoding.UTF8);
                System.Web.HttpContext.Current.Response.ContentType = "application/octet-stream";
                System.Web.HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;  filename=" + fileName);
                System.Web.HttpContext.Current.Response.BinaryWrite(bytes);
                System.Web.HttpContext.Current.Response.Flush();
                System.Web.HttpContext.Current.Response.End();
                return true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
   
    }
}
