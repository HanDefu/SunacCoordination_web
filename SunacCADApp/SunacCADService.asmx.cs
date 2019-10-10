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

        [WebMethod(Description = "标准部品库[外窗]所有")]
        public string GetAllWindows()
        {
            string xml = string.Empty;
            try
            {
               XMLCadDrawingWindow window =  XMLCadDrawingWindowDB.GetXMLCadDrawingWindow();
               xml = XmlSerializeHelper.XmlSerialize<XMLCadDrawingWindow>(window);
            }
            catch (Exception ex) 
            {
              
            }
            return xml;
             
        }

        [WebMethod(Description = "标准部品库[外窗]按条件查询")]
        public string GetWindows(double width = 0, double height = 0, string openType = "", string openNum = "", string gongNengQu = "") 
        {
            string xml = string.Empty;
            if (width < 1)
            {
                xml = XmlSerializeHelper.XmlSerialize<XMLCadDrawingWindow>(new XMLCadDrawingWindow() { Code = -100, Message = "外窗宽度不能为空" });
            }
            else
            {
                XMLCadDrawingWindow window = XMLCadDrawingWindowDB.GetXMLCadDrawingWindow(width, height,openType,openNum,gongNengQu);
                xml = XmlSerializeHelper.XmlSerialize<XMLCadDrawingWindow>(window);
            }
            return xml;

        }
           
        [WebMethod(Description = "标准部品库[门]所有")]
        public string GetAllDoor() 
        {
            string xml = string.Empty;
            try
            {
                IList<Door> doors = XMLCadDrawingDoorDB.GetCadDrawingDoorsListByWidth(0, 0);
                var retDoor = new XMLDoor{ Code = 100, Message = "门查询成功", Doors = doors.ToArray() };
                xml = XmlSerializeHelper.XmlSerialize<XMLDoor>(retDoor);
            }
            catch (Exception ex) 
            {
                var retDoor = new XMLDoor{ Code = -100, Message = ex.Message };
                xml = XmlSerializeHelper.XmlSerialize < XMLDoor>(retDoor);
            }
            return xml;
           

        }
        [WebMethod(Description = "标准部品库[门]条件查询")]
        public string GetAllDoorByParam(double width,int doorType)
        {
            string xml = string.Empty;
            try
            {
                IList<Door> doors = XMLCadDrawingDoorDB.GetCadDrawingDoorsListByWidth(width, doorType);
                var retDoor = new XMLDoor { Code = 100, Message = "门查询成功", Doors = doors.ToArray() };
                xml = XmlSerializeHelper.XmlSerialize<XMLDoor>(retDoor);
            }
            catch (Exception ex)
            {
                var retDoor = new XMLDoor { Code = -100, Message = ex.Message };
                xml = XmlSerializeHelper.XmlSerialize<XMLDoor>(retDoor);
            }
            return xml;


        }

         [WebMethod(Description = "标准部品库[厨房原型]")]
        public string GetAllKitchen() 
        {
            string xml = string.Empty;
            try
            {

                IList<Kitchen> kitchens = XMLCadDrawingKitchenDB.GetKitchenListByWidth(0, 0, 0, 0, 0);
                var retKitchen = new XMLKitchen { Code = 100, Message = "厨房原型查询成功", Kitchens = kitchens.ToArray() };
                xml = XmlSerializeHelper.XmlSerialize<XMLKitchen>(retKitchen);
            }
            catch (Exception ex)
            {
                var retDoor = new XMLKitchen { Code = -100, Message = ex.Message };
                xml = XmlSerializeHelper.XmlSerialize<XMLKitchen>(retDoor);
            }
            return xml;
        }

         [WebMethod(Description = "标准部品库[厨房原型]条件查询")]
         public string GetAllKitchenParam(double Width, double Height, int KitchenDoorWindowPosition, int KitchenType, int AirVent)
         {
             string xml = string.Empty;
             try
             {
                 if (Width < 1) 
                 {
                     var retDoor = new XMLKitchen { Code = -100, Message = "Width必须填写"};
                     xml = XmlSerializeHelper.XmlSerialize<XMLKitchen>(retDoor);
                 }

                 IList<Kitchen> kitchens = XMLCadDrawingKitchenDB.GetKitchenListByWidth(Width, Height, KitchenDoorWindowPosition, KitchenType, AirVent);
                 var retKitchen = new XMLKitchen { Code = 100, Message = "厨房原型查询成功", Kitchens = kitchens.ToArray() };
                 xml = XmlSerializeHelper.XmlSerialize<XMLKitchen>(retKitchen);
             }
             catch (Exception ex)
             {
                 var retDoor = new XMLKitchen { Code = -100, Message = ex.Message };
                 xml = XmlSerializeHelper.XmlSerialize<XMLKitchen>(retDoor);
             }
             return xml;
         }

         [WebMethod(Description = "标准部品库[卫生间原型]")]
         public string GetAllBathroom() 
         {
             string xml = string.Empty;
             try
             {

                 IList<Bathroom> bathroomList = XMLCadDrawingBathroomDB.GetCadDrawingBathroomListByWidth(0, 0, 0, 0, 0);
                 var retBathroom = new XMLBathroom { Code = 100, Message = "卫生间查询成功", Bathrooms = bathroomList.ToArray() };
                 xml = XmlSerializeHelper.XmlSerialize<XMLBathroom>(retBathroom);
             }
             catch (Exception ex)
             {
                 var retBathroom = new XMLBathroom { Code = -100, Message = ex.Message };
                 xml = XmlSerializeHelper.XmlSerialize<XMLBathroom>(retBathroom);
             }
             return xml;

         }

         [WebMethod(Description = "标准部品库[卫生间原型]条件查询")]
         public string GetAllBathroomByParam(double Width, double Height, int BathroomDoorWindowPosition, int ToiletType, int AirVent) 
         {
             string xml = string.Empty;
             try
             {

                 IList<Bathroom> bathroomList = XMLCadDrawingBathroomDB.GetCadDrawingBathroomListByWidth(Width, Height, BathroomDoorWindowPosition, ToiletType, AirVent);
                 var retBathroom = new XMLBathroom { Code = 100, Message = "卫生间查询成功", Bathrooms = bathroomList.ToArray() };
                 xml = XmlSerializeHelper.XmlSerialize<XMLBathroom>(retBathroom);
             }
             catch (Exception ex)
             {
                 var retBathroom = new XMLBathroom { Code = -100, Message = ex.Message };
                 xml = XmlSerializeHelper.XmlSerialize<XMLBathroom>(retBathroom);
             }
             return xml;
         
         
         }


         [WebMethod(Description = "标准部品库[栏杆原型]条件查询")]
         public string GetAllHandrailByParam(int RailingType)
         {
             string xml = string.Empty;
             try
             {

                 IList<Handrail> handrailList = XMLCadDrawingHandrailDB.GetCadDrawingHandrailListByParam(RailingType);
                 var retHandrail = new XMLHandrail { Code = 100, Message = "卫生间查询成功", Handrails = handrailList.ToArray() };
                 xml = XmlSerializeHelper.XmlSerialize<XMLHandrail>(retHandrail);
             }
             catch (Exception ex)
             {
                 var retBathroom = new XMLHandrail { Code = -100, Message = ex.Message };
                 xml = XmlSerializeHelper.XmlSerialize<XMLHandrail>(retBathroom);
             }
             return xml;
         }
         [WebMethod(Description = "标准部品库[空调原型]条件查询")]
         public string GetAllAirconditionerByParam(int AirconditionerPower, int AirconditionerPipePosition, int AirconditionerIsRainpipe, int RainpipePosition)
         {
             string xml = string.Empty;
             try
             {

                 IList<Airconditioner> airconditionerList = XMLCadDrawingAirconditionerDB.GetCadDrawingAirconditionerListByParam(AirconditionerPower, AirconditionerPipePosition, AirconditionerIsRainpipe, RainpipePosition);
                 var retAirconditioner = new XMLAirconditioner { Code = 100, Message = "卫生间查询成功", Airconditioners = airconditionerList.ToArray() };
                 xml = XmlSerializeHelper.XmlSerialize<XMLAirconditioner>(retAirconditioner);
             }
             catch (Exception ex)
             {
                 var retAirconditioner = new XMLAirconditioner { Code = -100, Message = ex.Message };
                 xml = XmlSerializeHelper.XmlSerialize<XMLAirconditioner>(retAirconditioner);
             }
             return xml;
         }




        [WebMethod(Description = "标准部品库[ CAD文件下载]")]
        public string CadFileDownload(int Id) 
        {
            return XMLCadDrawingWindowDB.GetCADFileDownloadByWhere(Id);
        }



    
    }

  

}
