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
using System.Data.Entity;

namespace Andromeda.Core.Managers
{
    public class BaseEntityManager
    {
        public static IViewModel GetEntityValue<TSource, TValue>(Func<TSource, bool> searchFunc, Func<TSource, TValue> selectFunc) where TSource : class
        {
            try
            {
                using (DBContext context = new DBContext())
                {
                    EntityViewModel<TValue> result = new EntityViewModel<TValue> { Result = Result.Ok };
                    result.Entity = context.Set<TSource>().AsNoTracking().Where(searchFunc).Select(selectFunc).FirstOrDefault();

                    return result;
                }
            }
            catch (Exception exc)
            {
                return LogErrorManager.Add(exc);
            }
        }

        public static TValue GetEntityValue<TSource, TValue>(DBContext context, Func<TSource, bool> searchFunc, Func<TSource, TValue> selectFunc) where TSource : class
        {
            TValue result = context.Set<TSource>().AsNoTracking().Where(searchFunc).Select(selectFunc).FirstOrDefault();

            return result;
        }

        public static IViewModel GetEntity<TSource, TViewModel>(Func<TSource, bool> searchFunc, Func<TSource, TViewModel> selectFunc) where TSource : class
        {
            try
            {
                using (DBContext context = new DBContext())
                {
                    EntityViewModel<TViewModel> result = new EntityViewModel<TViewModel> { Result = Result.Ok };
                    result.Entity = context.Set<TSource>().AsNoTracking().Where(searchFunc).Select(selectFunc).FirstOrDefault();

                    return result;
                }
            }
            catch(Exception exc)
            {
                return LogErrorManager.Add(exc);
            }
        }
        public static TViewModel GetEntity<TSource, TViewModel>(DBContext context, Func<TSource, bool> searchFunc, Func<TSource, TViewModel> selectFunc) where TSource : class
        {
            TViewModel result = context.Set<TSource>().AsNoTracking().Where(searchFunc).Select(selectFunc).FirstOrDefault();

            return result;
        }

        public static IViewModel GetEntities<TSource, TViewModel>(Func<TSource, bool> searchFunc, Func<TSource, TViewModel> selectFunc)
            where TSource : class
        {
            try
            {
                using (DBContext context = new DBContext())
                {
                    EntitiesViewModel<TViewModel> result = new EntitiesViewModel<TViewModel>
                    {
                        Result = Result.Ok
                    };
                    result.Entities = context.Set<TSource>()
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
        public static IViewModel GetEntities<TSource, TViewModel>(int page, int limit, string order, bool isAscending, Func<TSource, bool> searchFunc, Func<TSource, TViewModel> selectFunc)
            where TSource : class
        {
            try
            {
                using (DBContext context = new DBContext())
                {
                    EntitiesViewModel<TViewModel> result = new EntitiesViewModel<TViewModel>
                    {
                        Result = Result.Ok
                    };
                    result.Entities = isAscending ?
                        context.Set<TSource>()
                        .AsNoTracking()
                        .Where(searchFunc)
                        .OrderBy(order)
                        .Select(selectFunc)
                        .Skip((page - 1) * limit)
                        .Take(limit)
                        .ToList() : 
                        context.Set<TSource>()
                        .AsNoTracking()
                        .Where(searchFunc)
                        .OrderByDescending(order)
                        .Select(selectFunc)
                        .Skip((page - 1) * limit)
                        .Take(limit)
                        .ToList();

                    result.Total = context.Set<TSource>()
                        .AsNoTracking().Count();
                    result.Page = page;
                    return result;
                }
            }
            catch (Exception exc)
            {
                return LogErrorManager.Add(exc);
            }
        }

        public static List<TViewModel> GetEntities<TSource, TViewModel>(DBContext context, int page, int limit, string order, bool isAscending, Func<TSource, bool> searchFunc, Func<TSource, TViewModel> selectFunc)
            where TSource : class
        {
            List<TViewModel> result = isAscending ?
                context.Set<TSource>()
                .AsNoTracking()
                .Where(searchFunc)
                .OrderBy(order)
                .Select(selectFunc)
                .Skip((page - 1) * limit)
                .Take(limit)
                .ToList() :
                context.Set<TSource>()
                .AsNoTracking()
                .Where(searchFunc)
                .OrderByDescending(order)
                .Select(selectFunc)
                .Skip((page - 1) * limit)
                .Take(limit)
                .ToList();

            return result;
        }
        public static List<TViewModel> GetEntities<TSource, TViewModel>(DBContext context, Func<TSource, bool> searchFunc, Func<TSource, TViewModel> selectFunc)
            where TSource : class
        {
            List<TViewModel> result = context.Set<TSource>()
                .AsNoTracking()
                .Where(searchFunc)
                .Select(selectFunc)
                .ToList();

            return result;
        }

        public static IViewModel GetEntitiesWithJoin<TSource1, TSource2, TViewModel>(Func<TSource1, bool> searchFunc, Func<TSource2, TSource1, TViewModel> joinSelector)
            where TSource1 : class, IKeyEntity<Guid>
            where TSource2 : class, IKeyEntity<Guid>
        {
            try
            {
                using (DBContext context = new DBContext())
                {
                    EntitiesViewModel<TViewModel> result = new EntitiesViewModel<TViewModel>
                    {
                        Result = Result.Ok
                    };
                    IEnumerable<TSource1> tempCollection = context.Set<TSource1>()
                        .AsNoTracking()
                        .Where(searchFunc);
                    result.Entities = context.Set<TSource2>()
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
        public static IViewModel GetEntitiesWithJoin<TSource1, TSource2, TKey, TViewModel>(Func<TSource1, bool> searchFunc, Func<TSource1, TKey> innerKeySelector, Func<TSource2, TKey> outerKeySelector, Func<TSource2, TSource1, TViewModel> joinSelector)
            where TSource1 : class
            where TSource2 : class
        {
            try
            {
                using (DBContext context = new DBContext())
                {
                    EntitiesViewModel<TViewModel> result = new EntitiesViewModel<TViewModel>
                    {
                        Result = Result.Ok
                    };
                    IEnumerable<TSource1> tempCollection = context.Set<TSource1>()
                        .AsNoTracking()
                        .Where(searchFunc);
                    result.Entities = context.Set<TSource2>()
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
        public static IViewModel GetEntitiesWithJoin<TSource1, TSource2, TViewModel>(int page, int limit, string order, bool isAscending, Func<TSource1, bool> searchFunc, Func<TSource2, TSource1, TViewModel> joinSelector) 
            where TSource1 : class, IKeyEntity<Guid>
            where TSource2 : class, IKeyEntity<Guid>
        {
            try
            {
                using (DBContext context = new DBContext())
                {
                    EntitiesViewModel<TViewModel> result = new EntitiesViewModel<TViewModel>
                    {
                        Result = Result.Ok
                    };
                    IEnumerable<TSource1> joinTempCollection = isAscending ?
                        context.Set<TSource1>()
                        .AsNoTracking()
                        .Where(searchFunc)
                        .OrderBy(order)
                        .Skip((page - 1) * limit)
                        .Take(limit) :
                        context.Set<TSource1>()
                        .AsNoTracking()
                        .Where(searchFunc)
                        .OrderByDescending(order)
                        .Skip((page - 1) * limit)
                        .Take(limit);
                    result.Entities = context.Set<TSource2>()
                        .Join(joinTempCollection, fo => fo.Id, so => so.Id, joinSelector)
                        .ToList();
                    result.Total = context.Set<TSource1>()
                        .AsNoTracking().Count();
                    result.Page = page;

                    return result;
                }
            }
            catch (Exception exc)
            {
                return LogErrorManager.Add(exc);
            }
        }
        public static IViewModel GetEntitiesWithJoin<TSource1, TSource2, TKey, TViewModel>(int page, int limit, string order, bool isAscending, Func<TSource1, bool> searchFunc, Func<TSource1, TKey> innerKeySelector, Func<TSource2, TKey> outerKeySelector, Func<TSource2, TSource1, TViewModel> joinSelector)
            where TSource1 : class
            where TSource2 : class
        {
            try
            {
                using (DBContext context = new DBContext())
                {
                    EntitiesViewModel<TViewModel> result = new EntitiesViewModel<TViewModel>
                    {
                        Result = Result.Ok
                    };
                    IEnumerable<TSource1> tempCollection = isAscending ?
                        context.Set<TSource1>()
                        .AsNoTracking()
                        .Where(searchFunc)
                        .OrderBy(order)
                        .Skip((page - 1) * limit)
                        .Take(limit) :
                        context.Set<TSource1>()
                        .AsNoTracking()
                        .Where(searchFunc)
                        .OrderByDescending(order)
                        .Skip((page - 1) * limit)
                        .Take(limit);
                    result.Entities = context.Set<TSource2>()
                        .Join(tempCollection, outerKeySelector, innerKeySelector, joinSelector)
                        .ToList();
                    result.Total = context.Set<TSource1>()
                        .AsNoTracking().Count();
                    result.Page = page;

                    return result;
                }
            }
            catch (Exception exc)
            {
                return LogErrorManager.Add(exc);
            }
        }
        
        public static List<TViewModel> GetEntitiesWithJoin<TSource1, TSource2, TViewModel>(DBContext context, Func<TSource1, bool> searchFunc, Func<TSource2, TSource1, TViewModel> joinSelector)
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
        public static List<TViewModel> GetEntitiesWithJoin<TSource1, TSource2, TKey, TViewModel>(DBContext context, Func<TSource1, bool> searchFunc, Func<TSource1, TKey> innerKeySelector, Func<TSource2, TKey> outerKeySelector, Func<TSource2, TSource1, TViewModel> joinSelector)
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
        public static List<TViewModel> GetEntitiesWithJoin<TSource1, TSource2, TViewModel>(DBContext context, int page, int limit, string order, bool isAscending, Func<TSource1, bool> searchFunc, Func<TSource2, TSource1, TViewModel> joinSelector)
            where TSource1 : class, IKeyEntity<Guid>
            where TSource2 : class, IKeyEntity<Guid>
        {
            IEnumerable<TSource1> tempCollection = isAscending ?
                context.Set<TSource1>()
                .AsNoTracking()
                .Where(searchFunc)
                .OrderBy(order)
                .Skip((page - 1) * limit)
                .Take(limit) :
                context.Set<TSource1>()
                .AsNoTracking()
                .Where(searchFunc)
                .OrderByDescending(order)
                .Skip((page - 1) * limit)
                .Take(limit);
            List<TViewModel> result = context.Set<TSource2>()
                .Join(tempCollection, fo => fo.Id, so => so.Id, joinSelector)
                .ToList();

            return result;
        }
        public static List<TViewModel> GetEntitiesWithJoin<TSource1, TSource2, TKey, TViewModel>(DBContext context, int page, int limit, string order, bool isAscending, Func<TSource1, bool> searchFunc, Func<TSource1, TKey> innerKeySelector, Func<TSource2, TKey> outerKeySelector, Func<TSource2, TSource1, TViewModel> joinSelector)
            where TSource1 : class
            where TSource2 : class
        {
            IEnumerable<TSource1> tempCollection = isAscending ?
                context.Set<TSource1>()
                .AsNoTracking()
                .Where(searchFunc)
                .OrderBy(order)
                .Skip((page - 1) * limit)
                .Take(limit) :
                context.Set<TSource1>()
                .AsNoTracking()
                .Where(searchFunc)
                .OrderByDescending(order)
                .Skip((page - 1) * limit)
                .Take(limit);
            List<TViewModel> result = context.Set<TSource2>()
                .Join(tempCollection, outerKeySelector, innerKeySelector, joinSelector)
                .ToList();

            return result;
        }

        public static IViewModel AddEntity<T>(T entity) where T : class
        {
            try
            {
                using (DBContext context = DBContext.Create())
                {
                    context.Entry(entity).State = EntityState.Added;

                    context.SaveChanges();
                }

                return new ResultViewModel { Result = Result.Ok };
            }
            catch(Exception exc)
            {
                return LogErrorManager.Add(exc);
            }
        }
        public static IViewModel ModifyEntity<T>(T entity) where T : class
        {
            try
            {
                using (DBContext context = DBContext.Create())
                {
                    context.Entry(entity).State = EntityState.Modified;

                    context.SaveChanges();
                }

                return new ResultViewModel { Result = Result.Ok };
            }
            catch (Exception exc)
            {
                return LogErrorManager.Add(exc);
            }
        }
        public static IViewModel DeleteEntity<T>(T entity) where T : class
        {
            try
            {
                using (DBContext context = DBContext.Create())
                {
                    context.Entry(entity).State = EntityState.Deleted;

                    context.SaveChanges();
                }

                return new ResultViewModel { Result = Result.Ok };
            }
            catch (Exception exc)
            {
                return LogErrorManager.Add(exc);
            }
        }
        public static IViewModel AddEntities<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {
            try
            {
                using (DBContext context = DBContext.Create())
                {
                    context.Configuration.AutoDetectChangesEnabled = false;
                    context.Configuration.ValidateOnSaveEnabled = false;

                    foreach (TEntity entity in entities)
                        context.Entry(entity).State = EntityState.Added;
                    context.SaveChanges();

                    context.Configuration.AutoDetectChangesEnabled = true;
                    context.Configuration.ValidateOnSaveEnabled = true;
                }
                return new ResultViewModel { Result = Result.Ok };
            }
            catch(Exception exc)
            {
                return LogErrorManager.Add(exc);
            }
        }
        public static IViewModel ModifyEntities<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {
            try
            {
                using (DBContext context = DBContext.Create())
                {
                    context.Configuration.AutoDetectChangesEnabled = false;
                    context.Configuration.ValidateOnSaveEnabled = false;

                    foreach (TEntity entity in entities)
                        context.Entry(entity).State = EntityState.Modified;
                    context.SaveChanges();

                    context.Configuration.AutoDetectChangesEnabled = true;
                    context.Configuration.ValidateOnSaveEnabled = true;
                }
                return new ResultViewModel { Result = Result.Ok };
            }
            catch (Exception exc)
            {
                return LogErrorManager.Add(exc);
            }
        }
        public static IViewModel DeleteEntities<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {
            try
            {
                using (DBContext context = DBContext.Create())
                {
                    context.Configuration.AutoDetectChangesEnabled = false;
                    context.Configuration.ValidateOnSaveEnabled = false;

                    foreach (TEntity entity in entities)
                        context.Entry(entity).State = EntityState.Deleted;
                    context.SaveChanges();

                    context.Configuration.AutoDetectChangesEnabled = true;
                    context.Configuration.ValidateOnSaveEnabled = true;
                }
                return new ResultViewModel { Result = Result.Ok };
            }
            catch (Exception exc)
            {
                return LogErrorManager.Add(exc);
            }
        }

        public static void AddEntity<T>(DBContext context, T entity) where T : class
        {
            context.Entry(entity).State = EntityState.Added;
        }
        public static void ModifyEntity<T>(DBContext context, T entity) where T : class
        {
            context.Entry(entity).State = EntityState.Modified;
        }
        public static void DeleteEntity<T>(DBContext context, T entity) where T : class
        {
            context.Entry(entity).State = EntityState.Deleted;
        }
        public static void AddEntities<TEntity>(DBContext context, IEnumerable<TEntity> entities) where TEntity : class
        {
            context.Configuration.AutoDetectChangesEnabled = false;
            context.Configuration.ValidateOnSaveEnabled = false;

            foreach (TEntity entity in entities)
                context.Entry(entity).State = EntityState.Added;

            context.Configuration.AutoDetectChangesEnabled = true;
            context.Configuration.ValidateOnSaveEnabled = true;
        }
        public static void ModifyEntities<TEntity>(DBContext context, IEnumerable<TEntity> entities) where TEntity : class
        {
            context.Configuration.AutoDetectChangesEnabled = false;
            context.Configuration.ValidateOnSaveEnabled = false;

            foreach (TEntity entity in entities)
                context.Entry(entity).State = EntityState.Modified;

            context.Configuration.AutoDetectChangesEnabled = true;
            context.Configuration.ValidateOnSaveEnabled = true;
        }
        public static void DeleteEntities<TEntity>(DBContext context, IEnumerable<TEntity> entities) where TEntity : class
        {
            context.Configuration.AutoDetectChangesEnabled = false;
            context.Configuration.ValidateOnSaveEnabled = false;

            foreach (TEntity entity in entities)
                context.Entry(entity).State = EntityState.Deleted;

            context.Configuration.AutoDetectChangesEnabled = true;
            context.Configuration.ValidateOnSaveEnabled = true;
        }
    }
}
