using Microsoft.Extensions.DependencyInjection;
using TaskManager.Infrastructure;
using TaskManager.IntegrationTests.Fixtures;
using Xunit;

namespace TaskManager.IntegrationTests;

[CollectionDefinition("Integration tests")]
public class IntegrationContainerCollection : ICollectionFixture<WebApplicationFixture>;

[Collection("Integration tests")]
public class IntegrationTestsBase
{
    protected HttpClient Client { get; }
    protected IApplicationDbContext Context { get; }

    protected IntegrationTestsBase(WebApplicationFixture webApplicationFixture)
    {
        Client = webApplicationFixture.Client;
        var scope = webApplicationFixture.Services.CreateScope();
        Context = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();
    }
}