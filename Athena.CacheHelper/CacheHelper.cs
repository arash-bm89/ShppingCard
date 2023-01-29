using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Athena.CacheHelper;

public class CacheHelper : ICacheHelper
{
    private readonly IDatabase _db;
    private readonly ILogger<CacheHelper> _logger;

    public CacheHelper(IDatabase db, ILogger<CacheHelper> logger)
    {
        _db = db;
        _logger = logger;
    }

    public IOptions<CacheConfiguration> _options { get; set; }

    public async Task StoreAsync<T>(string key, T value, TimeSpan? expiration = null)
    {
        try
        {
            var valueString = JsonConvert.SerializeObject(value, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            await _db.StringSetAsync(key, valueString, expiration);
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
            var value = await _db.StringGetAsync(key);

            if (value.IsNull || !value.HasValue)
                return default;

            return JsonConvert.DeserializeObject<T>(value, new JsonSerializerSettings
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
        if (removeAt.HasValue)
            await _db.KeyExpireAsync(key, removeAt);
        else
            await _db.KeyDeleteAsync(key);
    }
}