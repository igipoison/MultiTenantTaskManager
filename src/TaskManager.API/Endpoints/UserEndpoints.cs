using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;
using TaskManager.Domain;
using TaskManager.Infrastructure;

namespace TaskManager.API.Endpoints;

[UsedImplicitly]
public class UserEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/auth/register/tenant={tenantId}", Authenticate);
    }

    private static async Task<IResult> Authenticate(string tenantId,
        RegisterRequest request,
        UserManager<ApplicationUser> userManager,
        ApplicationDbContext dbContext)
    {
        var tenant = await dbContext.Tenants.FirstOrDefaultAsync(t => t.Domain == tenantId);

        if (tenant is null) return Results.BadRequest("Tenant not found");

        var user = new ApplicationUser { Email = request.Email, UserName = request.Password, TenantId = tenant.Id };

        var result = await userManager.CreateAsync(user, request.Password);
        return result.Succeeded ? Results.Ok("User registered successfully") : Results.BadRequest(result.Errors);
    }
}