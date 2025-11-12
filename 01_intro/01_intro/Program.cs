using System.Diagnostics;
using _01_intro;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

WebApplication app = builder.Build();


//app.UseExceptionHandler();
//app.UseHttpsRedirection();
//app.UseStaticFiles();
//app.UseRouting();
//app.UseAuthentication();
//app.UseAuthorization();
//app.MapControllers();
//app.MapRazorPages();

//app.UseMiddleware<LoggingMiddleware>();
//app.Use(async (ctx, next) =>
//{
//    Console.WriteLine("Before");
//    await next();
//    Console.WriteLine("After");
//});

//app.UseWelcomePage();

//app.Run();




//app.Use(async (ctx, next) =>
//{
//    var sw = Stopwatch.StartNew();
//    await next(ctx);
//    sw.Stop();
//    Console.WriteLine($"Time = {sw.ElapsedMilliseconds} ms");
//});

//app.Run(async ctx =>
//{
//    await Task.Delay(5000);
//    await ctx.Response.WriteAsync("Hello Vasia");
//});

//app.Run();




int count = 0;
app.Run(async ctx =>
{
    count += 1;
    await ctx.Response.WriteAsync($"Count = {count}");
});

app.Run();
