using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace Lake.ADream.EntityFrameworkCore
{
    public static class ADreamDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<ADreamDbContext> builder, string connectionString) => builder.UseMySQL(connectionString);

        public static void Configure(DbContextOptionsBuilder<ADreamDbContext> builder, DbConnection connection) => builder.UseMySQL(connection);
    }
}
