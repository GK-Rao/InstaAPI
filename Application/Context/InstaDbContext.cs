using Microsoft.EntityFrameworkCore;
using InstaAPI.Application.Models;
using System.Threading.Tasks.Dataflow;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace InstaAPI.Application.Context;

public class InstaDbContext : DbContext{

    public InstaDbContext(DbContextOptions<InstaDbContext> options): base(options)
    { }      

    public DbSet<UserData> InstaUser {get;set;}

    public DbSet<MediaPost> MediaPosts {get;set;}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserData>()
            .HasMany(e => e.posts);

        modelBuilder.Entity<UserData>()
            .Property(e => e.followers)
            .HasConversion(typeof(string));

        modelBuilder.Entity<UserData>()
            .Property(e => e.following)
            .HasConversion(typeof(string));

        modelBuilder.Entity<MediaPost>()
            .HasOne<UserData>(e => e.userDetail)
            .WithMany(e => e.posts)
            .HasForeignKey(e => e.userId);
    }

}