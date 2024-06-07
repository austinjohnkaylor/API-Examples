using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityAPI.EntityFramework;

public class IdentityDatabaseContext : IdentityDbContext<IdentityUser>
{
    public IdentityDatabaseContext(DbContextOptions<IdentityDbContext> options) :
        base(options)
    { }
}