using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Text;

namespace Lake.ADream.Infrastructure.Attributes
{
    /// <summary>
    /// 唯一性标识
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class UniqueAttribute : ValidationAttribute
    {
        protected string tableName;
        protected string filedName;

        public UniqueAttribute(string tableName, string filedName)
        {
            this.tableName = tableName;
            this.filedName = filedName;
          //var  uni=  UniqueConstraint=new UniqueConstraint()
        }
        public UniqueAttribute()
        {
        }

        //public override Boolean IsValid(Object value)
        //{
        //    bool validResult = false;
        //    using (DbContext context = new DbContext())
        //    {
        //        string sqlCmd = string.Format("select count(1) from [{0}] where [{1}]=@p0", tableName, filedName);
        //        context.Database.Connection.Open();
        //        var cmd = context.Database.Connection.CreateCommand();
        //        cmd.CommandText = sqlCmd;
        //        var p0 = cmd.CreateParameter();
        //        p0.ParameterName = "@p0";
        //        p0.Value = value;
        //        cmd.Parameters.Add(p0);
        //        int result = Convert.ToInt32(cmd.ExecuteScalar());
        //        validResult = (result <= 0);
        //    }
        //    return validResult;
        //}
    }
}
