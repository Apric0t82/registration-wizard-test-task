using API.Extensions;
using Core.Entities;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace Identity.Tests
{
    public class ValidationTests
    {
        private readonly CustomPasswordValidator<AppUser> _passwordValidator;
        private readonly CustomUserValidator<AppUser> _userValidator;
        private readonly UserManager<AppUser> _userManager;

        public ValidationTests()
        {
            _passwordValidator = new CustomPasswordValidator<AppUser>();
            _userValidator = new CustomUserValidator<AppUser>();

            var userStoreMock = new Mock<IUserStore<AppUser>>();
            _userManager = new UserManager<AppUser>(
                userStoreMock.Object, null, null, null, null, null, null, null, null);
        }

        [Theory]
        [InlineData("Password1", true)] // Valid
        [InlineData("123456", false)]   // No letter
        [InlineData("abcdef", false)]   // No digit
        [InlineData("", false)]         // Empty
        public async Task ValidateAsync_ChecksPasswordIsCorrect(string password, bool expectedValid)
        {
            // Arrange

            // Act
            var result = await _passwordValidator.ValidateAsync(_userManager, new AppUser(), password);

            // Assert
            Assert.Equal(expectedValid, result.Succeeded);
        }

        [Theory]
        [InlineData("valid@example.com", true)]   // Valid email
        [InlineData("invalid-email", false)]     // Invalid format
        [InlineData("invalidEmail", false)]     // Invalid format
        [InlineData("", false)]                  // Empty
        public async Task ValidateAsync_ChecksUserLoginCorrectly(string userName, bool expectedValid)
        {
            // Arrange
            var user = new AppUser { UserName = userName, Email = userName };

            // Act
            var result = await _userValidator.ValidateAsync(_userManager, user);

            // Assert
            Assert.Equal(expectedValid, result.Succeeded);
        }
    }

}
