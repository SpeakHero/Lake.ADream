using JetBrains.Annotations;
using Lake.ADream.Entities.Framework;
using Lake.ADream.Infrastructure.Attributes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
namespace Lake.ADream.EntityFrameworkCore
{
    public partial class ADreamDbContext : DbContext
    {
        private static readonly MethodInfo SetGlobalQueryMethod = typeof(ADreamDbContext).GetMethods(BindingFlags.Public | BindingFlags.Instance)
                                                       .Single(t => t.IsGenericMethod && t.Name == "SetGlobalQuery");

        public void SetGlobalQuery<T>(ModelBuilder builder) where T : EntityBase
        {
            // builder.Entity<T>().HasQueryFilter(fileter => fileter.IsDelete == false);
        }
        public async Task InitializeAsync(Type item)
        {
            var TIMESTAMPS = new List<string>();
            var UNIQUES = new List<string>();

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
            var TIMESTAMP = $"ALTER TABLE  `{tableName}`  CHANGE COLUMN `timespan` `timespan` TIMESTAMP NULL ON UPDATE CURRENT_TIMESTAMP DEFAULT CURRENT_TIMESTAMP";
            Database.ExecuteSqlCommand(TIMESTAMP);
            TIMESTAMPS.Append(TIMESTAMP + "\n\r");
            var fileds = item.GetProperties().Where(a => a.GetCustomAttributes(typeof(UniqueAttribute), true).Count() > 0);
            foreach (var field in fileds)
            {
                try
                {
                    var UNIQUE = $"ALTER TABLE {tableName} ADD CONSTRAINT con_{tableName}_{field.Name} UNIQUE({field.Name})";
                    UNIQUES.Append(UNIQUE + "\n\r");
                    Database.ExecuteSqlCommand(UNIQUE);
                }
                catch
                {
                    // Database.ExecuteSqlCommandAsync
                }
                await File.WriteAllLinesAsync($"d:\\TIMESTAMP.sql", TIMESTAMPS);
                await File.WriteAllLinesAsync($"d\\UNIQUE.sql", UNIQUES);
            }
        }
        public async Task<TEntity> ExecuteSqlCommandAsync<TEntity>(RawSqlString rawSqlString,Func<IDataReader,TEntity> func, params object[] parameters) where TEntity : class
        {
            TEntity entity = default;
            using (var connection = Database.GetDbConnection())
            {
                await connection.OpenAsync();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = rawSqlString.Format;
                    if (parameters != null)
                    {
                        int p = 0;
                        foreach (var item in parameters)
                        {
                            DbParameter dbParameter = command.CreateParameter();
                            dbParameter.ParameterName = "@p" + p;
                            dbParameter.Value = item;
                            dbParameter.Direction = ParameterDirection.ReturnValue;
                            command.Parameters.Add(dbParameter);
                            p += 1;
                        }
                    }
                    using (var reader = await command.ExecuteReaderAsync() )
                    {
                        if (reader == null)
                        {
                            return null;
                        }
                        //var count = reader.RecordsAffected;
                        //if (count == 1)
                        //{
                        //    var model = reader.ReaderToModel<TEntity>();
                        //    if (model != null)
                        //        entity.Add(reader.ReaderToModel<TEntity>());
                        //}
                        //entity = reader.ReaderToList<TEntity>();
                        func(reader);
                    }
                }
            }
            return entity;
        }

    }
}
