using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GPCT_Coin.Models
{
    public class ResultModel
    {
        /// <summary>
        /// 是否通过
        /// </summary>
        public bool Result { get; set; }
        /// <summary>
        /// 消息
        /// </summary>
        public string Msg { get; set; }
        /// <summary>
        /// 附加数据
        /// </summary>
        public object Data { get; set; }
    }
}