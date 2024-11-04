﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using VehicleService.Infrastructure.Shared.Context;

#nullable disable

namespace VehicleService.Migrations
{
    [DbContext(typeof(VehicleDbContext))]
    partial class VehicleDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("VehicleService.Domain.Publishing.Models.Entities.Vehicle", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Brand")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("CarClass")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("Consumption")
                        .HasColumnType("int");

                    b.Property<string>("Dimensions")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<bool>("IsActive")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("MaximumSpeed")
                        .HasColumnType("int");

                    b.Property<string>("Model")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("OwnerId")
                        .HasColumnType("int");

                    b.Property<string>("PickUpPlace")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("RentStatus")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("RentalCost")
                        .HasColumnType("int");

                    b.Property<string>("TimeType")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Transmission")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("UrlImage")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("Weight")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Vehicles", (string)null);
                });
#pragma warning restore 612, 618
        }
    }
}