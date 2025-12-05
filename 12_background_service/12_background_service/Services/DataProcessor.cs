namespace _12_background_service.Services;

public class DataProcessor : IDataProcessor
{
    private static int batchCounter = 0;
    public void ProcessBatch()
    {
        batchCounter++;
        Console.WriteLine($"Processing batch #{DataProcessor.batchCounter}");
    }
}
