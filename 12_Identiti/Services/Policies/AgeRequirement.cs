using System;
using Microsoft.AspNetCore.Authorization;

namespace _12_Identiti.Services.Policies;

public class AgeRequirement :IAuthorizationRequirement
{
    public int Age { get; set; }

    public AgeRequirement(int age)
    {
        Age = age;
    }
}

