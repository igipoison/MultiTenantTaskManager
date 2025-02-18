using Microsoft.AspNetCore.Identity;

namespace TaskManager.Domain;

public class ApplicationUser : IdentityUser
{
    public string TenantId { get; set; }
}