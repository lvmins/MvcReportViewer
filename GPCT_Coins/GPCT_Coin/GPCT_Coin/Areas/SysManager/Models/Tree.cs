using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GPCT_Coin.Models
{
    /// <summary>
    /// ztree结构
    /// </summary>
    public class Tree
    {
        /// <summary>
        /// Id
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 父级Id
        /// </summary>
        public int pId { get; set; }

        /// <summary>
        /// 节点名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 是否展开节点
        /// </summary>
        public bool open { get; set; }

        /// <summary>
        /// 是否无子节点
        /// </summary>
        public bool isParent { get; set; }

        /// <summary>
        /// 是否选中
        /// </summary>
        public bool @checked { get; set; }

        /// <summary>
        /// 子节点集合
        /// </summary>
        public List<Tree> children { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        public string icon { get; set; }

        /// <summary>
        /// 设置个性图标的 className
        /// </summary>
        public string iconSkin { get; set; }
    }
}