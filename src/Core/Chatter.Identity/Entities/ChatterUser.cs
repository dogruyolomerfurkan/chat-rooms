using Microsoft.AspNetCore.Identity;

namespace Chatter.Identity.Entities;

public class ChatterUser : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? StatusDescription { get; set; }

    
}
