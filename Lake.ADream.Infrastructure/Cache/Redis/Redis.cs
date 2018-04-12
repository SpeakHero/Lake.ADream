using Lake.ADream.Log4;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ServiceStack;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Lake.ADream.Infrastructure.Cache
{
    /// <summary>
    /// 
    /// </summary>
    public class Redis : ICache
    {
        string address;
        ConnectionMultiplexer connectionMultiplexer;
        IDatabase database;

        class CacheObject<T>
        {
            public int ExpireTime { get; set; }
            public bool ForceOutofDate { get; set; }
            public T Value { get; set; }
        }
        /// <summary>
        /// 
        /// </summary>
        public Redis()
        {
            address = ConfigHelper.GetJsonConfig().GetConnectionString("RedisConnection") ?? "127.0.0.1:6379";

            if (address == null || string.IsNullOrWhiteSpace(address))
            {
                var ex = new ApplicationException("配置文件中未找到RedisServer的有效配置");
                Loger.Error(ex);
                throw ex;
            }
            connectionMultiplexer = ConnectionMultiplexer.Connect(address);
            database = connectionMultiplexer.GetDatabase();
        }

        /// <summary>
        /// 连接超时设置
        /// </summary>
        public int TimeOut { get; set; } = 600;

        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object Get(string key)
        {
            return Get<object>(key);
        }
        /// <summary>
        /// 获取
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T Get<T>(string key) where T : class
        {

            DateTime begin = DateTime.Now;
            var cacheValue = database.StringGet(key);
            DateTime endCache = DateTime.Now;
            var value = default(T);
            if (!cacheValue.IsNull)
            {
                var cacheObject = JsonConvert.DeserializeObject<CacheObject<T>>(cacheValue);
                if (!cacheObject.ForceOutofDate)
                    database.KeyExpire(key, new TimeSpan(0, 0, cacheObject.ExpireTime));
                value = cacheObject.Value;
            }
            DateTime endJson = DateTime.Now;
            return value;

        }

        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        public async Task InsertAsync(string key, object data)
        {
            var jsonData = GetJsonData(data, TimeOut, false);
            await database.StringSetAsync(key, jsonData);
        }
        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <param name="cacheTime"></param>
        public async Task InsertAsync(string key, object data, int cacheTime)
        {
            var timeSpan = TimeSpan.FromSeconds(cacheTime);
            var jsonData = GetJsonData(data, TimeOut, true);
            await database.StringSetAsync(key, jsonData, timeSpan);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <param name="cacheTime"></param>
        public async Task InsertAsync(string key, object data, DateTime cacheTime)
        {
            var timeSpan = cacheTime - DateTime.Now;
            var jsonData = GetJsonData(data, TimeOut, true);
            await database.StringSetAsync(key, jsonData, timeSpan);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="data"></param>
        public async Task InsertAsync<T>(string key, T data) where T : class
        {
            var jsonData = GetJsonData<T>(data, TimeOut, false);
            await database.StringSetAsync(key, jsonData);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <param name="cacheTime"></param>
        public async Task InsertAsync<T>(string key, T data, int cacheTime) where T : class
        {
            var timeSpan = TimeSpan.FromSeconds(cacheTime);
            var jsonData = GetJsonData(data, TimeOut, true);
            await database.StringSetAsync(key, jsonData, timeSpan);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <param name="cacheTime"></param>
        public async Task InsertAsync<T>(string key, T data, DateTime cacheTime) where T : class
        {
            var timeSpan = cacheTime - DateTime.Now;
            var jsonData = GetJsonData<T>(data, TimeOut, true);
            await database.StringSetAsync(key, jsonData, timeSpan);
        }


        string GetJsonData(object data, int cacheTime, bool forceOutOfDate)
        {
            var cacheObject = new CacheObject<object>() { Value = data, ExpireTime = cacheTime, ForceOutofDate = forceOutOfDate };
            return cacheObject.ToJson();//序列化对象
        }

        string GetJsonData<T>(T data, int cacheTime, bool forceOutOfDate)
        {
            var cacheObject = new CacheObject<T>() { Value = data, ExpireTime = cacheTime, ForceOutofDate = forceOutOfDate };
            return cacheObject.ToJson();//序列化对象
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="key"></param>
        public async Task RemoveAsync(string key)
        {
            await database.KeyDeleteAsync(key, CommandFlags.HighPriority);
        }

        /// <summary>
        /// 
        /// </summary>
        public async Task Clear()
        {
            await database.ExecuteAsync("flushdb");
        }
        /// <summary>
        /// 判断key是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<bool> ExistsAsnyc(string key)
        {
            return await database.KeyExistsAsync(key);
        }
    }
}
