using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunacCADApp.Entity
{
    public  class Bpm_Req_BaseInfo
    {
        /// <summary>
        /// 32位唯一系统请求号， 非空
        /// </summary>
        public string REQ_TRACE_ID { get; set; }
        /// <summary>
        /// 发送时间 非空、前8位表示日期后 6位表示时间 20180808080808
        /// </summary>
        public string REQ_SEND_TIME { get; set; }
        /// <summary>
        /// 发送方系统标识（发起端）
        /// </summary>
        public string REQ_SRC_SYS { get; set; }
        /// <summary>
        /// 接收方系统标识（接收端）
        /// </summary>
        public string REQ_TAR_SYS { get; set; }
        /// <summary>
        /// 服务名称， 必须按照PO 或ESB命名规范， 有PO 或ESB顾问提供
        /// </summary>
        public string REQ_SERVER_NAME { get; set; }
        /// <summary>
        /// 是否为异步请求 1
        /// </summary>
        public int REQ_SYN_FLAG { get; set; }
        /// <summary>
        /// 业务唯一编号， 非必填
        /// </summary>
        public string REQ_BSN_ID { get; set; }
        /// <summary>
        /// 重试次数， 非必填
        /// </summary>
        public int REQ_RETRY_TIMES { get; set; }
        /// <summary>
        /// 是否为重复请求报文
        /// </summary>
        public int REQ_REPEAT_FLAG { get; set; }
        /// <summary>
        /// 是否为重复请求报文， 单位秒， 非必填
        /// </summary>
        public int REQ_REPEAT_CYCLE { get; set; }
        /// <summary>
        /// ESB事物ID，通过ESB集成平台的为必填
        /// </summary>
        public string BIZTRANSACTIONID { get; set; }
        /// <summary>
        /// 业务数据条目数，非必填
        /// </summary>
        public int COUNT { get; set; }



    }
}
