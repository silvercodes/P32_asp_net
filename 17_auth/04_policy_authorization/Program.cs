using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
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

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ITOnly", policy =>
        policy.RequireClaim("Department", "IT"));

    options.AddPolicy("TopSecret", policy => 
        policy.RequireClaim("Level", "TopSecret"));

    options.AddPolicy("FinanceManager", policy =>
    {
        policy.RequireClaim("Department", "Finance");
        policy.RequireClaim("Position", "Manager");
    });

    options.AddPolicy("CanEditProfile", policy =>
    {
        policy.RequireAssertion(authCtx =>
        {
            var userId = authCtx.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return false;

            if (authCtx.Resource is not HttpContext httpCtx)
                return false;

            if (!httpCtx.Request.RouteValues.TryGetValue("userId", out var routeUserId))
                return false;

            var requestedUserId = routeUserId?.ToString();

            return userId == requestedUserId || authCtx.User.IsInRole("admin");
        });
    });
});

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/generate-token/{role}", (string role) =>
{
    var claims = new List<Claim>
        {
            new(ClaimTypes.Name, "vasia"),
            new(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
            new("Department", role == "admin" ? "IT" : "Finance"),
            new("Position", role == "admin" ? "Director" : "Analyst"),
            new("Level", role == "admin" ? "TopSecret" : "Common"),
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

app.MapGet("/it-resource", () => "IT Department only")
    .RequireAuthorization("ITOnly");

app.MapGet("/top-secret", () => "TOP SECRET")
    .RequireAuthorization("TopSecret");

app.MapGet("/profile/{userId}", (string userId) => $"Profile id is {userId}")
    .RequireAuthorization("CanEditProfile");

app.Run();
