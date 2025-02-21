using Microsoft.Extensions.DependencyInjection;
using Infrastructure.Data;
using API.Services;

namespace Identity.IntegrationTests;

public abstract class BaseIntegrationTest : IClassFixture<IntegrationTestWebAppFactory>, IDisposable
{
    private readonly IServiceScope _scope;
    protected readonly UserService _userService;
    protected readonly AppDbContext _dbContext;

    protected BaseIntegrationTest(IntegrationTestWebAppFactory factory)
    {
        _scope = factory.Services.CreateScope();
        _userService = _scope.ServiceProvider.GetRequiredService<UserService>();
        _dbContext = _scope.ServiceProvider.GetRequiredService<AppDbContext>();
    }

    public void Dispose()
    {
        _scope?.Dispose();
        _dbContext?.Dispose();
    }
}