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
    public   class IdmCommonLibDB
    {
        ///<summary>
        /// IDM公司组织 分页查询
        ///</summary>
        public static IList<BasIdmOrganization> GetPageInfoByParameter(string _where)
        {

            IList<BasIdmOrganization> _bas_idm_organizations = new List<BasIdmOrganization>();
            string sql = string.Format(@"SELECT Id,OrgCode,OrgName,OrgTypeCode,OrgTypeDesc,UpOrgCode,
                                                                    UpOrgName,OrgDate,[Enabled] 
                                                        FROM dbo.Bas_Idm_Organization WHERE UpOrgCode='E01' AND OrgTypeCode='A' {0}", _where);
            _bas_idm_organizations = MsSqlHelperEx.ExecuteDataTable(sql).ConvertListModel<BasIdmOrganization>(new BasIdmOrganization());
            return _bas_idm_organizations;
        }


        public static IList<Bas_Idm_City> GetBasIdmCityByAreaCode(string areaCode) 
        {
            IList<Bas_Idm_City> IdmCity = new List<Bas_Idm_City>();
            string sql = string.Format(@"SELECT Id,CityCode,CityName,AreaCode,Enabled
                                                          FROM dbo.Bas_Idm_City WHERE AreaCode='{0}' ORDER BY Id DESC",areaCode);
            IdmCity = MsSqlHelperEx.ExecuteDataTable(sql).ConvertListModel<Bas_Idm_City>(new Bas_Idm_City());
            return IdmCity;
        }

        public static IList<BasIdmOrganization> GetIdmCityAndCompanyByArea(string AreaCode) 
        {
            IList<BasIdmOrganization> _bas_idm_organizations = new List<BasIdmOrganization>();
            string sql = string.Format(@"SELECT Id,OrgCode,OrgName,OrgTypeCode,OrgTypeDesc,UpOrgCode,UpOrgName 
                                                          FROM dbo.Bas_Idm_Organization 
                                                        WHERE OrgTypeCode='C' AND UpOrgCode='{0}'",AreaCode);
            _bas_idm_organizations = MsSqlHelperEx.ExecuteDataTable(sql).ConvertListModel<BasIdmOrganization>(new BasIdmOrganization());
            return _bas_idm_organizations;
        }

        public static IList<BasIdmOrganization> GetAreaCityByArea(string AreaCode)
        {
            string _where = string.Empty;
            if (!string.IsNullOrEmpty(AreaCode))
            {
                _where = string.Format(@" AND UpOrgCode='{0}'",AreaCode);
            }
            IList<BasIdmOrganization> _bas_idm_organizations = new List<BasIdmOrganization>();
            string sql = string.Format(@"SELECT Id,OrgCode,OrgName,OrgName as Name,OrgTypeCode,OrgTypeDesc,UpOrgCode,UpOrgName 
                                                          FROM dbo.Bas_Idm_Organization 
                                                        WHERE OrgTypeCode='C' {0}", _where);
            _bas_idm_organizations = MsSqlHelperEx.ExecuteDataTable(sql).ConvertListModel<BasIdmOrganization>(new BasIdmOrganization());
            return _bas_idm_organizations;
        }
    }
}
