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
    ///  门窗原型尺寸表
    ///</summary>
    public class CadDrawingParameterDB
    {
        ///<summary>
        /// 门窗原型尺寸表 分页查询
        ///</summary>
        public static IList<CadDrawingParameter> GetPageInfoByParameter(string _where, string orderby, int start, int end)
        {

            IList<CadDrawingParameter> _caddrawingparameters = new List<CadDrawingParameter>();
            string sql = string.Format(@"SELECT  * FROM 
                                                   ( SELECT   ( ROW_NUMBER() OVER ( ORDER BY a.id DESC ) ) AS RowNumber , *
                                                      FROM    dbo.CadDrawingParameter  a
                                                      WHERE   {0}
                                                    ) T
                                                   WHERE    T.RowNumber BETWEEN {1} AND {2}  ORDER BY T.Reorder DESC,T.CreateOn DESC {3}", _where, start, end, orderby);

            _caddrawingparameters = MsSqlHelperEx.ExecuteDataTable(sql).ConvertListModel<CadDrawingParameter>(new CadDrawingParameter());
            return _caddrawingparameters;
        }

        ///<summary>
        /// 门窗原型尺寸表  分页数据总数量
        ///<summary>
        public static int GetPageCountByParameter(string _where)
        {
            string sql = string.Format(@"SELECT COUNT(*) AS RowNum  FROM dbo.CadDrawingParameter WHERE 1=1 AND {0}", _where);
            return MsSqlHelperEx.ExecuteScalar(sql).ConvertToInt32(0);
        }

        ///<summary>
        /// 门窗原型尺寸表 根据ID查询
        ///</summary>
        public static CadDrawingParameter GetSingleEntityById(int id)
        {

            CadDrawingParameter caddrawingparameter = new CadDrawingParameter();
            string sql = string.Format("select top 1 *  from dbo.CadDrawingParameter where id={0}", id);
            return MsSqlHelperEx.ExecuteDataTable(sql).ConverToModel<CadDrawingParameter>(new CadDrawingParameter());
        }
        ///<summary>
        /// 门窗原型尺寸表 查询根据条件
        ///</summary>
        public static CadDrawingParameter GetSingleEntityByparam(string param)
        {

            CadDrawingParameter caddrawingparameter = new CadDrawingParameter();
            string sql = string.Format("select top 1 *  from dbo.CadDrawingParameter where 1=1 {0}", param);
            return MsSqlHelperEx.ExecuteDataTable(sql).ConverToModel<CadDrawingParameter>(new CadDrawingParameter());

        }

        ///<summary>
        /// 门窗原型尺寸表-添加方法
        ///</summary>

        public static int AddHandle(CadDrawingParameter caddrawingparameter)
        {


            string sql = string.Format(@"INSERT INTO dbo.caddrawingparameter(MId,SizeNo,ValueType,Val,MinValue,MaxValue,DefaultValue,[Desc],
                                     Enabled ,Reorder ,CreateOn ,CreateUserId ,CreateBy)  
                                     VALUES ({0},'{1}',{2},'{3}',{4},{5},{6},'{7}',{8},{9},getdate(),{10},'{11}')", caddrawingparameter.MId, caddrawingparameter.SizeNo, caddrawingparameter.ValueType, caddrawingparameter.Val, caddrawingparameter.MinValue, caddrawingparameter.MaxValue, caddrawingparameter.DefaultValue, caddrawingparameter.Desc, caddrawingparameter.Enabled, caddrawingparameter.Reorder, caddrawingparameter.CreateUserId, caddrawingparameter.CreateBy);
            return MsSqlHelperEx.Execute(sql);
        }
        ///<summary>
        /// 门窗原型尺寸表-修改方法
        ///</summary>

        public static int EditHandle(CadDrawingParameter caddrawingparameter, string editparam)
        {


            string _wh = string.IsNullOrEmpty(editparam) ? " and id=" + caddrawingparameter.Id : editparam;
            string sql = "UPDATE [dbo].[CadDrawingParameter] SET [MId]=" + caddrawingparameter.MId + ",[SizeNo]='" + caddrawingparameter.SizeNo + "',[ValueType]=" + caddrawingparameter.ValueType + ",[Val]='" + caddrawingparameter.Val + "',[MinValue]=" + caddrawingparameter.MinValue + ",[MaxValue]=" + caddrawingparameter.MaxValue + ",[DefaultValue]=" + caddrawingparameter.DefaultValue + ",[Desc]='" + caddrawingparameter.Desc + "',[Enabled]=" + caddrawingparameter.Enabled + ",[Reorder]=" + caddrawingparameter.Reorder + "  where 1=1 " + _wh;
            return MsSqlHelperEx.Execute(sql);
        }

        ///<summary>
        /// 门窗原型尺寸表-根据ID删除
        ///</summary>
        public static int DeleteHandleById(int Id)
        {
            string sql = string.Format("DELETE FROM dbo.CadDrawingParameter WHERE Id={0}", Id);
            return MsSqlHelperEx.Execute(sql);
        }

        ///<summary>
        /// 门窗原型尺寸表-删除多个ID
        ///</summary>  
        public static int DeleteHandleByIds(string Ids)
        {
            string sql = string.Format("DELETE FROM dbo.CadDrawingParameter WHERE Id in ({0})", Ids);
            return MsSqlHelperEx.Execute(sql);
        }
        ///<summary>
        /// 门窗原型尺寸表-根据条件删除
        ///</summary> 
        public static int DeleteHandleByParam(string param)
        {
            string sql = string.Format("DELETE FROM dbo.CadDrawingParameter WHERE {0} ", param);
            return MsSqlHelperEx.Execute(sql);
        }

        public static IList<CadDrawingParameter> GetCadDrawingParameterByWhereList(string _where) 
        {
            IList<CadDrawingParameter> _caddrawingparameters = new List<CadDrawingParameter>();
            string sql = string.Format(@"SELECT a.*,b.StateName AS ValueTypeName 
                                                        FROM dbo.CadDrawingParameter  a 
                                                INNER JOIN dbo.Sys_State b ON a.ValueType=b.StateId AND b.StateFixFlag='WindowArgument'
                                                WHERE {0}",_where);
            _caddrawingparameters = MsSqlHelperEx.ExecuteDataTable(sql).ConvertListModel<CadDrawingParameter>(new CadDrawingParameter());
            return _caddrawingparameters;
        }

    }
}