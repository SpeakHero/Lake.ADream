using Lake.ADream.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lake.ADream.Infrastucture.Test
{
    [TestClass]
    public class SignInManagerServicesTest : TestBase
    {
        private readonly SignInManagerService signInManagerService;
        public SignInManagerServicesTest()
        {
              signInManagerService = GetService<SignInManagerService>();
        }

        [TestMethod]
        public  void SignIn()
        {
        }
    }
}
