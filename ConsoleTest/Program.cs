using Lake.ADream.EntityFrameworkCore;
using Lake.ADream.Infrastructure.Attributes;
using Lake.ADream.Models.Entities.Identity;
using Lake.ADream.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace ConsoleTest
{
    class Program
    {
        private static IList<Type> _entityTypeCache;
        private static string tableName;

        static void Main(string[] args)
        {
            //ADreamDbContext aDreamDbContext = new ADreamDbContext();
            //User user = aDreamDbContext.Users.FirstOrDefault();
            //aDreamDbContext.Users.Remove(user);
            // aDreamDbContext.SaveChangesAsync();

            ADreamDbContext aDreamDbContext = new ADreamDbContext();
            var loggerFactory = new LoggerFactory();
            var loger = loggerFactory.CreateLogger<SignInManager>();
            //UserManagerService userManagerService = new UserManagerService(aDreamDbContext, loger,null);
            //string userid = "e148d56e-66ab-43f3-a3c5-48de5b6fd983";
            //string username = userManagerService.GetUserNameById(userid).Result;
            //var user = new User { Id = userid };
            ////userManagerService.DeleteAsync(user).Wait();
            ////userManagerService.RestoreDeleting(user).Wait();
            //userManagerService.ChangePasswordAsync(userid, null, "123456").Wait();
            //userManagerService.Dispose();
            //SignInManager signInManager = new SignInManager(aDreamDbContext, null);
            //var sin = signInManager.SignIn("SuperAdmin", "123456");
            //IList<string> filedname = new List<string>();
            //foreach (var item in GetReferencingAssemblies())
            //{
            //    var mytableName = item.GetCustomAttributes(typeof(TableAttribute), true);
            //    if (mytableName.Length > 0)
            //    {
            //        TableAttribute mytable = mytableName[0] as TableAttribute;
            //        tableName = mytable.Name;
            //    }
            //    else
            //    {
            //        tableName = item.Name;
            //    }

            //   var fileds= item.GetProperties().Where(a => a.GetCustomAttributes(typeof(UniqueAttribute), true).Count() > 0);
            //    foreach (var field in fileds)
            //    {
            //        filedname.Add(field.Name);
            //    }
            //}
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
                var reslut =  userManagerService.CreateAsync(false, user2, user1, user3).Result;
                var a =  userManagerService.SaveChangesAsync().Result;
            }
            aDreamDbContext.Dispose();
        }

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
    }
}
