#region Base Example
//var builder = WebApplication.CreateBuilder(args);
//builder.Services.AddAuthentication();
//builder.Services.AddAuthorization();
//var app = builder.Build();

//// 1. Middleware до роутинга
//app.Use(async (ctx, next) =>
//{
//    Console.WriteLine("Before routing");
//    await next();
//});

//// 2. Выбор эндпоинта
//app.UseRouting();

//// 3. Middleware, имеющие доступ к эндпоинту
//app.UseAuthentication();
//app.UseAuthorization();

//// 4. Middleware между выбором и выполненем эндпоинта
//app.Use(async (ctx, next) =>
//{
//    var ep = ctx.GetEndpoint();
//    if (ep is not null)
//        Console.WriteLine($"Selected: {ep.DisplayName}");

//    await next();
//});

//// 5. Регистрация эндпоинтов
//#pragma warning disable ASP0014 // Suggest using top level route registrations
//app.UseEndpoints(ep =>
//{
//    ep.MapGet("/", () => "Home page")
//        .WithDisplayName("Home_Endpoint");
//    ep.MapGet("/secret", () => "Secret data")
//        .RequireAuthorization()
//        .WithDisplayName("Secret_Endpoint");
//});
//#pragma warning restore ASP0014 // Suggest using top level route registrations

//app.Run();

#endregion


#region Map()

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

//app.Map("/admin", adminApp =>     // branch
//{
//    adminApp.Use(...);
//    adminApp.Run(...);
//});

app.Map("/admin", (HttpContext ctx) => { });       // endpoint

#endregion
