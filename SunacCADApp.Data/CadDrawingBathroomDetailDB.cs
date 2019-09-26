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
    ///  卫生间CAD原型属性表
    ///</summary>
    public class CadDrawingBathroomDetailDB
    {
        ///<summary>
        /// 卫生间CAD原型属性表 分页查询
        ///</summary>
        public static IList<CadDrawingBathroomDetail> GetPageInfoByParameter(string _where, string orderby, int start, int end)
        {

            IList<CadDrawingBathroomDetail> _caddrawingbathroomdetails = new List<CadDrawingBathroomDetail>();
            string sql = string.Format(@"SELECT  * FROM 
                                                   ( SELECT   ( ROW_NUMBER() OVER ( ORDER BY a.id DESC ) ) AS RowNumber , *
                                                      FROM    dbo.CadDrawingBathroomDetail  a
                                                      WHERE   {0}
                                                    ) T
                                                   WHERE    T.RowNumber BETWEEN {1} AND {2}  ORDER BY T.Reorder DESC,T.CreateOn DESC {3}", _where, start, end, orderby);

            _caddrawingbathroomdetails = MsSqlHelperEx.ExecuteDataTable(sql).ConvertListModel<CadDrawingBathroomDetail>(new CadDrawingBathroomDetail());
            return _caddrawingbathroomdetails;
        }

        ///<summary>
        /// 卫生间CAD原型属性表  分页数据总数量
        ///<summary>
        public static int GetPageCountByParameter(string _where)
        {
            string sql = string.Format(@"SELECT COUNT(*) AS RowNum  FROM dbo.CadDrawingBathroomDetail WHERE 1=1 AND {0}", _where);
            return MsSqlHelperEx.ExecuteScalar(sql).ConvertToInt32(0);
        }

        ///<summary>
        /// 卫生间CAD原型属性表 根据ID查询
        ///</summary>
        public static CadDrawingBathroomDetail GetSingleEntityById(int id)
        {

            CadDrawingBathroomDetail caddrawingbathroomdetail = new CadDrawingBathroomDetail();
            string sql = string.Format("select top 1 *  from dbo.CadDrawingBathroomDetail where id={0}", id);
            return MsSqlHelperEx.ExecuteDataTable(sql).ConverToModel<CadDrawingBathroomDetail>(new CadDrawingBathroomDetail());
        }
        ///<summary>
        /// 卫生间CAD原型属性表 查询根据条件
        ///</summary>
        public static CadDrawingBathroomDetail GetSingleEntityByparam(string param)
        {

            CadDrawingBathroomDetail caddrawingbathroomdetail = new CadDrawingBathroomDetail();
            string sql = string.Format("select top 1 *  from dbo.CadDrawingBathroomDetail where 1=1 {0}", param);
            return MsSqlHelperEx.ExecuteDataTable(sql).ConverToModel<CadDrawingBathroomDetail>(new CadDrawingBathroomDetail());

        }

        ///<summary>
        /// 卫生间CAD原型属性表-添加方法
        ///</summary>

        public static int AddHandle(CadDrawingBathroomDetail caddrawingbathroomdetail)
        {


            string sql = string.Format(@"INSERT INTO dbo.caddrawingbathroomdetail(MId,BathroomShortSideMin,BathroomShortSideMax,BathroomLongSizeMin,BathroomLongSizeMax,BathroomCabinetSize,BathroomClosestoolSize,
                                     Enabled ,Reorder ,CreateOn ,CreateUserId ,CreateBy)  
                                     VALUES ('{0}',{1},{2},{3},{4},{5},{6},{7},{8},getdate(),{9},'{10}')", caddrawingbathroomdetail.MId, caddrawingbathroomdetail.BathroomShortSideMin, caddrawingbathroomdetail.BathroomShortSideMax, caddrawingbathroomdetail.BathroomLongSizeMin, caddrawingbathroomdetail.BathroomLongSizeMax, caddrawingbathroomdetail.BathroomCabinetSize, caddrawingbathroomdetail.BathroomClosestoolSize, caddrawingbathroomdetail.Enabled, caddrawingbathroomdetail.Reorder, caddrawingbathroomdetail.CreateUserId, caddrawingbathroomdetail.CreateBy);
            return MsSqlHelperEx.Execute(sql);
        }
        ///<summary>
        /// 卫生间CAD原型属性表-修改方法
        ///</summary>

        public static int EditHandle(CadDrawingBathroomDetail caddrawingbathroomdetail, string editparam)
        {


            string _wh = string.IsNullOrEmpty(editparam) ? " and id=" + caddrawingbathroomdetail.Id : editparam;
            string sql = "UPDATE [dbo].[CadDrawingBathroomDetail] SET [MId]='" + caddrawingbathroomdetail.MId + "',[BathroomShortSideMin]=" + caddrawingbathroomdetail.BathroomShortSideMin + ",[BathroomShortSideMax]=" + caddrawingbathroomdetail.BathroomShortSideMax + ",[BathroomLongSizeMin]=" + caddrawingbathroomdetail.BathroomLongSizeMin + ",[BathroomLongSizeMax]=" + caddrawingbathroomdetail.BathroomLongSizeMax + ",[BathroomCabinetSize]=" + caddrawingbathroomdetail.BathroomCabinetSize + ",[BathroomClosestoolSize]=" + caddrawingbathroomdetail.BathroomClosestoolSize + ",[Enabled]=" + caddrawingbathroomdetail.Enabled + ",[Reorder]=" + caddrawingbathroomdetail.Reorder + "  where 1=1 " + _wh;
            return MsSqlHelperEx.Execute(sql);
        }

        ///<summary>
        /// 卫生间CAD原型属性表-根据ID删除
        ///</summary>
        public static int DeleteHandleById(int Id)
        {
            string sql = string.Format("DELETE FROM dbo.CadDrawingBathroomDetail WHERE Id={0}", Id);
            return MsSqlHelperEx.Execute(sql);
        }

        ///<summary>
        /// 卫生间CAD原型属性表-删除多个ID
        ///</summary>  
        public static int DeleteHandleByIds(string Ids)
        {
            string sql = string.Format("DELETE FROM dbo.CadDrawingBathroomDetail WHERE Id in ({0})", Ids);
            return MsSqlHelperEx.Execute(sql);
        }
        ///<summary>
        /// 卫生间CAD原型属性表-根据条件删除
        ///</summary> 
        public static int DeleteHandleByParam(string param)
        {
            string sql = string.Format("DELETE FROM dbo.CadDrawingBathroomDetail WHERE {0} ", param);
            return MsSqlHelperEx.Execute(sql);
        }

    }
}
