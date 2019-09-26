using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using SunacCADApp.Entity;
using SunacCADApp.Data;
using SunacCADApp.Library;


namespace SunacCADApp
{
    /// <summary>
    /// ArgumentSettingService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class ArgumentSettingService : System.Web.Services.WebService
    {

        [WebMethod(Description="标准模块属性；1、标准部品库：外窗、门、栏杆;2、厨房、卫生间、空调")]
        public string StandardDesignAttribute(string AtrributeName)
        {
            XMLArgumentSetting setting =null;
            try
            {
                string where = string.Format(" and b.TypeCode='{0}'", AtrributeName);
                IList<ArgumentSetting> list = BasArgumentSettingDB.GetXMLBasArgumentSettingByWhere(where);
                if (list != null)
                {
                    setting = new XMLArgumentSetting()
                    {
                        Code = 100,
                        Message = "数据返回成功",
                        ArgumentSettings = list.ToArray<ArgumentSetting>()
                    };
                }
                else
                {
                    setting = new XMLArgumentSetting()
                    {
                        Code = -100,
                        Message = "数据返回失败"
                    };
                }
            }
            catch (Exception ex)
            {
                setting = new XMLArgumentSetting()
                {
                    Code = -110,
                    Message = ex.Message
                };
            }
            string xml=XmlSerializeHelper.XmlSerialize<XMLArgumentSetting>(setting);
            return xml;
        }
    }
}
