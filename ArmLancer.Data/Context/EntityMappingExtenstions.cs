using System.Net;
using ArmLancer.Data.Models;
using ArmLancer.Data.Models.Enums;
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

        public static ModelBuilder MapUser(this ModelBuilder modelBuilder)
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

        public static ModelBuilder MapClient(this ModelBuilder modelBuilder)
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
            
            builder.HasMany(c => c.Submissions)
                .WithOne(js => js.Client)
                .HasForeignKey(js => js.ClientId);

            builder.HasMany(c => c.Ratings)
                .WithOne(r => r.ClientTo)
                .HasForeignKey(r => r.ClientIdTo);
            
            builder.HasMany(c => c.Rates)
                .WithOne(r => r.ClientFrom)
                .HasForeignKey(r => r.ClientIdFrom);
            
            return modelBuilder;
        }

        public static ModelBuilder MapCategory(this ModelBuilder modelBuilder)
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

        public static ModelBuilder MapJob(this ModelBuilder modelBuilder)
        {
            var builder = modelBuilder.Entity<Job>();

            builder.HasKey(j => j.Id);
            builder.HasIndex(j => j.Title);
            
            builder.HasIndex(j => j.Status);
            
            builder.Property(j => j.Description);
            builder.Property(j => j.Price).IsRequired();

            builder.HasOne(j => j.Client)
                .WithMany(c => c.Jobs)
                .HasForeignKey(j => j.ClientId);

            builder.HasOne(j => j.Category)
                .WithMany(c => c.Jobs)
                .HasForeignKey(c => c.CategoryId);

            builder.HasMany(j => j.Submissions)
                .WithOne(js => js.Job)
                .HasForeignKey(js => js.JobId);
            
            return modelBuilder;
        }

        public static ModelBuilder MapJobSubmission(this ModelBuilder modelBuilder)
        {
            var builder = modelBuilder.Entity<JobSubmission>();

            builder.HasKey(js => js.Id);
            builder.HasIndex(js => js.JobId);
            builder.HasIndex(js => js.ClientId);
            builder.HasIndex(js => js.Status);
            
            builder.Property(js => js.Text).IsRequired();

            builder.HasOne(js => js.Client)
                .WithMany(c => c.Submissions)
                .HasForeignKey(js => js.ClientId);

            builder.HasOne(js => js.Job)
                .WithMany(j => j.Submissions)
                .HasForeignKey(js => js.JobId);
            
            return modelBuilder;
        }

        public static ModelBuilder MapRating(this ModelBuilder modelBuilder)
        {
            var builder = modelBuilder.Entity<Rating>();

            builder.HasKey(r => r.Id);
            builder.HasIndex(r => r.ClientIdFrom);
            builder.HasIndex(r => r.ClientIdTo);

            builder.Property(r => r.Score).IsRequired();
            builder.Property(r => r.Review);

            builder.HasOne(r => r.ClientFrom)
                .WithMany(c => c.Rates)
                .HasForeignKey(r => r.ClientIdFrom);
            
            builder.HasOne(r => r.ClientTo)
                .WithMany(c => c.Ratings)
                .HasForeignKey(r => r.ClientIdTo);
            
            return modelBuilder;
        }
        
        public static ModelBuilder MapFavorite(this ModelBuilder modelBuilder)
        {
            var builder = modelBuilder.Entity<Favorite>();

            builder.HasKey(f => f.Id);
            builder.HasIndex(f => f.ClientId);
            builder.HasIndex(f => f.JobId);

            builder.HasOne(f => f.Client)
                .WithMany(c => c.Favorites)
                .HasForeignKey(f => f.ClientId);
            
            builder.HasOne(f => f.Job)
                .WithMany(j => j.Favorites)
                .HasForeignKey(f => f.JobId);
            
            return modelBuilder;
        }
    }
}