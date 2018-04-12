using JetBrains.Annotations;
using Lake.ADream.Entities.Framework;
using Lake.ADream.Infrastructure.Attributes;
using Lake.ADream.Models.Entities;
using Lake.ADream.Models.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyModel;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Permissions;
using System.Text;
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
        public async Task<IDataReader> ExecuteSqlCommandAsync<T>(string sql, IList<T> pars) where T : class
        {
            var rawSqlCommand = Database.GetService<IRawSqlCommandBuilder>().Build(sql, new ReadOnlyCollection<T>(pars));
            RelationalDataReader query = await rawSqlCommand.RelationalCommand.ExecuteReaderAsync(Database.GetService<IRelationalConnection>(), parameterValues: rawSqlCommand.ParameterValues);
            var result = query.DbDataReader;
            return result;
        }

        //创建日志工厂
        private static ILoggerFactory logger => new LoggerFactory()
                 .AddDebug((categoryName, logLevel) => (logLevel == LogLevel.Information) && (categoryName == DbLoggerCategory.Database.Command.Name))
                .AddConsole((categoryName, logLevel) => (logLevel == LogLevel.Information) && (categoryName == DbLoggerCategory.Database.Command.Name));


        /// <summary>
        /// 链接字符串
        /// </summary>
        public string ConnectionString { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            string tableName = string.Empty;
            base.OnModelCreating(builder);
            foreach (var item in GetReferencingAssemblies())
            {
                var method = SetGlobalQueryMethod.MakeGenericMethod(item);
                method.Invoke(this, new object[] { builder });
                SetUnique(item);
            }

        }
        private static readonly MethodInfo SetGlobalQueryMethod = typeof(ADreamDbContext).GetMethods(BindingFlags.Public | BindingFlags.Instance)
                                                         .Single(t => t.IsGenericMethod && t.Name == "SetGlobalQuery");

        public void SetGlobalQuery<T>(ModelBuilder builder) where T : EntityBase
        {
            builder.Entity<T>().HasQueryFilter(e => !e.IsDelete);
        }
        private static IList<Type> _entityTypeCache;
        private static IEnumerable<Type> GetReferencingAssemblies()
        {
            if (_entityTypeCache != null)
            {
                return _entityTypeCache.ToList();
            }
            var s = Assembly.Load("Lake.ADream.Models");
            foreach (var item in s.GetTypes())
            {
                if (item.BaseType != null)
                {
                    if (item.BaseType.Name.Equals("EntityBase"))
                    {
                        if (_entityTypeCache == null)
                        {
                            _entityTypeCache = new List<Type>();
                        }
                        _entityTypeCache.Add(item);
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
                    case EntityState.Deleted: entry.State = EntityState.Modified; entry.CurrentValues["IsDelete"] = true; break;
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
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var jsonconfig = ConfigHelper.GetJsonConfig();
            var value = jsonconfig?.GetValue("DataBase").ToLower();
            var conn = ConfigHelper.GetJsonConfig().GetSection("ConnectionStrings").GetSection("DefaultConnection").Value;
            ConnectionString = ConnectionString ?? conn;
            optionsBuilder.UseLoggerFactory(logger);
            if (!string.IsNullOrEmpty(ConnectionString))
            {
                optionsBuilder = value == "mysql" ? optionsBuilder.UseMySQL(ConnectionString) : optionsBuilder.UseSqlServer(ConnectionString);
            }
            optionsBuilder.EnableSensitiveDataLogging();
            MemoryCache cache = new MemoryCache(new MemoryCacheOptions());
            optionsBuilder.UseMemoryCache(cache);
            base.OnConfiguring(optionsBuilder);
        }
        private void SetUnique(Type item)
        {

            string tableName = string.Empty;

            var mytableName = item.GetCustomAttributes(typeof(TableAttribute), true);
            if (mytableName.Length > 0)
            {
                TableAttribute mytable = mytableName[0] as TableAttribute;
                tableName = mytable.Name;
            }
            else
            {
                tableName = item.Name + "s";
            }
            var fileds = item.GetProperties().Where(a => a.GetCustomAttributes(typeof(UniqueAttribute), true).Count() > 0);
            foreach (var field in fileds)
            {
                try
                {
                    Database.ExecuteSqlCommand("ALTER TABLE " + tableName + " ADD CONSTRAINT con_Unique_" + tableName + "_" + field.Name + " UNIQUE (" + field.Name + ")");
                }
                catch
                {

                }
            }
        }

    }
}
