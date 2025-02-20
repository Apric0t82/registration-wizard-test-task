using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using FluentAssertions;
using Infrastructure.Data;
using API.DTOs;
using API.Services;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Identity.Tests;

public class UserIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public UserIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                var dbContextDescriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                    typeof(IDbContextOptionsConfiguration<AppDbContext>));

                if (dbContextDescriptor != null)
                {
                    services.Remove(dbContextDescriptor);
                }

                services.AddDbContext<AppDbContext>(options =>
                {
                    options.UseInMemoryDatabase("TestDb");
                });

            });
        });
    }


    [Fact]
    public async Task RegisterUser_ShouldSaveUserToDatabase()
    {
        // Arrange - Get UserService from DI container
        using var scope = _factory.Services.CreateScope();
        var userService = scope.ServiceProvider.GetRequiredService<UserService>();

        var userDto = new RegisterDto
        {
            Email = "test@example.com",
            Password = "Password1!",
            CountryId = 1,
            ProvinceId = 1
        };

        // Act - Call UserService to register user
        var result = await userService.CreateUserAsync(userDto);
        result.Succeeded.Should().BeTrue();

        // Assert - Ensure user is saved in database
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var userExists = await dbContext.Users.AnyAsync(u => u.Email == userDto.Email);
        userExists.Should().BeTrue();

        // Assert - Ensure user with duplicate user name should not be saved in database
        result = await userService.CreateUserAsync(userDto);
        result.Succeeded.Should().BeFalse();
        result.Errors.Select(x => x.Code == "DuplicateUserName").First().Should().BeTrue();
    }
}