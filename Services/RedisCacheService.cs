using System.Text.Json;
using StackExchange.Redis;

namespace RedisCacheWebApi.Services;

public class RedisCacheService : IRedisCacheService
{
    private readonly IDatabase _cacheDb;
    //private readonly IConfiguration _configuration;

    public RedisCacheService(IConfiguration configuration)
    {
        var redis = ConnectionMultiplexer.Connect(configuration.GetConnectionString("RedisCache"));
        _cacheDb = redis.GetDatabase();
    }
    public T GetData<T>(string key)
    {
        var data = _cacheDb.StringGet(key);

        if(!string.IsNullOrEmpty(data))
            return JsonSerializer.Deserialize<T>(data);
        return default;
    }

    public bool RemoveData(string key)
    {
        var existData = _cacheDb.KeyExists(key);

        if(existData)
            return _cacheDb.KeyDelete(key);
        return false;
    }

    public bool SetData<T>(string key, T value, DateTimeOffset expirationTime)
    {
        var expTime = expirationTime.DateTime.Subtract(DateTime.Now);

        return _cacheDb.StringSet(key, JsonSerializer.Serialize(value), expTime);
    }
}