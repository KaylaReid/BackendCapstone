using System;
using System.Collections.Generic;
using System.Text;
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

            ApplicationUser Kayla = new ApplicationUser
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
            Kayla.PasswordHash = passwordHash.HashPassword(Kayla, "Kayla3!");
            modelBuilder.Entity<ApplicationUser>().HasData(Kayla);

            ApplicationUser Ricky = new ApplicationUser
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
            var passwordHash2 = new PasswordHasher<ApplicationUser>();
            Ricky.PasswordHash = passwordHash.HashPassword(Ricky, "Ricky3!");
            modelBuilder.Entity<ApplicationUser>().HasData(Ricky);

            modelBuilder.Entity<Continent> ().HasData (
                new Continent()
                {
                    ContinentId = 1,
                    Code = "AF",
                    Name = "Africa"
                },
                new Continent()
                {
                    ContinentId = 2,
                    Code = "AN",
                    Name = "Antarctica"
                },
                new Continent()
                {
                    ContinentId = 3,
                    Code = "AS",
                    Name = "Asia"
                },
                new Continent()
                {
                    ContinentId = 4,
                    Code = "EU",
                    Name = "Europe"
                },
                new Continent()
                {
                    ContinentId = 5,
                    Code = "NA",
                    Name = "North America"
                },
                new Continent()
                {
                    ContinentId = 6,
                    Code = "OC",
                    Name = "Oceania"
                },
                new Continent () {
                    ContinentId = 7,
                    Code = "SA",
                    Name = "South America"
                }
            );

            modelBuilder.Entity<TravelType>().HasData(
               new TravelType()
               {
                   TravelTypeId = 1,
                   Type = "Bus"
               },
               new TravelType()
               {
                   TravelTypeId = 2,
                   Type = "Plane"
               },
               new TravelType()
               {
                   TravelTypeId = 3,
                   Type = "Car"
               },
               new TravelType()
               {
                   TravelTypeId = 4,
                   Type = "Ride Share"
               },
               new TravelType()
               {
                   TravelTypeId = 5,
                   Type = "Train"
               },
               new TravelType()
               {
                   TravelTypeId = 6,
                   Type = "Shuttle"
               },
               new TravelType()
               {
                   TravelTypeId = 7,
                   Type = "Boat"
               },
               new TravelType()
               {
                   TravelTypeId = 8,
                   Type = "Public Transport"
               }
            );

            modelBuilder.Entity<LocationType>().HasData(
               new LocationType()
               {
                   LocationTypeId = 1,
                   Type = "Food"
               },
               new LocationType()
               {
                   LocationTypeId = 2,
                   Type = "Place"
               }
            );

            modelBuilder.Entity<RetroType>().HasData(
               new RetroType()
               {
                   RetroTypeId = 1,
                   Type = "Do Again"
               },
               new RetroType()
               {
                   RetroTypeId = 2,
                   Type = "Do Different"
               }
            );

            modelBuilder.Entity<Trip>().HasData(
                new Trip()
                {
                    TripId = 1,
                    UserId = Kayla.Id,
                    ContinentId = 5,
                    Location = "Orlando Flordia",
                    TripDates = "5/20/17-5/26/17",
                    Accommodation = "Cabana Bay",
                    Title = "1st Harry Potter World Trip",
                    Budget = "",
                    IsPreTrip = false
                },
                new Trip()
                {
                    TripId = 2,
                    UserId = Kayla.Id,
                    ContinentId = 1,
                    Location = "Uganda",
                    TripDates = "2010",
                    Accommodation = "Campus",
                    Title = "Mission Trip",
                    Budget = "",
                    IsPreTrip = false
                },
                new Trip()
                {
                    TripId = 3,
                    UserId = Ricky.Id,
                    ContinentId = 5,
                    Location = "Orlando Flordia",
                    TripDates = "5/20/17-5/26/17",
                    Accommodation = "Cabana Bay",
                    Title = "1st Harry Potter World Trip/Engagement Trip",
                    Budget = "",
                    IsPreTrip = false
                },
                new Trip()
                {
                    TripId = 4,
                    UserId = Ricky.Id,
                    ContinentId = 5,
                    Location = "Portland OR",
                    TripDates = "2019",
                    Accommodation = "Airbnb",
                    Title = "Visit H & T Trip",
                    Budget = "",
                    IsPreTrip = true
                }
            );

            modelBuilder.Entity<TripTravelType>().HasData(
                new TripTravelType()
                {
                    TripTravelTypeId = 1,
                    TripId = 1,
                    TravelTypeId = 2
                },
                new TripTravelType()
                {
                    TripTravelTypeId = 2,
                    TripId = 1,
                    TravelTypeId = 4
                },
                new TripTravelType()
                {
                    TripTravelTypeId = 3,
                    TripId = 2,
                    TravelTypeId = 2
                },
                new TripTravelType()
                {
                    TripTravelTypeId = 4,
                    TripId = 2,
                    TravelTypeId = 8
                },
                new TripTravelType()
                {
                    TripTravelTypeId = 5,
                    TripId = 3,
                    TravelTypeId = 2
                },
                new TripTravelType()
                {
                    TripTravelTypeId = 6,
                    TripId = 3,
                    TravelTypeId = 4
                },
                new TripTravelType()
                {
                    TripTravelTypeId = 7,
                    TripId = 4,
                    TravelTypeId = 2
                },
                new TripTravelType()
                {
                    TripTravelTypeId = 8,
                    TripId = 4,
                    TravelTypeId = 3
                }
            );
        }
    }
}
