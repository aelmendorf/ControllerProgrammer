using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControllerProgrammer.Data.Model {
    public class ProgrammerContext:DbContext {

        //public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Led> Leds { get; set; }
        public DbSet<PowerDensity> PowerDensities { get; set; }

        public ProgrammerContext(DbContextOptions<ProgrammerContext> options) : base(options) {
            //this.ChangeTracker.LazyLoadingEnabled = false;
            //this.ChangeTracker.AutoDetectChangesEnabled = false;
            //this.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public ProgrammerContext() : base() {
            this.ChangeTracker.LazyLoadingEnabled = false;
            this.ChangeTracker.AutoDetectChangesEnabled = false;
            //this.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            //optionsBuilder.UseLazyLoadingProxies(false);
            //optionsBuilder.EnableSensitiveDataLogging(true);
            //optionsBuilder.EnableDetailedErrors(true);
            optionsBuilder.UseSqlite("Data Source=ControllerData.db");

            //optionsBuilder.UseSqlServer("server=172.20.4.20;database=manufacturing_inventory_dev;user=aelmendorf;password=Drizzle123!;MultipleActiveResultSets=true");

        }

        protected override void OnModelCreating(ModelBuilder builder) {
            builder.Entity<Led>()
                .HasMany(e => e.PowerDensities)
                .WithOne(e => e.Led)
                .HasForeignKey(e=>e.LedId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
