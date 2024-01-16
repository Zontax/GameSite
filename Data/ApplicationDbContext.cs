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

		modelBuilder.UseIdentityColumns();
        
        modelBuilder.Entity<Post>()
            .HasMany(p => p.LikedByUsers)
            .WithMany(u => u.LikedPosts)
            .UsingEntity(j => j.ToTable("PostUserLikes"));

        modelBuilder.Entity<Post>()
            .HasMany(p => p.DislikedByUsers)
            .WithMany(u => u.DislikedPosts)
            .UsingEntity(j => j.ToTable("PostUserDislikes"));

        modelBuilder.Entity<Post>()
            .HasMany(p => p.SavedByUsers)
            .WithMany(u => u.SavedPosts)
            .UsingEntity(j => j.ToTable("PostUserSaved"));

        modelBuilder.Entity<Post>()
            .HasMany(p => p.Comments)
            .WithOne(c => c.Post)
            .HasForeignKey(c => c.PostId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    public override DbSet<IdentityRole> Roles { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<Comment> Comments { get; set; }
}
