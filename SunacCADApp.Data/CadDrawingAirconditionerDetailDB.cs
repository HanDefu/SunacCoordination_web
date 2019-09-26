using System;
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
    ///  空调CAD原型属性表
    ///</summary>
    public class CadDrawingAirconditionerDetailDB
    {
        ///<summary>
        /// 空调CAD原型属性表 分页查询
        ///</summary>
        public static IList<CadDrawingAirconditionerDetail> GetPageInfoByParameter(string _where, string orderby, int start, int end)
        {

            IList<CadDrawingAirconditionerDetail> _caddrawingairconditionerdetails = new List<CadDrawingAirconditionerDetail>();
            string sql = string.Format(@"SELECT  * FROM 
                                                   ( SELECT   ( ROW_NUMBER() OVER ( ORDER BY a.id DESC ) ) AS RowNumber , *
                                                      FROM    dbo.CadDrawingAirconditionerDetail  a
                                                      WHERE   {0}
                                                    ) T
                                                   WHERE    T.RowNumber BETWEEN {1} AND {2}  ORDER BY T.Reorder DESC,T.CreateOn DESC {3}", _where, start, end, orderby);

            _caddrawingairconditionerdetails = MsSqlHelperEx.ExecuteDataTable(sql).ConvertListModel<CadDrawingAirconditionerDetail>(new CadDrawingAirconditionerDetail());
            return _caddrawingairconditionerdetails;
        }

        ///<summary>
        /// 空调CAD原型属性表  分页数据总数量
        ///<summary>
        public static int GetPageCountByParameter(string _where)
        {
            string sql = string.Format(@"SELECT COUNT(*) AS RowNum  FROM dbo.CadDrawingAirconditionerDetail WHERE 1=1 AND {0}", _where);
            return MsSqlHelperEx.ExecuteScalar(sql).ConvertToInt32(0);
        }

        ///<summary>
        /// 空调CAD原型属性表 根据ID查询
        ///</summary>
        public static CadDrawingAirconditionerDetail GetSingleEntityById(int id)
        {

            CadDrawingAirconditionerDetail caddrawingairconditionerdetail = new CadDrawingAirconditionerDetail();
            string sql = string.Format("select top 1 *  from dbo.CadDrawingAirconditionerDetail where id={0}", id);
            return MsSqlHelperEx.ExecuteDataTable(sql).ConverToModel<CadDrawingAirconditionerDetail>(new CadDrawingAirconditionerDetail());
        }
        ///<summary>
        /// 空调CAD原型属性表 查询根据条件
        ///</summary>
        public static CadDrawingAirconditionerDetail GetSingleEntityByparam(string param)
        {

            CadDrawingAirconditionerDetail caddrawingairconditionerdetail = new CadDrawingAirconditionerDetail();
            string sql = string.Format("select top 1 *  from dbo.CadDrawingAirconditionerDetail where 1=1 {0}", param);
            return MsSqlHelperEx.ExecuteDataTable(sql).ConverToModel<CadDrawingAirconditionerDetail>(new CadDrawingAirconditionerDetail());

        }

        ///<summary>
        /// 空调CAD原型属性表-添加方法
        ///</summary>

        public static int AddHandle(CadDrawingAirconditionerDetail caddrawingairconditionerdetail)
        {


            string sql = string.Format(@"INSERT INTO dbo.caddrawingairconditionerdetail(MId,AirconditionerPower,AirconditionerMinWidth,AirconditionerMinLength,AirconditionerPipePosition,AirconditionerIsRainPipe,
                                     Enabled ,Reorder ,CreateOn ,CreateUserId ,CreateBy,ModifiedOn,ModifiedUserId,ModifiedBy)
                                     VALUES ({0},{1},{2},{3},{4},{5},{6},{7},getdate(),{8},'{9}',getdate(),{10},'{11}')", caddrawingairconditionerdetail.MId, caddrawingairconditionerdetail.AirconditionerPower, caddrawingairconditionerdetail.AirconditionerMinWidth, caddrawingairconditionerdetail.AirconditionerMinLength, caddrawingairconditionerdetail.AirconditionerPipePosition, caddrawingairconditionerdetail.AirconditionerIsRainPipe, caddrawingairconditionerdetail.Enabled, caddrawingairconditionerdetail.Reorder, caddrawingairconditionerdetail.CreateUserId, caddrawingairconditionerdetail.CreateBy, caddrawingairconditionerdetail.ModifiedUserId, caddrawingairconditionerdetail.ModifiedBy);
            return MsSqlHelperEx.Execute(sql);
        }
        ///<summary>
        /// 空调CAD原型属性表-修改方法
        ///</summary>

        public static int EditHandle(CadDrawingAirconditionerDetail caddrawingairconditionerdetail, string editparam)
        {


            string _wh = string.IsNullOrEmpty(editparam) ? " and id=" + caddrawingairconditionerdetail.Id : editparam;
            string sql = "UPDATE [dbo].[CadDrawingAirconditionerDetail] SET [MId]=" + caddrawingairconditionerdetail.MId + ",[AirconditionerPower]=" + caddrawingairconditionerdetail.AirconditionerPower + ",[AirconditionerMinWidth]=" + caddrawingairconditionerdetail.AirconditionerMinWidth + ",[AirconditionerMinLength]=" + caddrawingairconditionerdetail.AirconditionerMinLength + ",[AirconditionerPipePosition]=" + caddrawingairconditionerdetail.AirconditionerPipePosition + ",[AirconditionerIsRainPipe]=" + caddrawingairconditionerdetail.AirconditionerIsRainPipe + ",[Enabled]=" + caddrawingairconditionerdetail.Enabled + ",[Reorder]=" + caddrawingairconditionerdetail.Reorder + ",[ModifiedUserId]=" + caddrawingairconditionerdetail.ModifiedUserId + ",[ModifiedBy]='" + caddrawingairconditionerdetail.ModifiedBy + "'  where 1=1 " + _wh;
            return MsSqlHelperEx.Execute(sql);
        }

        ///<summary>
        /// 空调CAD原型属性表-根据ID删除
        ///</summary>
        public static int DeleteHandleById(int Id)
        {
            string sql = string.Format("DELETE FROM dbo.CadDrawingAirconditionerDetail WHERE Id={0}", Id);
            return MsSqlHelperEx.Execute(sql);
        }

        ///<summary>
        /// 空调CAD原型属性表-删除多个ID
        ///</summary>  
        public static int DeleteHandleByIds(string Ids)
        {
            string sql = string.Format("DELETE FROM dbo.CadDrawingAirconditionerDetail WHERE Id in ({0})", Ids);
            return MsSqlHelperEx.Execute(sql);
        }
        ///<summary>
        /// 空调CAD原型属性表-根据条件删除
        ///</summary> 
        public static int DeleteHandleByParam(string param)
        {
            string sql = string.Format("DELETE FROM dbo.CadDrawingAirconditionerDetail WHERE {0} ", param);
            return MsSqlHelperEx.Execute(sql);
        }

    }
}
