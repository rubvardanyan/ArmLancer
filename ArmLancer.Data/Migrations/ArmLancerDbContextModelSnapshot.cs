﻿// <auto-generated />
using System;
using ArmLancer.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ArmLancer.Data.Migrations
{
    [DbContext(typeof(ArmLancerDbContext))]
    partial class ArmLancerDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.0-rtm-30799")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("ArmLancer.Data.Models.Category", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<long?>("ParentId");

                    b.HasKey("Id");

                    b.HasIndex("ParentId");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("ArmLancer.Data.Models.Client", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Created");

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("FirstName")
                        .IsRequired();

                    b.Property<string>("LastName")
                        .IsRequired();

                    b.Property<string>("Phone")
                        .IsRequired();

                    b.Property<string>("Picture");

                    b.Property<DateTime?>("Removed");

                    b.Property<DateTime>("Updated");

                    b.Property<long>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("Clients");
                });

            modelBuilder.Entity("ArmLancer.Data.Models.Favorite", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("ClientId");

                    b.Property<long>("JobId");

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.HasIndex("JobId");

                    b.ToTable("Favorite");
                });

            modelBuilder.Entity("ArmLancer.Data.Models.Job", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("CategoryId");

                    b.Property<long>("ClientId");

                    b.Property<string>("Description");

                    b.Property<int>("Duration");

                    b.Property<decimal>("Price");

                    b.Property<int>("Status");

                    b.Property<string>("Title");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("ClientId");

                    b.HasIndex("Status");

                    b.HasIndex("Title");

                    b.ToTable("Jobs");
                });

            modelBuilder.Entity("ArmLancer.Data.Models.JobSubmission", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("ClientId");

                    b.Property<long>("JobId");

                    b.Property<int>("Status");

                    b.Property<string>("Text")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.HasIndex("JobId");

                    b.HasIndex("Status");

                    b.ToTable("JobSubmissions");
                });

            modelBuilder.Entity("ArmLancer.Data.Models.Rating", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("ClientIdFrom");

                    b.Property<long>("ClientIdTo");

                    b.Property<string>("Review");

                    b.Property<int>("Score");

                    b.HasKey("Id");

                    b.HasIndex("ClientIdFrom");

                    b.HasIndex("ClientIdTo");

                    b.ToTable("Rating");
                });

            modelBuilder.Entity("ArmLancer.Data.Models.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Created");

                    b.Property<string>("Password")
                        .IsRequired();

                    b.Property<DateTime?>("Removed");

                    b.Property<int>("Role");

                    b.Property<DateTime>("Updated");

                    b.Property<string>("UserName");

                    b.HasKey("Id");

                    b.HasIndex("UserName");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("ArmLancer.Data.Models.Category", b =>
                {
                    b.HasOne("ArmLancer.Data.Models.Category", "ParentCategory")
                        .WithMany("Children")
                        .HasForeignKey("ParentId");
                });

            modelBuilder.Entity("ArmLancer.Data.Models.Client", b =>
                {
                    b.HasOne("ArmLancer.Data.Models.User", "User")
                        .WithOne("Client")
                        .HasForeignKey("ArmLancer.Data.Models.Client", "UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ArmLancer.Data.Models.Favorite", b =>
                {
                    b.HasOne("ArmLancer.Data.Models.Client", "Client")
                        .WithMany("Favorites")
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ArmLancer.Data.Models.Job", "Job")
                        .WithMany("Favorites")
                        .HasForeignKey("JobId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ArmLancer.Data.Models.Job", b =>
                {
                    b.HasOne("ArmLancer.Data.Models.Category", "Category")
                        .WithMany("Jobs")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ArmLancer.Data.Models.Client", "Client")
                        .WithMany("Jobs")
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ArmLancer.Data.Models.JobSubmission", b =>
                {
                    b.HasOne("ArmLancer.Data.Models.Client", "Client")
                        .WithMany("Submissions")
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ArmLancer.Data.Models.Job", "Job")
                        .WithMany("Submissions")
                        .HasForeignKey("JobId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ArmLancer.Data.Models.Rating", b =>
                {
                    b.HasOne("ArmLancer.Data.Models.Client", "ClientFrom")
                        .WithMany("Rates")
                        .HasForeignKey("ClientIdFrom")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ArmLancer.Data.Models.Client", "ClientTo")
                        .WithMany("Ratings")
                        .HasForeignKey("ClientIdTo")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
