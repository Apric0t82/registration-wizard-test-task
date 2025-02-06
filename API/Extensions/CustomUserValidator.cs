using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace API.Extensions
{
    public class CustomUserValidator<TUser> : IUserValidator<TUser> where TUser : IdentityUser
    {
        public Task<IdentityResult> ValidateAsync(UserManager<TUser> manager, TUser user)
        {
            var errors = new List<IdentityError>();

            if (string.IsNullOrWhiteSpace(user.UserName) || !new EmailAddressAttribute().IsValid(user.UserName))
            {
                errors.Add(new IdentityError
                {
                    Code = "InvalidEmail",
                    Description = "User login must be a valid email address."
                });
            }

            return errors.Count > 0 ? Task.FromResult(IdentityResult.Failed(errors.ToArray())) : Task.FromResult(IdentityResult.Success);
        }
    }

}
