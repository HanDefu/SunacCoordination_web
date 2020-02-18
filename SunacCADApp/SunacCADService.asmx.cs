using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using SunacCADApp.Entity;
using SunacCADApp.Data;
using SunacCADApp.Library;
using Common.Utility;
using Common.Utility.Extender;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Text;
using System.IO;

namespace SunacCADApp
{
    /// <summary>
    /// ArgumentSettingService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class ArgumentSettingService : System.Web.Services.WebService
    {

        [WebMethod(Description = "标准模块属性；1、标准部品库：外窗、门、栏杆;2、厨房、卫生间、空调")]
        public string StandardDesignAttribute(string AtrributeName)
        {
            XMLArgumentSetting setting = null;
            try
            {
                string where = string.Format(" and b.TypeCode='{0}'", AtrributeName);
                IList<ArgumentSetting> list = BasArgumentSettingDB.GetXMLBasArgumentSettingByWhere(where);
                if (list != null)
                {
                    setting = new XMLArgumentSetting()
                    {
                        Code = 100,
                        Message = "数据返回成功",
                        ArgumentSettings = list.ToArray<ArgumentSetting>()
                    };
                }
                else
                {
                    setting = new XMLArgumentSetting()
                    {
                        Code = -100,
                        Message = "数据返回失败"
                    };
                }
            }
            catch (Exception ex)
            {
                setting = new XMLArgumentSetting()
                {
                    Code = -110,
                    Message = ex.Message
                };
            }
            string xml = XmlSerializeHelper.XmlSerialize<XMLArgumentSetting>(setting);
            return xml;
        }

        [WebMethod(Description = "标准部品库[外窗]所有")]
        public string GetAllWindows()
        {
            string xml = string.Empty;
            try
            {
                XMLCadDrawingWindow window = XMLCadDrawingWindowDB.GetXMLCadDrawingWindow();
                xml = XmlSerializeHelper.XmlSerialize<XMLCadDrawingWindow>(window);
            }
            catch (Exception ex)
            {

            }
            return xml;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="openType"></param>
        /// <param name="openNum"></param>
        /// <param name="gongNengQu"></param>
        /// <returns></returns>
        [WebMethod(Description = "标准部品库[外窗]按条件查询")]
        public string GetWindows(double width = 0, double height = 0, string openType = "", string openNum = "", string gongNengQu = "")
        {
            string xml = string.Empty;
            if (width < 1)
            {
                xml = XmlSerializeHelper.XmlSerialize<XMLCadDrawingWindow>(new XMLCadDrawingWindow() { Code = -100, Message = "外窗宽度不能为空" });
            }
            else
            {
                XMLCadDrawingWindow window = XMLCadDrawingWindowDB.GetXMLCadDrawingWindow(width, height, openType, openNum, gongNengQu);
                xml = XmlSerializeHelper.XmlSerialize<XMLCadDrawingWindow>(window);
            }
            return xml;

        }

        [WebMethod(Description = "标准部品库[门]所有")]
        public string GetAllDoor()
        {
            string xml = string.Empty;
            try
            {
                IList<Door> doors = XMLCadDrawingDoorDB.GetCadDrawingDoorsListByWidth(0, string.Empty);
                var retDoor = new XMLDoor { Code = 100, Message = "门查询成功", Doors = doors.ToArray() };
                xml = XmlSerializeHelper.XmlSerialize<XMLDoor>(retDoor);
            }
            catch (Exception ex)
            {
                var retDoor = new XMLDoor { Code = -100, Message = ex.Message };
                xml = XmlSerializeHelper.XmlSerialize<XMLDoor>(retDoor);
            }
            return xml;


        }
        [WebMethod(Description = "标准部品库[门]条件查询")]
        public string GetAllDoorByParam(double width, string doorType)
        {
            string xml = string.Empty;
            try
            {
                IList<Door> doors = XMLCadDrawingDoorDB.GetCadDrawingDoorsListByWidth(width, doorType);
                var retDoor = new XMLDoor { Code = 100, Message = "门查询成功", Doors = doors.ToArray() };
                xml = XmlSerializeHelper.XmlSerialize<XMLDoor>(retDoor);
            }
            catch (Exception ex)
            {
                var retDoor = new XMLDoor { Code = -100, Message = ex.Message };
                xml = XmlSerializeHelper.XmlSerialize<XMLDoor>(retDoor);
            }
            return xml;


        }

        [WebMethod(Description = "标准部品库[厨房原型]")]
        public string GetAllKitchen()
        {
            string xml = string.Empty;
            try
            {

                IList<Kitchen> kitchens = XMLCadDrawingKitchenDB.GetKitchenListByWidth(0, 0, string.Empty, string.Empty, string.Empty);
                var retKitchen = new XMLKitchen { Code = 100, Message = "厨房原型查询成功", Kitchens = kitchens.ToArray() };
                xml = XmlSerializeHelper.XmlSerialize<XMLKitchen>(retKitchen);
            }
            catch (Exception ex)
            {
                var retDoor = new XMLKitchen { Code = -100, Message = ex.Message };
                xml = XmlSerializeHelper.XmlSerialize<XMLKitchen>(retDoor);
            }
            return xml;
        }

        [WebMethod(Description = "标准部品库[厨房原型]条件查询")]
        public string GetAllKitchenParam(double Width, double Height, string KitchenDoorWindowPosition, string KitchenType, string AirVent)
        {
            string xml = string.Empty;
            try
            {
                if (Width < 1)
                {
                    var retDoor = new XMLKitchen { Code = -100, Message = "Width必须填写" };
                    xml = XmlSerializeHelper.XmlSerialize<XMLKitchen>(retDoor);
                }

                IList<Kitchen> kitchens = XMLCadDrawingKitchenDB.GetKitchenListByWidth(Width, Height, KitchenDoorWindowPosition, KitchenType, AirVent);
                var retKitchen = new XMLKitchen { Code = 100, Message = "厨房原型查询成功", Kitchens = kitchens.ToArray() };
                xml = XmlSerializeHelper.XmlSerialize<XMLKitchen>(retKitchen);
            }
            catch (Exception ex)
            {
                var retDoor = new XMLKitchen { Code = -100, Message = ex.Message };
                xml = XmlSerializeHelper.XmlSerialize<XMLKitchen>(retDoor);
            }
            return xml;
        }

        [WebMethod(Description = "标准部品库[卫生间原型]")]
        public string GetAllBathroom()
        {
            string xml = string.Empty;
            try
            {

                IList<Bathroom> bathroomList = XMLCadDrawingBathroomDB.GetCadDrawingBathroomListByWidth(0, 0, string.Empty, string.Empty, string.Empty);
                var retBathroom = new XMLBathroom { Code = 100, Message = "卫生间查询成功", Bathrooms = bathroomList.ToArray() };
                xml = XmlSerializeHelper.XmlSerialize<XMLBathroom>(retBathroom);
            }
            catch (Exception ex)
            {
                var retBathroom = new XMLBathroom { Code = -100, Message = ex.Message };
                xml = XmlSerializeHelper.XmlSerialize<XMLBathroom>(retBathroom);
            }
            return xml;

        }

        [WebMethod(Description = "标准部品库[卫生间原型]条件查询")]
        public string GetAllBathroomByParam(double Width, double Height, string BathroomDoorWindowPosition, string ToiletType, string AirVent)
        {
            string xml = string.Empty;
            try
            {

                IList<Bathroom> bathroomList = XMLCadDrawingBathroomDB.GetCadDrawingBathroomListByWidth(Width, Height, BathroomDoorWindowPosition, ToiletType, AirVent);
                var retBathroom = new XMLBathroom { Code = 100, Message = "卫生间查询成功", Bathrooms = bathroomList.ToArray() };
                xml = XmlSerializeHelper.XmlSerialize<XMLBathroom>(retBathroom);
            }
            catch (Exception ex)
            {
                var retBathroom = new XMLBathroom { Code = -100, Message = ex.Message };
                xml = XmlSerializeHelper.XmlSerialize<XMLBathroom>(retBathroom);
            }
            return xml;


        }


        [WebMethod(Description = "标准部品库[栏杆原型]条件查询")]
        public string GetAllHandrailByParam(string RailingType)
        {
            string xml = string.Empty;
            try
            {

                IList<Handrail> handrailList = XMLCadDrawingHandrailDB.GetCadDrawingHandrailListByParam(RailingType);
                var retHandrail = new XMLHandrail { Code = 100, Message = "栏杆原型查询成功", Handrails = handrailList.ToArray() };
                xml = XmlSerializeHelper.XmlSerialize<XMLHandrail>(retHandrail);
            }
            catch (Exception ex)
            {
                var retBathroom = new XMLHandrail { Code = -100, Message = ex.Message };
                xml = XmlSerializeHelper.XmlSerialize<XMLHandrail>(retBathroom);
            }
            return xml;
        }
        [WebMethod(Description = "标准部品库[空调原型]条件查询")]
        public string GetAllAirconditionerByParam(string AirconditionerPower, string AirconditionerPipePosition, string AirconditionerIsRainpipe, string RainpipePosition)
        {
            string xml = string.Empty;
            try
            {

                IList<Airconditioner> airconditionerList = XMLCadDrawingAirconditionerDB.GetCadDrawingAirconditionerListByParam(AirconditionerPower, AirconditionerPipePosition, AirconditionerIsRainpipe, RainpipePosition);
                var retAirconditioner = new XMLAirconditioner { Code = 100, Message = "卫生间查询成功", Airconditioners = airconditionerList.ToArray() };
                xml = XmlSerializeHelper.XmlSerialize<XMLAirconditioner>(retAirconditioner);
            }
            catch (Exception ex)
            {
                var retAirconditioner = new XMLAirconditioner { Code = -100, Message = ex.Message };
                xml = XmlSerializeHelper.XmlSerialize<XMLAirconditioner>(retAirconditioner);
            }
            return xml;
        }

        [WebMethod(Description = "标准部品库[ CAD文件下载]")]
        public string CadFileDownload(int Id, string Type = "CAD")
        {
            return XMLCadDrawingWindowDB.GetCADFileDownloadByWhere(Id);
        }

        [WebMethod(Description = "标准部品库[原型文件缩略图下载]")]
        public string CadImgDownload(int Id)
        {
            return XMLCadDrawingWindowDB.GeImgFileDownloadByID(Id);
        }


        /// <summary>
        /// 项目分类文件夹新建
        /// </summary>
        /// <param name="UID"></param>
        /// <param name="OID"></param>
        /// <param name="DrawingDir"></param>
        /// <param name="ParentDirId"></param>
        /// <returns></returns>
        [WebMethod(Description = "项目分类文件夹新建")]
        public string NewCadFileDir(string UID, string OID, string DrawingDir, int ParentDirId)
        {
            int DirId = BasIdmProjectDirectoryDB.HasExistsDirectory(OID, DrawingDir, ParentDirId);
            if (DirId == 0)
            {
                string DirName = BasIdmProjectDirectoryDB.GetDirNameByDirID(ParentDirId);
                Bas_Idm_ProjectDirectory directory = new Bas_Idm_ProjectDirectory();
                directory.DirName = DrawingDir;
                directory.OID = OID;
                directory.ParentDirId = ParentDirId;
                directory.ParentDirName = DirName;
                directory.CreateUserId = UID.ConvertToInt32(0);
                directory.CreateBy = UID;
                int rtv = BasIdmProjectDirectoryDB.AddHandle(directory);
                if (rtv > 0)
                {
                    return XmlSerializeHelper.XmlSerialize<XML_Result>(new XML_Result() { Code = 100, Message = "目录创建成功", KeyId = rtv });
                }
                else
                {
                    return XmlSerializeHelper.XmlSerialize<XML_Result>(new XML_Result() { Code = -100, Message = "目录创建失败", KeyId = rtv });
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
            string _wh = string.Format(@" DirName='{0}' AND OID='{1}' AND ParentDirId='{2}'", DrawingDir, OID, ParentDir);
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

        /// <summary>
        /// 项目分类文件夹删除[根据文件夹ID]
        /// </summary>
        /// <param name="DirId">文件夹ID</param>
        /// <returns></returns>
        [WebMethod(Description = "项目分类文件夹删除[根据文件夹ID]")]
        public string DeleteCadFileDirByDirId(int DirId)
        {
            int rtv = BasIdmProjectDirectoryDB.SetEnabledProjectDirectoryById(-1, DirId);
            if (rtv > 0)
            {
                return XmlSerializeHelper.XmlSerialize<XML_Result>(new XML_Result() { Code = 100, Message = "目录删除成功" });
            }
            else
            {
                return XmlSerializeHelper.XmlSerialize<XML_Result>(new XML_Result() { Code = -100, Message = "目录删除失败" });
            }
        }

        [WebMethod(Description = "项目分类文件夹重命名")]
        public string RenameCadFileDir(string UID, string OID, string DrawingDir, int ParentDirId, string NewDrawingDir)
        {
            int has_drawing_dir = BasIdmProjectDirectoryDB.HasExistsDirectory(OID, DrawingDir, ParentDirId);
            if (has_drawing_dir == 0)
            {
                return XmlSerializeHelper.XmlSerialize<XML_Result>(new XML_Result() { Code = -100, Message = "原目录不存在" });
            }

            int has_new_drawing_dir = BasIdmProjectDirectoryDB.HasExistsDirectory(OID, DrawingDir, ParentDirId);
            if (has_new_drawing_dir > 0)
            {
                return XmlSerializeHelper.XmlSerialize<XML_Result>(new XML_Result() { Code = -100, Message = "新目录已存在" });
            }

            int rtvDir = BasIdmProjectDirectoryDB.RenameDirectory(NewDrawingDir, has_drawing_dir);
            if (rtvDir > 0)
            {
                return XmlSerializeHelper.XmlSerialize<XML_Result>(new XML_Result() { Code = 100, Message = "文件夹重命名成功", KeyId = has_drawing_dir });
            }
            else
            {
                return XmlSerializeHelper.XmlSerialize<XML_Result>(new XML_Result() { Code = -100, Message = "重命名成功失败" });
            }
        }


        /// <summary>
        /// 获取项目信息
        /// </summary>
        /// <param name="UID"></param>
        /// <returns></returns>
        [WebMethod(Description = "项目信息获取")]
        public string GetProjectInfo(string UID)
        {
            try
            {
                XML_IDM_Project project = XML_IDM_ProjectDB.Get_IDM_Project(UID);
                return XmlSerializeHelper.XmlSerialize<XML_IDM_Project>(project);
            }
            catch (Exception ex)
            {
                XML_IDM_Project project = new XML_IDM_Project() { Code = -100, Message = ex.Message };
                return XmlSerializeHelper.XmlSerialize<XML_IDM_Project>(project);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="UID">用户ID </param>
        /// <param name="OID">项目ID</param>
        /// <param name="FileSaveName"></param>
        /// <param name="DrawingFile"></param>
        /// <param name="DrawingDirId"></param>
        /// <returns></returns>
        [WebMethod(Description = "项目图纸上传")]
        public string UpdateCadDrawing(string UID, string OID, string FileSaveName, string DrawingFile, int DrawingDirId)
        {
            int hasDrawing = BasIdmProjectFileDB.HasBasIdmProjectFile(OID, FileSaveName, DrawingFile, DrawingDirId);
            string DirName = BasIdmProjectDirectoryDB.GetDirNameByDirID(DrawingDirId);
            string userName = Sys_UserDB.GetUserNameByUId(UID);
            if (string.IsNullOrEmpty(userName)) 
            {
                return XmlSerializeHelper.XmlSerialize<XML_Result>(new XML_Result() { Code = -110, Message = "用户名不存在" });
            }
            int rtnFlag = 0;
            int fileId = 0;
            int keyid = 0;
            DateTime dateTime = DateTime.Now;
            long fileSize = 0;
            string CadFilePath = string.Concat(API_Common.WebURL, "/upfile/", dateTime.ToString("yyyyMMdd"), "/", FileSaveName);
            string localFilePathURL = string.Concat(API_Common.GlobalParam("projectFilePath"),"\\", dateTime.ToString("yyyyMMdd"), "\\", FileSaveName);
            if (File.Exists(localFilePathURL))
            {
                fileSize = new FileInfo(localFilePathURL).Length;
            }
            else 
            {
                return XmlSerializeHelper.XmlSerialize<XML_Result>(new XML_Result() { Code = -100, Message = "服务器文件不存在" });
            }
            if (hasDrawing == 0)
            {

                Bas_Idm_ProjectFile file = new Bas_Idm_ProjectFile();
                file.OID = OID.ConvertToInt32(0);
                file.DirId = DrawingDirId;
                file.DirName = DirName;
                file.FileName = DrawingFile;
                file.SaveName = FileSaveName;
                file.CreateUserId = UID.ConvertToInt32(0);
                file.CreateOn = dateTime;
                file.CreateBy = userName;
                file.FileSize = fileSize.ConventToString("0");
                rtnFlag = BasIdmProjectFileDB.AddHandle(file);
                keyid = rtnFlag;
                fileId = rtnFlag;

            }
            else
            {
                Bas_Idm_ProjectFile file = new Bas_Idm_ProjectFile();
                file.OID = OID.ConvertToInt32(0);
                file.DirId = DrawingDirId;
                file.DirName = DirName;
                file.FileName = DrawingFile;
                file.SaveName = FileSaveName;
                file.ModifiedUserId = UID.ConvertToInt32(0);
                file.ModifiedOn = dateTime;
                file.ModifiedBy = userName;
                file.FileSize = fileSize.ConventToString("0");
                file.Id = hasDrawing;
                rtnFlag = BasIdmProjectFileDB.EditHandle(file, string.Empty);
                keyid = hasDrawing;
                fileId = hasDrawing;
            }
            if (rtnFlag > 0)
            {
                string webURL = API_Common.GlobalParam("WebURL");
                CadFilePath = string.Concat(webURL, "/", "projectInfo/filedownload", "/", fileId);
                return XmlSerializeHelper.XmlSerialize<XML_Result>(new XML_Result() { Code = 100, Message = "文件上传成功", KeyId = keyid, CadUriPath = CadFilePath });
            }
            else
            {
                return XmlSerializeHelper.XmlSerialize<XML_Result>(new XML_Result() { Code = -100, Message = "文件上传失败" });
            }

        }
        /// <summary>
        /// 项目文件删除
        /// </summary>
        /// <param name="UID"></param>
        /// <param name="OID"></param>
        /// <param name="DrawingDir"></param>
        /// <param name="DrawingFile"></param>
        /// <returns></returns>
        [WebMethod(Description = "项目文件删除")]
        public string DeleteCadDrawing(string UID, string OID, string DrawingDir, string DrawingFile)
        {
            int rtnFlag = 0;
            string _where = string.Format(@"  OID='{0}'  AND [FileName]='{1}' AND DirName='{2}'", OID, DrawingFile, DrawingDir);
            rtnFlag = BasIdmProjectFileDB.DeleteHandleByParam(_where);
            if (rtnFlag > 0)
            {
                return XmlSerializeHelper.XmlSerialize<XML_Result>(new XML_Result() { Code = 100, Message = "目录删除成功" });
            }
            else
            {
                return XmlSerializeHelper.XmlSerialize<XML_Result>(new XML_Result() { Code = -100, Message = "目录删除失败" });
            }
        }
        /// <summary>
        /// 项目图纸上传
        /// </summary>
        /// <param name="FileID">项目ID</param>
        /// <returns></returns>
        [WebMethod(Description = "项目文件删除根据文件ID")]
        public string DeleteCadDrawingByFileID(int FileID)
        {
            int rtnFlag = 0;
            //string _where = string.Format(@"  Id='{0}'", FileID);
            //rtnFlag = BasIdmProjectFileDB.DeleteHandleByParam(_where);
            rtnFlag = BasIdmProjectFileDB.SetProjectFileEnabledById(-1, FileID);
            if (rtnFlag > 0)
            {
                return XmlSerializeHelper.XmlSerialize<XML_Result>(new XML_Result() { Code = 100, Message = "目录删除成功" });
            }
            else
            {
                return XmlSerializeHelper.XmlSerialize<XML_Result>(new XML_Result() { Code = -100, Message = "目录删除失败" });
            }
        }

        [WebMethod(Description = "验证用户登陆")]
        public string CheckUserInfo(string userName, string password)
        {
            XMLResultUser user = new XMLResultUser();
            Sys_User sysUser = Sys_UserDB.GetSysUserByUserName(userName);
            if (sysUser.Id < 1)
            {
                user = new XMLResultUser() { Code = -100, Message = "登陆名错误" };
                return XmlSerializeHelper.XmlSerialize<XMLResultUser>(user);
            }
            else if (sysUser.Is_Internal == 2)
            {
                string _pwd = CommonLib.UserMd5(password);
                if (sysUser.User_Psd != _pwd)
                {
                    user = new XMLResultUser() { Code = -100, Message = "密码不正确" };
                    return XmlSerializeHelper.XmlSerialize<XMLResultUser>(user);
                }
                else
                {
                    XMLUser xmlUser = new XMLUser();
                    xmlUser.Id = sysUser.Id;
                    xmlUser.UserName = sysUser.User_Name;
                    xmlUser.IsInternal = 1;
                    user = new XMLResultUser() { Code = 100, Message = "登陆成功", User = xmlUser };
                    return XmlSerializeHelper.XmlSerialize<XMLResultUser>(user);

                }
            }
            else if (sysUser.Is_Internal == 1)
            {
                string webURL = "http://192.168.2.219:8002/WP_SUNAC/APP_RYG_SERVICES/Proxy_Services/TA_EOP/RYG_SUNAC_486_ValidatePwd_PS";
                HttpWebRequest request = WebRequest.Create(webURL) as HttpWebRequest;
                request.Method = "POST";
                request.ContentType = "application/json";
                string data = "{\n\"username\": " + userName + ",\n\"password\": " + password + "\n}";
                byte[] byteData = UTF8Encoding.UTF8.GetBytes(data.ToString());
                request.ContentLength = byteData.Length;
                using (Stream postStream = request.GetRequestStream())
                {
                    postStream.Write(byteData, 0, byteData.Length);
                }

                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    string rescontent = reader.ReadToEnd();
                    JObject jO = JObject.Parse(rescontent);
                    string successCode = jO["successCode"].ConventToString(string.Empty);
                    if (successCode == "Y")
                    {
                        XMLUser xmlUser = new XMLUser();
                        xmlUser.Id = sysUser.Id;
                        xmlUser.UserName = sysUser.User_Name;
                        xmlUser.IsInternal = 2;
                        string errorText = jO["errorText"].ConventToString(string.Empty);
                        user = new XMLResultUser() { Code = 100, Message = errorText, User = xmlUser };
                        return XmlSerializeHelper.XmlSerialize<XMLResultUser>(user);
                    }
                    else if (successCode == "N")
                    {
                        string errorText = jO["errorText"].ConventToString(string.Empty);
                        user = new XMLResultUser() { Code = 100, Message = errorText, User = null };
                        return XmlSerializeHelper.XmlSerialize<XMLResultUser>(user);
                    }

                }
                user = new XMLResultUser() { Code = -100, Message = "未知错误" };
                return XmlSerializeHelper.XmlSerialize<XMLResultUser>(user);
            }

            user = new XMLResultUser() { Code = -100, Message = "未知错误" };
            return XmlSerializeHelper.XmlSerialize<XMLResultUser>(user);

        }

    }
}
