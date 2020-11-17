using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
            //this.ChangeTracker.LazyLoadingEnabled = false;
            //this.ChangeTracker.AutoDetectChangesEnabled = false;
            //this.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            //optionsBuilder.UseLazyLoadingProxies(false);
            //optionsBuilder.EnableSensitiveDataLogging(true);
            //optionsBuilder.EnableDetailedErrors(true);
            optionsBuilder.UseSqlite("Filename=ControllerData.db", options =>
            {
                string assemble = Assembly.GetExecutingAssembly().FullName;
                options.MigrationsAssembly(assemble);
            });

            //optionsBuilder.UseSqlServer("server=172.20.4.20;database=manufacturing_inventory_dev;user=aelmendorf;password=Drizzle123!;MultipleActiveResultSets=true");
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder) {
            builder.Entity<Led>().ToTable("Leds","controlDb");
            builder.Entity<PowerDensity>().ToTable("PowerDensities","controlDb");
            base.OnModelCreating(builder);
            //builder.Entity<Led>()
            //    .ToTable("Leds")
            //    .HasMany(e => e.PowerDensities)
            //    .WithOne(e => e.Led)
            //    .HasForeignKey(e=>e.LedId)
            //    .IsRequired(false)
            //    .OnDelete(DeleteBehavior.NoAction);

            //builder.Entity<PowerDensity>()
            //    .ToTable("PowerDensities")
            //    .HasOne(e => e.Led)
            //    .WithMany(e => e.PowerDensities)
            //    .IsRequired(false)
            //    .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
