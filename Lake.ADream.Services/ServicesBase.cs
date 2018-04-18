using Lake.ADream.Entities.Framework;
using Lake.ADream.EntityFrameworkCore;
using Lake.ADream.Infrastructure.Identity;
using Lake.ADream.IServices;
using log4net;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
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
        public virtual ILog Logger
        {
            get;
            set;
        } = LogManager.GetLogger(typeof(TEntity));
        DbUpdateConcurrencyException dbUpdateConcurrencyException = null;
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
            { return dbContext.Set<TEntity>(); }
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
        public virtual async Task<IQueryable<TResult>> FromSqlAsync<TResult, Entity>(Expression<Func<Entity, bool>> predicate, Expression<Func<Entity, TResult>> selector, string sql) where TResult : class where Entity : EntityBase
        {
            var queryable = GetQueryable(predicate, selector);
            return await Task.FromResult(queryable.FromSql(sql));
        }
        public virtual IQueryable<Entity> GetQueryable<Entity>() where Entity : EntityBase
        {
            return dbContext.Set<Entity>().Where(detele => !detele.IsDelete);
        }
        public virtual IQueryable<TEntity> GetQueryable()
        {
            return GetQueryable<TEntity>();
        }
        public virtual IQueryable<TResult> GetQueryable<TResult, Entity>(Expression<Func<Entity, TResult>> selector) where TResult : class where Entity : EntityBase
        {
            selector.CheakArgument();
            return GetQueryable<Entity>().Select(selector);
        }
        public virtual IQueryable<Entity> GetQueryable<Entity>(Expression<Func<Entity, bool>> predicate) where Entity : EntityBase
        {
            predicate.CheakArgument();
            return GetQueryable<Entity>().Where(predicate);
        }
        public virtual IQueryable<TResult> GetQueryable<TResult, Entity>(Expression<Func<Entity, bool>> predicate, Expression<Func<Entity, TResult>> selector) where TResult : class where Entity : EntityBase
        {
            selector.CheakArgument();
            return GetQueryable(predicate).Select(selector);
        }

        public virtual async Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await SingleOrDefaultAsync<TEntity>(predicate);
        }
        public virtual async Task<Entity> SingleOrDefaultAsync<Entity>(Expression<Func<Entity, bool>> predicate) where Entity : EntityBase
        {
            return await GetQueryable(predicate).SingleOrDefaultAsync();
        }
        public virtual async Task<TResult> SingleOrDefaultAsync<TResult>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TResult>> selector) where TResult : class
        {
            return await SingleOrDefaultAsync<TResult, TEntity>(predicate, selector);
        }
        public virtual async Task<TResult> SingleOrDefaultAsync<TResult, Entity>(Expression<Func<Entity, bool>> predicate, Expression<Func<Entity, TResult>> selector) where TResult : class where Entity : EntityBase
        {
            return await GetQueryable(predicate, selector).SingleOrDefaultAsync();
        }
        public virtual async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await FirstOrDefaultAsync(predicate);
        }

        public virtual async Task<Entity> FirstOrDefaultAsync<Entity>(Expression<Func<Entity, bool>> predicate) where Entity : EntityBase
        {
            return await GetQueryable(predicate).FirstOrDefaultAsync();
        }
        public virtual async Task<TResult> FirstOrDefaultAsync<TResult>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TResult>> selector) where TResult : class
        {
            return await FirstOrDefaultAsync<TResult, TEntity>(predicate, selector);
        }
        public virtual async Task<TResult> FirstOrDefaultAsync<TResult, Entity>(Expression<Func<Entity, bool>> predicate, Expression<Func<Entity, TResult>> selector) where TResult : class where Entity : EntityBase
        {
            return await GetQueryable(predicate, selector).FirstOrDefaultAsync();
        }
        public virtual async Task<TResult> FindByIdAsync<TResult, Entity>(Expression<Func<Entity, TResult>> selector, string key) where TResult : class where Entity : EntityBase
        {
            return await FirstOrDefaultAsync(d => d.Id.Equals(key), selector);
        }
        public virtual async Task<TEntity> FindByIdAsync( string key)
        {
            return await FirstOrDefaultAsync(d => d.Id.Equals(key));
        }
        public virtual async Task<TResult> FindByIdAsync<TResult>(Expression<Func<TEntity, TResult>> selector, string key) where TResult : class
        {
            return await FindByIdAsync<TResult, TEntity>(selector, key);
        }
        public virtual async Task<int> GetCountAsync()
        {
            return await GetCountAsync<TEntity>();

        }
        public virtual async Task<int> GetCountAsync<Entity>() where Entity : EntityBase
        {
            return await GetQueryable<Entity>().CountAsync();

        }
        public virtual async Task<int> GetCountAsync<Entity>(Expression<Func<Entity, bool>> predicate) where Entity : EntityBase
        {
            return await GetQueryable(predicate).CountAsync();
        }
        public virtual async Task<int> GetCountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await GetCountAsync<TEntity>(predicate);
        }
        public virtual async Task<IList<Entity>> GetListAsync<Entity>() where Entity : EntityBase
        {
            return await GetQueryable<Entity>().ToListAsync();
        }
        public virtual async Task<IList<Entity>> GetListAsync<Entity>(Expression<Func<Entity, bool>> predicate) where Entity : EntityBase
        {
            return await GetQueryable(predicate).ToListAsync();
        }
        public virtual async Task<IList<TResult>> GetListAsync<TResult, Entity>(Expression<Func<Entity, bool>> predicate, Expression<Func<Entity, TResult>> selector) where TResult : class where Entity : EntityBase
        {
            return await GetQueryable(predicate, selector).ToListAsync();
        }
        public virtual async Task<IPagedList<TResult>> GetListAsync<TResult, Entity>(Expression<Func<Entity, TResult>> selector, int pageNumber = 1, int pageSize = 10) where TResult : class where Entity : EntityBase
        {
            return await GetQueryable(selector).ToPagedListAsync(pageNumber, pageSize);
        }
        public virtual async Task<TEntity> FindAsync(string key)
        {
            return await FindAsync<TEntity>(key);
        }
        public virtual async Task<Entity> FindAsync<Entity>(string key) where Entity : EntityBase
        {
            key.CheakArgument();
            return await FindAsync<Entity, Entity>(predicate => predicate.Id.Equals(key), selector => selector);
        }
        public async Task<TResult> FindAsync<TResult, Entity>(Expression<Func<Entity, bool>> predicate, Expression<Func<Entity, TResult>> selector, CancellationToken cancellationToken = default) where TResult : class where Entity : EntityBase
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            return await FirstOrDefaultAsync(predicate, selector);
        }
        public virtual async Task<Entity> FindAsync<Entity>(string key, byte[] timeSpan) where Entity : EntityBase
        {
            if (!key.IsNotNullOrEmpty())
            {
                return null;
            }
            return await FirstOrDefaultAsync<Entity>(d => d.Id.Equals(key) && d.TimeSpan.Equals(timeSpan));
        }
        public virtual async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await AnyAsync<TEntity>(predicate);
        }
        public virtual async Task<bool> AnyAsync<Entity>(Expression<Func<Entity, bool>> predicate) where Entity : EntityBase
        {
            return await GetQueryable(predicate).AnyAsync();
        }
        public virtual async Task<ADreamResult> UpdateAsync<Entity>(Entity entity, params string[] propertys) where Entity : EntityBase
        {
            CancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            entity.CheakArgument();
            var dbset = dbContext.Set<Entity>();
            dbContext.Entry(entity).State = EntityState.Unchanged;
            foreach (var property in propertys)
            {
                dbContext.Entry(entity).Property(property).IsModified = true;
            }
            dbContext.Entry(entity).Property(d => d.EditedTime).IsModified = true;

            return await SaveChangesAsync(CancellationToken);
        }
        public virtual async Task<ADreamResult> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default, params string[] propertys)
        {
            return await UpdateAsync<TEntity>(entity, propertys);
        }
        public virtual async Task<ADreamResult> UpdateAsync<Entity>(Entity entity, CancellationToken cancellationToken = default) where Entity : EntityBase
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            entity.CheakArgument();
            var dbset = dbContext.Set<Entity>();
            dbset.Attach(entity);
            dbset.Update(entity);
            return await SaveChangesAsync(cancellationToken);
        }
        public virtual async Task<ADreamResult> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            return await UpdateAsync<TEntity>(entity, cancellationToken);
        }
        public async Task<ADreamResult> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            if (!AutoSaveChanges)
            {
                return ADreamResult.Failed(new ADreamError { Code = nameof(AutoSaveChanges), Description = $"没有设置{ nameof(AutoSaveChanges)}为真，所以没有保存到数据库" });
            }
            using (var scope = dbContext.Database.BeginTransaction())
            {
                try
                {
                    var c = await dbContext.SaveChangesAsync(cancellationToken);
                    scope.Commit();
                    var result = ADreamResult.Success;
                    result.Result = c;
                    return result;
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    Logger.Error(ex.Message, ex);
                    scope.Rollback();
                    return ADreamResult.Failed(new ADreamError { Code = ex.HResult.ToString(), Description = ex.ToJson() });
                }
            }
        }

        private TEntity[] GetEntites(string[] id, DateTime[] timespan)
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
            var entites = GetQueryable().Where(d => id.Contains(d.Id) && timespan.Contains(d.TimeSpan)).ToArray();
            return entites;
        }

        private async Task<ADreamResult> DeleteAsync(TEntity entity, bool isdelete, CancellationToken cancellationToken = default)
        {
            entity.IsDelete = isdelete;
            return await UpdateAsync(entity, cancellationToken);
        }
        /// <summary>
        /// 恢复删除
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual async Task<ADreamResult> ReDeletedAsync(TEntity entity)
        {
            return await DeleteAsync(entity, false);
        }
        /// <summary>
        /// 检查数据是否存在
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        protected virtual async Task<bool> IsExist(string id)
        {
            return await IsExist<TEntity>(id);
        }
        /// <summary>
        /// 检查数据是否存在
        /// </summary>
        /// <typeparam name="Entity"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        protected virtual async Task<bool> IsExist<Entity>(string id) where Entity : EntityBase
        {
            return await AnyAsync<Entity>(s => s.Id.Equals(id));
        }


        public virtual async Task<ADreamResult> CreateAsync(TEntity entity)
        {
            return await CreateAsync(entity, default);
        }
        public virtual async Task<ADreamResult> CreateAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            entity.CheakArgument();
            await Dbset.AddAsync(entity, cancellationToken);
            return await SaveChangesAsync(cancellationToken);
        }

        protected string PrimaryKeyValue(TEntity entity)
        {
            var entry = dbContext.Entry(entity);
            return entry.Property(((Property[])(entry.Metadata.FindPrimaryKey().Properties))[0].Name).CurrentValue.ToString();
        }

        protected string GetTableName<Entity>(Entity entity) where Entity : EntityBase => RelationalMetadataExtensions.Relational(dbContext.Entry(entity).Metadata).TableName;


        protected readonly ADreamDbContext dbContext;
        protected bool _disposed;
        protected readonly ILogger _logger;
        private HttpContext _context;
        private readonly IHttpContextAccessor _contextAccessor;
        public ServicesBase(ADreamDbContext aDreamDbContext, IHttpContextAccessor contextAccessor, IServiceProvider services)
        {
            dbContext = aDreamDbContext;
            _contextAccessor = contextAccessor;
            Services = services ?? throw new ArgumentNullException(nameof(services));
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
        /// <summary>
        /// 获取或设置一个标志，指示是否应坚持CreateAsync的变化后，updateasync和DeleteAsync被称为。
        /// </summary>
        /// <value>
        /// 如果应自动保留更改，则为true，否则为false。
        /// </value>
        public bool AutoSaveChanges
        {
            get;
            set;
        } = true;
        public IServiceProvider Services { get; }
    }
}
