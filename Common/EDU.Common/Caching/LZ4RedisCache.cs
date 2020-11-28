
using CachingFramework.Redis;
using EDU.Common.Caching.Interfaces;
using MessagePack;
using Microsoft.Extensions.Configuration;
using NLog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EDU.Common.Caching
{
    public class LZ4RedisCache : ICacheProvider
    {
        private const int DefaultExpired = 24 * 60 * 60;
        private static string RedisHost;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        public LZ4RedisCache(IConfiguration configuration)
        { 
            RedisHost = configuration["RedisConfig:RedisHost"];
        }

        #region sync
        public bool Add(int dbNumber, string key, object value)
        {
            try
            {
                using (var context = new Context(RedisHost + ",defaultDatabase=" + dbNumber))
                {
                    var bytes = LZ4MessagePackSerializer.Serialize(value);
                    context.Cache.SetObject<byte[]>(key, bytes, TimeSpan.FromSeconds(DefaultExpired));
                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error($"Redis Add exception {key} key {0}, error {1}, inner {2}", key, ex.Message, ex.InnerException);
                return false;
            }
        }

        public bool Add(int dbNumber, string key, object value, int expiresAfter)
        {
            try
            {
                using (var context = new Context(RedisHost + ",defaultDatabase=" + dbNumber))
                {
                    var bytes = LZ4MessagePackSerializer.Serialize(value);
                    context.Cache.SetObject<byte[]>(key, bytes, TimeSpan.FromSeconds(expiresAfter));
                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error("Redis Add exception key {0}, error {1}, inner {2}", key, ex.Message, ex.InnerException);
                return false;
            }
        }

        public bool Add(int dbNumber, string key, object value, DateTime expiresAt)
        {
            try
            {
                using (var context = new Context(RedisHost + ",defaultDatabase=" + dbNumber))
                {
                    var bytes = LZ4MessagePackSerializer.Serialize(value);
                    context.Cache.SetObject<byte[]>(key, bytes, TimeSpan.FromSeconds((expiresAt - DateTime.Now).TotalSeconds));
                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error("Redis Add exception key {0}, error {1}, inner {2}", key, ex.Message, ex.InnerException);
                return false;
            }
        }

        public T Get<T>(int dbNumber, string key)
        {
            try
            {
                using (var context = new Context(RedisHost + ",defaultDatabase=" + dbNumber))
                {
                    var bytes = context.Cache.GetObject<byte[]>(key);
                    if (null == bytes)
                        return default(T);
                    var obj = LZ4MessagePackSerializer.Deserialize<T>(bytes);

                    return obj;
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Redis Get<T> exception key {0}, error {1}, inner {2}", key, ex.Message, ex.InnerException);
            }
            return default(T);
        }

        public bool Remove(int dbNumber, string key)
        {
            try
            {
                using (var context = new Context(RedisHost + ",defaultDatabase=" + dbNumber))
                {
                    context.Cache.Remove(key);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Redis Remove exception key {0}, error {1}, inner {2}", key, ex.Message, ex.InnerException);
                return false;
            }
        }

        public bool SetExpires(int dbNumber, string key, DateTime expiresAt)
        {
            try
            {
                if (expiresAt < DateTime.Now)
                {
                    Logger.Error("Redis SetExpires Invalid {0} < now {1}", expiresAt, DateTime.Now);
                    return false;
                }
                using (var context = new Context(RedisHost + ",defaultDatabase=" + dbNumber))
                {
                    return context.Cache.KeyExpire(key, expiresAt);
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Redis SetExpires exception key {0}, error {1}, inner {2}", key, ex.Message, ex.InnerException);
                return false;
            }
        }

        public bool SetExpires(int dbNumber, string key, int expiresAfter)
        {
            try
            {
                var expiresAt = DateTime.Now.AddSeconds(expiresAfter);
                return SetExpires(dbNumber, key, expiresAt);
            }
            catch (Exception ex)
            {
                Logger.Error("Redis SetExpires exception key {0}, error {1}, inner {2}", key, ex.Message, ex.InnerException);
                return false;
            }
        }
        public bool SetExpires(int dbNumber, string key, string field, int expiresAfter)
        {
            try
            {
                var value = GetHashed<string>(dbNumber, key, field);
                if (string.IsNullOrEmpty(value))
                    return false;
                RemoveHashed(dbNumber, key, field);
                SetHashedField(dbNumber, key, field, value, expiresAfter);
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error("Redis SetExpires exception key {0}, error {1}, inner {2}", key, ex.Message, ex.InnerException);
                return false;
            }
        }

        public bool SetHashed<T>(int dbNumber, string key, IDictionary<string, T> fieldValues)
        {
            try
            {
                Dictionary<string, byte[]> result = new Dictionary<string, byte[]>();
                using (var context = new Context(RedisHost + ",defaultDatabase=" + dbNumber))
                {
                    foreach (var obj in fieldValues)
                    {
                        var bytes = LZ4MessagePackSerializer.Serialize(obj.Value);
                        result.Add(obj.Key, bytes);
                    }
                    if (null != result)
                    {
                        context.Cache.SetHashed(key, result);
                        return true;
                    }
                    return false;
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Redis SetHashed exception key {0}, error {1}, inner {2}", key, ex.Message, ex.InnerException);
                //_errorHandler.Process(CachingFunc.SetHashed, dbNumber, key, fieldValues, false);
                return false;
            }
        }

        public bool SetHashedField<T>(int dbNumber, string key, string field, T value, int expiresAfter)
        {
            try
            {
                using (var context = new Context(RedisHost + ",defaultDatabase=" + dbNumber))
                {
                    var bytes = LZ4MessagePackSerializer.Serialize(value);
                    context.Cache.SetHashed<byte[]>(key, field, bytes, TimeSpan.FromSeconds(expiresAfter));
                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error("Redis SetHashed exception key {0}, error {1}, inner {2}", key, ex.Message, ex.InnerException);
                //_errorHandler.Process(CachingFunc.SetHashed, dbNumber, key, field, value, expiresAfter, false);
                return false;
            }
        }

        public bool SetUniqueHashedField<T>(int dbNumber, string key, string field, T value, int expiresAfter)
        {
            try
            {
                using (var context = new Context(RedisHost + ",defaultDatabase=" + dbNumber))
                {
                    var uniqueField = field.Split('_')[0];
                    RemoveUniqueField<T>(context, dbNumber, key, uniqueField);
                    var bytes = LZ4MessagePackSerializer.Serialize(value);
                    context.Cache.SetHashed<byte[]>(key, field, bytes, TimeSpan.FromSeconds(expiresAfter));
                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error("Redis SetUniqueHashedField exception key {0}, error {1}, inner {2}", key, ex.Message, ex.InnerException);
                //_errorHandler.Process(CachingFunc.SetHashed, dbNumber, key, field, value, expiresAfter, false);
                return false;
            }
        }

        private void RemoveUniqueField<T>(Context context, int dbNumber, string key, string uniqueField)
        {
            if (!string.IsNullOrEmpty(uniqueField))
            {
                var fields = GetHashedAll<T>(dbNumber, key);
                if (fields.Count > 0)
                {
                    foreach (var item in fields)
                    {
                        if (item.Key.StartsWith(uniqueField))
                            context.Cache.RemoveHashed(key, item.Key);
                    }
                }
            }
        }

        public bool RemoveUniqueHashed<T>(int dbNumber, string key, string field)
        {
            try
            {
                using (var context = new Context(RedisHost + ",defaultDatabase=" + dbNumber))
                {
                    field = field.Replace(',', ';');
                    var arr = field.Split(';');
                    foreach (var item in arr)
                    {
                        var uniqueField = item.Split('_')[0];
                        RemoveUniqueField<T>(context, dbNumber, key, uniqueField);
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Redis RemoveUniqueHashed exception key {0}, error {1}, inner {2}", key, ex.Message, ex.InnerException);
                //_errorHandler.Remove(CachingFunc.RemoveHashed, dbNumber, key, field, false);
                return false;
            }
        }

        public bool RemoveHashed(int dbNumber, string key, string field)
        {
            try
            {
                using (var context = new Context(RedisHost + ",defaultDatabase=" + dbNumber))
                {
                    field = field.Replace(',', ';');
                    var arr = field.Split(';');
                    foreach(var item in arr)
                    {
                        context.Cache.RemoveHashed(key, item);
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Redis RemoveHashed exception key {0}, error {1}, inner {2}", key, ex.Message, ex.InnerException);
                //_errorHandler.Remove(CachingFunc.RemoveHashed, dbNumber, key, field, false);
                return false;
            }
        }
        public T GetHashed<T>(int dbNumber, string key, string field)
        {
            try
            {
                using (var context = new Context(RedisHost + ",defaultDatabase=" + dbNumber))
                {
                    var bytes = context.Cache.GetHashed<byte[]>(key, field);
                    if (null == bytes)
                        return default(T);
                    var obj = LZ4MessagePackSerializer.Deserialize<T>(bytes);

                    return obj;
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Redis GetHashed<T> exception key {0}, error {1}, inner {2}", key, ex.Message, ex.InnerException);
                return default(T);
            }
        }

        public IDictionary<string, T> GetHashedAll<T>(int dbNumber, string key)
        {
            try
            {
                Dictionary<string, T> result = new Dictionary<string, T>();
                using (var context = new Context(RedisHost + ",defaultDatabase=" + dbNumber))
                {
                    var hash = context.Cache.GetHashedAll<byte[]>(key);
                    foreach (var obj in hash)
                    {
                        var item = LZ4MessagePackSerializer.Deserialize<T>(obj.Value);
                        result.Add(obj.Key, item);
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                Logger.Error("Redis GetHashedAll<T> exception key {0}, error {1}, inner {2}", key, ex.Message, ex.InnerException);
                return default(IDictionary<string, T>);
            }
        }

        public IList<T> GetHashedList<T>(int dbNumber, string key)
        {
            try
            {
                IList<T> result = new List<T>();
                using (var context = new Context(RedisHost + ",defaultDatabase=" + dbNumber))
                {
                    var hash = context.Cache.GetHashedAll<byte[]>(key);
                    foreach (var obj in hash)
                    {
                        var item = LZ4MessagePackSerializer.Deserialize<T>(obj.Value);
                        result.Add(item);
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                Logger.Error("Redis GetHashedList<T> exception key {0}, error {1}, inner {2}", key, ex.Message, ex.InnerException);
                return default(IList<T>);
            }
        }

        public bool FlushAll(int dbNumber)
        {
            try
            {
                using (var context = new Context(RedisHost + ",defaultDatabase=" + dbNumber))
                {
                    context.Cache.FlushAll();
                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error("Redis FlushAll exception db {0}, error {1}, inner {2}", dbNumber, ex.Message, ex.InnerException);
                return false;
            }
        }
        #endregion sync


        #region async
        public async Task<bool> AddAsync(int dbNumber, string key, object value)
        {
            try
            {
                using (var context = new Context(RedisHost + ",defaultDatabase=" + dbNumber))
                {
                    var bytes = LZ4MessagePackSerializer.Serialize(value);
                    await context.Cache.SetObjectAsync<byte[]>(key, bytes, TimeSpan.FromSeconds(DefaultExpired));
                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error("Redis Add exception key {0}, error {1}, inner {2}", key, ex.Message, ex.InnerException);
                return false;
            }
        }

        public async Task<bool> AddAsync(int dbNumber, string key, object value, int expiresAfter)
        {
            try
            {
                using (var context = new Context(RedisHost + ",defaultDatabase=" + dbNumber))
                {
                    var bytes = LZ4MessagePackSerializer.Serialize(value);
                    await context.Cache.SetObjectAsync<byte[]>(key, bytes, TimeSpan.FromSeconds(expiresAfter));
                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error("Redis Add exception key {0}, error {1}, inner {2}", key, ex.Message, ex.InnerException);
                return false;
            }
        }

        public async Task<bool> AddAsync(int dbNumber, string key, object value, DateTime expiresAt)
        {
            try
            {
                using (var context = new Context(RedisHost + ",defaultDatabase=" + dbNumber))
                {
                    var bytes = LZ4MessagePackSerializer.Serialize(value);
                    await context.Cache.SetObjectAsync<byte[]>(key, bytes, TimeSpan.FromSeconds((expiresAt - DateTime.Now).TotalSeconds));
                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error("Redis Add exception key {0}, error {1}, inner {2}", key, ex.Message, ex.InnerException);
                return false;
            }
        }

        public async Task<T> GetAsync<T>(int dbNumber, string key)
        {
            try
            {
                using (var context = new Context(RedisHost + ",defaultDatabase=" + dbNumber))
                {
                    var bytes = await context.Cache.GetObjectAsync<byte[]>(key);
                    if (null == bytes)
                        return default(T);
                    var obj = LZ4MessagePackSerializer.Deserialize<T>(bytes);

                    return obj;
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Redis Get<T> exception key {0}, error {1}, inner {2}", key, ex.Message, ex.InnerException);
            }
            return default(T);
        }

        public async Task<bool> RemoveAsync(int dbNumber, string key)
        {
            try
            {
                using (var context = new Context(RedisHost + ",defaultDatabase=" + dbNumber))
                {
                    return await context.Cache.RemoveAsync(key);
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Redis Remove exception key {0}, error {1}, inner {2}", key, ex.Message, ex.InnerException);
                //await _errorHandler.Remove(CachingFunc.RemoveAsync, dbNumber, key, true);
                return false;
            }
        }

        public async Task<bool> SetExpiresAsync(int dbNumber, string key, DateTime expiresAt)
        {
            try
            {
                if (expiresAt < DateTime.Now)
                {
                    Logger.Error("Redis SetExpires Invalid {0} < now {1}", expiresAt, DateTime.Now);
                    return false;
                }
                using (var context = new Context(RedisHost + ",defaultDatabase=" + dbNumber))
                {
                    return await context.Cache.KeyExpireAsync(key, expiresAt);
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Redis SetExpires exception key {0}, error {1}, inner {2}", key, ex.Message, ex.InnerException);
                return false;
            }
        }

        public async Task<bool> SetExpiresAsync(int dbNumber, string key, int expiresAfter)
        {
            try
            {
                var expiresAt = DateTime.Now.AddSeconds(expiresAfter);
                return await SetExpiresAsync(dbNumber, key, expiresAt);
            }
            catch (Exception ex)
            {
                Logger.Error("Redis SetExpires exception key {0}, error {1}, inner {2}", key, ex.Message, ex.InnerException);
                return false;
            }
        }

        public async Task<bool> SetExpiresAsync(int dbNumber, string key, string field, int expiresAfter)
        {
            try
            {
                var value = await GetHashedAsync<string>(dbNumber, key, field);
                if (string.IsNullOrEmpty(value))
                    return false;
                await RemoveHashedAsync(dbNumber, key, field);
                await SetHashedFieldAsync(dbNumber, key, field, value, expiresAfter);
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error("Redis SetExpires exception key {0}, error {1}, inner {2}", key, ex.Message, ex.InnerException);
                return false;
            }
        }


        public async Task<bool> SetHashedAsync<T>(int dbNumber, string key, IDictionary<string, T> fieldValues)
        {
            try
            {
                Dictionary<string, byte[]> result = new Dictionary<string, byte[]>();
                using (var context = new Context(RedisHost + ",defaultDatabase=" + dbNumber))
                {
                    foreach (var obj in fieldValues)
                    {
                        var bytes = LZ4MessagePackSerializer.Serialize(obj.Value);
                        result.Add(obj.Key, bytes);
                    }
                    if (null != result)
                    {
                        await context.Cache.SetHashedAsync(key, result);
                        return true;
                    }
                    return false;
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Redis SetHashedAsync exception key {0}, error {1}, inner {2}", key, ex.Message, ex.InnerException);
                //await _errorHandler.Process(CachingFunc.SetHashedAsync, dbNumber, key, fieldValues, true);
                return false;
            }
        }

        public async Task<bool> SetHashedFieldAsync<T>(int dbNumber, string key, string field, T value, int expiresAfter)
        {
            try
            {
                Logger.Info($"Start revice request data store {key}");
                using (var context = new Context(RedisHost + ",defaultDatabase=" + dbNumber))
                {
                    var bytes = LZ4MessagePackSerializer.Serialize(value);
                    await context.Cache.SetHashedAsync<byte[]>(key, field, bytes, TimeSpan.FromSeconds(expiresAfter));
                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error("Redis SetHashedAsync exception key {0}, error {1}, inner {2}", key, ex.Message, ex.InnerException);
                //await _errorHandler.Process(CachingFunc.SetHashedAsync, dbNumber, key, field, value, expiresAfter, true);
                return false;
            }
        }

        public async Task<bool> RemoveHashedAsync(int dbNumber, string key, string field)
        {
            try
            {
                using (var context = new Context(RedisHost + ",defaultDatabase=" + dbNumber))
                {
                    field = field.Replace(',', ';');
                    var arr = field.Split(';');
                    foreach (var item in arr)
                    {
                        await context.Cache.RemoveHashedAsync(key, field);
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Redis RemoveHashedAsync exception key {0}, error {1}, inner {2}", key, ex.Message, ex.InnerException);
                //await _errorHandler.Remove(CachingFunc.RemoveHashedAsync, dbNumber, key, field, true);
                return false;
            }
        }

        public async Task<T> GetHashedAsync<T>(int dbNumber, string key, string field)
        {
            try
            {
                using (var context = new Context(RedisHost + ",defaultDatabase=" + dbNumber))
                {
                    var bytes = await context.Cache.GetHashedAsync<byte[]>(key, field);
                    if (null == bytes)
                        return default(T);
                    var obj = LZ4MessagePackSerializer.Deserialize<T>(bytes);

                    return obj;
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Redis GetHashed<T> exception key {0}, error {1}, inner {2}", key, ex.Message, ex.InnerException);
                return default(T);
            }
        }

        public async Task<IDictionary<string, T>> GetHashedAllAsync<T>(int dbNumber, string key)
        {
            try
            {
                Dictionary<string, T> result = new Dictionary<string, T>();
                using (var context = new Context(RedisHost + ",defaultDatabase=" + dbNumber))
                {
                    var hash = await context.Cache.GetHashedAllAsync<byte[]>(key);
                    foreach (var obj in hash)
                    {
                        var item = LZ4MessagePackSerializer.Deserialize<T>(obj.Value);
                        result.Add(obj.Key, item);
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                Logger.Error("Redis GetHashedAllAsync<T> exception key {0}, error {1}, inner {2}", key, ex.Message, ex.InnerException);
                return default(IDictionary<string, T>);
            }
        }
        public async Task<bool> FlushAllAsync(int dbNumber)
        {
            try
            {
                using (var context = new Context(RedisHost + ",defaultDatabase=" + dbNumber))
                {
                    await context.Cache.FlushAllAsync();
                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error("Redis FlushAllAsync exception db {0}, error {1}, inner {2}", dbNumber, ex.Message, ex.InnerException);
                return false;
            }
        }

        public async Task<IList<T>> GetHashedListAsync<T>(int dbNumber, string key)
        {
            try
            {
                IList<T> result = new List<T>();
                using (var context = new Context(RedisHost + ",defaultDatabase=" + dbNumber))
                {
                    var hash = await context.Cache.GetHashedAllAsync<byte[]>(key);
                    foreach (var obj in hash)
                    {
                        var item = LZ4MessagePackSerializer.Deserialize<T>(obj.Value);
                        result.Add(item);
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                Logger.Error("Redis GetHashedListAsync<T> exception key {0}, error {1}, inner {2}", key, ex.Message, ex.InnerException);
                return default(IList<T>);
            }
        }
        #endregion async
    }
}