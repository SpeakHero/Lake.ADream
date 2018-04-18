using Lake.ADream.EntityFrameworkCore;
using Lake.ADream.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Lake.ADream.Models.Entities.Identity;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;
using System.Data;

namespace Lake.ADream.Infrastucture.Test
{
    [TestClass]
    public class UserManagerServiceTest : TestBase
    {
        User user = new User
        {
            UserName = "SuperAdmin1",
            PasswordHash = "123qwe",
            PhoneNumber = "15214067855",
            EmailConfirmed = true,
            Email = "49196352@qq.com",
            PhoneNumberConfirmed = true
        };
        [TestMethod]
        public async Task CreatAsnyc()
        {

            var result = await userManagerService.CreateAsync(user);
            Console.Write(result.Errors);
        }

        [TestMethod]
        public async Task SetUserNameAsync()
        {
            user = await userManagerService.FindByNameAsync(user.UserName, selector => new User { Id = selector.Id, TimeSpan = selector.TimeSpan });
            if (user != null)
            {
                var result = await userManagerService.SetUserNameAsync(user, "SuperAdmin");
            }
        }
        [TestMethod]
        public async Task SetNormalizedUserNameAsync()
        {
            user = await userManagerService.FindByNameAsync(user.UserName, selector => new User { Id = selector.Id, TimeSpan = selector.TimeSpan });
            if (user != null)
            {
                var result = await userManagerService.SetNormalizedUserNameAsync(user, "朱超平");
            }
        }
        [TestMethod]
        public async Task CheckPassword()
        {
            user = await userManagerService.FindByNameAsync(user.UserName,selector=>new User {Id=user.Id ,PasswordHash=selector.PasswordHash});
            var result =  userManagerService.CheckPassword(user,"dd");
        }
        [TestMethod]
        public async Task HasPasswordAsync()
        {
            var result = await userManagerService.HasPasswordAsync(user);
        }
        [TestMethod]
        public async Task ExecuteSqlCommandAsync()
        {
            var sql = @"SELECT `detele`.`Id`, `detele`.`TimeSpan` 
                       FROM `Users` AS `detele` ";
            var result = await aDreamDbContext.ExecuteSqlCommandAsync(sql, (reader) => reader.ReaderToList<User>());
        }
        IList<User> GetUserName(IDataReader reader)
        {
            return reader.ReaderToList<User>();
        }
    }
}
