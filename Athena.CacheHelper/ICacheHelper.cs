using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace Athena.CacheHelper
{
    public interface ICacheHelper
    {
        IOptions<CacheConfiguration> _options { get; set; }


        /// <summary>
        /// store in redis
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expiration"></param>
        /// <param name="ttl"></param>
        /// <returns></returns>
        Task StoreAsync<T>(string key, T value, TimeSpan? expiration = null);


        /// <summary>
        /// only fetch from database
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<T?> FetchAsync<T>(string key);


        /// <summary>
        /// remove from redis using key and at removeAt, by default in time.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="removeAt"></param>
        /// <returns></returns>
        Task RemoveAsync(string key, TimeSpan? removeAt = null);


    }
}
