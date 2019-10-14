using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Net;
using System.Web;
using System.Xml;

namespace SunacCADApp.Library
{
    public class XmlSerializeHelper
    {

        /// <summary>
        ///  实体序列化为 XML文件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string XmlSerialize<T>(T obj)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                XmlSerializer serializer = new XmlSerializer(obj.GetType());
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.NewLineChars = "\r\n";
                settings.Encoding = Encoding.UTF8;
                settings.IndentChars = "    ";
                using (XmlWriter writer = XmlWriter.Create(stream, settings))
                {
                    serializer.Serialize(writer, obj);
                    writer.Close();
                }
                stream.Position = 0;
                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                {
                    return reader.ReadToEnd();
                }
            }
        }


        /// <summary>
        ///  XML转换为实体类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="strXML"></param>
        /// <returns></returns>
        public static T DESerializer<T>(string strXML) where T : class
        {
            try
            {
                using (StringReader sr = new StringReader(strXML))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(T));
                    return serializer.Deserialize(sr) as T;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        /// <summary>
        /// 图片转base64
        /// </summary>
        /// <param name="path">图片路径</param><br>       
        /// <returns>返回一个base64字符串</returns>
        public static string DecodeImageToBase64(string path)
        {

            path = AppDomain.CurrentDomain.BaseDirectory + path;
            FileStream fsForRead = new FileStream(path, FileMode.Open);
            string base64Str = "";
            try
            {
                //读写指针移到距开头10个字节处
                fsForRead.Seek(0, SeekOrigin.Begin);
                byte[] bs = new byte[fsForRead.Length];
                int log = Convert.ToInt32(fsForRead.Length);
                //从文件中读取10个字节放到数组bs中
                fsForRead.Read(bs, 0, log);
                base64Str = Convert.ToBase64String(bs);
                return base64Str;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            finally
            {
                fsForRead.Close();
            }

       
        }
    }
}