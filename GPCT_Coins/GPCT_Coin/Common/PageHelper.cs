using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
   public class PageHelper
    {
        #region
        private static int? _currentPage = null;
        public static int CurrentPage
        {
            get
            {
                return _currentPage ?? 1;
            }
            set
            {
                _currentPage = value;
            }
        }
        private static int? _pageSize = null;
        public static int PageSize
        {
            get
            {
                return _pageSize ?? 20;
            }
            set
            {
                _pageSize = value;
            }
        }
        /// <summary>
        /// 是否显示上一页
        /// </summary>
        public static bool HasPreviousPage
        {
            get
            {
                return (CurrentPage > 1);
            }
        }
        /// <summary>
        /// 是否显示下一页
        /// </summary>
        public static bool HasNextPage
        {
            get
            {
                return (CurrentPage < TotalPages);
            }
        }
        /// <summary>
        /// 总条数
        /// </summary>
        public static int TotalCount
        {
            get;
            set;
        }
        public static int TotalPages
        {
            get
            {
                return (int)Math.Ceiling(TotalCount / (double)PageSize);
                //return (TotalCount % PageSize == 0 ? TotalCount / PageSize : TotalCount / PageSize + 1);
            }
        }
        /// <summary>
        /// 设置分页url eg:/Admin/Product/Index
        /// </summary>
        public static string PagingUrl
        {
            get;
            set;
        }
        /// <summary>
        /// 默认page,设置分页参数名 eg:/Admin/Product/Index?PagingParamName=1
        /// </summary>
        public static string PagingParamName
        {
            get;
            set;
        }
        //静态类不能有构造函数
        //public MvcPagingHelper(int? currentPage, int? pageSize,int totalCount)
        //{
        //    PageIndex = currentPage ?? 0;
        //    PageSize = pageSize ?? 10;
        //    TotalCount = totalCount;
        //}
        #endregion
    }
}
