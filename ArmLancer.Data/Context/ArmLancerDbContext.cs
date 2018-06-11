using System;
using ArmLancer.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace ArmLancer.Data.Context
{
    public class ArmLancerDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<JobSubmission> JobSubmissions { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Favorite> Favorites { get; set; }

        public ArmLancerDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .MapUser()
                .MapClient()
                .MapCategory()
                .MapJob()
                .MapJobSubmission()
                .MapRating()
                .MapFavorite();
            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            var entries = ChangeTracker.Entries<AbstractTrackingEntityModel>();
            if (entries == null) return base.SaveChanges();
            foreach (var item in entries)
            {
                if (item.Entity == null) continue;
                var entity = item.Entity;
                // ReSharper disable once SwitchStatementMissingSomeCases
                switch (item.State)
                {
                    case EntityState.Deleted:
                        entity.Removed = DateTime.UtcNow;
                        break;
                    case EntityState.Modified:
                        entity.Updated = DateTime.UtcNow;
                        break;
                    case EntityState.Added:
                        entity.Created = DateTime.UtcNow;
                        entity.Updated = DateTime.UtcNow;
                        break;
                }
            }
            return base.SaveChanges();
        }
    }
}