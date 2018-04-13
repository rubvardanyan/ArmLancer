using System.Collections.Generic;
using System.Linq;
using ArmLancer.Data.Context;
using ArmLancer.Data.Models;
using ArmLancer.Data.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace ArmLancer.API
{
    public static class Seeder
    {
        public static void EnsureSeedData(this ArmLancerDbContext context)
        {
            context.Database.Migrate();
            context.Database.EnsureCreated();
            
            context.SeedUsers();
            //context.SeedCategories();
            
        }

        private static void SeedUsers(this ArmLancerDbContext context)
        {
            if (context.Users.Any()) return;
            context.Users.AddRange(new List<User> {
                new User {
                    UserName = "admin",
                    Password = "admin",
                    Role = UserRole.Admin,
                    Client = new Client
                    {
                        FirstName = "Admin",
                        LastName = "Administrator",
                        Email = "some@domain.com",
                        Phone = "+37400000000",
                        Picture = "",
                    }
                },
                new User {
                    UserName = "freelancer",
                    Password = "freelancer",
                    Role = UserRole.FreeLancer,
                    Client = new Client
                    {
                        FirstName = "FreeLancer",
                        LastName = "FreeLanceryan",
                        Email = "freelancer@domain.com",
                        Phone = "+37400000000",
                    }
                },
                new User {
                    UserName = "employeer",
                    Password = "employeer",
                    Role = UserRole.Employeer,
                    Client = new Client
                    {
                        FirstName = "Employeer",
                        LastName = "Employeeryan",
                        Email = "employeer@domain.com",
                        Phone = "+37400000000",
                    }
                }
            });
            context.SaveChanges();
        }
        
        private static void SeedCategories(this ArmLancerDbContext context)
        {
            if (context.Categories.Any()) return;
            context.Categories.AddRange(new List<Category> {
                new Category {
                    Name = "IT",
                    Children = new List<Category> {
                        new Category {
                            Name = "C#",
                            Jobs = new List<Job> {
                                new Job
                                {
                                    Title = "Booking Package",
                                    Description = "Scheduller for dotnetcore",
                                    Price = 2000,
                                    Duration = JobDuration.ThreeMonths,
                                    ClientId = context.Clients.First(c => c.User.Role.Equals(UserRole.Employeer)).Id,
                                },
                                new Job
                                {
                                    Title = "List.am",
                                    Description = "Site similar to list.am",
                                    Price = 1500,
                                    Duration = JobDuration.OneMonth,
                                    ClientId = context.Clients.First(c => c.User.Role.Equals(UserRole.Employeer)).Id,
                                }
                            }
                        },
                        new Category {
                            Name = "PHP",
                        },
                        new Category {
                            Name = "MySQL",
                        }
                    }
                },
                new Category {
                    Name = "Hashvapahutyun",
                }
            });
            context.SaveChanges();
        }
    }
}