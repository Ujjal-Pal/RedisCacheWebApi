namespace RedisCacheWebApi.Services;

public interface IRedisCacheService
{
    T GetData<T>(string key);

    bool SetData<T>(string key, T value, DateTimeOffset expirationTime);

    bool RemoveData(string key);
}