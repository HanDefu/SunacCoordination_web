using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SunacCADApp.Entity
{
     [XmlRoot("File")]
    [Serializable]
    public class XML_IDM_File
    {

         public int Id { get; set; }
         /// <summary>
         /// 文件名称
         /// </summary>
         public string FileName { get; set; }
         /// <summary>
         /// 保存文件
         /// </summary>
         public string SaveName { get; set; }
         /// <summary>
         /// 文件路径
         /// </summary>
         public string FileUrl { get; set; }
         /// <summary>
         /// 文件大小
         /// </summary>
         public string FileSize { get; set; }
          /// <summary>
          /// 创建人
          /// </summary>
         public string Creator{ get; set; }
         /// <summary>
         /// 创建时间
         /// </summary>
         public string CreateTime { get; set; }
         /// <summary>
         /// 修改人
         /// </summary>
         public string Updator { get; set; }
         /// <summary>
         /// 修改时间
         /// </summary>
         public string UpdateTime { get; set; }

    }
}
