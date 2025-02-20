using Microsoft.AspNetCore.Identity;

namespace API.Extensions; 

public class CustomPasswordValidator<TUser> : IPasswordValidator<TUser> where TUser : class
{
    public Task<IdentityResult> ValidateAsync(UserManager<TUser> manager, TUser user, string? password)
    {
        var errors = new List<IdentityError>();

        if (string.IsNullOrEmpty(password) || !password.Any(char.IsLetter))
        {
            errors.Add(new IdentityError
            {
                Code = "PasswordRequiresLetter",
                Description = "Password must contain at least one letter."
            });
        }

        if (string.IsNullOrEmpty(password) || !password.Any(char.IsDigit))
        {
            errors.Add(new IdentityError
            {
                Code = "PasswordRequiresDigit",
                Description = "Password must contain at least one digit."
            });
        }

        return errors.Count > 0 ? Task.FromResult(IdentityResult.Failed(errors.ToArray())) : Task.FromResult(IdentityResult.Success);
    }
}
