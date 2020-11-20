using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EDU.Common.Caching.Interfaces
{
    public interface ICacheProviderAsync
    {
        /// <summary>
        /// Set the value of key
        /// </summary>
        /// <param name="dbNumber">The number of db</param>
        /// <param name="key">The cache key</param>
        /// <param name="value">The value</param>
        /// <returns>The key was store</returns>
        Task<bool> AddAsync(int dbNumber, string key, object value);
        /// <summary>
        /// Set the value of key with expires time
        /// </summary>
        /// <param name="dbNumber">The number of db</param>
        /// <param name="key">The cache key</param>
        /// <param name="value">The value</param>
        /// <param name="expiresAt">The expired time</param>
        /// <returns>The key was store</returns>
        Task<bool> AddAsync(int dbNumber, string key, object value, DateTime expiresAt);
        /// <summary>
        /// Set the value of key with expires after time span
        /// </summary>
        /// <param name="dbNumber">The number of db</param>
        /// <param name="key">The cache key</param>
        /// <param name="value">The value</param>
        /// <param name="expiresAfter">The expired time by second</param>
        /// <returns>The key was store</returns>
        Task<bool> AddAsync(int dbNumber, string key, object value, int expiresAfter);
        /// <summary>
        /// Re set expires time of key
        /// </summary>
        /// <param name="dbNumber">The db number</param>
        /// <param name="key">The cache key</param>
        /// <param name="expiresAt">The expired time</param>
        /// <returns>The expires time was store</returns>
        Task<bool> SetExpiresAsync(int dbNumber, string key, DateTime expiresAt);
        /// <summary>
        /// Re set expires time of key after time span
        /// </summary>
        /// <param name="dbNumber">The db number</param>
        /// <param name="key">The cache key</param>
        /// <param name="expiresAfter">The expired time by second</param>
        /// <returns>The expires time was store</returns>
        Task<bool> SetExpiresAsync(int dbNumber, string key, int expiresAfter);
        /// <summary>
        /// Re set expires time of key after time span for hash
        /// </summary>
        /// <param name="dbNumber">The db number</param>
        /// <param name="key">The cache key</param>
        /// <param name="field">The field</param>
        /// <param name="expiresAfter">The expired time by second</param>
        /// <returns>The expires time was store</returns>
        Task<bool> SetExpiresAsync(int dbNumber, string key, string field, int expiresAfter);
        /// <summary>
        /// Get value by key
        /// </summary>
        /// <typeparam name="T">Type of value</typeparam>
        /// <param name="dbNumber">The db number</param>
        /// <param name="key">The cache key</param>
        /// <returns>The value of key</returns>
        Task<T> GetAsync<T>(int dbNumber, string key);
        /// <summary>
        /// Remove cache key
        /// </summary>
        /// <param name="dbNumber">The db number</param>
        /// <param name="key">The cache key</param>
        /// <returns>The key was remove</returns>
        Task<bool> RemoveAsync(int dbNumber, string key);
        /// Sets the specified key/values pairs to a hashset.
        /// </summary>
        /// <typeparam name="T">Type of object</typeparam>
        /// <param name="dbNumber">The db number</param>
        /// <param name="key">The cache key</param>
        /// <param name="fieldValues">The field keys and values to store</param>
        /// <returns>The hash table was store</returns>
        Task<bool> SetHashedAsync<T>(int dbNumber, string key, IDictionary<string, T> fieldValues);
        /// <summary>
        /// Sets the specified value to a hashset using the pair hashKey+field.
        /// </summary>
        /// <typeparam name="T">Type of value</typeparam>
        /// <param name="dbNumber">The db number</param>
        /// <param name="key">The cache key</param>
        /// <param name="field">The field key</param>
        /// <param name="value">The value to store</param>
        /// <param name="expiresAfter">The expired time by second</param>
        /// <returns>The field of hash table was store</returns>
        Task<bool> SetHashedFieldAsync<T>(int dbNumber, string key, string field, T value, int expiresAfter);
        /// <summary>
        /// Remove field from hash key
        /// </summary>
        /// <param name="dbNumber">The db number</param>
        /// <param name="key">The cache key</param>
        /// <param name="field">The field key</param>
        /// <returns>The key was remove</returns>
        Task<bool> RemoveHashedAsync(int dbNumber, string key, string field);
        /// <summary>
        /// Get field value from hash key
        /// </summary>
        /// <typeparam name="T">Type of value</typeparam>
        /// <param name="dbNumber">The db number</param>
        /// <param name="key">The cache key</param>
        /// <param name="field">The field key</param>
        /// <returns>The value of field</returns>
        Task<T> GetHashedAsync<T>(int dbNumber, string key, string field);
        /// <summary>
        /// Get hash key to IDictionary
        /// </summary>
        /// <typeparam name="T">Type of value</typeparam>
        /// <param name="dbNumber">The db number</param>
        /// <param name="key">The cache key</param>
        /// <returns>The value of key</returns>
        Task<IDictionary<string, T>> GetHashedAllAsync<T>(int dbNumber, string key);
        /// <summary>
        /// Get hash key value to list object
        /// </summary>
        /// <typeparam name="T">Type of value</typeparam>
        /// <param name="dbNumber">The db number</param>
        /// <param name="key">The cache key</param>
        /// <returns>The value of field</returns>
        Task<bool> FlushAllAsync(int dbNumber);
        /// <summary>
        /// Flush all db with administrator permision
        /// </summary>
        /// <param name="dbNumber">The db number</param>
        /// <returns>The db was flush</returns>
        Task<IList<T>> GetHashedListAsync<T>(int dbNumber, string key);
    }
}