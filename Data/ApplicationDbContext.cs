using GameSite.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GameSite.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
        Database.EnsureCreated();
    }

    public DbSet<Publication> Publications { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<PublicationType> PublicationTypes { get; set; }
}