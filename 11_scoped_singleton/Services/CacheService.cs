namespace scoped_singleton.Services;

public class CacheService: ICacheService
{
    private string cacheData = "Initial cache data";

    public string GetCachData() => cacheData;

    public void UpdateCache(string newData) => cacheData = newData;
}
