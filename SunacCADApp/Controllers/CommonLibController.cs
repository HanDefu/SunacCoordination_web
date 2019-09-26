using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using SunacCADApp.Library;


namespace SunacCADApp.Controllers
{
    public class CommonLibController : Controller
    {
        // GET: CommonLib
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        ///  /CommonLib/UpFile
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public ActionResult UpFile() 
        {
            try
            {
                HttpPostedFileBase file = Request.Files["File"];
                if (file == null)
                {
                    return Json(new { Code = -110, Message = "上传文件不能为空" });
                }

                string fileName = file.FileName.ToLower();
                string extFileName = fileName.Substring(file.FileName.Length - 4);
                if (extFileName != ".dwg")
                {
                    return Json(new { Code = -110, Message = "文件格式异常" });
                }
                string uploader = Server.MapPath("~/uploader");
                string year = DateTime.Now.Year.ToString();
                string date = DateTime.Now.Month.ToString();
                string day = DateTime.Now.Day.ToString();

                string cadpath = Path.Combine(uploader, "cad", year, date, day);
                string imgpath = Path.Combine(uploader, "img", year, date, day);
                string guid = Guid.NewGuid().ToString();
                string newFileName = guid + extFileName;
                string my_image_name = guid + ".jpg";
                string mycadpath = string.Concat("/uploader", "/cad", "/", year, "/", date, "/", day, "/", newFileName);
                string myimgpaht = string.Concat("/uploader", "/", "img", "/", year, "/", date, "/", day, "/", my_image_name);

                if (!Directory.Exists(cadpath))
                {
                    Directory.CreateDirectory(cadpath);
                }

                if (!Directory.Exists(imgpath))
                {
                    Directory.CreateDirectory(imgpath);
                }
                cadpath = Path.Combine(cadpath, newFileName);
                file.SaveAs(cadpath);
                imgpath = Path.Combine(imgpath, my_image_name);
                ViewDWGHelper viewDwg = new ViewDWGHelper();
                viewDwg.GetDwgImage(cadpath, imgpath);

                return Json(new { Code = 100, Message = "文件上传成功", CadPath = mycadpath, ImgPath = myimgpaht });
            }
            catch (Exception ex) 
            {
                string exMessage = ex.Message;
                return Json(new { Code = 100, Message = ex.Message });
            }
        }
    }
}