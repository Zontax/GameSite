using GameSite.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GameSite.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        //Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Post>()
            .HasMany(c => c.LikedByUsers)
            .WithMany(s => s.LikedPosts)
            .UsingEntity(j => j.ToTable("PostUserLikes"));

        // modelBuilder.Entity<Comment>()
        //     .HasOne(c => c.Post)
        //     .WithMany(p => p.Comments)
        //     .HasForeignKey(c => c.PostId)
        //     .OnDelete(DeleteBehavior.ClientCascade);
    }

    public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    public override DbSet<IdentityRole> Roles { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<Comment> Comments { get; set; }
}