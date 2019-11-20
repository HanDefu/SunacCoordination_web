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
using SunacCADApp.Entity;
using SunacCADApp.Data;

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
            string fileName = Request.QueryString["filename"].ConventToString(string.Empty);
            fileName=string.Format(@"{0}.DWG",fileName.ToUpper());
            string filePath = Server.MapPath(file);
            string _fileName = string.IsNullOrEmpty(fileName) ? Path.GetFileName(file) : fileName; //客户端保存的文件名            
            
            //以字符流的形式下载文件
            FileStream fs = new FileStream(filePath, FileMode.Open);
            byte[] bytes = new byte[(int)fs.Length];
            fs.Read(bytes, 0, bytes.Length);
            fs.Close();
            Response.ContentType = "application/octet-stream";
            //通知浏览器下载文件而不是打开
            Response.AddHeader("Content-Disposition",
                "attachment;  filename=" + HttpUtility.UrlEncode(_fileName, System.Text.Encoding.UTF8));
            Response.BinaryWrite(bytes);
            Response.Flush();
            Response.End();
            return RedirectToAction("ListForStore");
        }

       /// <summary>
        ///  /CommonLib/HasDradrawingCode
       /// </summary>
       /// <returns></returns>

        public ActionResult HasDradrawingCode() 
        {
            string drawingCode = Request.Form["code"];
            string code = CadDrawingMasterDB.HasDrawingCode(drawingCode);
            if (string.IsNullOrEmpty(code))
            {
                return Json(new { Code = 100, Message =drawingCode+"系统中不存在" });
            }
            else 
            {
                return Json(new { Code = -100, Message = drawingCode + "系统中已存在,请更换" });
            }
        }

        /// <summary>
        /// /commonlib/downloadfile
        /// </summary>
        /// <returns></returns>
        public ActionResult DownloadFile() 
        {

            try 
            {
                string pathFile = string.Format(@"\\192.168.7.209\CAD\ProjectFiles\Window_NC2_0.dwg");
                FileStream fs = new FileStream(pathFile, FileMode.Open);
                byte[] bytes = new byte[(int)fs.Length];
                fs.Read(bytes, 0, bytes.Length);
                fs.Close();
                Response.ContentType = "application/octet-stream";
                //通知浏览器下载文件而不是打开
                Response.AddHeader("Content-Disposition","attachment;  filename=" + HttpUtility.UrlEncode("20190101.dwg", System.Text.Encoding.UTF8));
                Response.BinaryWrite(bytes);
                Response.Flush();
                Response.End();
                return RedirectToAction("ListForStore");
               
            }catch(Exception ex)
            {
                return Json(new { Code = ex.Message }, JsonRequestBehavior.AllowGet);
            }
           

            //Response.ContentType = "application/octet-stream";
            ////通知浏览器下载文件而不是打开
            //Response.AddHeader("Content-Disposition",
            //    "attachment;  filename=" + HttpUtility.UrlEncode("20190101.dwg", System.Text.Encoding.UTF8));
            //Response.BinaryWrite(bytes);
            //Response.Flush();
            //Response.End();
            //return RedirectToAction("ListForStore");
        }
    }
}