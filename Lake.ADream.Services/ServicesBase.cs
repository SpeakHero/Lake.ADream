using Lake.ADream.Entities.Framework;
using Lake.ADream.EntityFrameworkCore;
using Lake.ADream.Infrastructure.Identity;
using Lake.ADream.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Lake.ADream.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class ServicesBase<TEntity> : IServicesBase where TEntity : EntityBase
    {
        /// <summary>
        /// The <see cref="T:Microsoft.Extensions.Logging.ILogger" /> used to log messages from the manager.
        /// </summary>
        /// <value>
        /// The <see cref="T:Microsoft.Extensions.Logging.ILogger" /> used to log messages from the manager.
        /// </value>
        public virtual ILogger Logger
        {
            get;
            set;
        }

        /// <summary>
        /// The <see cref="T:Microsoft.AspNetCore.Http.HttpContext" /> used.
        /// </summary>
        public HttpContext Context
        {
            get
            {
                if (_context == null)
                {
                    IHttpContextAccessor contextAccessor = _contextAccessor ?? new HttpContextAccessor();
                    _context = ((contextAccessor != null) ? contextAccessor.HttpContext : null);
                }
                if (_context == null)
                {
                    throw new InvalidOperationException("HttpContext必须不为空。");
                }
                return _context;
            }
            set
            {
                _context = value;
            }
        }
        protected ISession Session
        {
            get
            {
                return Context.Session;
            }
        }

        protected DbSet<TEntity> Dbset
        {
            get
            { return DbContext.Set<TEntity>(); }
        }
        protected void Try(Action action)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                _logger.LogError("ServiceFactory.Try", ex);
            }
        }

        public virtual async Task<IList<TResult>> GetListAsync<TResult>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TResult>> selector) where TResult : class
        {
            return await GetQueryable(predicate, selector).ToListAsync();
        }
        public virtual IQueryable<TResult> GetQueryable<TResult>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TResult>> selector) where TResult : class
        {
            return Dbset.Where(predicate).Select(selector);
        }
        public virtual async Task<TResult> GetEntityFirstOrDefaultAsync<TResult>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TResult>> selector) where TResult : class
        {
            return await GetQueryable(predicate, selector).FirstOrDefaultAsync();
        }

        public virtual async Task<TResult> FindAsync<TResult>(Expression<Func<TEntity, TResult>> selector, string key) where TResult : class
        {
            return await GetQueryable(d => d.Id.Equals(key), selector).FirstOrDefaultAsync();
        }

        public virtual async Task<int> GetCountAsync()
        {
            return await Dbset.CountAsync();

        }
        public virtual async Task<IList<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await Dbset.Where(predicate).ToListAsync();
        }

        public virtual async Task<IList<TEntity>> GetListAsync()
        {
            return await Dbset.ToListAsync();
        }
        public virtual async Task<IPagedList<TResult>> GetListAsync<TResult>(Expression<Func<TEntity, TResult>> selector, int pageNumber = 1, int pageSize = 10) where TResult : class
        {
            return await Dbset.Select(selector).ToPagedListAsync(pageNumber, pageSize);
        }
        public virtual async Task<IList<TResult>> GetListAsync<TResult>(Expression<Func<TEntity, TResult>> selector) where TResult : class
        {
            return await Dbset.Select(selector).ToListAsync();
        }
        public virtual async Task<int> GetCountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await Dbset.CountAsync(predicate);
        }
        public virtual async Task<TEntity> FindAsync(params object[] key)
        {
            return await Dbset.FindAsync(key);
        }

        public virtual async Task<TEntity> FindAsync(string key, byte[] timeSpan)
        {
            return await Dbset.Where(d => d.Id.Equals(key) && d.TimeSpan.Equals(timeSpan)).FirstOrDefaultAsync();
        }
        public virtual async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await Dbset.AnyAsync(predicate);
        }
        public virtual async Task<IdentityResult> DeleteAsync(string id, byte[] timespan, bool SaveRight)
        {
            if (id.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(id));
            }
            if (timespan == null)
            {
                throw new ArgumentNullException(nameof(timespan));
            }
            var entites = Dbset.Where(d => id == d.Id && timespan == d.TimeSpan).ToArray();
            return await DeleteAsync(SaveRight, entites);
        }
        public virtual async Task<IdentityResult> DeleteAsync(string id, byte[] timespan)
        {
            return await DeleteAsync(id, timespan, true);
        }


        public virtual async Task<IdentityResult> UpdateRangeAsync(bool saveRight, params TEntity[] entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            if (entity.Length == 1)
            {
                Dbset.Update(entity[0]);

            }
            else
            {
                Dbset.UpdateRange(entity);
            }
            if (saveRight)
            {
                var save = await SaveChangesAsync();
                if (save > 0)
                {
                    return IdentityResult.Success;
                }
                else
                {
                    return IdentityResult.Failed(new IdentityError { Code = "entity", Description = "保存失败详细内容查看系统日志" });
                }
            }
            else
            {
                return IdentityResult.Failed(new IdentityError { Code = "SaveRight", Description = $"没有设置{ nameof(saveRight)}为真，所以没有保存到数据库" });
            }
        }

        public async Task<int> SaveChangesAsync()
        {
            using (var scope = DbContext.Database.BeginTransaction())
            {
                try
                {
                    var c = await DbContext.SaveChangesAsync();
                    scope.Commit();
                    return c;
                }
                catch (DbUpdateConcurrencyException ex)
                {

                    scope.Rollback();
                    return 0;
                }
            }
        }

        private TEntity[] GetEntites(string[] id, byte[][] timespan)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }
            if (timespan == null)
            {
                throw new ArgumentNullException(nameof(timespan));
            }
            if (id.Length != timespan.Length)
            {
                throw new ArgumentNullException($"{nameof(id)}和{nameof(timespan)}的维度不一样，两者必须要一样的维度！");
            }
            var entites = Dbset.Where(d => id.Contains(d.Id) && timespan.Contains(d.TimeSpan)).ToArray();
            return entites;
        }

        private async Task<int> DeleteAsync(EntityBase entity, bool isdelete)
        {
            entity.IsDelete = isdelete;
            DbContext.ChangeTracker.TrackGraph(entity, e =>
                e.Entry.Property("IsDelete").IsModified = true);
            return await SaveChangesAsync();
        }
        /// <summary>
        /// 检查数据是否存在
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        protected virtual async Task<bool> IsExist(string id)
        {
            return await Dbset.AnyAsync(s => s.Id.Equals(id));
        }
        /// <summary>
        /// 恢复删除
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual async Task<int> ReDeletedAsync(EntityBase entity)
        {
            return await DeleteAsync(entity, false);
        }
        protected readonly ADreamDbContext DbContext;
        protected bool _disposed;
        protected readonly ILogger _logger;
        private HttpContext _context;
        private readonly IHttpContextAccessor _contextAccessor;
        public ServicesBase(ADreamDbContext aDreamDbContext, IHttpContextAccessor contextAccessor)
        {
            DbContext = aDreamDbContext;
            _contextAccessor = contextAccessor;
            var loggerFactory = new LoggerFactory();
            _logger = loggerFactory.CreateLogger(GetType());
        }

        /// <summary>
        /// 用于取消操作的取消标记。
        /// </summary>
        protected virtual CancellationToken CancellationToken => CancellationToken.None;

        /// <summary>
        /// 如果该类已被处理，则抛出。
        /// </summary>
        protected void ThrowIfDisposed()
        {
            if (!_disposed)
            {
                return;
            }
            throw new ObjectDisposedException(GetType().Name);
        }

        /// <summary>
        /// 释放用户管理器使用的所有资源。
        /// </summary>
        public void Dispose()
        {
            _disposed = true;
            GC.SuppressFinalize(this);
        }

        public virtual async Task<IdentityResult> CreateAsync(params TEntity[] entity)
        {
            return await CreateAsync(true, entity);
        }
        public virtual async Task<IdentityResult> CreateAsync(bool SaveRight, params TEntity[] entity)
        {
            entity.CheakArgument();
            if (entity.Length == 1)
            {
                await Dbset.AddAsync(entity[0]);

            }
            else
            {
                await Dbset.AddRangeAsync(entity);
            }
            if (SaveRight)
            {
                var save = await SaveChangesAsync();
                if (save > 0)
                {
                    return IdentityResult.Success;
                }
                else
                {
                    return IdentityResult.Failed(new IdentityError { Code = "entity", Description = "保存失败详细内容查看系统日志" });
                }
            }
            else
            {
                return IdentityResult.Failed(new IdentityError { Code = "SaveRight", Description = $"没有设置{ nameof(SaveRight)}为真，所以没有保存到数据库" });
            }
        }
        public virtual async Task<IdentityResult> DeleteAsync(bool SaveRight, params TEntity[] entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            try
            {
                _logger.LogInformation(GetTableName(entity[0]), entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
            }
            if (entity.Length == 1)
            {
                Dbset.Remove(entity[0]);

            }
            else
            {
                Dbset.RemoveRange(entity);
            }
            if (SaveRight)
            {
                var save = await SaveChangesAsync();
                if (save > 0)
                {
                    return IdentityResult.Success;
                }
                else
                {
                    return IdentityResult.Failed(new IdentityError { Code = "entity", Description = "保存失败详细内容查看系统日志" });
                }
            }
            else
            {
                return IdentityResult.Failed(new IdentityError { Code = "SaveRight", Description = $"没有设置{ nameof(SaveRight)}为真，所以没有保存到数据库" });
            }
        }
        public virtual async Task<IdentityResult> DeleteAsync(params TEntity[] entity)
        {
            return await DeleteAsync(true, entity);
        }

        protected string PrimaryKeyValue(TEntity entity)
        {
            var entry = DbContext.Entry(entity);
            return entry.Property(((Property[])(entry.Metadata.FindPrimaryKey().Properties))[0].Name).CurrentValue.ToString();
        }

        protected string GetTableName(TEntity entity) => RelationalMetadataExtensions.Relational(DbContext.Entry(entity).Metadata).TableName;
    }
}
