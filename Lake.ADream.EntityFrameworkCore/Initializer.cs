using Lake.ADream.Infrastructure.Attributes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Text;

namespace Lake.ADream.EntityFrameworkCore
{
    public class Initializer
    {
        public void InitializeDatabase(ADreamDbContext context, Type item)
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
                context.Database.ExecuteSqlCommand("ALTER TABLE " + tableName + " ADD CONSTRAINT con_Unique_" + tableName + "_" + field.Name + " UNIQUE (" + field.Name + ")");

            }
        }
    }
}
