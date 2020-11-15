﻿// <auto-generated />
using ControllerProgrammer.Data.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ControllerProgrammer.Data.Migrations
{
    [DbContext(typeof(ProgrammerContext))]
    [Migration("20201114165139_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.0");

            modelBuilder.Entity("ControllerProgrammer.Data.Model.Led", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Wavelength")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Leds");
                });

            modelBuilder.Entity("ControllerProgrammer.Data.Model.PowerDensity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<double>("Current")
                        .HasColumnType("REAL");

                    b.Property<int>("LedId")
                        .HasColumnType("INTEGER");

                    b.Property<double>("PowerDenisty")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.HasIndex("LedId");

                    b.ToTable("PowerDensities");
                });

            modelBuilder.Entity("ControllerProgrammer.Data.Model.PowerDensity", b =>
                {
                    b.HasOne("ControllerProgrammer.Data.Model.Led", "Led")
                        .WithMany("PowerDensities")
                        .HasForeignKey("LedId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.Navigation("Led");
                });

            modelBuilder.Entity("ControllerProgrammer.Data.Model.Led", b =>
                {
                    b.Navigation("PowerDensities");
                });
#pragma warning restore 612, 618
        }
    }
}