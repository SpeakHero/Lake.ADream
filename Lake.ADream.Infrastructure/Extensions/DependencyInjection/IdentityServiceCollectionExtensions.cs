using Lake.ADream.Infrastructure.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lake.ADream.Infrastructure.Extensions.DependencyInjection
{

    /// <summary>
    /// Contains extension methods to <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> for configuring identity services.
    /// </summary>
    public static class IdentityServiceCollectionExtensions
    {
       ///
        public static void AddIdentityCore(this IServiceCollection services, Action<IdentityOptions> setupAction) 
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }
    }

}
