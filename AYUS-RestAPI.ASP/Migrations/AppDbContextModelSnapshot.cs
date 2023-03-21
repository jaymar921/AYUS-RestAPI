﻿// <auto-generated />
using System;
using AYUS_RestAPI.ASP.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace AYUS_RestAPI.ASP.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("AYUS_RestAPI.Data.Logs", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Info")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("logs");
                });

            modelBuilder.Entity("AYUS_RestAPI.Data.ServiceMapLocationAPI", b =>
                {
                    b.Property<string>("SessionID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<double>("ClientLocLat")
                        .HasColumnType("float");

                    b.Property<double>("ClientLocLon")
                        .HasColumnType("float");

                    b.Property<double>("MechanicLocLat")
                        .HasColumnType("float");

                    b.Property<double>("MechanicLocLon")
                        .HasColumnType("float");

                    b.HasKey("SessionID");

                    b.ToTable("serviceMaps");
                });

            modelBuilder.Entity("AYUS_RestAPI.Data.Session", b =>
                {
                    b.Property<string>("SessionID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ClientUUID")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MechanicUUID")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SessionDetails")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("TimeEnd")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("TimeStart")
                        .HasColumnType("datetime2");

                    b.Property<string>("TransactionID")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("isActive")
                        .HasColumnType("bit");

                    b.HasKey("SessionID");

                    b.ToTable("sessions");
                });

            modelBuilder.Entity("AYUS_RestAPI.Data.Transaction", b =>
                {
                    b.Property<string>("ID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("DateOfTransaction")
                        .HasColumnType("datetime2");

                    b.Property<string>("Remark")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ServiceName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("ServicePrice")
                        .HasColumnType("float");

                    b.HasKey("ID");

                    b.ToTable("transactions");
                });

            modelBuilder.Entity("AYUS_RestAPI.Entity.Metadata.AccountStatus", b =>
                {
                    b.Property<string>("UUID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<bool>("IsLocked")
                        .HasColumnType("bit");

                    b.Property<bool>("IsOnline")
                        .HasColumnType("bit");

                    b.Property<double>("Rating")
                        .HasColumnType("float");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ShopID")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UUID");

                    b.ToTable("accountStatus");
                });

            modelBuilder.Entity("AYUS_RestAPI.Entity.Metadata.Credential", b =>
                {
                    b.Property<string>("UUID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UUID");

                    b.ToTable("credential");
                });

            modelBuilder.Entity("AYUS_RestAPI.Entity.Metadata.Mechanic.Billing", b =>
                {
                    b.Property<string>("BillingID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("BillingDate")
                        .HasColumnType("datetime2");

                    b.Property<double>("ServiceFee")
                        .HasColumnType("float");

                    b.Property<string>("ServiceRemark")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ShopID")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("BillingID");

                    b.HasIndex("ShopID");

                    b.ToTable("billing");
                });

            modelBuilder.Entity("AYUS_RestAPI.Entity.Metadata.Mechanic.Service", b =>
                {
                    b.Property<string>("ServiceID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ServiceDescription")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ServiceName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ServiceID");

                    b.ToTable("services");
                });

            modelBuilder.Entity("AYUS_RestAPI.Entity.Metadata.Mechanic.ServiceOffer", b =>
                {
                    b.Property<string>("UUID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.Property<string>("ServiceExpertise")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ServiceID")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ShopID")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UUID");

                    b.HasIndex("ShopID");

                    b.ToTable("serviceOffers");
                });

            modelBuilder.Entity("AYUS_RestAPI.Entity.Metadata.Mechanic.Shop", b =>
                {
                    b.Property<string>("ShopID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ShopDescription")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ShopName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ShopID");

                    b.ToTable("shops");
                });

            modelBuilder.Entity("AYUS_RestAPI.Entity.Metadata.PersonalInformation", b =>
                {
                    b.Property<string>("UUID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Birthdate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Contact")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Expiry")
                        .HasColumnType("datetime2");

                    b.Property<string>("Firstname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Lastname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LicenseNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UUID");

                    b.ToTable("personalInformation");
                });

            modelBuilder.Entity("AYUS_RestAPI.Entity.Metadata.Vehicle", b =>
                {
                    b.Property<string>("PlateNumber")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Brand")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Color")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Model")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UUID")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("PlateNumber");

                    b.ToTable("vehicles");
                });

            modelBuilder.Entity("AYUS_RestAPI.Entity.Metadata.Wallet", b =>
                {
                    b.Property<string>("UUID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<double>("Balance")
                        .HasColumnType("float");

                    b.Property<string>("Pincode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UUID");

                    b.ToTable("wallets");
                });

            modelBuilder.Entity("AYUS_RestAPI.Entity.Metadata.Mechanic.Billing", b =>
                {
                    b.HasOne("AYUS_RestAPI.Entity.Metadata.Mechanic.Shop", null)
                        .WithMany("Billings")
                        .HasForeignKey("ShopID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("AYUS_RestAPI.Entity.Metadata.Mechanic.ServiceOffer", b =>
                {
                    b.HasOne("AYUS_RestAPI.Entity.Metadata.Mechanic.Shop", null)
                        .WithMany("ServiceOffers")
                        .HasForeignKey("ShopID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("AYUS_RestAPI.Entity.Metadata.Mechanic.Shop", b =>
                {
                    b.Navigation("Billings");

                    b.Navigation("ServiceOffers");
                });
#pragma warning restore 612, 618
        }
    }
}
