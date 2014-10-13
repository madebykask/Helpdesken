//using System.Collections.Generic;
//using System.Linq;
//using System.Web.Mvc;
//using System.Web.Routing;

//namespace dhHelpdesk_NG.Web.Infrastructure.Extensions
//{
//    public static class PagingExtensions
//    {
//        #region HtmlHelper extensions

//        public static string Pager(this HtmlHelper htmlHelper, int pageSize, int currentPage, int totalItemCount, object htmlAttributes)
//        {
//            var pager = new Pager(htmlHelper.ViewContext, pageSize, currentPage, totalItemCount, new RouteValueDictionary(), AnonymousObjectToHtmlAttributes(htmlAttributes));
//            return pager.RenderHtml();
//        }

//        #endregion

//        #region IQueryable<T> extensions

//        public static IPagedList<T> ToPagedList<T>(this IQueryable<T> source, int pageIndex, int pageSize)
//        {
//            return new PagedList<T>(source, pageIndex, pageSize);
//        }

//        public static IPagedList<T> ToPagedList<T>(this IQueryable<T> source, int pageIndex, int pageSize, int totalCount)
//        {
//            return new PagedList<T>(source, pageIndex, pageSize, totalCount);
//        }

//        #endregion

//        #region IEnumerable<T> extensions

//        public static IPagedList<T> ToPagedList<T>(this IEnumerable<T> source, int pageIndex, int pageSize)
//        {
//            return new PagedList<T>(source, pageIndex, pageSize);
//        }

//        public static IPagedList<T> ToPagedList<T>(this IEnumerable<T> source, int pageIndex, int pageSize, int totalCount)
//        {
//            return new PagedList<T>(source, pageIndex, pageSize, totalCount);
//        }

//        #endregion

//        private static RouteValueDictionary AnonymousObjectToHtmlAttributes(object htmlAttributes)
//        {
//            RouteValueDictionary result = new RouteValueDictionary();

//            if (htmlAttributes != null)
//            {
//                foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(htmlAttributes))
//                {
//                    result.Add(property.Name.Replace('_', '-'), property.GetValue(htmlAttributes));
//                }
//            }

//            return result;
//        }
//    }
//}