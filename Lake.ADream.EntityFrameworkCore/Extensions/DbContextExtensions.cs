using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace Lake.ADream.EntityFrameworkCore.Extensions
{
    internal static class DbContextExtensions
    {
        /// <summary>
        /// 具有关系事务管理器
        /// </summary>
        /// <param name="dbContext"></param>
        /// <returns></returns>
        public static bool HasRelationalTransactionManager(this DbContext dbContext)
        {
            return dbContext.Database.GetService<IDbContextTransactionManager>() is IRelationalTransactionManager;
        }
    }
}