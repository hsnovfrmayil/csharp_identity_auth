using System;
using _12_Identiti.Services.Policies;
using Microsoft.AspNetCore.Authorization;

namespace _12_Identiti.Services.Handler;

public class AgeRequirementHandler : AuthorizationHandler<AgeRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AgeRequirement requirement)
    {
        if (context.User.Identity != null)
        {
            var dateOfBirth = Convert.ToDateTime(context.User.FindFirst("DateOfBirth").Value);
            var age = DateTime.Today.Year - dateOfBirth.Year;

            if (age >= requirement.Age)
                context.Succeed(requirement);
            else
                context.Fail();
        }
        return Task.CompletedTask;
    }
}

