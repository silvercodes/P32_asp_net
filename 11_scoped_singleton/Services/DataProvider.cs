namespace scoped_singleton.Services;

public class DataProvider : IDataProvider
{
    public string GetCurrentData()
    {
        return $"Current data: {DateTime.Now.ToLongTimeString()}";
    }
}