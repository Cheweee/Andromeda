using Andromeda.Core.Logs;
using Andromeda.Models.Context;
using Andromeda.ViewModels.Server;
using Andromeda.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Andromeda.Models.Interfaces;

namespace Andromeda.Core.Managers
{
    public class BaseEntityManager
    {
        public static IViewModel GetItemValue<TSource, TValue>(Func<TSource, bool> searchFunc, Func<TSource, TValue> selectFunc) where TSource : class
        {
            try
            {
                using (DBContext context = new DBContext())
                {
                    ItemViewModel<TValue> result = new ItemViewModel<TValue> { Result = Result.Ok };
                    result.Item = context.Set<TSource>().AsNoTracking().Where(searchFunc).Select(selectFunc).FirstOrDefault();

                    return result;
                }
            }
            catch (Exception exc)
            {
                return LogErrorManager.Add(exc);
            }
        }
        public static TValue GetItemValue<TSource, TValue>(DBContext context, Func<TSource, bool> searchFunc, Func<TSource, TValue> selectFunc) where TSource : class
        {
            TValue result = context.Set<TSource>().AsNoTracking().Where(searchFunc).Select(selectFunc).FirstOrDefault();

            return result;
        }

        public static IViewModel GetItem<TSource, TViewModel>(Func<TSource, bool> searchFunc, Func<TSource, TViewModel> selectFunc) where TSource : class
        {
            try
            {
                using (DBContext context = new DBContext())
                {
                    ItemViewModel<TViewModel> result = new ItemViewModel<TViewModel> { Result = Result.Ok };
                    result.Item = context.Set<TSource>().AsNoTracking().Where(searchFunc).Select(selectFunc).FirstOrDefault();

                    return result;
                }
            }
            catch(Exception exc)
            {
                return LogErrorManager.Add(exc);
            }
        }
        public static TViewModel GetItem<TSource, TViewModel>(DBContext context, Func<TSource, bool> searchFunc, Func<TSource, TViewModel> selectFunc) where TSource : class
        {
            TViewModel result = context.Set<TSource>().AsNoTracking().Where(searchFunc).Select(selectFunc).FirstOrDefault();

            return result;
        }

        public static IViewModel GetCollection<TSource, TViewModel>(Func<TSource, bool> searchFunc, Func<TSource, TViewModel> selectFunc)
            where TSource : class
        {
            try
            {
                using (DBContext context = new DBContext())
                {
                    CollectionViewModel<TViewModel> result = new CollectionViewModel<TViewModel>
                    {
                        Result = Result.Ok
                    };
                    result.Collection = context.Set<TSource>()
                        .AsNoTracking()
                        .Where(searchFunc)
                        .Select(selectFunc)
                        .ToList();

                    return result;
                }
            }
            catch (Exception exc)
            {
                return LogErrorManager.Add(exc);
            }
        }
        public static List<TViewModel> GetCollection<TSource, TViewModel>(DBContext context, Func<TSource, bool> searchFunc, Func<TSource, TViewModel> selectFunc)
            where TSource : class
        {
            List<TViewModel> result = context.Set<TSource>()
                .AsNoTracking()
                .Where(searchFunc)
                .Select(selectFunc)
                .ToList();

            return result;
        }
        public static IViewModel GetCollection<TSource, TViewModel>(int page, int limit, string order, string search, bool isAscending, Func<TSource, bool> searchFunc, Func<TSource, TViewModel> selectFunc)
            where TSource : class
        {
            try
            {
                using (DBContext context = new DBContext())
                {
                    CollectionViewModel<TViewModel> result = new CollectionViewModel<TViewModel>
                    {
                        Result = Result.Ok
                    };
                    result.Collection = isAscending ?
                        context.Set<TSource>()
                        .AsNoTracking()
                        .Where(searchFunc)
                        .OrderBy(order)
                        .Select(selectFunc)
                        .Skip(page * limit)
                        .Take(limit)
                        .ToList() :
                        context.Set<TSource>()
                        .AsNoTracking()
                        .Where(searchFunc)
                        .OrderByDescending(order)
                        .Select(selectFunc)
                        .Skip(page * limit)
                        .Take(limit)
                        .ToList();

                    return result;
                }
            }
            catch (Exception exc)
            {
                return LogErrorManager.Add(exc);
            }
        }
        public static List<TViewModel> GetCollection<TSource, TViewModel>(DBContext context, int page, int limit, string order, string search, bool isAscending, Func<TSource, bool> searchFunc, Func<TSource, TViewModel> selectFunc)
            where TSource : class
        {
            List<TViewModel> result = isAscending ?
                context.Set<TSource>()
                .AsNoTracking()
                .Where(searchFunc)
                .OrderBy(order)
                .Select(selectFunc)
                .Skip(page * limit)
                .Take(limit)
                .ToList() :
                context.Set<TSource>()
                .AsNoTracking()
                .Where(searchFunc)
                .OrderByDescending(order)
                .Select(selectFunc)
                .Skip(page * limit)
                .Take(limit)
                .ToList();

            return result;
        }

        public static IViewModel GetCollectionWithJoin<TSource1, TSource2, TViewModel>(Func<TSource1, bool> searchFunc, Func<TSource2, TSource1, TViewModel> joinSelector)
            where TSource1 : class, IKeyEntity<Guid>
            where TSource2 : class, IKeyEntity<Guid>
        {
            try
            {
                using (DBContext context = new DBContext())
                {
                    CollectionViewModel<TViewModel> result = new CollectionViewModel<TViewModel>
                    {
                        Result = Result.Ok
                    };
                    IEnumerable<TSource1> tempCollection = context.Set<TSource1>()
                        .AsNoTracking()
                        .Where(searchFunc);
                    result.Collection = context.Set<TSource2>()
                        .Join(tempCollection, fo => fo.Id, so => so.Id, joinSelector)
                        .ToList();

                    return result;
                }
            }
            catch (Exception exc)
            {
                return LogErrorManager.Add(exc);
            }
        }
        public static List<TViewModel> GetCollectionWithJoin<TSource1, TSource2, TViewModel>(DBContext context, Func<TSource1, bool> searchFunc, Func<TSource2, TSource1, TViewModel> joinSelector)
            where TSource1 : class, IKeyEntity<Guid>
            where TSource2 : class, IKeyEntity<Guid>
        {
            IEnumerable<TSource1> tempCollection = context.Set<TSource1>()
                .AsNoTracking()
                .Where(searchFunc);
            List<TViewModel> result = context.Set<TSource2>()
                .Join(tempCollection, fo => fo.Id, so => so.Id, joinSelector)
                .ToList();

            return result;
        }
        public static IViewModel GetCollectionWithJoin<TSource1, TSource2, TKey, TViewModel>(Func<TSource1, bool> searchFunc, Func<TSource1, TKey> innerKeySelector, Func<TSource2, TKey> outerKeySelector, Func<TSource2, TSource1, TViewModel> joinSelector)
            where TSource1 : class
            where TSource2 : class
        {
            try
            {
                using (DBContext context = new DBContext())
                {
                    CollectionViewModel<TViewModel> result = new CollectionViewModel<TViewModel>
                    {
                        Result = Result.Ok
                    };
                    IEnumerable<TSource1> tempCollection = context.Set<TSource1>()
                        .AsNoTracking()
                        .Where(searchFunc);
                    result.Collection = context.Set<TSource2>()
                        .Join(tempCollection, outerKeySelector, innerKeySelector, joinSelector)
                        .ToList();

                    return result;
                }
            }
            catch (Exception exc)
            {
                return LogErrorManager.Add(exc);
            }
        }
        public static List<TViewModel> GetCollectionWithJoin<TSource1, TSource2, TKey, TViewModel>(DBContext context, Func<TSource1, bool> searchFunc, Func<TSource1, TKey> innerKeySelector, Func<TSource2, TKey> outerKeySelector, Func<TSource2, TSource1, TViewModel> joinSelector)
            where TSource1 : class
            where TSource2 : class
        {
            IEnumerable<TSource1> tempCollection = context.Set<TSource1>()
                .AsNoTracking()
                .Where(searchFunc);
            List<TViewModel> result = context.Set<TSource2>()
                .Join(tempCollection, outerKeySelector, innerKeySelector, joinSelector)
                .ToList();

            return result;
        }
        public static IViewModel GetCollectionWithJoin<TSource1, TSource2, TViewModel>(int page, int limit, string order, string search, bool isAscending, Func<TSource1, bool> searchFunc, Func<TSource2, TSource1, TViewModel> joinSelector) 
            where TSource1 : class, IKeyEntity<Guid>
            where TSource2 : class, IKeyEntity<Guid>
        {
            try
            {
                using (DBContext context = new DBContext())
                {
                    CollectionViewModel<TViewModel> result = new CollectionViewModel<TViewModel>
                    {
                        Result = Result.Ok
                    };
                    IEnumerable<TSource1> tempCollection = isAscending ? 
                        context.Set<TSource1>()
                        .AsNoTracking()
                        .Where(searchFunc)
                        .OrderBy(order) :
                        context.Set<TSource1>()
                        .AsNoTracking()
                        .Where(searchFunc)
                        .OrderByDescending(order);
                    result.Collection = context.Set<TSource2>()
                        .Join(tempCollection, fo => fo.Id, so => so.Id, joinSelector)
                        .Skip(page * limit)
                        .Take(limit)
                        .ToList();

                    return result;
                }
            }
            catch (Exception exc)
            {
                return LogErrorManager.Add(exc);
            }
        }
        public static List<TViewModel> GetCollectionWithJoin<TSource1, TSource2, TViewModel>(DBContext context, int page, int limit, string order, string search, bool isAscending, Func<TSource1, bool> searchFunc, Func<TSource2, TSource1, TViewModel> joinSelector)
            where TSource1 : class, IKeyEntity<Guid>
            where TSource2 : class, IKeyEntity<Guid>
        {
            IEnumerable<TSource1> tempCollection = isAscending ?
                context.Set<TSource1>()
                .AsNoTracking()
                .Where(searchFunc)
                .OrderBy(order) :
                context.Set<TSource1>()
                .AsNoTracking()
                .Where(searchFunc)
                .OrderByDescending(order);
            List<TViewModel> result = context.Set<TSource2>()
                .Join(tempCollection, fo => fo.Id, so => so.Id, joinSelector)
                .Skip(page * limit)
                .Take(limit)
                .ToList();

            return result;
        }
        public static IViewModel GetCollectionWithJoin<TSource1, TSource2, TKey, TViewModel>(int page, int limit, string order, string search, bool isAscending, Func<TSource1, bool> searchFunc, Func<TSource1, TKey> innerKeySelector, Func<TSource2, TKey> outerKeySelector, Func<TSource2, TSource1, TViewModel> joinSelector)
            where TSource1 : class
            where TSource2 : class
        {
            try
            {
                using (DBContext context = new DBContext())
                {
                    CollectionViewModel<TViewModel> result = new CollectionViewModel<TViewModel>
                    {
                        Result = Result.Ok
                    };
                    IEnumerable<TSource1> tempCollection = isAscending ?
                        context.Set<TSource1>()
                        .AsNoTracking()
                        .Where(searchFunc)
                        .OrderBy(order) :
                        context.Set<TSource1>()
                        .AsNoTracking()
                        .Where(searchFunc)
                        .OrderByDescending(order);
                    result.Collection = context.Set<TSource2>()
                        .Join(tempCollection, outerKeySelector, innerKeySelector, joinSelector)
                        .Skip(page * limit)
                        .Take(limit)
                        .ToList();

                    return result;
                }
            }
            catch (Exception exc)
            {
                return LogErrorManager.Add(exc);
            }
        }
        public static List<TViewModel> GetCollectionWithJoin<TSource1, TSource2, TKey, TViewModel>(DBContext context, int page, int limit, string order, string search, bool isAscending, Func<TSource1, bool> searchFunc, Func<TSource1, TKey> innerKeySelector, Func<TSource2, TKey> outerKeySelector, Func<TSource2, TSource1, TViewModel> joinSelector)
            where TSource1 : class
            where TSource2 : class
        {
            IEnumerable<TSource1> tempCollection = isAscending ?
                context.Set<TSource1>()
                .AsNoTracking()
                .Where(searchFunc)
                .OrderBy(order) :
                context.Set<TSource1>()
                .AsNoTracking()
                .Where(searchFunc)
                .OrderByDescending(order);
            List<TViewModel> result = context.Set<TSource2>()
                .Join(tempCollection, outerKeySelector, innerKeySelector, joinSelector)
                .Skip(page * limit)
                .Take(limit)
                .ToList();

            return result;
        }
    }
}
