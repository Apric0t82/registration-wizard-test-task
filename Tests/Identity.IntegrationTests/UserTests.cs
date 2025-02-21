using API.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Identity.IntegrationTests;

public class UserTests(IntegrationTestWebAppFactory factory) : BaseIntegrationTest(factory)
{
    [Fact]
    public async Task RegisterUser_ShouldSaveUserToDatabase()
    {
        // Arrange
        var userDto = new RegisterDto
        {
            Email = "test@example.com",
            Password = "Password1!",
            CountryId = 1,
            ProvinceId = 1
        };

        // Act
        var result = await _userService.CreateUserAsync(userDto);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Succeeded);

        var userExists = await _dbContext.Users.AnyAsync(u => u.Email == userDto.Email);
        Assert.True(userExists);
    }

    [Fact]
    public async Task RegisterUser_DuplicateUser_ShouldFailToSave()
    {
        // Arrange
        var userDto = new RegisterDto
        {
            Email = "test@example.com",
            Password = "Password1!",
            CountryId = 1,
            ProvinceId = 1
        };

        await _userService.CreateUserAsync(userDto);

        // Act
        var duplicateResult = await _userService.CreateUserAsync(userDto);

        // Assert
        Assert.False(duplicateResult.Succeeded);
        Assert.Contains(duplicateResult.Errors, e => e.Code == "DuplicateUserName");
    }
}
