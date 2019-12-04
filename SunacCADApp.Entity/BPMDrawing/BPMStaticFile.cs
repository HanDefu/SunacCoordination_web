using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunacCADApp.Entity
{
    [Serializable]
    public  class BPMStaticFile
    {
        /// <summary>
        /// BPM文件提交格式
        /// </summary>
        public int FILENUMBER { get; set; }

        public string FILENAME { get; set; }

        public string DESCRIPTION { get; set; }

        public int FILESIZE { get; set; }

        public string URL { get; set; }

        public string FILE_ID_DMS { get; set; }
    }
}
