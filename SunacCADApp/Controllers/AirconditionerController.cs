using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SunacCADApp.Entity;
using SunacCADApp.Data;
using Common.Utility;
using Common.Utility.Extender;

namespace SunacCADApp.Controllers
{
    public class AirconditionerController : Controller
    {
        // GET: Airconditioner
        public ActionResult Index()
        {
            string _where = "TypeCode='Area' And ParentID!=0";
            IList<BasArgumentSetting> Settings = BasArgumentSettingDB.GetBasArgumentSettingByWhere(_where);
            ViewBag.Settings = Settings;

            _where = "TypeCode='CondensatePipePosition' And ParentID!=0";
            IList<BasArgumentSetting> CondensatePipePositions = BasArgumentSettingDB.GetBasArgumentSettingByWhere(_where);
            ViewBag.CondensatePipePositions = CondensatePipePositions;

            _where = "TypeCode='AirConditionNumber' And ParentID!=0";
            IList<BasArgumentSetting> AirConditionNumbers = BasArgumentSettingDB.GetBasArgumentSettingByWhere(_where);
            ViewBag.AirConditionNumbers = AirConditionNumbers;
            
            _where = string.Empty;  //查询
            string _orderby = string.Empty;  //排序
            string _url = string.Empty;
            int recordCount = 0;    //记录总数
            int pageSize = 15;      //每页条数
            int currentPage = 0;    //当前页数
            int pageCount = 0;      //总页数
            int startRowNum = 0;    //开始行数
            int endRowNum = 0;      //结束行数
            currentPage = string.IsNullOrEmpty(Request.QueryString["page"]) ? 1 : Request.QueryString["page"].ConvertToInt32(0);
            startRowNum = ((currentPage - 1) * pageSize) + 1;
            endRowNum = currentPage * pageSize;

            IList<CadDrawingWindowSearch> lst = CadDrawingDoorDetailDB.GetSearchPageInfoByParameter(_where, _orderby, startRowNum, endRowNum);
            recordCount = CadDrawingDoorDetailDB.GetSearchPageCountByParameter(_where);
            pageCount = recordCount % pageSize == 0 ? recordCount / pageSize : ((recordCount / pageSize) + 1);
            ViewBag.URL = _url;
            ViewBag.List = lst;
            ViewBag.RecordCount = recordCount;
            ViewBag.CurrentPage = currentPage;
            ViewBag.PageCount = pageCount;
            return View();
        }

        /// <summary>
        /// GET:/Handrail/LookOver
        /// </summary>
        /// <returns></returns>
        public ActionResult LookOver()
        {
            string _where = "TypeCode='Area' And ParentID!=0";
            IList<BasArgumentSetting> Settings = BasArgumentSettingDB.GetBasArgumentSettingByWhere(_where);
            ViewBag.Settings = Settings;

            _where = "TypeCode='CondensatePipePosition' And ParentID!=0";
            IList<BasArgumentSetting> CondensatePipePositions = BasArgumentSettingDB.GetBasArgumentSettingByWhere(_where);
            ViewBag.CondensatePipePositions = CondensatePipePositions;

            _where = "TypeCode='AirConditionNumber' And ParentID!=0";
            IList<BasArgumentSetting> AirConditionNumbers = BasArgumentSettingDB.GetBasArgumentSettingByWhere(_where);
            ViewBag.AirConditionNumbers = AirConditionNumbers;

            _where = "TypeCode='RainPipePosition' And ParentID!=0";
            IList<BasArgumentSetting> RainPipePositions = BasArgumentSettingDB.GetBasArgumentSettingByWhere(_where);
            ViewBag.RainPipePositions = RainPipePositions;

            return View();
        }

        /// <summary>
        /// GET:/Handrail/Add
        /// </summary>
        /// <returns></returns>

        public ActionResult Add()
        {
            string _where = "TypeCode='Area' And ParentID!=0";
            IList<BasArgumentSetting> Settings = BasArgumentSettingDB.GetBasArgumentSettingByWhere(_where);
            ViewBag.Settings = Settings;

            _where = "TypeCode='CondensatePipePosition' And ParentID!=0";
            IList<BasArgumentSetting> CondensatePipePositions = BasArgumentSettingDB.GetBasArgumentSettingByWhere(_where);
            ViewBag.CondensatePipePositions = CondensatePipePositions;

            _where = "TypeCode='AirConditionNumber' And ParentID!=0";
            IList<BasArgumentSetting> AirConditionNumbers = BasArgumentSettingDB.GetBasArgumentSettingByWhere(_where);
            ViewBag.AirConditionNumbers = AirConditionNumbers;



            _where = "TypeCode='RainPipePosition' And ParentID!=0";
            IList<BasArgumentSetting> RainPipePositions = BasArgumentSettingDB.GetBasArgumentSettingByWhere(_where);
            ViewBag.RainPipePositions = RainPipePositions;
            



            return View();
        }

        /// <summary>
        ///  /Handrail/Edit
        /// </summary>
        /// <returns></returns>
        public ActionResult Edit()
        {
            string _where = "TypeCode='Area' And ParentID!=0";
            IList<BasArgumentSetting> Settings = BasArgumentSettingDB.GetBasArgumentSettingByWhere(_where);
            ViewBag.Settings = Settings;

            _where = "TypeCode='CondensatePipePosition' And ParentID!=0";
            IList<BasArgumentSetting> CondensatePipePositions = BasArgumentSettingDB.GetBasArgumentSettingByWhere(_where);
            ViewBag.CondensatePipePositions = CondensatePipePositions;

            _where = "TypeCode='AirConditionNumber' And ParentID!=0";
            IList<BasArgumentSetting> AirConditionNumbers = BasArgumentSettingDB.GetBasArgumentSettingByWhere(_where);
            ViewBag.AirConditionNumbers = AirConditionNumbers;

            _where = "TypeCode='RainPipePosition' And ParentID!=0";
            IList<BasArgumentSetting> RainPipePositions = BasArgumentSettingDB.GetBasArgumentSettingByWhere(_where);
            ViewBag.RainPipePositions = RainPipePositions;
            return View();
        }
    }
}