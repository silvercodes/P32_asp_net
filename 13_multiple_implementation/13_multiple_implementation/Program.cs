using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<IPaymentProcessor, StripeProcessor>();
builder.Services.AddTransient<IPaymentProcessor, BankProcessor>();
builder.Services.AddTransient<IPaymentProcessor, PayPalProcessor>();

builder.Services.AddScoped<OrderService>();

var app = builder.Build();


app.MapGet("/process", (OrderService os) => os.ProcessOrder(150.00m));

app.Run();


interface IPaymentProcessor
{
    string Process(decimal amount);
}

class StripeProcessor : IPaymentProcessor
{
    public string Process(decimal amount) =>
        $"Processed {amount} via Stripe";
}
class BankProcessor : IPaymentProcessor
{
    public string Process(decimal amount) =>
        $"Processed {amount} via Bank";
}
class PayPalProcessor : IPaymentProcessor
{
    public string Process(decimal amount) =>
        $"Processed {amount} via PayPal";
}

class OrderService
{
    private readonly IEnumerable<IPaymentProcessor> processors;
    public OrderService(IEnumerable<IPaymentProcessor> paymentProcessor)
    {
        processors = paymentProcessor;
    }

    public IEnumerable<string> ProcessOrder(decimal amount)
    {
        return processors.Select(p => p.Process(amount)).ToList();
    }
}