using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using travoul.Models;

namespace travoul.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Continent> Continent { get; set; }
        public DbSet<LocationType> LocationType { get; set; }
        public DbSet<TripRetro> TripRetro { get; set; }
        public DbSet<RetroType> RetroType { get; set; }
        public DbSet<TravelType> TravelType { get; set; }
        public DbSet<Trip> Trip { get; set; }
        public DbSet<TripTravelType> TripTravelType { get; set; }
        public DbSet<TripVisitLocation> TripVisitLocation { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Restrict deletion of related TravelType when TripTravelTypes entry is removed
            modelBuilder.Entity<TravelType>()
                .HasMany(t => t.TripTravelTypes)
                .WithOne(l => l.TravelType)
                .OnDelete(DeleteBehavior.Restrict);

            // Restrict deletion of related Trip when TripTravelTypes entry is removed
            modelBuilder.Entity<Trip>()
                .HasMany(t => t.TripTravelTypes)
                .WithOne(l => l.Trip)
                .OnDelete(DeleteBehavior.Restrict);

            // Restrict deletion of related LocationType when TripVisitLocations entry is removed
            modelBuilder.Entity<LocationType>()
                .HasMany(t => t.TripVisitLocations)
                .WithOne(l => l.LocationType)
                .OnDelete(DeleteBehavior.Restrict);

            // Restrict deletion of related Trip when TripVisitLocations entry is removed
            modelBuilder.Entity<Trip>()
                .HasMany(t => t.TripVisitLocations)
                .WithOne(l => l.Trip)
                .OnDelete(DeleteBehavior.Restrict);

            // Restrict deletion of related RetroType when TripRetros entry is removed
            modelBuilder.Entity<RetroType>()
                .HasMany(t => t.TripRetros)
                .WithOne(l => l.RetroType)
                .OnDelete(DeleteBehavior.Restrict);

            // Restrict deletion of related Trip when TripRetros entry is removed
            modelBuilder.Entity<Trip>()
                .HasMany(t => t.TripRetros)
                .WithOne(l => l.Trip)
                .OnDelete(DeleteBehavior.Restrict);


            var kayla = new ApplicationUser
            {
                FirstName = "Kayla",
                LastName = "Reid",
                City = "Nashville",
                State = "TN",
                UserName = "kayla@kayla.com",
                NormalizedUserName = "KAYLA@KAYLA.COM",
                Email = "kayla@kayla.com",
                NormalizedEmail = "KAYLA@KAYLA.COM",
                EmailConfirmed = true,
                LockoutEnabled = false,
                SecurityStamp = Guid.NewGuid().ToString("D")
            };
            var passwordHash = new PasswordHasher<ApplicationUser>();
            kayla.PasswordHash = passwordHash.HashPassword(kayla, "Kayla3!");
            modelBuilder.Entity<ApplicationUser>().HasData(kayla);

            var ricky = new ApplicationUser
            {
                FirstName = "Ricky",
                LastName = "Bruner",
                City = "Nashville",
                State = "TN",
                UserName = "ricky@ricky.com",
                NormalizedUserName = "RICKY@RICKY.COM",
                Email = "ricky@ricky.com",
                NormalizedEmail = "RICKY@RICKY.COM",
                EmailConfirmed = true,
                LockoutEnabled = false,
                SecurityStamp = Guid.NewGuid().ToString("D")
            };
            ricky.PasswordHash = passwordHash.HashPassword(ricky, "Ricky3!");
            modelBuilder.Entity<ApplicationUser>().HasData(ricky);

            modelBuilder.Entity<Continent> ().HasData (
                new Continent
                {
                    ContinentId = 1,
                    Code = "AF",
                    Name = "Africa"
                },
                new Continent
                {
                    ContinentId = 2,
                    Code = "AN",
                    Name = "Antarctica"
                },
                new Continent
                {
                    ContinentId = 3,
                    Code = "AS",
                    Name = "Asia"
                },
                new Continent
                {
                    ContinentId = 4,
                    Code = "EU",
                    Name = "Europe"
                },
                new Continent
                {
                    ContinentId = 5,
                    Code = "NA",
                    Name = "North America"
                },
                new Continent
                {
                    ContinentId = 6,
                    Code = "OC",
                    Name = "Oceania"
                },
                new Continent
                {
                    ContinentId = 7,
                    Code = "SA",
                    Name = "South America"
                }
            );

            modelBuilder.Entity<TravelType>().HasData(
               new TravelType
               {
                   TravelTypeId = 1,
                   Type = "Bus"
               },
               new TravelType
               {
                   TravelTypeId = 2,
                   Type = "Plane"
               },
               new TravelType
               {
                   TravelTypeId = 3,
                   Type = "Car"
               },
               new TravelType
               {
                   TravelTypeId = 4,
                   Type = "Ride Share"
               },
               new TravelType
               {
                   TravelTypeId = 5,
                   Type = "Train"
               },
               new TravelType
               {
                   TravelTypeId = 6,
                   Type = "Shuttle"
               },
               new TravelType
               {
                   TravelTypeId = 7,
                   Type = "Boat"
               },
               new TravelType
               {
                   TravelTypeId = 8,
                   Type = "Public Transport"
               }
            );

            modelBuilder.Entity<LocationType>().HasData(
               new LocationType
               {
                   LocationTypeId = 1,
                   Type = "Food"
               },
               new LocationType
               {
                   LocationTypeId = 2,
                   Type = "Place"
               }
            );

            modelBuilder.Entity<RetroType>().HasData(
               new RetroType
               {
                   RetroTypeId = 1,
                   Type = "Do Again"
               },
               new RetroType
               {
                   RetroTypeId = 2,
                   Type = "Do Different"
               }
            );

            modelBuilder.Entity<Trip>().HasData(
                new Trip
                {
                    TripId = 1,
                    UserId = kayla.Id,
                    ContinentId = 5,
                    Location = "Orlando Florida",
                    TripDates = "5/20/17-5/26/17",
                    Accommodation = "Cabana Bay",
                    Title = "1st Harry Potter World Trip",
                    ImagePath = "https://scontent.fbna1-1.fna.fbcdn.net/v/t31.0-8/19221504_10156323805382729_8735263925981311453_o.jpg?_nc_cat=111&_nc_ht=scontent.fbna1-1.fna&oh=27f178631784a8d9e88e2b7a117e8948&oe=5CC3F599",
                    Budget = null,
                    IsPreTrip = false,
                    DateFinished = DateTime.Now
                },
                new Trip
                {
                    TripId = 2,
                    UserId = kayla.Id,
                    ContinentId = 1,
                    Location = "Uganda",
                    TripDates = "2010",
                    Accommodation = "Campus",
                    Title = "Mission Trip",
                    ImagePath = "https://images.unsplash.com/photo-1516026672322-bc52d61a55d5?ixlib=rb-1.2.1&ixid=eyJhcHBfaWQiOjEyMDd9&auto=format&fit=crop&w=500&q=60",
                    Budget = 2000.00,
                    IsPreTrip = false,
                    DateFinished = DateTime.Now
                },
                new Trip
                {
                    TripId = 3,
                    UserId = ricky.Id,
                    ContinentId = 5,
                    Location = "Orlando Flordia",
                    TripDates = "5/20/17-5/26/17",
                    Accommodation = "Cabana Bay",
                    Title = "1st Harry Potter World Trip/Engagement Trip",
                    Budget = null,
                    ImagePath= "https://scontent.fbna1-1.fna.fbcdn.net/v/t31.0-8/19221723_10156323925257729_6632941831644369725_o.jpg?_nc_cat=108&_nc_ht=scontent.fbna1-1.fna&oh=25026fc7793a7dea17da4116d35de668&oe=5CC052A1",
                    IsPreTrip = false,
                    DateFinished = DateTime.Now
                },
                new Trip
                {
                    TripId = 4,
                    UserId = ricky.Id,
                    ContinentId = 5,
                    Location = "Portland OR",
                    TripDates = "2019",
                    Accommodation = "Airbnb",
                    Title = "Visit H & T Trip",
                    Budget = null,
                    IsPreTrip = true
                }
            );

            modelBuilder.Entity<TripTravelType>().HasData(
                new TripTravelType
                {
                    TripTravelTypeId = 1,
                    TripId = 1,
                    TravelTypeId = 2
                },
                new TripTravelType
                {
                    TripTravelTypeId = 2,
                    TripId = 1,
                    TravelTypeId = 4
                },
                new TripTravelType
                {
                    TripTravelTypeId = 3,
                    TripId = 2,
                    TravelTypeId = 2
                },
                new TripTravelType
                {
                    TripTravelTypeId = 4,
                    TripId = 2,
                    TravelTypeId = 8
                },
                new TripTravelType
                {
                    TripTravelTypeId = 5,
                    TripId = 3,
                    TravelTypeId = 2
                },
                new TripTravelType
                {
                    TripTravelTypeId = 6,
                    TripId = 3,
                    TravelTypeId = 4
                },
                new TripTravelType
                {
                    TripTravelTypeId = 7,
                    TripId = 4,
                    TravelTypeId = 2
                },
                new TripTravelType
                {
                    TripTravelTypeId = 8,
                    TripId = 4,
                    TravelTypeId = 3
                }
            );
            modelBuilder.Entity<TripRetro>().HasData(
                new TripRetro
                {
                    TripRetroId = 1,
                    TripId = 1,
                    RetroTypeId = 1,
                    Description = "It was sooooo amazing!!!!"
                },
                new TripRetro
                {
                    TripRetroId = 2,
                    TripId = 1,
                    RetroTypeId = 2,
                    Description = "It was Hot!!!!"
                },
                new TripRetro
                {
                    TripRetroId = 3,
                    TripId = 2,
                    RetroTypeId = 1,
                    Description = "It was sooooo amazing!!!!"
                },
                new TripRetro
                {
                    TripRetroId = 4,
                    TripId = 2,
                    RetroTypeId = 2,
                    Description = "It was Hot!!!!"
                },
                new TripRetro
                {
                    TripRetroId = 5,
                    TripId = 3,
                    RetroTypeId = 1,
                    Description = "It was sooooo amazing!!!!"
                },
                new TripRetro
                {
                    TripRetroId = 6,
                    TripId = 3,
                    RetroTypeId = 2,
                    Description = "It was Hot!!!!"
                }
            );

            modelBuilder.Entity<TripVisitLocation>().HasData(
                new TripVisitLocation
                {
                    TripVisitLocationId = 1,
                    TripId = 1,
                    LocationTypeId = 1,
                    Name = "The three broomsticks",
                    Description = "Yummy food joint that sells butter beer",
                    IsCompleted = true
                },
                new TripVisitLocation
                {
                    TripVisitLocationId = 2,
                    TripId = 1,
                    LocationTypeId = 2,
                    Name = "castle",
                    Description = "its sooo pretty",
                    IsCompleted = true
                },
                new TripVisitLocation
                {
                    TripVisitLocationId = 3,
                    TripId = 2,
                    LocationTypeId = 2,
                    Name = "Safari",
                    Description = "See an elephant",
                    IsCompleted = false
                },
                new TripVisitLocation
                {
                    TripVisitLocationId = 4,
                    TripId = 1,
                    LocationTypeId = 1,
                    Name = "Donut Place",
                    Description = "They have the pink donut thing from the show",
                    IsCompleted = false
                },
                new TripVisitLocation
                {
                    TripVisitLocationId = 5,
                    TripId = 3,
                    LocationTypeId = 1,
                    Name = "The three broomsticks",
                    Description = "Yummy food joint that sells butter beer",
                    IsCompleted = true
                },
                new TripVisitLocation
                {
                    TripVisitLocationId = 6,
                    TripId = 3,
                    LocationTypeId = 2,
                    Name = "HP world",
                    Description = "Its soooo pretty",
                    IsCompleted = true
                },
                new TripVisitLocation
                {
                    TripVisitLocationId = 7,
                    TripId = 4,
                    LocationTypeId = 2,
                    Name = "flee market",
                    Description = "they have awesome stuff",
                    IsCompleted = false
                },
                new TripVisitLocation
                {
                    TripVisitLocationId = 8,
                    TripId = 4,
                    LocationTypeId = 2,
                    Name = "H and T's house",
                    Description = "can't wait to see their cute place",
                    IsCompleted = false
                }
            );

        }
    }
}
