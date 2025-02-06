using API.DTOs;
using API.Services;
using Core.Entities;
using Microsoft.AspNetCore.Identity;
using Moq;

public class RegistrationTests
{
    private readonly Mock<UserManager<AppUser>> _userManagerMock;
    private readonly Mock<IUserValidator<AppUser>> _userValidatorMock;
    private readonly UserService _userService;

    public RegistrationTests()
    {
        var userStoreMock = new Mock<IUserStore<AppUser>>();
        _userManagerMock = new Mock<UserManager<AppUser>>(
            userStoreMock.Object, null, null, null, null, null, null, null, null
        );

        _userValidatorMock = new Mock<IUserValidator<AppUser>>();
        
        _userService = new UserService(_userManagerMock.Object);
    }


    [Fact]
    public async Task RegisterUserAsync_ValidUser_ReturnsSuccess()
    {
        // Arrange
        var userDto = new RegisterDto { Email = "test@example.com", Password = "Password1" };

        _userManagerMock.Setup(um => um.CreateAsync(It.IsAny<AppUser>(), userDto.Password))
            .ReturnsAsync(IdentityResult.Success);

        // Act
        var result = await _userService.CreateUserAsync(userDto);

        // Assert
        Assert.True(result.Succeeded);
    }

    [Fact]
    public async Task RegisterUserAsync_NoPassword_ReturnsFailedResult()
    {
        // Arrange
        var userDto = new RegisterDto { Email = "test@example.com", Password = "" }; // No password
        _userManagerMock.Setup(um => um.CreateAsync(It.IsAny<AppUser>(), userDto.Password))
            .ReturnsAsync(IdentityResult.Failed());

        // Act
        var result = await _userService.CreateUserAsync(userDto);

        // Assert
        Assert.False(result.Succeeded);
        Assert.Contains(result.Errors, e => e.Description == "Email and password are required.");
    }

    [Fact]
    public async Task RegisterUserAsync_InvalidEmail_ReturnsFailedResult()
    {
        // Arrange
        var userDto = new RegisterDto { Email = "invalid-email", Password = "Password1" };
        var user = new AppUser { Email = userDto.Email, UserName = userDto.Email };

        _userValidatorMock.Setup(v => v.ValidateAsync(_userManagerMock.Object, user))
            .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "User login must be a valid email address." }));

        // Act
        var result = await _userService.CreateUserAsync(userDto);

        // Assert
        Assert.False(result.Succeeded);
        Assert.Contains(result.Errors, e => e.Description == "User login must be a valid email address.");
    }

    [Fact]
    public async Task RegisterUserAsync_NoEmail_ReturnsFailedResult()
    {
        // Arrange
        var userDto = new RegisterDto { Email = "", Password = "Password1" }; // No email
        _userManagerMock.Setup(um => um.CreateAsync(It.IsAny<AppUser>(), userDto.Password))
            .ReturnsAsync(IdentityResult.Failed());

        // Act
        var result = await _userService.CreateUserAsync(userDto);

        // Assert
        Assert.False(result.Succeeded);
        Assert.Contains(result.Errors, e => e.Description == "Email and password are required.");
    }
}
