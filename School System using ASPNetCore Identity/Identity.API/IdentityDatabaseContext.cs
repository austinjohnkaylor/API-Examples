namespace Identity.API;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class IdentityDatabaseContext(DbContextOptions<IdentityDatabaseContext> options)
    : IdentityDbContext<SchoolSystemUser>(options);