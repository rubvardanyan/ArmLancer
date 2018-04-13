using System.Net;
using ArmLancer.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace ArmLancer.Data.Context
{
    public static class EntityMappingExtenstions
    {
        private static void MapAbstractTrackingEntityModel<T>(this EntityTypeBuilder<T> builder) where T : AbstractTrackingEntityModel
        {
            builder.Property(model => model.Created).IsRequired();
            builder.Property(model => model.Updated).IsRequired();
            builder.HasQueryFilter(model => model.Removed == null);
        }

        private static ModelBuilder MapUser(this ModelBuilder modelBuilder)
        {
            var builder = modelBuilder.Entity<User>();
            builder.MapAbstractTrackingEntityModel();
            
            builder.HasKey(u => u.Id);
            builder.HasIndex(u => u.UserName);
            builder.Property(u => u.Password).IsRequired();
            builder.Property(u => u.Role).IsRequired();

            builder.HasOne(u => u.Client)
                .WithOne(c => c.User);
                
            return modelBuilder;
        }

        private static ModelBuilder MapClient(this ModelBuilder modelBuilder)
        {
            var builder = modelBuilder.Entity<Client>();
            builder.MapAbstractTrackingEntityModel();

            builder.HasKey(c => c.Id);
            builder.Property(c => c.FirstName).IsRequired();
            builder.Property(c => c.LastName).IsRequired();
            builder.Property(c => c.Email).IsRequired();
            builder.Property(c => c.Phone).IsRequired();

            builder.HasOne(c => c.User)
                .WithOne(u => u.Client);

            builder.HasMany(c => c.Jobs)
                .WithOne(j => j.Client)
                .HasForeignKey(j => j.ClientId);
            
            return modelBuilder;
        }

        private static ModelBuilder MapCategory(this ModelBuilder modelBuilder)
        {
            var builder = modelBuilder.Entity<Category>();

            builder.HasKey(c => c.Id);
            builder.Property(c => c.Name).IsRequired();
            builder.HasIndex(c => c.ParentId);

            builder.HasOne(c => c.ParentCategory)
                .WithMany(c => c.Children)
                .HasForeignKey(c => c.ParentId);

            builder.HasMany(c => c.Jobs)
                .WithOne(j => j.Category)
                .HasForeignKey(j => j.CategoryId);
            
            return modelBuilder;
        }

        private static ModelBuilder MapJob(this ModelBuilder modelBuilder)
        {
            var builder = modelBuilder.Entity<Job>();

            builder.HasKey(j => j.Id);
            builder.HasIndex(j => j.Title);
            
            builder.Property(j => j.Description);
            builder.Property(j => j.Price).IsRequired();

            builder.HasOne(j => j.Client)
                .WithMany(c => c.Jobs)
                .HasForeignKey(j => j.ClientId);

            builder.HasOne(j => j.Category)
                .WithMany(c => c.Jobs)
                .HasForeignKey(c => c.CategoryId);
            
            return modelBuilder;
        }
    }
}