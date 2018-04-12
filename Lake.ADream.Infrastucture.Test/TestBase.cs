using Lake.ADream.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lake.ADream.Infrastucture.Test
{
    [TestClass]
    public class TestBase : IDisposable
    {
        private static ILoggerFactory logger => new Microsoft.Extensions.Logging.LoggerFactory()
       .AddDebug((categoryName, logLevel) => (logLevel == LogLevel.Information) && (categoryName == DbLoggerCategory.Database.Command.Name))
      .AddConsole((categoryName, logLevel) => (logLevel == LogLevel.Information) && (categoryName == DbLoggerCategory.Database.Command.Name));
      public  ADreamDbContext aDreamDbContext = new ADreamDbContext();
        ~TestBase()
        {
            Dispose();
            GC.SuppressFinalize(this);
        }
        public virtual void Dispose()
        {
            aDreamDbContext.Dispose();
        }
    }
}
