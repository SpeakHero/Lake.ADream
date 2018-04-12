using Lake.ADream.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Lake.ADream.Infrastucture.Test
{
    [TestClass]
    public class UserManagerServiceTest : TestBase
    {
        UserManagerService userManagerService = null;

        public UserManagerServiceTest()
        {
            userManagerService = new UserManagerService(aDreamDbContext, null);
        }

        [TestMethod]
        public async Task ChangPassWord()
        {
            userManagerService.ChangePasswordAsync()
        }
    }
}
