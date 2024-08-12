using System;
using _12_Identiti.Models.Entities;
using Microsoft.AspNetCore.Identity;

namespace _12_Identiti.Validators;

public class PasswordValidator : IPasswordValidator<AppUser>
{
    public Task<IdentityResult> ValidateAsync(UserManager<AppUser> manager, AppUser user, string? password)
    {
        List<IdentityError> identityErrors = new List<IdentityError>();

        if (password.ToLower().Contains(user.UserName.ToLower())){
            identityErrors.Add(new IdentityError
            {
                Code="PasswordContainsUserName",
                Description="Password cannot contain username"
            });
        }
        if (password.ToLower().EndsWith(user.DateOfBirth.ToString()))
        {
            identityErrors.Add(new IdentityError
            {
                Code = "PasswordEndWithDateOfBirth",
                Description = "Password cannot end with date of birth"
            });
        }

        if (identityErrors.Count == 0)
            return Task.FromResult(IdentityResult.Success);

        else
            return Task.FromResult(IdentityResult.Failed(identityErrors.ToArray()));
    }
}

