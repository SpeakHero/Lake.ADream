﻿using log4net;
using log4net.Util;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Lake.ADream.Infrastructure.Cache
{
    /// <summary>
    /// 缓存
    /// </summary>
    public static class Cache
    {
        private static object cacheLocker = new object();//缓存锁对象
        private static ICache cache = null;//缓存接口

        static Cache()
        {
            Load();
        }

        /// <summary>
        /// 加载缓存
        /// </summary>
        private static void Load()
        {
            try
            {
                cache = cache ?? new Redis();
            }
            catch (Exception ex)
            {
                LogLog.Error(ex.GetType(), ex.Message);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static ICache GetCache()
        {
            return cache;
        }


        /// <summary>
        /// 缓存过期时间
        /// </summary>
        public static int TimeOut
        {
            get
            {
                return cache.TimeOut;
            }
            set
            {
                lock (cacheLocker)
                {
                    cache.TimeOut = value;
                }
            }
        }

        /// <summary>
        /// 获得指定键的缓存值
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>缓存值</returns>
        public static object Get(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                return null;
            return cache.Get(key);
        }

        /// <summary>
        /// 获得指定键的缓存值
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <returns>缓存值</returns>
        public static T Get<T>(string key) where T : class
        {
            return cache.Get<T>(key);
        }

        /// <summary>
        /// 将指定键的对象添加到缓存中
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="data">缓存值</param>
        public static void Insert(string key, object data)
        {
            if (string.IsNullOrWhiteSpace(key) || data == null)
                return;
            lock (cacheLocker)
            {
                cache.InsertAsync(key, data);
            }
        }
        /// <summary>
        /// 将指定键的对象添加到缓存中
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="data">缓存值</param>
        public static void Insert<T>(string key, T data) where T : class
        {
            if (string.IsNullOrWhiteSpace(key) || data == null)
                return;
           lock (cacheLocker)
            {
                cache.InsertAsync<T>(key, data);
            }
        }
        /// <summary>
        /// 将指定键的对象添加到缓存中，并指定过期时间
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="data">缓存值</param>
        /// <param name="cacheTime">缓存过期时间(分钟)</param>
        public static void Insert(string key, object data, int cacheTime)
        {
            if (!string.IsNullOrWhiteSpace(key) && data != null)
            {
               lock (cacheLocker)
                {
                    cache.InsertAsync(key, data, cacheTime);
                }
            }
        }

        /// <summary>
        /// 将指定键的对象添加到缓存中，并指定过期时间
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="data">缓存值</param>
        /// <param name="cacheTime">缓存过期时间(分钟)</param>
        public static void Insert<T>(string key, T data, int cacheTime) where T : class
        {
            if (!string.IsNullOrWhiteSpace(key) && data != null)
            {
                lock (cacheLocker)
                {
                    cache.InsertAsync(key, data, cacheTime);
                }
            }
        }

        /// <summary>
        /// 将指定键的对象添加到缓存中，并指定过期时间
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="data">缓存值</param>
        /// <param name="cacheTime">缓存过期时间</param>
        public static void Insert(string key, object data, DateTime cacheTime)
        {
            if (!string.IsNullOrWhiteSpace(key) && data != null)
            {
                lock (cacheLocker)
                {
                    cache.InsertAsync(key, data, cacheTime);
                }
            }
        }

        /// <summary>
        /// 将指定键的对象添加到缓存中，并指定过期时间
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="data">缓存值</param>
        /// <param name="cacheTime">缓存过期时间</param>
        public static void Insert<T>(string key, T data, DateTime cacheTime)where T :class
        {
            if (!string.IsNullOrWhiteSpace(key) && data != null)
            {
                lock (cacheLocker)
                {
                    cache.InsertAsync<T>(key, data, cacheTime);
                }
            }
        }

        /// <summary>
        /// 从缓存中移除指定键的缓存值
        /// </summary>
        /// <param name="key">缓存键</param>
        public static void Remove(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                return;
            lock (cacheLocker)
            {
                cache.RemoveAsync(key);
            }
        }

        /// <summary>
        /// 判断key是否存在
        /// </summary>
        public static async Task<bool> Exists(string key)
        {
            return await cache.ExistsAsnyc(key);
        }

    }
}
