using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lake.ADream.Infrastructure.Cache
{
    /// <summary>
    /// 
    /// </summary>
    public class MemoryCacheService 
    {
        static MemoryCache cache = new MemoryCache(new MemoryCacheOptions());
        /// <summary>
        /// 获取缓存值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private object GetCacheValue(string key)
        {
            object val = null;
            if (key != null && cache.TryGetValue(key, out val))
            {

                return val;
            }
            else
            {
                return default(object);
            }
        }
        /// <summary>
        /// 添加缓存内容
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void SetChacheValue(string key, object value)
        {
            if (key != null)
            {
                cache.Set(key, value, new MemoryCacheEntryOptions
                {
                    SlidingExpiration = TimeSpan.FromHours(1)
                });
            }
        }
    }
}
