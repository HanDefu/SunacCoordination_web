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
    ///  门CAD原型属性表
    ///</summary>
    public class CadDrawingDoorDetailDB
    {
        ///<summary>
        /// 门CAD原型属性表 分页查询
        ///</summary>
        public static IList<CadDrawingDoorDetail> GetPageInfoByParameter(string _where, string orderby, int start, int end)
        {

            IList<CadDrawingDoorDetail> _caddrawingdoordetails = new List<CadDrawingDoorDetail>();
            string sql = string.Format(@"SELECT  * FROM 
                                                   ( SELECT   ( ROW_NUMBER() OVER ( ORDER BY a.id DESC ) ) AS RowNumber , *
                                                      FROM    dbo.CadDrawingDoorDetail  a
                                                      WHERE   {0}
                                                    ) T
                                                   WHERE    T.RowNumber BETWEEN {1} AND {2}  ORDER BY T.Reorder DESC,T.CreateOn DESC {3}", _where, start, end, orderby);

            _caddrawingdoordetails = MsSqlHelperEx.ExecuteDataTable(sql).ConvertListModel<CadDrawingDoorDetail>(new CadDrawingDoorDetail());
            return _caddrawingdoordetails;
        }

        ///<summary>
        /// 门CAD原型属性表  分页数据总数量
        ///<summary>
        public static int GetPageCountByParameter(string _where)
        {
            string sql = string.Format(@"SELECT COUNT(*) AS RowNum  FROM dbo.CadDrawingDoorDetail WHERE 1=1 AND {0}", _where);
            return MsSqlHelperEx.ExecuteScalar(sql).ConvertToInt32(0);
        }

        ///<summary>
        /// 门CAD原型属性表 根据ID查询
        ///</summary>
        public static CadDrawingDoorDetail GetSingleEntityById(int id)
        {

            CadDrawingDoorDetail caddrawingdoordetail = new CadDrawingDoorDetail();
            string sql = string.Format("select top 1 *  from dbo.CadDrawingDoorDetail where id={0}", id);
            return MsSqlHelperEx.ExecuteDataTable(sql).ConverToModel<CadDrawingDoorDetail>(new CadDrawingDoorDetail());
        }
        ///<summary>
        /// 门CAD原型属性表 查询根据条件
        ///</summary>
        public static CadDrawingDoorDetail GetSingleEntityByparam(string param)
        {

            CadDrawingDoorDetail caddrawingdoordetail = new CadDrawingDoorDetail();
            string sql = string.Format("select top 1 *  from dbo.CadDrawingDoorDetail where 1=1 {0}", param);
            return MsSqlHelperEx.ExecuteDataTable(sql).ConverToModel<CadDrawingDoorDetail>(new CadDrawingDoorDetail());

        }

        ///<summary>
        /// 门CAD原型属性表-添加方法
        ///</summary>

        public static int AddHandle(CadDrawingDoorDetail caddrawingdoordetail)
        {


            string sql = string.Format(@"INSERT INTO dbo.caddrawingdoordetail(MId,DoorType,WindowSizeMin,WindowSizeMax,
                                     Enabled ,Reorder ,CreateOn ,CreateUserId ,CreateBy,ModifiedOn,ModifiedUserId,ModifiedBy)
                                     VALUES ({0},{1},{2},{3},{4},{5},getdate(),{6},'{7}',getdate(),{8},'{9}')", caddrawingdoordetail.MId, caddrawingdoordetail.DoorType, caddrawingdoordetail.WindowSizeMin, caddrawingdoordetail.WindowSizeMax, caddrawingdoordetail.Enabled, caddrawingdoordetail.Reorder, caddrawingdoordetail.CreateUserId, caddrawingdoordetail.CreateBy, caddrawingdoordetail.ModifiedUserId, caddrawingdoordetail.ModifiedBy);
            return MsSqlHelperEx.Execute(sql);
        }
        ///<summary>
        /// 门CAD原型属性表-修改方法
        ///</summary>

        public static int EditHandle(CadDrawingDoorDetail caddrawingdoordetail, string editparam)
        {


            string _wh = string.IsNullOrEmpty(editparam) ? " and id=" + caddrawingdoordetail.Id : editparam;
            string sql = "UPDATE [dbo].[CadDrawingDoorDetail] SET [MId]=" + caddrawingdoordetail.MId + ",[DoorType]=" + caddrawingdoordetail.DoorType + ",[WindowSizeMin]=" + caddrawingdoordetail.WindowSizeMin + ",[WindowSizeMax]=" + caddrawingdoordetail.WindowSizeMax + ",[Enabled]=" + caddrawingdoordetail.Enabled + ",[Reorder]=" + caddrawingdoordetail.Reorder + ",[ModifiedUserId]=" + caddrawingdoordetail.ModifiedUserId + ",[ModifiedBy]='" + caddrawingdoordetail.ModifiedBy + "'  where 1=1 " + _wh;
            return MsSqlHelperEx.Execute(sql);
        }

        ///<summary>
        /// 门CAD原型属性表-根据ID删除
        ///</summary>
        public static int DeleteHandleById(int Id)
        {
            string sql = string.Format("DELETE FROM dbo.CadDrawingDoorDetail WHERE Id={0}", Id);
            return MsSqlHelperEx.Execute(sql);
        }

        ///<summary>
        /// 门CAD原型属性表-删除多个ID
        ///</summary>  
        public static int DeleteHandleByIds(string Ids)
        {
            string sql = string.Format("DELETE FROM dbo.CadDrawingDoorDetail WHERE Id in ({0})", Ids);
            return MsSqlHelperEx.Execute(sql);
        }
        ///<summary>
        /// 门CAD原型属性表-根据条件删除
        ///</summary> 
        public static int DeleteHandleByParam(string param)
        {
            string sql = string.Format("DELETE FROM dbo.CadDrawingDoorDetail WHERE {0} ", param);
            return MsSqlHelperEx.Execute(sql);
        }


        public static IList<CadDrawingWindowSearch> GetSearchPageInfoByParameter(string _where, string orderby, int start, int end)
        {

            IList<CadDrawingWindowSearch> _caddrawingwindowsearchs = new List<CadDrawingWindowSearch>();
            string sql = string.Format(@"SELECT  * FROM 
                                                   (     SELECT   ( ROW_NUMBER() OVER ( ORDER BY a.id DESC ) ) AS RowNumber, a.Id,
                                                                       a.DrawingCode,a.DrawingName,c.DWGPath,a.Reorder,a.CreateOn 
                                                             FROM dbo.CaddrawingMaster a 
                                                    INNER JOIN dbo.CadDrawingDoorDetail b ON a.Id=b.MId
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
                                                        INNER JOIN   dbo.CadDrawingDoorDetail b ON a.Id=b.MId
                                                           LEFT JOIN   (SELECT  Id  MId,DWGPath,FileClass FROM dbo.CadDrawingDWG  WHERE  FileClass='JPG') c ON c.MId = a.Id
                                                               WHERE  1=1  {0}", _where);
            return MsSqlHelperEx.ExecuteScalar(sql).ConvertToInt32(0);
        }


    }
}