using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Common.Utility.Extender;
using Common.Utility;
using AFrame.DBUtility;
using SunacCADApp.Entity;
using System.Configuration;
namespace SunacCADApp.Data
{
    public class CommonLib
    {
       public  static string UserMd5(string str)
        {
            string cl = str;
            string pwd = "";
            MD5 md5 = MD5.Create();//实例化一个md5对像
            // 加密后是一个字节类型的数组，这里要注意编码UTF8/Unicode等的选择　
            byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(cl));
            // 通过使用循环，将字节类型的数组转换为字符串，此字符串是常规字符格式化所得
            for (int i = 0; i < s.Length; i++)
            {
                // 将得到的字符串使用十六进制类型格式。格式后的字符是小写的字母，如果使用大写（X）则格式后的字符是大写字符
                pwd = pwd + s[i].ToString("X");
            }
            return pwd;
        }

       public static IList<DataSourceMember> GetWindowArgument(string StateFixFlag = "WindowArgument") 
       {
           IList<DataSourceMember> members = new List<DataSourceMember>();
           string sql=string.Format(@"SELECT StateId AS ValueMember,StateName AS DisplayMember
                                                  FROM dbo.Sys_State WHERE StateFixFlag='{0}' AND Enabled=1", StateFixFlag);
           members = MsSqlHelperEx.ExecuteDataTable(sql).ConvertListModel<DataSourceMember>(new DataSourceMember());
           return members;
       }

       public static IList<PageNum> GetPageNum()
       {
           IList<PageNum> pages = new List<PageNum>();
           PageNum page = new PageNum { Code = 10, Text = "10 条/页" };
           pages.Add(page);
           page = new PageNum { Code = 20, Text = "20 条/页" };
           pages.Add(page);
           page = new PageNum { Code = 40, Text = "40 条/页" };
           pages.Add(page);
           page = new PageNum { Code = 100, Text = "100 条/页" };
           pages.Add(page);
           return pages;

       }

       public static int[] PageHelper(int pageCount, int currentPage, int showPage = 5) 
       {
           int[] page = new int[2];
           int _show=(int)(showPage / 2);
           int _current = (int)Math.Ceiling(currentPage / 2.0);
           if (pageCount >= showPage)
           {
               if (currentPage <showPage)
               {
                   page[0] = 1;
                   page[1] = showPage;
               }
               else 
               {
                   if (pageCount - showPage >= currentPage)
                   {
                       page[0] = currentPage - _show;
                       page[1] = currentPage + _show;
                   }
                   else 
                   {
                       page[0] = pageCount-showPage-1;
                       page[1] = pageCount;
                   }
               }
           }
           else
           {
               page[0] = 1;
               page[1] = pageCount;
           }


           return page;
       }

       public static IList<DataSourceMember> GetBPMStateInfo() 
       {
           IList<DataSourceMember> memberList = new List<DataSourceMember>();
           memberList.Add(new DataSourceMember { DisplayMember = "0", ValueMember = "全部" });
           memberList.Add(new DataSourceMember { DisplayMember = "1", ValueMember = "待发布" });
           memberList.Add(new DataSourceMember { DisplayMember = "2", ValueMember = "审批中" });
           memberList.Add(new DataSourceMember { DisplayMember = "3", ValueMember = "已发布" });
           return memberList;
       }

       public static string WebURL 
       {
           get 
           {
               string URL=string.Empty;
               URL = System.Configuration.ConfigurationSettings.AppSettings["WebURL"].ConvertToTrim();
               return URL;
           }
       }
        
    }
}
