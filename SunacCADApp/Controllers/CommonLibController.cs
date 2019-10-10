using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using SunacCADApp.Library;
using Common.Utility.Extender;

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
                string cadFileName =Path.GetFileName(file.FileName).ToUpper();
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
                string myimgpath = string.Concat("/uploader", "/", "img", "/", year, "/", date, "/", day, "/", my_image_name);

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
                bool ispath = System.IO.File.Exists(imgpath);
                if (ispath)
                {
                    return Json(new { Code = 100, Message = "CAD文件上传成功", CadPath = mycadpath, ImgPath = myimgpath,FileName=cadFileName });
                }
                else 
                {
                    return Json(new { Code = -100, Message = "CAD生成缩图生成失败"});
                }

                
            }
            catch (Exception ex) 
            {
                string exMessage = ex.Message;
                return Json(new { Code = 100, Message = ex.Message });
            }
        }

        public ActionResult DownFile(string filepath = "") 
        {
            string file = Request.QueryString["file"].ConventToString(string.Empty);
            string filePath = Server.MapPath("~/uploader/cad/2019/10/7/3308c244-1d5b-450b-9c76-bf06a163cce9.dwg");
            string fileName = Path.GetFileName(file); //客户端保存的文件名            
            
            //以字符流的形式下载文件
            FileStream fs = new FileStream(filePath, FileMode.Open);
            byte[] bytes = new byte[(int)fs.Length];
            fs.Read(bytes, 0, bytes.Length);
            fs.Close();
            Response.ContentType = "application/octet-stream";
            //通知浏览器下载文件而不是打开
            Response.AddHeader("Content-Disposition",
                "attachment;  filename=" + HttpUtility.UrlEncode(fileName, System.Text.Encoding.UTF8));
            Response.BinaryWrite(bytes);
            Response.Flush();
            Response.End();
            return RedirectToAction("ListForStore");
        }
    }
}