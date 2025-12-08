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

//var builder = WebApplication.CreateBuilder(args);
//var app = builder.Build();

////app.Map("/admin", adminApp =>     // branch
////{
////    adminApp.Use(...);
////    adminApp.Run(...);
////});

//app.Map("/admin", (HttpContext ctx) => { });       // endpoint






//var builder = WebApplication.CreateBuilder(args);
//var app = builder.Build();

//app.Map("/test", () => "test string");

//app.MapGet("/users", () => "users list");
//app.MapPost("/users", () => "user created");
//app.MapDelete("/users", () => "user deleted");

//app.MapGet("/users/{id}", (int id) => $"User {id}");

//app.Map("/debug", (HttpContext ctx) =>
//{
//    var req = ctx.Request;
//    return $"Method: {req.Method}, Path: {req.Path}";
//});

//app.MapGet("/async", async () =>
//{
//    await Task.Delay(1000);
//    return "DATA";
//});

//var usersGroup = app.MapGroup("/users");
//usersGroup.MapGet("/", () => "All users");
//usersGroup.MapGet("/{id}", (int id) => $"User {id}");
//usersGroup.MapPost("/", () => "Create user");

//app.Run();






//var builder = WebApplication.CreateBuilder(args);
//var app = builder.Build();

//// === web routes
//app.MapGet("/", () => "Home page");

//app.MapGet("/users/{name}", (string name) => $"Hello {name}");

//app.MapGet("/weather", async () =>
//{
//    await Task.Delay(1000);
//    return new { Temp = 25, Hum = 60 };
//});

//// === api routes
//var api = app.MapGroup("/api");
//api.MapGet("/products", () => new[] { "Laptop", "Phone", "Tablet" });
//api.MapGet("/products/{id}", (int id) => $"Product {id}");

//// === web routes
//app.Map("/info", (HttpContext ctx) =>
//{
//    var req = ctx.Request;
//    return $"Method: {req.Method}, Path: {req.Path}";
//});

//app.Run();

#endregion


#region Parameters

//var builder = WebApplication.CreateBuilder(args);
//var app = builder.Build();

//// Базовые параметры
//app.MapGet("/users/{id}", (int id) => $"User {id}");

//// Ограничения типов
//app.MapGet("/posts/{postId:int}", (int postId) => $"Post {postId}");
//app.MapGet("/products/{title:alpha}", (string title) => $"Product {title}");

//// Необязательные параметры
//app.MapGet("/books/{id?}", (int? id) => id.HasValue ? $"Book {id}" : "All books");

//// Значение по-умолчанию
//app.MapGet("/items/{category=all}", (string category) => $"Show {category}");

//// Catch-all
//app.MapGet("/files/{**path}", (string path) => $"File path: {path}");

//// Regex
//app.MapGet(@"/orders/{orderId:regex(^ORD-\d{{4}}$)}", (string orderId) => $"Order {orderId}");

//// Параметры с дефисом и точкой
//app.MapGet("/products/{*title}", (string title) => $"Product {title}");

//// Multiple parameters
//app.MapGet("/orders/{userId:int}/{orderId:int}", (string userId, string orderId) => $"{userId} - {orderId}");

//app.Run();
#endregion


#region Binding
//using Microsoft.AspNetCore.Mvc;

//// TODO: ????

//using Microsoft.AspNetCore.Mvc;

//var builder = WebApplication.CreateBuilder(args);
//var app = builder.Build();

//app.MapGet("/search", ([AsParameters] SearchParams p) => p);

//app.Run();
//record SearchParams
//(
//    string Query,
//    int Page = 1,
//    string Sort = "asc"
//);
#endregion


#region Constraints

//var builder = WebApplication.CreateBuilder(args);
//var app = builder.Build();

//// Data type
//app.MapGet("/int/{id:int}", (int id) => $"Int: {id}");
//app.MapGet("/bool/{flag:bool}", (bool flag) => $"Bool: {flag}");
//app.MapGet("/guid/{id:guid}", (Guid id) => $"Guid: {id}");

//// Length/range
//app.MapGet("/minlen/{text:alpha:minlength(5)}", (string text) => $"Text: {text}");
//app.MapGet("/len/{text:length(3, 8)}", (string text) => $"Text: {text}");
//app.MapGet("/range/{num:int:range(1, 100)}", (int num) => $"Num: {num}");

//// Files
//app.MapGet("/file/{title:file}", (string title) => $"File: {title}");
//app.MapGet("/nofile/{title:nofile}", (string title) => $"No file: {title}");

//app.Run();

#endregion


#region Custom constraints
//var builder = WebApplication.CreateBuilder(args);

//builder.Services.Configure<RouteOptions>(options =>
//{
//    options.ConstraintMap.Add("even", typeof(EvenNumberConstraint));
//    options.ConstraintMap.Add("valid-category", typeof(ValidCategoryConstraint));
//});

//var app = builder.Build();

//app.MapGet("/even/{num:even}", (int num) => $"Even num: {num}");
//app.MapGet("/store/{category:valid-category}", (string category) => $"Category: {category}");

//app.Run();


//class EvenNumberConstraint : IRouteConstraint
//{
//    public bool Match(
//        HttpContext? httpContext, 
//        IRouter? route, 
//        string routeKey, 
//        RouteValueDictionary values, 
//        RouteDirection routeDirection)
//    {
//        if (! values.TryGetValue(routeKey, out var routeVal))
//            return false;

//        if (! int.TryParse(routeVal?.ToString(), out var num))
//            return false;

//        return num % 2 == 0;
//    }
//}

//class ValidCategoryConstraint : IRouteConstraint
//{
//    private readonly string[] validCategories = { "electronics", "books", "products" };

//    public bool Match(
//        HttpContext? httpContext,
//        IRouter? route,
//        string routeKey,
//        RouteValueDictionary values,
//        RouteDirection routeDirection)
//    {
//        if (!values.TryGetValue(routeKey, out var category))
//            return false;

//        return validCategories.Contains(category?.ToString());
//    }
//}

#endregion


#region Complex example

//var builder = WebApplication.CreateBuilder(args);
//var app = builder.Build();

//app.Use(async (ctx, next) =>
//{
//    ctx.Response.ContentType = "text/plain; charset=utf-8;";

//    Console.WriteLine("MW_1: BEGIN");
//    await ctx.Response.WriteAsync("MW_1: BEGIN\n");
//    await next();
//    Console.WriteLine("MW_1: FINISH");
//});

//app.Use(async (ctx, next) =>
//{
//    Console.WriteLine("MW_2: BEGIN");
//    await ctx.Response.WriteAsync("MW_2: BEGIN\n");
//    await next();
//    Console.WriteLine("MW_2: FINISH");
//});

//app.UseRouting();

//app.Use(async (ctx, next) => 
//{
//    Console.WriteLine("MW_3: BEGIN");

//    Endpoint? ep = ctx.GetEndpoint();
//    if (ep is not null)
//        ctx.Response.WriteAsync($"MW_3: Endpoint: {ep.DisplayName}\n");
//    else
//        ctx.Response.WriteAsync($"MW_3: Endpoint not found\n");

//    await next();

//    Console.WriteLine("MW_3: FINISH");
//});

//app.MapGet("/", () => "Endpoint: HOME PAGE")
//    .WithDisplayName("home_page");

//app.MapGet("/about", () => "Endpoint: ABOUT US")
//    .WithDisplayName("about_us");

//app.Use(async (ctx, next) =>
//{
//    Console.WriteLine("MW_4: BEGIN");
//    await ctx.Response.WriteAsync("MW_4: BEGIN\n");
//    await next();
//    Console.WriteLine("MW_4: FINISH");
//});

//app.UseEndpoints(eps => { });

//app.Run();

#endregion


#region Query parameters





#endregion
