using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lake.ADream.Infrastructure.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serviceCollection"></param>
        /// <returns></returns>
        public static T GetService<T>(this IServiceCollection serviceCollection) where T : class
        {
            serviceCollection = serviceCollection ?? new ServiceCollection();
            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();
            var obj = serviceProvider.GetService<T>();
            return obj ?? default(T);
        }
    }
}
