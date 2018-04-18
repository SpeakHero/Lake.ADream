using Lake.ADream.Entities.Framework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Lake.ADream.EntityFrameworkCore
{
    public partial class ADreamDbContext : DbContext
    {
        public ADreamDbContext(DbContextOptions<ADreamDbContext> options)
            : base(options)
        {
        }
        public ADreamDbContext() : base()
        {
        }

        //创建日志工厂
        private static ILoggerFactory Logger => new LoggerFactory()
.AddDebug((categoryName, logLevel) => (logLevel == LogLevel.Information) && (categoryName == DbLoggerCategory.Database.Command.Name))
.AddConsole((categoryName, logLevel) => (logLevel == LogLevel.Information) && (categoryName == DbLoggerCategory.Database.Command.Name));


        /// <summary>
        /// 链接字符串
        /// </summary>
        public string ConnectionString { get; set; }
        private static IList<Type> _entityTypeCache;
        public static IEnumerable<Type> GetReferencingAssemblies()
        {
            if (_entityTypeCache == null)
            {
                
                var s = Assembly.Load("Lake.ADream.Models");
                foreach (var item in s.GetTypes())
                {
                    if (item.BaseType != null)
                    {
                        if (item.GetInterface(nameof(IEntityBase)) != null)
                        {
                            if (_entityTypeCache == null)
                            {
                                _entityTypeCache = new List<Type>();
                            }
                            _entityTypeCache.Add(item);
                        }
                    }
                }
            }
            return _entityTypeCache;
        }
        private void OnBeforeSaving()
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                switch (entry.State)
                {
                    case EntityState.Added: entry.CurrentValues["IsDelete"] = false; break;
                    case EntityState.Deleted: entry.State = EntityState.Modified; entry.CurrentValues["IsDelete"] = true; entry.Property("IsDelete").IsModified = true; break;
                }
            }
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            OnBeforeSaving();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }
        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            OnBeforeSaving();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
        public override int SaveChanges()
        {
            OnBeforeSaving();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            OnBeforeSaving();
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
