using API.DTOs;
using API.Extensions;
using Core.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace API.Services
{
    public class UserService
    {
        private readonly UserManager<AppUser> _userManager;

        public UserService(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IdentityResult> CreateUserAsync(RegisterDto model)
        {
            if (string.IsNullOrWhiteSpace(model.Email) || string.IsNullOrWhiteSpace(model.Password))
            {
                return IdentityResult.Failed(new IdentityError { Description = "Email and password are required." });
            }

            var user = new AppUser
            {
                Email = model.Email,
                UserName = model.Email,
                CountryId = model.CountryId,
                ProvinceId = model.ProvinceId
            };

            var passwordValidator = new CustomPasswordValidator<AppUser>();
            var passwordValidationResult = await passwordValidator.ValidateAsync(_userManager, user, model.Password);

            if (!passwordValidationResult.Succeeded)
            {
                return IdentityResult.Failed(new IdentityError { Description = string.Join(", ", passwordValidationResult.Errors.Select(e => e.Description)), 
                                                                 Code = string.Join(", ", passwordValidationResult.Errors.Select(e => e.Code)) });
            }

            var userValidator = new CustomUserValidator<AppUser>();
            var validationResults = await userValidator.ValidateAsync(_userManager, user);
            if (!validationResults.Succeeded)
            {
                return validationResults;
            }

            var result = await _userManager.CreateAsync(user, model.Password);
            return result;
        }
    }

}
