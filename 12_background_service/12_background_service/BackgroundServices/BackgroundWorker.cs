using _12_background_service.Services;

namespace _12_background_service.BackgroundServices;

public class BackgroundWorker
{
    private readonly IServiceProvider serviceProvider;
    private readonly Timer timer;

    public BackgroundWorker(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
        timer = new Timer(Execute, null, 0, 3000);
    }

    private void Execute(object? state)
    {
        using var scope = serviceProvider.CreateScope();

        var processor = scope.ServiceProvider.GetRequiredService<IDataProcessor>();

        processor.ProcessBatch();
    }
}
