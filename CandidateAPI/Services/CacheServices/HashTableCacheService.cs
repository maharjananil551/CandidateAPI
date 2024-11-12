using System;
using System.Collections.Concurrent;
using Microsoft.Extensions.Configuration;

public class HashTableCacheService : ICacheService
{
    private readonly ConcurrentDictionary<string, (object Value, DateTime Expiration)> _cache = new();
    private readonly TimeSpan _defaultExpiration;

    // Constructor that accepts IConfiguration to read app settings
    public HashTableCacheService(IConfiguration configuration)
    {
        // Reading the expiration time in minutes from appsettings.json
        var expirationMinutes = configuration.GetValue<int>("CacheSettings:ExpirationTimeInMinutes");
        _defaultExpiration = TimeSpan.FromMinutes(expirationMinutes);
    }

    public T Get<T>(string key)
    {
        if (_cache.TryGetValue(key, out var cachedItem) && cachedItem.Expiration > DateTime.UtcNow)
        {
            return (T)cachedItem.Value;
        }
        return default;
    }

    public void Set<T>(string key, T value, TimeSpan? expiration = null)
    {
        var cacheExpiration = expiration ?? _defaultExpiration;
        _cache[key] = (value, DateTime.UtcNow.Add(cacheExpiration));
    }

    public void Remove(string key)
    {
        _cache.TryRemove(key, out _);
    }

    public bool Exists(string key)
    {
        return _cache.TryGetValue(key, out var cachedItem) && cachedItem.Expiration > DateTime.UtcNow;
    }
}
