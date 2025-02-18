using System.Reflection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using TaskManager.API.Endpoints;
using TaskManager.Infrastructure;

namespace TaskManager.API;

public static class ProgramExtensions
{
    public static void AppConfigureWebApplication(this WebApplication app)
    {
        var configuration = app.Configuration;

        if (configuration.GetValue("UseDeveloperExceptionPage", true))
        {
            app.UseDeveloperExceptionPage();
        }
        
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();

        app.MapEndpoints();
    }

    private static WebApplication MapEndpoints(this WebApplication app)
    {
        var endpoints = app.Services.GetRequiredService<IEnumerable<IEndpoint>>();

        foreach (var endpoint in endpoints)
        {
            endpoint.MapEndpoint(app);
        }

        return app;
    }

    public static IServiceCollection AddEndpoints(this IServiceCollection services, Assembly assembly)
    {
        var serviceDescriptors = assembly.GetTypes()
            .Where(type => type is { IsAbstract: false, IsInterface: false } && type.IsAssignableTo(typeof(IEndpoint)))
            .Select(type => ServiceDescriptor.Transient(typeof(IEndpoint), type))
            .ToArray();
        
        services.TryAddEnumerable(serviceDescriptors);
        
        return services;
    }
}