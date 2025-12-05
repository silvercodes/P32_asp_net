namespace scoped_singleton.Services;

public class CacheUpdater
{
    private readonly ICacheService cache;
    public CacheUpdater(ICacheService cache)
    {
        this.cache = cache;
    }

    public void RefreshCache(IDataProvider dataProvider)
    {
        string freshData = dataProvider.GetCurrentData();
        cache.UpdateCache(freshData);
    }
}
