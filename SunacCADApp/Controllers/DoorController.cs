﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common.Utility;
using Common.Utility.Extender;
using SunacCADApp.Entity;
using SunacCADApp.Data;

namespace SunacCADApp.Controllers
{
    public class DoorController : Controller
    {
        // GET: Door
        public ActionResult Index()
        {
            string _where = "TypeCode='Area' And ParentID!=0";
            IList<BasArgumentSetting> Settings = BasArgumentSettingDB.GetBasArgumentSettingByWhere(_where);
            ViewBag.Settings = Settings;

            _where = "TypeCode='DoorType' And ParentID!=0";
            IList<BasArgumentSetting> DoorTypes = BasArgumentSettingDB.GetBasArgumentSettingByWhere(_where);
            ViewBag.DoorTypes = DoorTypes;

          
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
        /// GET:/Door/LookOver
        /// </summary>
        /// <returns></returns>
        public ActionResult LookOver()
        {
            return View();
        }

        public ActionResult Add()
        {
            string _where = "TypeCode='Area' And ParentID!=0";
            IList<BasArgumentSetting> Settings = BasArgumentSettingDB.GetBasArgumentSettingByWhere(_where);
            ViewBag.Settings = Settings;

            _where = "TypeCode='DoorType' And ParentID!=0";
            IList<BasArgumentSetting> DoorTypes = BasArgumentSettingDB.GetBasArgumentSettingByWhere(_where);
            ViewBag.DoorTypes = DoorTypes;

           
            return View();
        }

        /// <summary>
        ///  /door/edit
        /// </summary>
        /// <returns></returns>
        public ActionResult Edit()
        {
            string _where = "TypeCode='Area' And ParentID!=0";
            IList<BasArgumentSetting> Settings = BasArgumentSettingDB.GetBasArgumentSettingByWhere(_where);
            ViewBag.Settings = Settings;

            _where = "TypeCode='DoorType' And ParentID!=0";
            IList<BasArgumentSetting> DoorTypes = BasArgumentSettingDB.GetBasArgumentSettingByWhere(_where);
            ViewBag.DoorTypes = DoorTypes;
            return View();
        }


    }
}