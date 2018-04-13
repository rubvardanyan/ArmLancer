using System;
using ArmLancer.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace ArmLancer.Data.Context
{
    public class ArmLancerDbContext : DbContext
    {
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
                .MapJobSubmission();
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