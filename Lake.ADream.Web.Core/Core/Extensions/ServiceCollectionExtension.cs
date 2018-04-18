using Lake.ADream.EntityFrameworkCore;
using Lake.ADream.Models.Entities.Identity;
using Lake.ADream.Services;
using Lake.ADream.Services.Identity;
using Lake.ADream.Web.Core.ViewExpand;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lake.ADream.Web.Core.Core.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddADream(this IServiceCollection services, IConfiguration configuration)
        {
        
            services.AddMemoryCache()
                .AddDbContext<ADreamDbContext>()
                    .AddDistributedServiceStackRedisCache(option =>
            {
                option.Host = configuration.GetConnectionString("RedisConnection");
                option.Port = 6379;
            })
            .AddSession(option => { })
            .AddOptions()
            .AddDistributedMemoryCache()
            .Configure<RazorViewEngineOptions>(options =>
            {
                options.ViewLocationExpanders.Add(new ThemeViewLocationExpander());
            })
            .AddApplicationInsightsTelemetry(configuration).AddMvc();
          
            services.AddScoped<RoleManagerService>();
            services.AddLogging();
            services.AddResponseCaching();
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            return services;
        }
     
    }
}
