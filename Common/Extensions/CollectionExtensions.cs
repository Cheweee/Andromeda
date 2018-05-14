using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Andromeda.Common.Extensions
{
    public static class CollectionExtensions
    {
        public static Expression<Func<TSource, object>> GetExpression<TSource>(string propertyName)
        {
            if (propertyName.FirstOrDefault() == '-')
            {
                propertyName = propertyName.Remove(0, 1);
            }
            var param = Expression.Parameter(typeof(TSource), "x");
            var property = Expression.Property(param, propertyName);
            Expression conversion = Expression.Convert(property, typeof(object));
            return Expression.Lambda<Func<TSource, object>>(conversion, param);
        }
        public static Func<TSource, object> GetFunc<TSource>(string propertyName)
        {
            return GetExpression<TSource>(propertyName).Compile();
        }
        /// <summary>
        /// Sorts the elements of the sequence in ascending order by key.
        /// </summary>
        /// <typeparam name="TSource">Type of source.</typeparam>
        /// <param name="source">A sequence of values to be ordered.</param>
        /// <param name="propertyName">Key of sorting.</param>
        /// <returns>Elements of which are sorted by key.</returns>
        public static IOrderedQueryable<TSource> OrderBy<TSource>(this IQueryable<TSource> source, string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                return source as IOrderedQueryable<TSource>;
            }
            return source.OrderBy(GetExpression<TSource>(propertyName));
        }
        /// <summary>
        /// Sorts the elements of the sequence in descending order by key.
        /// </summary>
        /// <typeparam name="TSource">Type of source.</typeparam>
        /// <param name="source">A sequence of values to be ordered.</param>
        /// <param name="propertyName">Key of sorting.</param>
        /// <returns>Elements of which are sorted by key.</returns>
        public static IOrderedQueryable<TSource> OrderByDescending<TSource>(this IQueryable<TSource> source, string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                return source as IOrderedQueryable<TSource>;
            }
            return source.OrderByDescending(GetExpression<TSource>(propertyName));
        }
        /// <summary>
        /// Sorts the elements of the ordered sequence in ascending order by key.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name=""></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static IOrderedQueryable<TSource> ThenBy<TSource>(this IOrderedQueryable<TSource> source, string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                return source as IOrderedQueryable<TSource>;
            }
            return source.ThenBy(GetExpression<TSource>(propertyName));
        }
        /// <summary>
        /// Sorts the elements of the ordered sequence in ascending order by key.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name=""></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static IOrderedQueryable<TSource> ThenByDescending<TSource>(this IOrderedQueryable<TSource> source, string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                return source as IOrderedQueryable<TSource>;
            }
            return source.ThenByDescending(GetExpression<TSource>(propertyName));
        }
        /// <summary>
        /// Sorts the elements of the sequence in ascending order by key.
        /// </summary>
        /// <typeparam name="TSource">Type of source.</typeparam>
        /// <param name="source">A sequence of values to be ordered.</param>
        /// <param name="propertyName">Key of sorting.</param>
        /// <returns>Elements of which are sorted by key.</returns>
        public static IOrderedEnumerable<TSource> OrderBy<TSource>(this IEnumerable<TSource> source, string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                return source as IOrderedEnumerable<TSource>;
            }
            return source.OrderBy(GetFunc<TSource>(propertyName));
        }
        /// <summary>
        /// Sorts the elements of the sequence in descending order by key.
        /// </summary>
        /// <typeparam name="TSource">Type of source.</typeparam>
        /// <param name="source">A sequence of values to be ordered.</param>
        /// <param name="propertyName">Key of sorting.</param>
        /// <returns>Elements of which are sorted by key.</returns>
        public static IOrderedEnumerable<TSource> OrderByDescending<TSource>(this IEnumerable<TSource> source, string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                return source as IOrderedEnumerable<TSource>;
            }
            return source.OrderByDescending(GetFunc<TSource>(propertyName));
        }
        /// <summary>
        /// Sorts the elements of the ordered sequence in ascending order by key.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name=""></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static IOrderedEnumerable<TSource> ThenBy<TSource>(this IOrderedEnumerable<TSource> source, string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                return source as IOrderedEnumerable<TSource>;
            }
            return source.ThenBy(GetFunc<TSource>(propertyName));
        }
        /// <summary>
        /// Sorts the elements of the ordered sequence in ascending order by key.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name=""></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static IOrderedEnumerable<TSource> ThenByDescending<TSource>(this IOrderedEnumerable<TSource> source, string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                return source as IOrderedEnumerable<TSource>;
            }
            return source.ThenByDescending(GetFunc<TSource>(propertyName));
        }
    }
}
