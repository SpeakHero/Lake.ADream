using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Lake.ADream.EntityFrameworkCore
{
    /* This class is needed to run "dotnet ef ..." commands from command line on development. Not used anywhere else */
    public class ADreamDbContextFactory : IDesignTimeDbContextFactory<ADreamDbContext>
    {
        public ADreamDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<ADreamDbContext>();
            var conn = ConfigHelper.GetJsonConfig().GetSection("ConnectionStrings").GetSection("DefaultConnection").Value;
            ADreamDbContextConfigurer.Configure(builder, conn);
            var adreamdb = new ADreamDbContext(builder.Options);
            //adreamdb.Database.Migrate();
            return adreamdb; ;
        }
    }
}
