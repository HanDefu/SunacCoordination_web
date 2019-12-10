using Common.Utility;
using SunacCADApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common.Utility.Extender;
using SunacCADApp.Entity;

namespace SunacCADApp
{
    public class MyController : Controller
    {
        protected int UserId = 0;
        protected string UserName = string.Empty;
        protected string _power_wh = string.Empty;

        public MyController() 
        {
            IList<DataSourceMember> StateList = CommonLib.GetBPMStateInfo();
            UserId = InitUtility.Instance.InitSessionHelper.Get("UserID").ConvertToInt32(0);
            UserName = InitUtility.Instance.InitSessionHelper.Get("UserName");
            int RoleId = InitUtility.Instance.InitSessionHelper.Get("RoleId").ConvertToInt32(0);
            int IsInternal = InitUtility.Instance.InitSessionHelper.Get("IsInternal").ConvertToInt32(0);
            ViewBag.SelectModel = 5;
 
            if (IsInternal == 1)
            {
                ViewBag.PrototypeView = CommonLib.HasPowerByModelName(RoleId, "原型查看");
                ViewBag.PrototypeAdd = CommonLib.HasPowerByModelName(RoleId, "原型新增");
                ViewBag.PrototypeRemove = CommonLib.HasPowerByModelName(RoleId, "原型删除");
                ViewBag.PrototypeEdit = CommonLib.HasPowerByModelName(RoleId, "原型修改");
                StateList.Add(new DataSourceMember() { ValueMember = "草稿", DisplayMember = "0" });
                StateList.Add(new DataSourceMember() { ValueMember = "待发布", DisplayMember = "1" });
                StateList.Add(new DataSourceMember() { ValueMember = "退回", DisplayMember = "4" });
                StateList.Add(new DataSourceMember() { ValueMember = "拒绝", DisplayMember = "5" });
                StateList.Add(new DataSourceMember() { ValueMember = "作废", DisplayMember = "6" });

            }
            else
            {
                _power_wh = string.Format(@" AND  EXISTS(SELECT 1 FROM dbo.Sys_User_Area_Relation R WHERE R.User_ID ={0} AND R.Area_ID=pa.AreaID) ", UserId);
                ViewBag.PrototypeView = CommonLib.HasPowerByModelName(RoleId, "原型查看");
                ViewBag.PrototypeAdd = 0;
                ViewBag.PrototypeRemove = 0;
                ViewBag.PrototypeEdit = 0;
            }
            if (RoleId == 3) 
            {
                ViewBag.PrototypeView = CommonLib.HasPowerByModelName(RoleId, "原型查看");
                ViewBag.PrototypeAdd = CommonLib.HasPowerByModelName(RoleId, "原型新增");
                ViewBag.PrototypeRemove = CommonLib.HasPowerByModelName(RoleId, "原型删除");
                ViewBag.PrototypeEdit =1;
            }

            ViewBag.StateList = StateList;
        }
    }
}