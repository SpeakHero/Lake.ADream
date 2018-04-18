using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Lake.ADream.EntityFrameworkCore
{
    public partial class ADreamDbContext : DbContext
    {

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            var jsonconfig = ConfigHelper.GetJsonConfig();
            var value = jsonconfig?.GetValue("DataBase").ToLower();
            var conn = ConfigHelper.GetJsonConfig().GetSection("ConnectionStrings").GetSection("DefaultConnection").Value;
            ConnectionString = ConnectionString ?? conn;
            optionsBuilder.UseLoggerFactory(Logger);
            if (!string.IsNullOrEmpty(ConnectionString))
            {
                optionsBuilder = value == "mysql" ? optionsBuilder.UseMySQL(ConnectionString) : optionsBuilder.UseSqlServer(ConnectionString);
            }
            optionsBuilder.EnableSensitiveDataLogging();
            MemoryCache cache = new MemoryCache(new MemoryCacheOptions());
            optionsBuilder.UseMemoryCache(cache);
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            string tableName = string.Empty;
            foreach (var item in GetReferencingAssemblies())
            {
                var method = SetGlobalQueryMethod.MakeGenericMethod(item);
#if Debug
                Debug.WriteLine("Adding global query for: " + typeof(T));
#endif
                method.Invoke(this, new object[] { builder });
            }
            base.OnModelCreating(builder);
        }
    }
}
