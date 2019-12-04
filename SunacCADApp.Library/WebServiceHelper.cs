using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SunacCADApp.Library
{
   public   class WebServiceHelper
    {
        public static XmlDocument QuerySoapWebService(String URL, String MethodName, Hashtable Pars)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(URL);
            request.Method = "POST";
            request.Accept = @"gzip,deflate";
            request.ContentType = @"text/xml;charset=utf-8";
            request.UserAgent = @"Jakarta Commons-HttpClient/3.1";
            request.Credentials = CredentialCache.DefaultCredentials;
            request.Timeout = 10000;
            byte[] data = EncodeParsToSoap(Pars, MethodName);
            WriteRequestData(request, data);//将处理成字节组的XML写到流中发送到服务端
            XmlDocument doc = new XmlDocument();
            doc = ReadXmlResponse(request.GetResponse());//读取服务端返回的结果
            return doc;
        }
        private static Hashtable hshtableXML = new Hashtable();
        /// <summary>
        /// 处理要发送的XML文档
        /// </summary>
        /// <param name="Pars">参数</param>
        /// <param name="MethodName">方法名</param>
        private static byte[] EncodeParsToSoap(Hashtable Pars, String MethodName)
        {
            XmlDocument xml = null;
            if (hshtableXML.ContainsKey(MethodName))
            {//如果已经加载过，则从缓存中读取
                xml = (XmlDocument)hshtableXML[MethodName];
            }
            else
            {//如果还未加载则进行加载，并放入缓存

                //从资源文件得到文件流
                Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("异步调用.file." + MethodName + ".xml");
                xml = new XmlDocument();
                xml.Load(stream);
                hshtableXML.Add(MethodName, xml);
            }

            //修改参数的值
            foreach (DictionaryEntry de in Pars)
            {
                XmlNamespaceManager nsmgr = new XmlNamespaceManager(xml.NameTable);
                nsmgr.AddNamespace("soapenv", "http://schemas.xmlsoap.org/soap/envelope/");
                nsmgr.AddNamespace("web", "http://WebXml.com.cn/");
                Hashtable subpars = de.Value as Hashtable;
                if (subpars == null)
                {
                    string subNode = "soapenv:Envelope/soapenv:Body/web:" + MethodName + "/web:" + de.Key.ToString();
                    XmlNode node = xml.SelectSingleNode(subNode, nsmgr);
                    node.InnerText = de.Value.ToString();
                }
                else
                {
                    foreach (DictionaryEntry subde in subpars)
                    {
                        string subNode = "soapenv:Envelope/soapenv:Body/ws:" + MethodName + "/" + de.Key.ToString() + "/" + subde.Key.ToString();
                        XmlNode node = xml.SelectSingleNode(subNode, nsmgr);
                        node.InnerText = subde.Value.ToString();
                    }
                }

            }

            //将修改后的XML文件保存到流中
            //这样做还可以保证发送的XML文件也是格式化的那种形式，而不是一整行
            //如通过OuterXml获取的就是一整行，这样也可能会导致服务端解析失败，个人这次就碰到这种情况了
            MemoryStream outStream = new MemoryStream();
            xml.Save(outStream);

            byte[] buffer = new byte[outStream.Length];
            byte[] temp = outStream.GetBuffer();
            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = temp[i];
            }
            outStream.Close();

            return buffer;
        }

        /// <summary>
        /// 写到流中，发送给服务端
        /// </summary>
        /// <param name="request">HttpWebRequest连接对象</param>
        /// <param name="data">要写入连接流发给服务端的内容</param>
        private static void WriteRequestData(HttpWebRequest request, byte[] data)
        {
            request.ContentLength = data.Length;
            Stream writer = request.GetRequestStream();
            writer.Write(data, 0, data.Length);
            writer.Close();
        }

        /// <summary>
        /// 读取服务端返回的结果
        /// </summary>
        private static XmlDocument ReadXmlResponse(WebResponse response)
        {

            StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
            String retXml = sr.ReadToEnd();
            sr.Close();
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(retXml);
            return doc;
        }
    }
}
