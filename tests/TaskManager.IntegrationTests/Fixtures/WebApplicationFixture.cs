using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using TaskManager.Infrastructure;
using Xunit;

namespace TaskManager.IntegrationTests.Fixtures;

public class WebApplicationFixture : WebApplicationFactory<Program>, IAsyncLifetime
{
    private DatabaseFixture DatabaseFixture { get; }
    
    public HttpClient Client { get; private set; }

    public WebApplicationFixture()
    {
        DatabaseFixture = new DatabaseFixture();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<DbContextOptions<ApplicationDbContext>>(); 
            
            services.AddDbContextFactory<ApplicationDbContext>(opts =>
            {
                opts.UseSqlServer(DatabaseFixture.ConnectionString,
                    (options) =>
                    {
                        options.EnableRetryOnFailure();
                    });
            });
        });
    }
    
    async Task IAsyncLifetime.InitializeAsync()
    {
        await DatabaseFixture.InitializeAsync();

        Client = CreateClient();
        
        using var scope = Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();
        
        context.Migrate();
    }

    async Task IAsyncLifetime.DisposeAsync()
    {
        Client.Dispose();
        await DatabaseFixture.DisposeAsync();
    }
}