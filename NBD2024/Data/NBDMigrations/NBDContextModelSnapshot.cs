﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NBD2024.Data;

#nullable disable

namespace NBD2024.Data.NBDMigrations
{
    [DbContext(typeof(NBDContext))]
    partial class NBDContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.16");

            modelBuilder.Entity("NBD2024.Models.Bid", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("BidDate")
                        .HasColumnType("TEXT");

                    b.Property<int>("LabourID")
                        .HasColumnType("INTEGER");

                    b.Property<int>("MaterialID")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ProjectID")
                        .HasColumnType("INTEGER");

                    b.HasKey("ID");

                    b.HasIndex("LabourID");

                    b.HasIndex("MaterialID");

                    b.HasIndex("ProjectID");

                    b.ToTable("Bids");
                });

            modelBuilder.Entity("NBD2024.Models.City", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("TEXT");

                    b.Property<string>("ProvinceID")
                        .IsRequired()
                        .HasMaxLength(2)
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.HasIndex("ProvinceID");

                    b.HasIndex("Name", "ProvinceID")
                        .IsUnique();

                    b.ToTable("Cities");
                });

            modelBuilder.Entity("NBD2024.Models.Client", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("AddressCountry")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<string>("AddressStreet")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("TEXT");

                    b.Property<int?>("CityID")
                        .HasColumnType("INTEGER");

                    b.Property<string>("CompanyName")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("TEXT");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<string>("MiddleName")
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("TEXT");

                    b.Property<string>("PostalCode")
                        .IsRequired()
                        .HasMaxLength(6)
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.HasIndex("CityID");

                    b.ToTable("Clients");
                });

            modelBuilder.Entity("NBD2024.Models.Inventory", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(80)
                        .HasColumnType("TEXT");

                    b.Property<double>("Price")
                        .HasColumnType("REAL");

                    b.Property<double>("PurchasePrice")
                        .HasColumnType("REAL");

                    b.Property<int>("Quantity")
                        .HasColumnType("INTEGER");

                    b.Property<string>("type")
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Inventories");
                });

            modelBuilder.Entity("NBD2024.Models.Labour", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("LabourDescription")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<double>("LabourHours")
                        .HasColumnType("REAL");

                    b.Property<int>("LabourTypeID")
                        .HasColumnType("INTEGER");

                    b.Property<double>("LabourUnitPrice")
                        .HasColumnType("REAL");

                    b.HasKey("ID");

                    b.HasIndex("LabourTypeID");

                    b.ToTable("Labours");
                });

            modelBuilder.Entity("NBD2024.Models.LabourType", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(120)
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("LabourTypes");
                });

            modelBuilder.Entity("NBD2024.Models.Material", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<int>("InventoryID")
                        .HasColumnType("INTEGER");

                    b.Property<double>("PerYardCharge")
                        .HasColumnType("REAL");

                    b.Property<int>("Quantity")
                        .HasColumnType("INTEGER");

                    b.HasKey("ID");

                    b.HasIndex("InventoryID");

                    b.ToTable("Materials");
                });

            modelBuilder.Entity("NBD2024.Models.Position", b =>
                {
                    b.Property<int>("ProjectID")
                        .HasColumnType("INTEGER");

                    b.Property<int>("StaffID")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ID")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.HasKey("ProjectID", "StaffID");

                    b.HasIndex("StaffID");

                    b.ToTable("Positions");
                });

            modelBuilder.Entity("NBD2024.Models.Project", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("CityID")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ClientID")
                        .HasColumnType("INTEGER");

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("CreatedOn")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("EndTime")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("ProjectName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<string>("ProjectSite")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("BLOB");

                    b.Property<string>("SetupNotes")
                        .HasMaxLength(3000)
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("UpdatedBy")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("UpdatedOn")
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.HasIndex("CityID");

                    b.HasIndex("ClientID");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("NBD2024.Models.Province", b =>
                {
                    b.Property<string>("ID")
                        .HasMaxLength(2)
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.ToTable("Provinces");
                });

            modelBuilder.Entity("NBD2024.Models.Staff", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<string>("MiddleName")
                        .HasMaxLength(30)
                        .HasColumnType("TEXT");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.ToTable("Staff");
                });

            modelBuilder.Entity("NBD2024.Models.Bid", b =>
                {
                    b.HasOne("NBD2024.Models.Labour", "Labour")
                        .WithMany()
                        .HasForeignKey("LabourID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NBD2024.Models.Material", "Material")
                        .WithMany()
                        .HasForeignKey("MaterialID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NBD2024.Models.Project", "Project")
                        .WithMany("Bids")
                        .HasForeignKey("ProjectID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Labour");

                    b.Navigation("Material");

                    b.Navigation("Project");
                });

            modelBuilder.Entity("NBD2024.Models.City", b =>
                {
                    b.HasOne("NBD2024.Models.Province", "Province")
                        .WithMany("Cities")
                        .HasForeignKey("ProvinceID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Province");
                });

            modelBuilder.Entity("NBD2024.Models.Client", b =>
                {
                    b.HasOne("NBD2024.Models.City", "City")
                        .WithMany("Clients")
                        .HasForeignKey("CityID");

                    b.Navigation("City");
                });

            modelBuilder.Entity("NBD2024.Models.Labour", b =>
                {
                    b.HasOne("NBD2024.Models.LabourType", "LabourType")
                        .WithMany()
                        .HasForeignKey("LabourTypeID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("LabourType");
                });

            modelBuilder.Entity("NBD2024.Models.Material", b =>
                {
                    b.HasOne("NBD2024.Models.Inventory", "Inventory")
                        .WithMany()
                        .HasForeignKey("InventoryID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Inventory");
                });

            modelBuilder.Entity("NBD2024.Models.Position", b =>
                {
                    b.HasOne("NBD2024.Models.Project", "Project")
                        .WithMany("Positions")
                        .HasForeignKey("ProjectID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NBD2024.Models.Staff", "Staff")
                        .WithMany("Positions")
                        .HasForeignKey("StaffID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Project");

                    b.Navigation("Staff");
                });

            modelBuilder.Entity("NBD2024.Models.Project", b =>
                {
                    b.HasOne("NBD2024.Models.City", "City")
                        .WithMany()
                        .HasForeignKey("CityID");

                    b.HasOne("NBD2024.Models.Client", "Client")
                        .WithMany("Projects")
                        .HasForeignKey("ClientID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("City");

                    b.Navigation("Client");
                });

            modelBuilder.Entity("NBD2024.Models.City", b =>
                {
                    b.Navigation("Clients");
                });

            modelBuilder.Entity("NBD2024.Models.Client", b =>
                {
                    b.Navigation("Projects");
                });

            modelBuilder.Entity("NBD2024.Models.Project", b =>
                {
                    b.Navigation("Bids");

                    b.Navigation("Positions");
                });

            modelBuilder.Entity("NBD2024.Models.Province", b =>
                {
                    b.Navigation("Cities");
                });

            modelBuilder.Entity("NBD2024.Models.Staff", b =>
                {
                    b.Navigation("Positions");
                });
#pragma warning restore 612, 618
        }
    }
}
