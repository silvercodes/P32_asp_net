//var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddControllers();
//builder.Services.AddRazorPages();
//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowAll", builder =>
//    {
//        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
//    });
//});


//var app = builder.Build();

//app.MapGet("/", () => "Hello World!");

//app.Run();


#region Embedded services list

//using System.Text;

//var builder = WebApplication.CreateBuilder(args);
//var services = builder.Services;

//var app = builder.Build();

//app.MapGet("/", async ctx =>
//{
//    int count = 0;
//    var sb = new StringBuilder();
//    sb.Append("<table>");
//    sb.Append("<tr><th>#</th><th>Abstraction</th><th>Implementation</th></tr>");

//    foreach (var item in services)
//    {
//        sb.Append("<tr>");
//        sb.Append($"<td>{++count}</td>");
//        sb.Append($"<td>{item.ServiceType.Name}</td>");
//        sb.Append($"<td>{item.ImplementationType?.Name}</td>");
//        sb.Append("</tr>");
//    }
//    sb.Append("</table>");

//    ctx.Response.ContentType = "text/html";
//    await ctx.Response.WriteAsync(sb.ToString());
//});

//app.Run();



#endregion


#region Example_1

//var builder = WebApplication.CreateBuilder(args);
//builder.Services.AddSingleton<IRandomService, RandomService>();

//var app = builder.Build();

//app.Use(async (ctx, next) =>
//{
//    ctx.Response.ContentType = "text/plain;charset=utf-8";

//    await next();
//});

//app.MapGet("/random", async (HttpContext ctx, IRandomService rnd) =>
//{
//    await ctx.Response.WriteAsync($"count: {rnd.Count}, random: {rnd.NextValue(1, 99)}");
//});

//app.MapGet("/day", async (HttpContext ctx, IRandomService rnd) =>
//{
//    await ctx.Response.WriteAsync($"count: {rnd.Count}, day: {rnd.NextValue(1, 7)}");
//});

//app.MapGet("/coin", async (HttpContext ctx, IRandomService rnd) =>
//{
//    string res = rnd.NextValue(0, 2) == 0 ? "Орёл" : "Решка";
//    await ctx.Response.WriteAsync($"count: {rnd.Count}, coin: {res}");
//});

//app.Run();

//public interface IRandomService
//{
//    public int Count { get; set; }
//    int NextValue(int min, int max); 
//}

//public class RandomService : IRandomService
//{
//    public int Count { get; set; } = 0;
//    private readonly Random random = new Random();
//    public int NextValue(int min, int max)
//    {
//        ++Count;
//        return random.Next(min, max);
//    }
//}

#endregion


#region HttpClient

//var builder = WebApplication.CreateBuilder(args);
//builder.Services.AddHttpClient();
//builder.Services.AddSingleton<DataService>();

//var app = builder.Build();

//app.MapGet("/", async(HttpContext ctx, DataService ds) =>
//{
//    return await ds.GetData();
//});

//app.Run();

//class DataService
//{
//    private readonly HttpClient httpClient;

//    public DataService(HttpClient httpClient)
//    {
//        this.httpClient = httpClient;
//    }
//    public async Task<string> GetData()
//    {
//        var response = await httpClient.GetAsync("https://jsonplaceholder.typicode.com/users");
//        return await response.Content.ReadAsStringAsync();
//    }
//}





//var builder = WebApplication.CreateBuilder(args);
//builder.Services.AddHttpClient("jsonph", client =>
//{
//    client.BaseAddress = new Uri("https://jsonplaceholder.typicode.com/");
//    client.DefaultRequestHeaders.Add("User-Agent", "MyApplication");
//    client.Timeout = TimeSpan.FromSeconds(10);
//});
//builder.Services.AddSingleton<DataService>();

//var app = builder.Build();

//app.MapGet("/", async (HttpContext ctx, DataService ds) =>
//{
//    return await ds.GetData();
//});

//app.Run();

//class DataService
//{
//    private readonly IHttpClientFactory cFactory;

//    public DataService(IHttpClientFactory cFactory)
//    {
//        this.cFactory = cFactory;
//    }
//    public async Task<string> GetData()
//    {
//        var client = cFactory.CreateClient("jsonph");

//        var response = await client.GetAsync("/users");
//        return await response.Content.ReadAsStringAsync();
//    }
//}


#endregion


#region Example_2

//var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddScoped<IUserService, UserService>();
//builder.Services.AddHttpClient<IUserApiClient, UserApiClient>(client =>
//{
//    client.BaseAddress = new Uri("https://jsonplaceholder.typicode.com/");
//});

//var app = builder.Build();

//app.MapGet("/users", async (HttpContext ctx, IUserService us) =>
//{
//    await ctx.Response.WriteAsJsonAsync(await us.GetUsersAsync());
//});

//app.Run();

//// ------ 
//public interface IUserService
//{
//    Task<User[]?> GetUsersAsync();
//}
//public class UserService(IUserApiClient api) : IUserService
//{
//    public async Task<User[]?> GetUsersAsync() => await api.GetUsersAsync();
//}

//public interface IUserApiClient
//{
//    Task<User[]?> GetUsersAsync();
//}
//public class UserApiClient(HttpClient http) : IUserApiClient
//{
//    public async Task<User[]?> GetUsersAsync() =>
//        await http.GetFromJsonAsync<User[]>("/users");
//}


//public record class User(int Id, string Name, string Username, string Email);


#endregion


#region Get services

//// --- Use ctor

//var builder = WebApplication.CreateBuilder(args);
//builder.Services.AddSingleton<IRandomService, RandomService>();
//builder.Services.AddSingleton<MyService>();

//var app = builder.Build();
//app.Run();

//public class MyService
//{
//    private readonly IRandomService randomService;
//    public MyService(IRandomService randomService)
//    {
//        this.randomService = randomService;
//    }
//}



// --- Use parameters

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<IRandomService, RandomService>();
builder.Services.AddSingleton<MyService>();

var app = builder.Build();
app.MapGet("/", async(HttpContext ctx, MyService ms) =>
{
    await ctx.Response.WriteAsync(ms.GetVal().ToString());
});
app.Run();

public class MyService
{
    private readonly IRandomService randomService;
    public MyService(IRandomService randomService)
    {
        this.randomService = randomService;
    }
    public int GetVal() => randomService.NextValue(1, 99);
}




public interface IRandomService
{
    int NextValue(int min, int max);
}

public class RandomService : IRandomService
{
    private readonly Random random = new Random();
    public int NextValue(int min, int max)
    {
        return random.Next(min, max);
    }
}

#endregion





