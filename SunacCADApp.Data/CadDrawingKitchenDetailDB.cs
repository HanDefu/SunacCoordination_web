﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Utility.Extender;
using Common.Utility;
using AFrame.DBUtility;
using SunacCADApp.Entity;
namespace SunacCADApp.Data
{

    /// <summary>
    ///  厨房CAD原型属性表
    ///</summary>
    public class CadDrawingKitchenDetailDB
    {
        ///<summary>
        /// 厨房CAD原型属性表 分页查询
        ///</summary>
        public static IList<CadDrawingKitchenDetail> GetPageInfoByParameter(string _where, string orderby, int start, int end)
        {

            IList<CadDrawingKitchenDetail> _caddrawingkitchendetails = new List<CadDrawingKitchenDetail>();
            string sql = string.Format(@"SELECT  * FROM 
                                                   ( SELECT   ( ROW_NUMBER() OVER ( ORDER BY a.id DESC ) ) AS RowNumber , *
                                                      FROM    dbo.CadDrawingKitchenDetail  a
                                                      WHERE   {0}
                                                    ) T
                                                   WHERE    T.RowNumber BETWEEN {1} AND {2}  ORDER BY T.Reorder DESC,T.CreateOn DESC {3}", _where, start, end, orderby);

            _caddrawingkitchendetails = MsSqlHelperEx.ExecuteDataTable(sql).ConvertListModel<CadDrawingKitchenDetail>(new CadDrawingKitchenDetail());
            return _caddrawingkitchendetails;
        }

        ///<summary>
        /// 厨房CAD原型属性表  分页数据总数量
        ///<summary>
        public static int GetPageCountByParameter(string _where)
        {
            string sql = string.Format(@"SELECT COUNT(*) AS RowNum  FROM dbo.CadDrawingKitchenDetail WHERE 1=1 AND {0}", _where);
            return MsSqlHelperEx.ExecuteScalar(sql).ConvertToInt32(0);
        }

        ///<summary>
        /// 厨房CAD原型属性表 根据ID查询
        ///</summary>
        public static CadDrawingKitchenDetail GetSingleEntityById(int id)
        {

            CadDrawingKitchenDetail caddrawingkitchendetail = new CadDrawingKitchenDetail();
            string sql = string.Format("select top 1 *  from dbo.CadDrawingKitchenDetail where id={0}", id);
            return MsSqlHelperEx.ExecuteDataTable(sql).ConverToModel<CadDrawingKitchenDetail>(new CadDrawingKitchenDetail());
        }
        ///<summary>
        /// 厨房CAD原型属性表 查询根据条件
        ///</summary>
        public static CadDrawingKitchenDetail GetSingleEntityByparam(string param)
        {

            CadDrawingKitchenDetail caddrawingkitchendetail = new CadDrawingKitchenDetail();
            string sql = string.Format("select top 1 *  from dbo.CadDrawingKitchenDetail where 1=1 {0}", param);
            return MsSqlHelperEx.ExecuteDataTable(sql).ConverToModel<CadDrawingKitchenDetail>(new CadDrawingKitchenDetail());

        }

        ///<summary>
        /// 厨房CAD原型属性表-添加方法
        ///</summary>

        public static int AddHandle(CadDrawingKitchenDetail caddrawingkitchendetail)
        {


            string sql = string.Format(@"INSERT INTO dbo.caddrawingkitchendetail(MId,KitchenType,KitchenPosition,KitchenIsAirduct,KitchenOpenSizeMin,KitchenOpenSizeMax,KitchenDepthsizeMin,KitchenDepthsizeMax,KitchenBasinSize,KitchenFridgSize,KitchenHearthSize,
                                     Enabled ,Reorder ,CreateOn ,CreateUserId ,CreateBy,ModifiedOn,ModifiedUserId,ModifiedBy)
                                     VALUES ({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},getdate(),{13},'{14}',getdate(),{15},'{16}')", caddrawingkitchendetail.MId, caddrawingkitchendetail.KitchenType, caddrawingkitchendetail.KitchenPosition, caddrawingkitchendetail.KitchenIsAirduct, caddrawingkitchendetail.KitchenOpenSizeMin, caddrawingkitchendetail.KitchenOpenSizeMax, caddrawingkitchendetail.KitchenDepthsizeMin, caddrawingkitchendetail.KitchenDepthsizeMax, caddrawingkitchendetail.KitchenBasinSize, caddrawingkitchendetail.KitchenFridgSize, caddrawingkitchendetail.KitchenHearthSize, caddrawingkitchendetail.Enabled, caddrawingkitchendetail.Reorder, caddrawingkitchendetail.CreateUserId, caddrawingkitchendetail.CreateBy, caddrawingkitchendetail.ModifiedUserId, caddrawingkitchendetail.ModifiedBy);
            return MsSqlHelperEx.Execute(sql);
        }
        ///<summary>
        /// 厨房CAD原型属性表-修改方法
        ///</summary>

        public static int EditHandle(CadDrawingKitchenDetail caddrawingkitchendetail, string editparam)
        {


            string _wh = string.IsNullOrEmpty(editparam) ? " and id=" + caddrawingkitchendetail.Id : editparam;
            string sql = "UPDATE [dbo].[CadDrawingKitchenDetail] SET [MId]=" + caddrawingkitchendetail.MId + ",[KitchenType]=" + caddrawingkitchendetail.KitchenType + ",[KitchenPosition]=" + caddrawingkitchendetail.KitchenPosition + ",[KitchenIsAirduct]=" + caddrawingkitchendetail.KitchenIsAirduct + ",[KitchenOpenSizeMin]=" + caddrawingkitchendetail.KitchenOpenSizeMin + ",[KitchenOpenSizeMax]=" + caddrawingkitchendetail.KitchenOpenSizeMax + ",[KitchenDepthsizeMin]=" + caddrawingkitchendetail.KitchenDepthsizeMin + ",[KitchenDepthsizeMax]=" + caddrawingkitchendetail.KitchenDepthsizeMax + ",[KitchenBasinSize]=" + caddrawingkitchendetail.KitchenBasinSize + ",[KitchenFridgSize]=" + caddrawingkitchendetail.KitchenFridgSize + ",[KitchenHearthSize]=" + caddrawingkitchendetail.KitchenHearthSize + ",[Enabled]=" + caddrawingkitchendetail.Enabled + ",[Reorder]=" + caddrawingkitchendetail.Reorder + ",[ModifiedUserId]=" + caddrawingkitchendetail.ModifiedUserId + ",[ModifiedBy]='" + caddrawingkitchendetail.ModifiedBy + "'  where 1=1 " + _wh;
            return MsSqlHelperEx.Execute(sql);
        }

        ///<summary>
        /// 厨房CAD原型属性表-根据ID删除
        ///</summary>
        public static int DeleteHandleById(int Id)
        {
            string sql = string.Format("DELETE FROM dbo.CadDrawingKitchenDetail WHERE Id={0}", Id);
            return MsSqlHelperEx.Execute(sql);
        }

        ///<summary>
        /// 厨房CAD原型属性表-删除多个ID
        ///</summary>  
        public static int DeleteHandleByIds(string Ids)
        {
            string sql = string.Format("DELETE FROM dbo.CadDrawingKitchenDetail WHERE Id in ({0})", Ids);
            return MsSqlHelperEx.Execute(sql);
        }
        ///<summary>
        /// 厨房CAD原型属性表-根据条件删除
        ///</summary> 
        public static int DeleteHandleByParam(string param)
        {
            string sql = string.Format("DELETE FROM dbo.CadDrawingKitchenDetail WHERE {0} ", param);
            return MsSqlHelperEx.Execute(sql);
        }

        public static IList<CadDrawingWindowSearch> GetSearchPageInfoByParameter(string _where, string orderby, int start, int end)
        {

            IList<CadDrawingWindowSearch> _caddrawingwindowsearchs = new List<CadDrawingWindowSearch>();
            string sql = string.Format(@"SELECT  * FROM 
                                                   (     SELECT   ( ROW_NUMBER() OVER ( ORDER BY a.id DESC ) ) AS RowNumber, a.Id,
                                                                       a.DrawingCode,a.DrawingName,c.DWGPath,a.Reorder,a.CreateOn 
                                                             FROM dbo.CaddrawingMaster a 
                                                    INNER JOIN dbo.CadDrawingKitchenDetail b ON a.Id=b.MId
                                                      LEFT JOIN (SELECT  Id  MId,DWGPath,FileClass FROM dbo.CadDrawingDWG  WHERE  FileClass='JPG') c ON c.MId = a.Id
                                                      WHERE 1=1  {0}
                                                    ) T
                                                   WHERE    T.RowNumber BETWEEN {1} AND {2}  ORDER BY T.Reorder DESC,T.CreateOn DESC {3}", _where, start, end, orderby);

            _caddrawingwindowsearchs = MsSqlHelperEx.ExecuteDataTable(sql).ConvertListModel<CadDrawingWindowSearch>(new CadDrawingWindowSearch());
            return _caddrawingwindowsearchs;
        }

        ///<summary>
        /// 外窗原型查询  分页数据总数量
        ///<summary>
        public static int GetSearchPageCountByParameter(string _where)
        {
            string sql = string.Format(@"      SELECT   COUNT(*) AS CNT  FROM dbo.CaddrawingMaster a 
                                                        INNER JOIN   dbo.CadDrawingKitchenDetail b ON a.Id=b.MId
                                                           LEFT JOIN   (SELECT  Id  MId,DWGPath,FileClass FROM dbo.CadDrawingDWG  WHERE  FileClass='JPG') c ON c.MId = a.Id
                                                               WHERE  1=1  {0}", _where);
            return MsSqlHelperEx.ExecuteScalar(sql).ConvertToInt32(0);
        }

    }
}
