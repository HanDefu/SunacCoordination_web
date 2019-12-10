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
using System.Text.RegularExpressions;

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

        public static string BIZTRANSACTIONID
        {
            get 
            {
                return string.Format("CAD_SUNAC_564_WriteSAPXmlToBPM_PS{0}", DateTime.Now.ToString("yyyyMMddHHmmss"));
            }
            //getCAD_SUNAC_564_WriteSAPXmlToBPM
        }

        public static string BIZTRANSACTIONIDValidatePwd
        {
            get
            {
                return string.Format("IDM_SUNAC_392_validatePwd_PS{0}", DateTime.Now.ToString("yyyyMMddHHmmss"));
            }
            //getCAD_SUNAC_564_WriteSAPXmlToBPM
        }


        /// <summary>
        ///审批过程中修改BIZTRANSACTIONID
        /// </summary>
        public static string BIZTRANSACTIONIDUpdateAndApproveFlow 
        {
            get 
            {
                return string.Format(@"CAD_SUNAC_565_UpdateAndApproveFlow_PS{0}", DateTime.Now.ToString("yyyyMMddHHmmss"));
            }
        }

        /// <summary>
        /// 流程作废BIZTRANSACTIONID
        /// </summary>
        public static string BIZTRANSACTIONIDDoInvalid 
        {
            get
            {
                return string.Format(@"CAD_SUNAC_566_DoInvalid_PS{0}", DateTime.Now.ToString("yyyyMMddHHmmss"));
            }
        }


        public static string BIZTRANSACTIONIDGetFlowState 
        {
            get
            {
                return string.Format(@"CAD_SUNAC_567_GetFlowState_PS{0}", DateTime.Now.ToString("yyyyMMddHHmmss"));
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

        public static string FilterXSS(string html) 
        {
            if (string.IsNullOrEmpty(html)) return string.Empty;

            // CR(0a) ，LF(0b) ，TAB(9) 除外，过滤掉所有的不打印出来字符.    
            // 目的防止这样形式的入侵 ＜java\0script＞   
            // 注意：\n, \r,  \t 可能需要单独处理，因为可能会要用到   
            string ret = System.Text.RegularExpressions.Regex.Replace(html, "([\x00-\x08][\x0b-\x0c][\x0e-\x20])", string.Empty);

            //替换所有可能的16进制构建的恶意代码   
            //<IMG SRC=&#X40&#X61&#X76&#X61&#X73&#X63&#X72&#X69&#X70&#X74&#X3A&#X61&_#X6C&#X65&#X72&#X74&#X28&#X27&#X58&#X53&#X53&#X27&#X29>  
            string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()~`;:?+/={}[]-_|'\"\\";
            for (int i = 0; i < chars.Length; i++)
            {
                ret = System.Text.RegularExpressions.Regex.Replace(ret, string.Concat("(&#[x|X]0{0,}", Convert.ToString((int)chars[i], 16).ToLower(), ";?)"),
                    chars[i].ToString(), System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            }

            //过滤\t, \n, \r构建的恶意代码  
            string[] keywords = {"javascript", "vbscript", "expression", "applet", "meta", "xml", "blink", "link", "style", "script", "embed", "object", "iframe", "frame", "frameset", "ilayer", "layer", "bgsound", "title", "base" 
        ,"onabort", "onactivate", "onafterprint", "onafterupdate", "onbeforeactivate", "onbeforecopy", "onbeforecut", "onbeforedeactivate", "onbeforeeditfocus", "onbeforepaste", "onbeforeprint", "onbeforeunload", "onbeforeupdate", "onblur", "onbounce", "oncellchange", "onchange", "onclick", "oncontextmenu", "oncontrolselect", "oncopy", "oncut", "ondataavailable", "ondatasetchanged", "ondatasetcomplete", "ondblclick", "ondeactivate", "ondrag", "ondragend", "ondragenter", "ondragleave", "ondragover", "ondragstart", "ondrop", "onerror", "onerrorupdate", "onfilterchange", "onfinish", "onfocus", "onfocusin", "onfocusout", "onhelp", "onkeydown", "onkeypress", "onkeyup", "onlayoutcomplete", "onload", "onlosecapture", "onmousedown", "onmouseenter", "onmouseleave", "onmousemove", "onmouseout", "onmouseover", "onmouseup", "onmousewheel", "onmove", "onmoveend", "onmovestart", "onpaste", "onpropertychange", "onreadystatechange", "onreset", "onresize", "onresizeend", "onresizestart", "onrowenter", "onrowexit", "onrowsdelete", "onrowsinserted", "onscroll", "onselect", "onselectionchange", "onselectstart", "onstart", "onstop", "onsubmit", "onunload"};

            bool found = true;
            while (found)
            {
                var retBefore = ret;
                for (int i = 0; i < keywords.Length; i++)
                {
                    string pattern = "/";
                    for (int j = 0; j < keywords[i].Length; j++)
                    {
                        if (j > 0)
                            pattern = string.Concat(pattern, '(', "(&#[x|X]0{0,8}([9][a][b]);?)?", "|(&#0{0,8}([9][10][13]);?)?", ")?");
                        pattern = string.Concat(pattern, keywords[i][j]);
                    }
                    string replacement = string.Concat(keywords[i].Substring(0, 2), "＜x＞", keywords[i].Substring(2));
                    ret = System.Text.RegularExpressions.Regex.Replace(ret, pattern, replacement, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                    if (ret == retBefore)
                        found = false;
                }

            }
            return ret;
  
        }
        public static string  FilterIllegalChar(string sWord)
        {
            var keyWord = @"select|insert|delete|from|count\(|drop table|update|truncate|asc\(|mid\(|char\(|xp_cmdshell|exec master|netlocalgroup administrators|:|net user|""|or|and";
            string StrRegex = @"[-|;|,|/|\(|\)|\[|\]|}|{|%|\@|*|!|']";
            if (Regex.IsMatch(sWord, keyWord, RegexOptions.IgnoreCase) || Regex.IsMatch(sWord, StrRegex))
                return "";

            return sWord;
        }


        public static string GetBSID 
        {
            get 
            {
                string _param = GlobalParam("BPMBSID");
                return _param;
            }
        }
    }
}
