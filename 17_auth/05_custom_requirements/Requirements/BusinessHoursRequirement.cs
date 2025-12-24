using Microsoft.AspNetCore.Authorization;

namespace _05_custom_requirements.Requirements;

public class BusinessHoursRequirement : IAuthorizationRequirement
{
    public TimeOnly StartTime { get; }
    public TimeOnly EndTime { get; }
    public BusinessHoursRequirement(TimeOnly startTime, TimeOnly endTime)
    {
        StartTime = startTime;
        EndTime = endTime;
    }
}
