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
    ///  CAD原型图纸信息表
    ///</summary>
    public class CadDrawingDWGDB
    {
        ///<summary>
        /// CAD原型图纸信息表 分页查询
        ///</summary>
        public static IList<CadDrawingDWG> GetPageInfoByParameter(string _where, string orderby, int start, int end)
        {

            IList<CadDrawingDWG> _caddrawingdwgs = new List<CadDrawingDWG>();
            string sql = string.Format(@"SELECT  * FROM 
                                                   ( SELECT   ( ROW_NUMBER() OVER ( ORDER BY a.id DESC ) ) AS RowNumber , *
                                                      FROM    dbo.CadDrawingDWG  a
                                                      WHERE   {0}
                                                    ) T
                                                   WHERE    T.RowNumber BETWEEN {1} AND {2}  ORDER BY T.Reorder DESC,T.CreateOn DESC {3}", _where, start, end, orderby);

            _caddrawingdwgs = MsSqlHelperEx.ExecuteDataTable(sql).ConvertListModel<CadDrawingDWG>(new CadDrawingDWG());
            return _caddrawingdwgs;
        }

        ///<summary>
        /// CAD原型图纸信息表  分页数据总数量
        ///<summary>
        public static int GetPageCountByParameter(string _where)
        {
            string sql = string.Format(@"SELECT COUNT(*) AS RowNum  FROM dbo.CadDrawingDWG WHERE 1=1 AND {0}", _where);
            return MsSqlHelperEx.ExecuteScalar(sql).ConvertToInt32(0);
        }

        ///<summary>
        /// CAD原型图纸信息表 根据ID查询
        ///</summary>
        public static CadDrawingDWG GetSingleEntityById(int id)
        {

            CadDrawingDWG caddrawingdwg = new CadDrawingDWG();
            string sql = string.Format("select top 1 *  from dbo.CadDrawingDWG where id={0}", id);
            return MsSqlHelperEx.ExecuteDataTable(sql).ConverToModel<CadDrawingDWG>(new CadDrawingDWG());
        }
        ///<summary>
        /// CAD原型图纸信息表 查询根据条件
        ///</summary>
        public static CadDrawingDWG GetSingleEntityByparam(string param)
        {

            CadDrawingDWG caddrawingdwg = new CadDrawingDWG();
            string sql = string.Format("select top 1 *  from dbo.CadDrawingDWG where 1=1 {0}", param);
            return MsSqlHelperEx.ExecuteDataTable(sql).ConverToModel<CadDrawingDWG>(new CadDrawingDWG());

        }

        ///<summary>
        /// CAD原型图纸信息表-添加方法
        ///</summary>

        public static int AddHandle(CadDrawingDWG caddrawingdwg)
        {


            string sql = string.Format(@"INSERT INTO dbo.caddrawingdwg(MId,DWGPath,FileClass,CADPath,
                                     Enabled ,Reorder ,CreateOn ,CreateUserId ,CreateBy)  
                                     VALUES ({0},'{1}','{2}','{3}',{4},{5},getdate(),{6},'{7}')", caddrawingdwg.MId, caddrawingdwg.DWGPath, caddrawingdwg.FileClass, caddrawingdwg.CADPath, caddrawingdwg.Enabled, caddrawingdwg.Reorder, caddrawingdwg.CreateUserId, caddrawingdwg.CreateBy);
            return MsSqlHelperEx.Execute(sql);
        }
        ///<summary>
        /// CAD原型图纸信息表-修改方法
        ///</summary>

        public static int EditHandle(CadDrawingDWG caddrawingdwg, string editparam)
        {


            string _wh = string.IsNullOrEmpty(editparam) ? " and id=" + caddrawingdwg.Id : editparam;
            string sql = "UPDATE [dbo].[CadDrawingDWG] SET [MId]=" + caddrawingdwg.MId + ",[DWGPath]='" + caddrawingdwg.DWGPath + "',[FileClass]='" + caddrawingdwg.FileClass + "',[CADPath]='" + caddrawingdwg.CADPath + "',[Enabled]=" + caddrawingdwg.Enabled + ",[Reorder]=" + caddrawingdwg.Reorder + "  where 1=1 " + _wh;
            return MsSqlHelperEx.Execute(sql);
        }

        ///<summary>
        /// CAD原型图纸信息表-根据ID删除
        ///</summary>
        public static int DeleteHandleById(int Id)
        {
            string sql = string.Format("DELETE FROM dbo.CadDrawingDWG WHERE Id={0}", Id);
            return MsSqlHelperEx.Execute(sql);
        }

        ///<summary>
        /// CAD原型图纸信息表-删除多个ID
        ///</summary>  
        public static int DeleteHandleByIds(string Ids)
        {
            string sql = string.Format("DELETE FROM dbo.CadDrawingDWG WHERE Id in ({0})", Ids);
            return MsSqlHelperEx.Execute(sql);
        }
        ///<summary>
        /// CAD原型图纸信息表-根据条件删除
        ///</summary> 
        public static int DeleteHandleByParam(string param)
        {
            string sql = string.Format("DELETE FROM dbo.CadDrawingDWG WHERE {0} ", param);
            return MsSqlHelperEx.Execute(sql);
        }

        public static IList<Drawing> GetDrawingByWhere(string where) 
        {
            IList<Drawing> _drawing = new  List<Drawing>();
            string sql = string.Format(@"SELECT Id,DWGPath AS ImgPath,CADPath FROM dbo.CadDrawingDWG WHERE {0}",where);
            _drawing = MsSqlHelperEx.ExecuteDataTable(sql).ConvertListModel<Drawing>(new Drawing());
            return _drawing;
        }
        public static Drawing GetDrawingSingeByWhere(string where)
        {
            Drawing _drawing = new Drawing();
            string sql = string.Format(@"SELECT  ISNULL(Id,0) AS  Id ,ISNULL(DWGPath,'') AS ImgPath, ISNULL(CADPath,'') AS CADPath FROM dbo.CadDrawingDWG WHERE {0}", where);
            _drawing = MsSqlHelperEx.ExecuteDataTable(sql).ConverToModel<Drawing>(new Drawing());
            return _drawing;
        }



    }
}