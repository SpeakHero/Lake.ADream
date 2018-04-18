using Lake.ADream.EntityFrameworkCore;
using Lake.ADream.Models.Entities.Identity;
using Lake.ADream.Services;
using Lake.ADream.Services.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Lake.ADream.Infrastucture.Test
{
    [TestClass]
    public class TestBase
    {
        //public  readonly ADreamDbContext aDreamDbContext;
        public readonly UserManagerService userManagerService;
        public readonly IServiceCollection services = new ServiceCollection();
        public readonly ServiceProvider serviceProvider;
        //UserStore
        public readonly ADreamDbContext aDreamDbContext;

        public TestBase()
        {
            services.AddDbContext<ADreamDbContext>();
            AddTransient();
            serviceProvider = services.BuildServiceProvider();
            aDreamDbContext = serviceProvider.GetService<ADreamDbContext>();
            userManagerService = serviceProvider.GetService<UserManagerService>();
        }
        public TResult GetService<TResult>() where TResult : class
        {
            return serviceProvider.GetService<TResult>();
        }
        public void AddTransient()
        {
            //Transient ServiceProvider总是创建一个新的服务实例。
            //Scoped ServiceProvider创建的服务实例由自己保存，（同一次请求）所以同一个ServiceProvider对象提供的服务实例均是同一个对象。
            //Singleton 始终是同一个实例对象
            services.AddMemoryCache()
               .AddDbContext<ADreamDbContext>(options => { })
           .AddSession(option => { })
           .AddOptions()
           .AddDistributedMemoryCache()
           .AddMvc();
            services.TryAddScoped<UserManagerService>();
            services.TryAddScoped<UserManagerService>();
            services.TryAddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
            services.TryAddScoped<IPasswordValidator<User>, PasswordValidator<User>>();
            services.TryAddScoped<IUserClaimsPrincipalFactory<User>, ADreamClaimsPrincipalFactory>();
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddLogging();
            services.AddResponseCaching();
            services.TryAddScoped<SignInManagerService>();

        }
    }
}
