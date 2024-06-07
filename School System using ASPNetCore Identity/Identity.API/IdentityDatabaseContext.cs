using Microsoft.AspNetCore.Identity;

namespace Identity.API;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class IdentityDatabaseContext(DbContextOptions<IdentityDatabaseContext> options)
    : IdentityDbContext<SchoolSystemUser>(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<SchoolSystemUser>(entity =>
        {
            entity.ToTable("Users");
        });
    }
}
    
   