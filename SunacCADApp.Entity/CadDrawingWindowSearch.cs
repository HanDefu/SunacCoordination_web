
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace SunacCADApp.Entity
{

    ///外窗原型查询
    [Serializable]
    public class CadDrawingWindowSearch
    {
        /// <summary>
        /// 主键
        ///</summary>
        ///
        public int Id { get; set; }
        /// <summary>
        /// 外窗编号
        /// </summary>
        public string DrawingCode { get; set; }
        /// <summary>
        /// 外窗名称
        /// </summary>
        public string DrawingName { get; set; }
        /// <summary>
        /// 外窗图片
        /// </summary>
        public string DWGPath { get; set; }

        /// <summary>
        /// 单据状态
        /// </summary>
        public int BillStatus { get; set; }

    }
}