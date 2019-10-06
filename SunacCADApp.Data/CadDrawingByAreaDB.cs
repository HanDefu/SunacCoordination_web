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
    ///  CAD原型使用区域
    ///</summary>
    public class CadDrawingByAreaDB
    {
        ///<summary>
        /// CAD原型使用区域 分页查询
        ///</summary>
        public static IList<CadDrawingByArea> GetPageInfoByParameter(string _where, string orderby, int start, int end)
        {

            IList<CadDrawingByArea> _caddrawingbyareas = new List<CadDrawingByArea>();
            string sql = string.Format(@"SELECT  * FROM 
                                                   ( SELECT   ( ROW_NUMBER() OVER ( ORDER BY a.id DESC ) ) AS RowNumber , *
                                                      FROM    dbo.CadDrawingByArea  a
                                                      WHERE   {0}
                                                    ) T
                                                   WHERE    T.RowNumber BETWEEN {1} AND {2}  ORDER BY T.Reorder DESC,T.CreateOn DESC {3}", _where, start, end, orderby);

            _caddrawingbyareas = MsSqlHelperEx.ExecuteDataTable(sql).ConvertListModel<CadDrawingByArea>(new CadDrawingByArea());
            return _caddrawingbyareas;
        }

        ///<summary>
        /// CAD原型使用区域  分页数据总数量
        ///<summary>
        public static int GetPageCountByParameter(string _where)
        {
            string sql = string.Format(@"SELECT COUNT(*) AS RowNum  FROM dbo.CadDrawingByArea WHERE 1=1 AND {0}", _where);
            return MsSqlHelperEx.ExecuteScalar(sql).ConvertToInt32(0);
        }

        ///<summary>
        /// CAD原型使用区域 根据ID查询
        ///</summary>
        public static CadDrawingByArea GetSingleEntityById(int id)
        {

            CadDrawingByArea caddrawingbyarea = new CadDrawingByArea();
            string sql = string.Format("select top 1 *  from dbo.CadDrawingByArea where id={0}", id);
            return MsSqlHelperEx.ExecuteDataTable(sql).ConverToModel<CadDrawingByArea>(new CadDrawingByArea());
        }
        ///<summary>
        /// CAD原型使用区域 查询根据条件
        ///</summary>
        public static CadDrawingByArea GetSingleEntityByparam(string param)
        {

            CadDrawingByArea caddrawingbyarea = new CadDrawingByArea();
            string sql = string.Format("select top 1 *  from dbo.CadDrawingByArea where 1=1 {0}", param);
            return MsSqlHelperEx.ExecuteDataTable(sql).ConverToModel<CadDrawingByArea>(new CadDrawingByArea());

        }

        ///<summary>
        /// CAD原型使用区域-添加方法
        ///</summary>

        public static int AddHandle(CadDrawingByArea caddrawingbyarea)
        {


            string sql = string.Format(@"INSERT INTO dbo.caddrawingbyarea(MId,AreaID,
                                     Enabled ,Reorder ,CreateOn ,CreateUserId ,CreateBy)  
                                     VALUES ({0},{1},{2},{3},getdate(),{4},'{5}')", caddrawingbyarea.MId, caddrawingbyarea.AreaID, caddrawingbyarea.Enabled, caddrawingbyarea.Reorder, caddrawingbyarea.CreateUserId, caddrawingbyarea.CreateBy);
            return MsSqlHelperEx.Execute(sql);
        }
        ///<summary>
        /// CAD原型使用区域-修改方法
        ///</summary>

        public static int EditHandle(CadDrawingByArea caddrawingbyarea, string editparam)
        {


            string _wh = string.IsNullOrEmpty(editparam) ? " and id=" + caddrawingbyarea.Id : editparam;
            string sql = "UPDATE [dbo].[CadDrawingByArea] SET [MId]=" + caddrawingbyarea.MId + ",[AreaID]=" + caddrawingbyarea.AreaID + ",[Enabled]=" + caddrawingbyarea.Enabled + ",[Reorder]=" + caddrawingbyarea.Reorder + "  where 1=1 " + _wh;
            return MsSqlHelperEx.Execute(sql);
        }

        ///<summary>
        /// CAD原型使用区域-根据ID删除
        ///</summary>
        public static int DeleteHandleById(int Id)
        {
            string sql = string.Format("DELETE FROM dbo.CadDrawingByArea WHERE Id={0}", Id);
            return MsSqlHelperEx.Execute(sql);
        }

        ///<summary>
        /// CAD原型使用区域-删除多个ID
        ///</summary>  
        public static int DeleteHandleByIds(string Ids)
        {
            string sql = string.Format("DELETE FROM dbo.CadDrawingByArea WHERE Id in ({0})", Ids);
            return MsSqlHelperEx.Execute(sql);
        }
        ///<summary>
        /// CAD原型使用区域-根据条件删除
        ///</summary> 
        public static int DeleteHandleByParam(string param)
        {
            string sql = string.Format("DELETE FROM dbo.CadDrawingByArea WHERE {0} ", param);
            return MsSqlHelperEx.Execute(sql);
        }

        public static IList<CadDrawingByArea> GetCadDrawingByAreasByWhere(string _wh) 
        {
            string sql = string.Format(@"SELECT a.AreaID,a.Id,b.ArgumentText AS AreaName ,a.MId  FROM dbo.CadDrawingByArea a
                             INNER JOIN dbo.BasArgumentSetting b ON a.AreaID=b.Id WHERE  {0} ORDER BY a.Id ASC", _wh);
            return MsSqlHelperEx.ExecuteDataTable(sql).ConvertListModel<CadDrawingByArea>(new CadDrawingByArea());
        }

        public static IList<Area> GetAreaByWhere(string _wh) 
        {
            IList<Area> area = new List<Area>();
            string sql = string.Format(@"SELECT a.AreaID,a.Id,b.ArgumentText AS AreaName  FROM dbo.CadDrawingByArea a
                             INNER JOIN dbo.BasArgumentSetting b ON a.AreaID=b.Id WHERE  {0} ORDER BY a.Id ASC", _wh);
            return MsSqlHelperEx.ExecuteDataTable(sql).ConvertListModel<Area>(new Area());
        }

    }
}