using HMS.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace HMS.Data
{
    public class HMSContext : IdentityDbContext<ApplicationUser>
    {
        public HMSContext(DbContextOptions<HMSContext> options) : base(options)
        {

        }

        public DbSet<Accomodation> Accomodations { get; set; }
        public DbSet<AccomodationPackage> AccomodationPackages { get; set; }
        public DbSet<AccomodationType> AccomodationTypes { get; set; }
        public DbSet<Booking> Booking { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
