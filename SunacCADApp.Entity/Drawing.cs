using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunacCADApp.Entity
{
    public   class Drawing
    {
        /// <summary>
        /// 图CAD文件ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 图纸图片地址
        /// </summary>
        public string ImgPath { get; set;}
        /// <summary>
        /// CAD图纸地址
        /// </summary>
        public string CADPath { get; set; }
    }
}
