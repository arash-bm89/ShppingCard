using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Athena.CacheHelper
{
    public class CacheHelper: ICacheHelper
    {
        private readonly IDatabase _db;
        public IOptions<CacheConfiguration> _options { get; set; } 
        private readonly ILogger<CacheHelper> _logger;

        public CacheHelper(IDatabase db, IOptions<CacheConfiguration> options, ILogger<CacheHelper> logger)
        {
            _db = db;
            _options = options;
            _logger = logger;
        }

        public async Task StoreAsync<T>(string key, T value, TimeSpan? expiration = null)
        {
            try
            {
                string finalKey = $"{_options.Value.Prefix} - {key}";

                var valueString = JsonConvert.SerializeObject(value, Formatting.Indented, new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
                await _db.StringSetAsync(finalKey, valueString, expiration);
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, "StoreAsync has exception.");
                throw new RedisException("Redis cache provider exception on Store", e.InnerException);
            }

        }

        public async Task<T?> FetchAsync<T>(string key)
        {
            try
            {
                string finalKey = $"{_options.Value.Prefix} - {key}";

                var value = await _db.StringGetAsync(finalKey);

                if (value.IsNull || !value.HasValue)
                    return default(T);

                return JsonConvert.DeserializeObject<T>(value, new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
            }
            catch (Exception e)
            {
                _logger.LogCritical(e, "GetAsync has exception.");
                throw new RedisException("Redis cache provider exception on Get", e.InnerException);
            }
        }

        public async Task RemoveAsync(string key, TimeSpan? removeAt = null)
        {
            string finalKey = $"{_options.Value.Prefix} - {key}";

            if (removeAt.HasValue)
            {
                await _db.KeyExpireAsync(finalKey, removeAt);
            }
            else
            {
                await _db.KeyDeleteAsync(finalKey);
            }
        }
    }
}
