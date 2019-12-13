using Common.Utility;
using SunacCADApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common.Utility.Extender;
using SunacCADApp.Entity;
using SunacCADApp.Library;

namespace SunacCADApp
{
    public class MyController : Controller
    {
        protected int UserId = 0;
        protected string UserName = string.Empty;
        protected string _power_wh = string.Empty;
        protected string _power_area_where = string.Empty;
        protected bool _IsSuper = false;
        


        public MyController()
        {
            //IList<DataSourceMember> StateList = CommonLib.GetBPMStateInfo();
            IList<DataSourceMember> StateList = new List<DataSourceMember>();
            UserId = InitUtility.Instance.InitSessionHelper.Get("UserID").ConvertToInt32(0);
            UserName = InitUtility.Instance.InitSessionHelper.Get("UserName");
            int RoleId = InitUtility.Instance.InitSessionHelper.Get("RoleId").ConvertToInt32(0);
            int IsInternal = InitUtility.Instance.InitSessionHelper.Get("IsInternal").ConvertToInt32(0);
            ViewBag.SelectModel = 5;
            ViewBag.BPMWEBURL = API_Common.BPMWEBURL;
            if (IsInternal == 1)
            {
                if (RoleId == 3)
                {
                    _power_wh = string.Format(@" AND  EXISTS(SELECT 1 FROM dbo.Sys_User_Area_Relation R WHERE R.User_ID ={0} AND R.Area_ID=pa.AreaID) ", UserId);
                    _power_area_where = string.Format(@" AND  EXISTS(SELECT 1 FROM dbo.Sys_User_Area_Relation R WHERE R.User_ID ={0} AND R.Area_ID=a.Id)", UserId);
                    ViewBag.PrototypeView = CommonLib.HasPowerByModelName(RoleId, "原型查看");
                    ViewBag.PrototypeAdd = CommonLib.HasPowerByModelName(RoleId, "原型新增");
                    ViewBag.PrototypeRemove = CommonLib.HasPowerByModelName(RoleId, "原型删除");
                    ViewBag.PrototypeEdit = CommonLib.HasPowerByModelName(RoleId, "原型修改");
                    ViewBag.PrototypeApprove = 0;
                    StateList = CommonLib.GetBPMStateInfo();
                    _IsSuper = true;
                }
                else 
                {
                    _power_wh = string.Format(@" AND  EXISTS(SELECT 1 FROM dbo.Sys_User_Area_Relation R WHERE R.User_ID ={0} AND R.Area_ID=pa.AreaID) ", UserId);
                    _power_area_where = string.Format(@" AND  EXISTS(SELECT 1 FROM dbo.Sys_User_Area_Relation R WHERE R.User_ID ={0} AND R.Area_ID=a.Id)", UserId);
                    ViewBag.PrototypeView = CommonLib.HasPowerByModelName(RoleId, "原型查看");
                    ViewBag.PrototypeAdd = CommonLib.HasPowerByModelName(RoleId, "原型新增");
                    ViewBag.PrototypeRemove = CommonLib.HasPowerByModelName(RoleId, "原型删除");
                    ViewBag.PrototypeEdit = CommonLib.HasPowerByModelName(RoleId, "原型修改");
                    ViewBag.PrototypeApprove = 1;
                    StateList = CommonLib.GetBPMStateInfo();
                }
              


            }
            else if (IsInternal == 2)
            {
                _power_wh = string.Format(@" AND  EXISTS(SELECT 1 FROM dbo.Sys_User_Area_Relation R WHERE R.User_ID ={0} AND R.Area_ID=pa.AreaID) ", UserId);
                _power_area_where = string.Format(@" AND  EXISTS(SELECT 1 FROM dbo.Sys_User_Area_Relation R WHERE R.User_ID ={0} AND R.Area_ID=a.Id)", UserId);
                ViewBag.PrototypeView = CommonLib.HasPowerByModelName(RoleId, "原型查看");
                ViewBag.PrototypeAdd = 0;
                ViewBag.PrototypeRemove = 0;
                ViewBag.PrototypeEdit = 0;
                ViewBag.PrototypeApprove = 0;
                StateList.Add(new DataSourceMember { DisplayMember = "3", ValueMember = "已发布" });
            }
            ViewBag.StateList = StateList;
        }

        protected void HasUserRole(int billStatus, int userId) 
        {
            if (billStatus == 1 || billStatus == 2) 
            {
                ViewBag.PrototypeRemove = 0;
                ViewBag.PrototypeEdit = 0;
                return;
            }

            if (_IsSuper || userId == UserId) 
            {
                ViewBag.PrototypeRemove = 1;
                ViewBag.PrototypeEdit = 1;
                return;
            }
        }

    }
}