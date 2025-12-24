using Microsoft.AspNetCore.Authorization;

namespace _05_custom_requirements.Requirements;

public class BusinessHoursHandler : AuthorizationHandler<BusinessHoursRequirement>
{
    private readonly ILogger<BusinessHoursHandler> logger;
    public BusinessHoursHandler(ILogger<BusinessHoursHandler> logger) => this.logger = logger;
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context, 
        BusinessHoursRequirement requirement
    )
    {
        var now = TimeOnly.FromDateTime(DateTime.UtcNow);
        var user = context.User.Identity?.Name ?? "anonymous";

        logger.LogInformation($"Checking access for {user} at {now}");

        if (now >= requirement.StartTime && now <= requirement.EndTime)
        {
            logger.LogInformation($"Access granted");
            context.Succeed(requirement);
        }
        else
        {
            logger.LogInformation($"Access denied");
            context.Fail();
        }

        return Task.CompletedTask;
    }
}
