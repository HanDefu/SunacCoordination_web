using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using SunacCADApp.Entity;
using SunacCADApp.Data;
using Common.Utility;
using Common.Utility.Extender;
using SunacCADApp.Library;

namespace SunacCADApp
{
    /// <summary>
    /// IDMProjectService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class IDMProjectService : System.Web.Services.WebService
    {

        [WebMethod(Description = "用户登陆验证")]
        public string CheckLogin(string Login,string Password) 
        {
            return string.Empty;

        }
         [WebMethod(Description = "项目分类文件夹新建")]
        public string NewCadFileDir(string UID, string OID, string DrawingDir, int ParentDirId=0) 
        {
            int DirId = BasIdmProjectDirectoryDB.HasExistsDirectory(OID, DrawingDir, ParentDirId);
            if (DirId == 0)
            {
                string DirName = BasIdmProjectDirectoryDB.GetDirNameByDirID(ParentDirId);
                Bas_Idm_ProjectDirectory directory = new Bas_Idm_ProjectDirectory();
                directory.DirName = DrawingDir;
                directory.OID = OID;
                directory.ParentDirId = ParentDirId;
           
                directory.CreateUserId = UID.ConvertToInt32(0);
                directory.CreateBy = UID;
                int rtv = BasIdmProjectDirectoryDB.AddHandle(directory);
                if (rtv > 0)
                {
                    return XmlSerializeHelper.XmlSerialize<XML_Result>(new XML_Result() { Code = 100, Message = "目录创建成功" });
                }
                else
                {
                    return XmlSerializeHelper.XmlSerialize<XML_Result>(new XML_Result() { Code = -100, Message = "目录创建失败" });
                }
            }
            else 
            {
                return XmlSerializeHelper.XmlSerialize<XML_Result>(new XML_Result() { Code = -100, Message = "目录已存在" });
            }
        }
  
         [WebMethod(Description = "项目分类文件夹删除")]

         public string DeleteCadFileDir(string UID, string OID, string DrawingDir, string ParentDir)
         {
             string _wh = string.Format(@" DirName='{0}' AND OID='{1}' AND ISNULL(ParentDirName,'')='{2}'", DrawingDir, OID, ParentDir);
             int rtv = BasIdmProjectDirectoryDB.DeleteHandleByParam(_wh);
             if (rtv > 0)
             {
                 return XmlSerializeHelper.XmlSerialize<XML_Result>(new XML_Result() { Code = 100, Message = "目录删除成功" });
             }
             else
             {
                 return XmlSerializeHelper.XmlSerialize<XML_Result>(new XML_Result() { Code = -100, Message = "目录删除失败" });
             }
         }
    
    }
}
