namespace ClimateDatabase.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using ClimateDatabase.Common;
    using ClimateDatabase.Data.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;

    public static class ApplicationDbContextSeeder
    {
        public static void Seed(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            if (serviceProvider == null)
            {
                throw new ArgumentNullException(nameof(serviceProvider));
            }

            var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            Seed(dbContext, userManager, roleManager);
        }

        private static void Seed(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            if (userManager == null)
            {
                throw new ArgumentNullException(nameof(userManager));
            }

            if (roleManager == null)
            {
                throw new ArgumentNullException(nameof(roleManager));
            }

            SeedRoles(roleManager);
            //SeedAdmin(userManager);
            SeedStations(dbContext);
            SeedStationReadings(dbContext);
        }

        private static void SeedAdmin(UserManager<ApplicationUser> userManager)
        {
            ApplicationUser admin = new ApplicationUser
            {
                Firstname = "System",
                Lastname = "Administrator",
                Email = "admin@admin.com",
                UserName = "admin@admin.com",
                EmailConfirmed = true,
                IsActive = true,
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now
            };

            var result = userManager.CreateAsync(admin, "123456").GetAwaiter().GetResult();

            if (!result.Succeeded)
            {
                throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
            }

            var roleResult = userManager.AddToRoleAsync(admin, GlobalConstants.AdministratorRoleName).GetAwaiter().GetResult();

            if (!roleResult.Succeeded)
            {
                throw new Exception(string.Join(Environment.NewLine, roleResult.Errors.Select(e => e.Description)));
            }
        }

        private static void SeedRoles(RoleManager<ApplicationRole> roleManager)
        {
            SeedRole(GlobalConstants.AdministratorRoleName, roleManager);
            SeedRole(GlobalConstants.UserRoleName, roleManager);
        }

        private static void SeedRole(string roleName, RoleManager<ApplicationRole> roleManager)
        {
            var role = roleManager.FindByNameAsync(roleName).GetAwaiter().GetResult();
            if (role == null)
            {
                var result = roleManager.CreateAsync(new ApplicationRole(roleName)).GetAwaiter().GetResult();

                if (!result.Succeeded)
                {
                    throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
                }
            }
        }

        private static void SeedStations(ApplicationDbContext context)
        {
            if (context.ClimateStations.Any())
            {
                return;
            }

            var stationNames = new List<string>
            {
                "Vraca",
                "Sofia",
                "Stara Zagora",
                "Pazardzhik",
                "Varna",
                "Burgas",
                "Pernik"
            };

            foreach (var stationName in stationNames)
            {
                context.ClimateStations.Add(new ClimateStation
                {
                    Name = stationName,
                    Weight = 0,
                    CreatedOn = DateTime.Now.AddYears(-2),
                    ModifiedOn = DateTime.Now,
                    Readings = new List<ClimateStationReading>()
                });
            }

            context.SaveChanges();
        }

        private static void SeedStationReadings(ApplicationDbContext context)
        {
            if (context.ClimateStationReadings.Any())
            {
                return;
            }

            var randGen = new Random();

            List<ClimateStation> climateStations = context.ClimateStations.ToList();
            foreach (var station in climateStations)
            {
                for (int i = 1; i <= 6; i++)
                {
                    context.ClimateStationReadings.Add(new ClimateStationReading
                    {
                        ClimateStationId = station.Id,
                        ClimateStation = station,
                        AverageTemperature = randGen.Next(-10, 35),
                        ClimateStationIntervalWeight = 0,
                        CreatedOn = DateTime.Now.AddMonths(-i),
                        Month = DateTime.Now.AddMonths(-i).Month,
                        Year = DateTime.Now.AddMonths(-i).Year,
                        DaysWithRainMoreThan1mm = randGen.Next(0, 30),
                        DaysWithRainMoreThan10mm = randGen.Next(0, 30),
                        DaysWithThunder = randGen.Next(0, 30),
                        DaysWithWindFasterThan14ms = randGen.Next(0, 30),
                        MinimumTemperature = randGen.Next(-20, 15),
                        MaximumTemperature = randGen.Next(15, 40),
                        TemperatureDeviation = randGen.NextDouble() * 20,
                        RainRatio = randGen.NextDouble() * 10,
                        RainSum = randGen.NextDouble() * 50,
                        MaximumRain = randGen.NextDouble() * 100,
                        MaximumRainDay = randGen.Next(10, 50),
                        MaximumTemperatureDay = randGen.Next(0, 100),
                        MinimumTemperatureDay = randGen.Next(0, 100)
                    });
                }
            }

            context.SaveChanges();
        }
    }
}
