using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Reflection;
using Lake.ADream.Infrastucture;
using Lake.ADream.Infrastructure.Localization;
using System;
using System.Linq;
using Lake.ADream.Entities.Framework;
using System.Collections;
using System.Collections.Generic;
using Lake.ADream.EntityFrameworkCore;
using System.Threading.Tasks;
using Lake.ADream.Models.Entities.Identity;
using Lake.ADream.Services;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace Lake.ADream.Infrastucture.Test
{
    [TestClass]
    public class UnitTest1
    {
        private static ILoggerFactory logger => new LoggerFactory()
         .AddDebug((categoryName, logLevel) => (logLevel == LogLevel.Information) && (categoryName == DbLoggerCategory.Database.Command.Name))
        .AddConsole((categoryName, logLevel) => (logLevel == LogLevel.Information) && (categoryName == DbLoggerCategory.Database.Command.Name));
        ADreamDbContext aDreamDbContext = new ADreamDbContext();
        [TestMethod]
        public void TestMethod1()
        {
            var s = Assembly.Load("Lake.ADream.Models");
            //"D:\Users\SuperAdmin\source\repos\Lake.ADream\Lake.ADream.Infrastructure\bin\Debug\netcoreapp2.0\Localization\SourceFiles\ADream.xml"
            // var classs=s.GetTypes().Where(d => !d.BaseType.Equals(null)&& d.BaseType.Equals(typeof(EntityBase)));
            var list = new List<string>();
            foreach (var item in s.GetTypes())
            {
                if (item.BaseType != null)
                {
                    if (item.BaseType.Name.Equals("EntityBase"))
                    {
                        list.Add(item.Name);
                    }
                }
            }
            var f = Path.GetDirectoryName(s.Location) + $@"Localization\SourceFiles\";
            var c = ConfigHelper.GetXmlConfig($"{ LocalizationConfig.LangFile}.xml", @"D:\Users\SuperAdmin\source\repos\Lake.ADream\Lake.ADream.Infrastructure\bin\Debug\netcoreapp2.0\Localization\SourceFiles");
            c.GetSection("texts:text:RoleName:name");
            var j = ConfigHelper.GetJsonConfig(basePath: @"d:\Users\SuperAdmin\source\repos\Lake.ADream\Lake.ADream.Host\");
        }
        [TestMethod]
        public async Task TestADreamDbContext()
        {
            using (UserManagerService userManagerService = new UserManagerService(aDreamDbContext, null))
            {
                var user3 = new User();
                user3.UserName = "SuperAdmin4";
                user3.PasswordHash = "123qwe";
                user3.PhoneNumber = user3.Email = user3.Id;
                user3.EmailConfirmed = true;
                user3.PhoneNumberConfirmed = true;
                var user2 = new User();
                user2.UserName = "SuperAdmin3";
                user2.PasswordHash = "123qwe";
                user2.PhoneNumber = user2.Email = user2.Id;
                user2.EmailConfirmed = true;
                user2.PhoneNumberConfirmed = true;
                var user1 = new User();
                user1.UserName = "SuperAdmin3";
                user1.PasswordHash = "123qwe";
                user1.PhoneNumber = user1.Email = user1.Id;
                user1.EmailConfirmed = true;
                user1.PhoneNumberConfirmed = true;
                var reslut = await userManagerService.CreateAsync(false, user2, user1, user3);
                var a = await userManagerService.SaveChangesAsync();
            }
        }
        [TestMethod]
        public async Task TestADreamDbContextDel()
        {
            User user = aDreamDbContext.Users.SingleOrDefault(d => d.UserName == "SuperAdmin");
            aDreamDbContext.Users.Remove(user);
            await aDreamDbContext.SaveChangesAsync();
        }

        [TestMethod]
        public async Task GetUserNameById()
        {
            var loggerFactory = new LoggerFactory();
            var loger = loggerFactory.CreateLogger<UserManagerService>();
            UserManagerService userManagerService = new UserManagerService(aDreamDbContext, null);
            string userid = "e148d56e-66ab-43f3-a3c5-48de5b6fd983";
            string username = await userManagerService.GetUserNameById(userid);
            userManagerService.Dispose();
        }
      
        ~UnitTest1()
        {
            aDreamDbContext.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
