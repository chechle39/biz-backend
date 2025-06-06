﻿using Microsoft.EntityFrameworkCore;
using PlatformService.Models;

namespace PlatformService.Data
{
    public static class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder app, bool IsProduction)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>(), IsProduction);
            }

        }
        private static void SeedData(AppDbContext context, bool IsProduction)
        {
            if (IsProduction)
            {
                Console.WriteLine("--> apply migrations...");
                try
                {
                    context.Database.Migrate();

                }
                catch (Exception ex) 
                {
                    Console.WriteLine($"--> could not run migration: {ex.Message}");
                }
            }
            if (!context.Platforms.Any())
            {
                Console.WriteLine("--> Seeding data...");
                context.Platforms.AddRange(
                    new Platform()
                    {
                        Name = "dot net",
                        Publisher = "mricrosoft",
                        Cost = "free",
                        Description = "test app",
                        //Id = 1
                    },
                    new Platform()
                    {
                        Name = "sql",
                        Publisher = "mricrosoft",
                        Cost = "free",
                        Description = "test app",
                        //Id = 2
                    },
                    new Platform()
                    {
                        Name = "K8s",
                        Publisher = "mricrosoft",
                        Cost = "free",
                        Description = "test app",
                        //Id = 1
                    }
                );
                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("--> we alredy have data");
            }

        }

    }
}
