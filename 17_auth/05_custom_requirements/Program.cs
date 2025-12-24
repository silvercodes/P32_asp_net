using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using _05_custom_requirements.Requirements;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme; // --> 401
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = "https://myapp.cpm",
        ValidAudience = "app_users",

        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes("ewrposdfnsoew843750lskeu20ekdj0djf02ienfo2934y5y5ols")),
    };
});

builder.Services.AddSingleton<IAuthorizationHandler, BusinessHoursHandler>();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("BusinessHoursOnly", policy =>
    {
        policy.Requirements.Add(new BusinessHoursRequirement(
            startTime: new TimeOnly(9, 0),
            endTime: new TimeOnly(17, 0)
        ));
    });

    options.AddPolicy("AdminAnyTime", policy =>
    {
        policy.RequireRole("Admin");
        policy.Requirements.Add(new BusinessHoursRequirement(
            startTime: new TimeOnly(0, 0),
            endTime: new TimeOnly(23, 59, 59)
        ));
    });
});

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/generate-token/{role}", (string role) =>
{
    var claims = new List<Claim>
        {
            new(ClaimTypes.Name, $"user_{Guid.NewGuid()}"),
            new(ClaimTypes.Role, role)
        };

    var securityKey = new SymmetricSecurityKey(
        Encoding.UTF8.GetBytes("ewrposdfnsoew843750lskeu20ekdj0djf02ienfo2934y5y5ols"));

    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

    var token = new JwtSecurityToken(
        issuer: "https://myapp.cpm",
        audience: "app_users",
        claims: claims,
        expires: DateTime.Now.AddMinutes(30),
        signingCredentials: credentials
    );

    return new JwtSecurityTokenHandler().WriteToken(token);
});

app.MapGet("/business-resources", () => "Only working hours")
    .RequireAuthorization("BusinessHoursOnly");

app.MapGet("/admin-resources", () => "Any time admin resources")
    .RequireAuthorization("AdminAnyTime");

app.MapGet("/time", () => $"Current system time: {DateTime.UtcNow:HH:mm:ss}");

app.MapGet("/access-info", async (HttpContext ctx, IAuthorizationService authService) =>
{
    var requirement = new BusinessHoursRequirement(
        startTime: new TimeOnly(9, 0),
        endTime: new TimeOnly(18, 0)
    );

    var result = await authService.AuthorizeAsync(ctx.User, null, "BusinessHoursOnly");

    return new
    {
        CurrentTime = DateTime.UtcNow.ToString("HH:mm:ss"),
        IsAuthenticated = ctx.User.Identity?.IsAuthenticated,
        UserName = ctx.User.Identity?.Name,
        Roles = ctx.User.Claims
            .Where(c => c.Type == ClaimTypes.Role)
            .Select(c => c.Value),
        BusinessHoursAccess = result.Succeeded,
    };
});

app.Run();
