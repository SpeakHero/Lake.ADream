using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Lake.ADream.EntityFrameworkCore.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="IQueryable"/> and <see cref="IQueryable{T}"/>.
    /// </summary>
    public static class QueryableExtensions
    {
        //EntityFramework does not support Include with string path.

        ///// <summary>
        ///// 指定要包含在查询结果中的相关对象。
        ///// </summary>
        ///// <param name="source">The source <see cref="IQueryable"/> on which to call Include.</param>
        ///// <param name="condition">确定包含的布尔值。<see cref="path"/> or not.</param>
        ///// <param name="path">The dot-separated list of related objects to return in the query results.</param>
        //public static IQueryable IncludeIf(this IQueryable source, bool condition, string path)
        //{
        //    return condition
        //        ? source.Include(path)
        //        : source;
        //}

        ///// <summary>
        /////指定要包含在查询结果中的相关对象。
        ///// </summary>
        ///// <param name="source">The source <see cref="IQueryable{T}"/> on which to call Include.</param>
        ///// <param name="condition">确定包含的布尔值。<see cref="path"/> or not.</param>
        ///// <param name="path">The dot-separated list of related objects to return in the query results.</param>
        //public static IQueryable<T> IncludeIf<T>(this IQueryable<T> source, bool condition, string path)
        //{
        //    return condition
        //        ? source.Include(path)
        //        : source;
        //}

        /// <summary>
        ///指定要包含在查询结果中的相关对象。
        /// </summary>
        /// <param name="source">The source <see cref="IQueryable{T}"/> on which to call Include.</param>
        /// <param name="condition">确定包含的布尔值。<paramref name="path"/> or not.</param>
        /// <param name="path">包含导航属性的类型。</param>
        public static IQueryable<T> IncludeIf<T, TProperty>(this IQueryable<T> source, bool condition, Expression<Func<T, TProperty>> path)
            where T : class 
        {
            return condition
                ? source.Include(path)
                : source;
        }
    }
}