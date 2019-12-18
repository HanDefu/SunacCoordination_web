using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SunacCADApp.Entity;
using SunacCADApp.Entity.IDM;
using SunacCADApp.Data;
using SunacCADApp.Library;
using System.Xml;
using System.Xml.Linq;
using AFrame.DBUtility;


namespace SunacCADApp
{
    public class IdmPublicService
    {

        public static int ReaderIDMPublic()
        {

            
            string beginDate = DateTime.Now.AddDays(-3).ToString("yyyy-mm-dd");
            string endDate = DateTime.Now.ToString("yyyy-mm-dd");

            WebService.IDM.Public.Header head = new WebService.IDM.Public.Header();
            head.ACCOUNT = "idmadmin";
            head.PASSWORD = "idmpass";
            head.BIZTRANSACTIONID = "vsheji";
            WebService.IDM.Public.queryDto dto = new WebService.IDM.Public.queryDto();
            dto.beginDate = string.Format(@"{0} 00:00:00.000",beginDate);
            dto.endDate = string.Format(@"{0} 00:00:00.000", endDate);
            dto.systemID = "CADSJXTOrg";
            dto.pageNo = "1";
            dto.pageRowNo = "100";
            string list;
            WebService.IDM.Public.PUBLIC_SUNAC_301_queryIdmOrgData_pttbindingQSService service = new WebService.IDM.Public.PUBLIC_SUNAC_301_queryIdmOrgData_pttbindingQSService();
            service.commonHeader = head;
            service.PUBLIC_SUNAC_301_queryIdmOrgData(dto, out  list);
            string loginfo = string.Empty;
            Sys_Operate_Log log = new Sys_Operate_Log
            {
                SysTypeCode = 10,
                SysTypeName = string.Format("QueryIdmOrgData接口获取 beginDate={0},endDate={1}",beginDate,endDate),
                LogInfo = list,
                CreateBy = " 系统获取",
            };
            SysOperateLogDB.AddHandle(log);
            int returnVal = GetOrgPublicList(list);
            return returnVal;
        }

        public static int GetOrgPublicList(string xml)
        {
            try
            {
                XElement xElement = XElement.Parse(xml, LoadOptions.None);
                var xEles = xElement.Elements("ORG");
                if (xEles == null)
                    return 0;
                foreach (XElement xele in xEles)
                {
                    string OrganName = xele.Element("OrganName").Value;
                    string OrganNumber = xele.Element("OrganNumber").Value;
                    string OrganParentNo = xele.Element("OrganParentNo").Value;
                    string OrganStatus = xele.Element("OrganStatus").Value;
                    AddIdmOrg(OrganName, OrganNumber, OrganParentNo, OrganStatus);

                }
                return 1;
            }
            catch
            {
                return 0;
            }

        }


        public static int AddIdmOrg(string organName, string organNumber, string OrganParentNo, string OrganStatus)
        {
            string sql = string.Format(@"DELETE FROM dbo.Bas_Idm_Organ WHERE OrganNumber='{1}';
                                                        INSERT INTO dbo.Bas_Idm_Organ
                                                        (OrganName, OrganNumber,OrganParentNo,OrganStatus,Enabled,Reorder,CreateOn, ModifiedOn )
                                                         VALUES 
                                                        ( N'{0}',N'{1}',N'{2}',N'{3}',1,0,GETDATE(),GETDATE())", organName, organNumber, OrganParentNo, OrganStatus);
            return MsSqlHelperEx.Execute(sql);
        }


        public static int ReaderIDMUser()
        {

            string beginDate = string.Concat(DateTime.Now.AddDays(-2).ToString("yyyy-MM-dd"), " 00:00:00.000");
            string endDate = string.Concat(DateTime.Now.AddDays(1).ToString("yyyy-MM-dd"), " 00:00:00.000");
            WebService.Idm.User.Header header = new WebService.Idm.User.Header();
            header.ACCOUNT = "idmadmin";
            header.PASSWORD = "idmpass";
            header.BIZTRANSACTIONID = "vsheji";
            WebService.Idm.User.queryDto dto = new WebService.Idm.User.queryDto();
            dto.beginDate = beginDate;
            dto.endDate = endDate;
            dto.systemID = "CADSJXTUser";
            dto.pageNo = "1";
            dto.pageRowNo = "40";
            string list = "";
            WebService.Idm.User.PUBLIC_SUNAC_300_queryIdmUserData_pttbindingQSService service = new WebService.Idm.User.PUBLIC_SUNAC_300_queryIdmUserData_pttbindingQSService();
            service.commonHeader = header;
            WebService.Idm.User.HEADER head = service.PUBLIC_SUNAC_300_queryIdmUserData(dto, out list);
            string loginfo = string.Empty;
            Sys_Operate_Log log = new Sys_Operate_Log
            {
                SysTypeCode = 10,
                SysTypeName = string.Format("QueryIdmUserData接口获取 beginDate={0},endDate={1}", beginDate, endDate),
                LogInfo = list,
                CreateBy = " 系统获取",
            };
            SysOperateLogDB.AddHandle(log);
            return AddSysUser(list);
        }

        public static int AddSysUser(string xml)
        {
            try
            {
                XElement xElement = XElement.Parse(xml, LoadOptions.None);
                var xEles = xElement.Elements("USER");
                if (xEles == null)
                    return 0;
                foreach (XElement xele in xEles)
                {
                    string UserLogin = xele.Element("UserLogin").Value;
                    string Username = xele.Element("Username").Value;
                    string UserEmpNo = xele.Element("UserEmpNo").Value;
                    string UserEmployeeID = xele.Element("UserEmployeeID").Value;
                    string Email = xele.Element("Email").Value;
                    string Mobile = xele.Element("Mobile").Value;
                    string UserDeptNo = xele.Element("UserDeptNo").Value;
                    string UserPositionID = xele.Element("UserPositionID").Value;
                    string UserSex = xele.Element("UserSex").Value;
                    string UserOrgDisplayName = xele.Element("UserOrgDisplayName").Value;
                    string UserStatus = xele.Element("UserStatus").Value;
      
                    int userId = Sys_UserDB.GetUserIdByLoginName(UserLogin);
                    Sys_User user = new Sys_User { User_Name = UserLogin, True_Name = Username, Email = Email, Telephone = Mobile, Is_Used = "0", Is_Internal = 1,CreateOn=DateTime.Now,ModifiedOn=DateTime.Now,Id=userId};
                    if (userId == 0)
                    {
                        user.Used_Begin_DateTime = DateTime.Now.AddDays(-7);
                        user.Used_End_DateTime = DateTime.Now.AddYears(5);
                        Sys_UserDB.AddHandle(user);
                    }
                    else
                    {
                        Sys_User _user = Sys_UserDB.GetSingleEntityById(userId);
                        user.Used_Begin_DateTime = _user.Used_Begin_DateTime;
                        user.Used_End_DateTime = _user.Used_End_DateTime;
                        user.CreateOn = DateTime.Now;
                        user.Is_Used = _user.Is_Used;
                        user.Is_Internal = _user.Is_Internal;
                        Sys_UserDB.EditHandle(user,string.Empty);
                    }
                }
                return 1;
            }
            catch
            {
                return 0;
            }

        }




    }
}