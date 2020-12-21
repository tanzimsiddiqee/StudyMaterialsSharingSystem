﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using StudyMaterialsSharingSystem.Data;

namespace StudyMaterialsSharingSystem.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.2-servicing-10034")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128);

                    b.Property<string>("ProviderKey")
                        .HasMaxLength(128);

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128);

                    b.Property<string>("Name")
                        .HasMaxLength(128);

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("StudyMaterialsSharingSystem.Areas.Identity.Data.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<DateTime>("DOB");

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("Name");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("StudyMaterialsSharingSystem.Models.Book", b =>
                {
                    b.Property<string>("BookID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("AdTime");

                    b.Property<string>("BookName");

                    b.Property<string>("Condition");

                    b.Property<string>("Description");

                    b.Property<string>("Email");

                    b.Property<string>("ImagePath");

                    b.Property<string>("Location");

                    b.Property<string>("OwnerID");

                    b.Property<string>("Phone");

                    b.Property<string>("Price");

                    b.Property<string>("PriceType");

                    b.Property<int>("Status");

                    b.Property<int>("SubCategoryID");

                    b.HasKey("BookID");

                    b.HasIndex("SubCategoryID");

                    b.ToTable("Books");
                });

            modelBuilder.Entity("StudyMaterialsSharingSystem.Models.Category", b =>
                {
                    b.Property<int>("CategoryID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CategoryName");

                    b.HasKey("CategoryID");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("StudyMaterialsSharingSystem.Models.Document", b =>
                {
                    b.Property<string>("DocumentID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("AdTime");

                    b.Property<string>("Description");

                    b.Property<string>("DocumentFormat");

                    b.Property<string>("DocumentName");

                    b.Property<string>("DownloadAddress");

                    b.Property<string>("ImagePath");

                    b.Property<string>("OwnerID");

                    b.Property<int>("Status");

                    b.HasKey("DocumentID");

                    b.ToTable("Documents");
                });

            modelBuilder.Entity("StudyMaterialsSharingSystem.Models.Feedback", b =>
                {
                    b.Property<string>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Comment");

                    b.Property<bool>("Read");

                    b.Property<string>("User");

                    b.Property<DateTime>("dateTime");

                    b.HasKey("ID");

                    b.ToTable("Feedbacks");
                });

            modelBuilder.Entity("StudyMaterialsSharingSystem.Models.House", b =>
                {
                    b.Property<string>("HouseID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("AdTime");

                    b.Property<string>("AvailableRoom");

                    b.Property<string>("AvailableSeat");

                    b.Property<string>("Category");

                    b.Property<string>("Description");

                    b.Property<string>("Email");

                    b.Property<string>("Facilities");

                    b.Property<string>("HouseName");

                    b.Property<string>("HouseType");

                    b.Property<string>("ImagePath");

                    b.Property<string>("Location");

                    b.Property<string>("OwnerID");

                    b.Property<string>("Phone");

                    b.Property<string>("RentPerRoom");

                    b.Property<string>("RentPerSeat");

                    b.Property<string>("ServiceCharge");

                    b.Property<int>("Status");

                    b.Property<DateTime>("dateTime");

                    b.HasKey("HouseID");

                    b.ToTable("Houses");
                });

            modelBuilder.Entity("StudyMaterialsSharingSystem.Models.Reply", b =>
                {
                    b.Property<string>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Message");

                    b.Property<string>("RequestID");

                    b.Property<string>("Sender");

                    b.Property<DateTime>("dateTime");

                    b.HasKey("ID");

                    b.HasIndex("RequestID");

                    b.ToTable("Replies");
                });

            modelBuilder.Entity("StudyMaterialsSharingSystem.Models.Request", b =>
                {
                    b.Property<string>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Material");

                    b.Property<string>("MaterialID");

                    b.Property<bool>("Read");

                    b.Property<string>("Receiver");

                    b.Property<string>("Sender");

                    b.Property<DateTime>("dateTime");

                    b.HasKey("ID");

                    b.ToTable("Requests");
                });

            modelBuilder.Entity("StudyMaterialsSharingSystem.Models.Software", b =>
                {
                    b.Property<string>("SoftwareID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("AdTime");

                    b.Property<string>("Description");

                    b.Property<string>("DownloadAddress");

                    b.Property<string>("DownloadingProcess");

                    b.Property<string>("ImagePath");

                    b.Property<string>("InstallingProcess");

                    b.Property<string>("OwnerID");

                    b.Property<string>("SoftwareName");

                    b.Property<int>("SoftwareTypeID");

                    b.Property<int>("Status");

                    b.Property<string>("Version");

                    b.HasKey("SoftwareID");

                    b.HasIndex("SoftwareTypeID");

                    b.ToTable("Softwares");
                });

            modelBuilder.Entity("StudyMaterialsSharingSystem.Models.SoftwareType", b =>
                {
                    b.Property<int>("TypeID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("TypeName");

                    b.HasKey("TypeID");

                    b.ToTable("SoftwareTypes");
                });

            modelBuilder.Entity("StudyMaterialsSharingSystem.Models.SubCategory", b =>
                {
                    b.Property<int>("SubCategoryID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CategoryID");

                    b.Property<string>("SubCategoryName");

                    b.HasKey("SubCategoryID");

                    b.HasIndex("CategoryID");

                    b.ToTable("SubCategories");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("StudyMaterialsSharingSystem.Areas.Identity.Data.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("StudyMaterialsSharingSystem.Areas.Identity.Data.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("StudyMaterialsSharingSystem.Areas.Identity.Data.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("StudyMaterialsSharingSystem.Areas.Identity.Data.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("StudyMaterialsSharingSystem.Models.Book", b =>
                {
                    b.HasOne("StudyMaterialsSharingSystem.Models.SubCategory", "SubCategory")
                        .WithMany("Books")
                        .HasForeignKey("SubCategoryID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("StudyMaterialsSharingSystem.Models.Reply", b =>
                {
                    b.HasOne("StudyMaterialsSharingSystem.Models.Request", "Request")
                        .WithMany("Replies")
                        .HasForeignKey("RequestID");
                });

            modelBuilder.Entity("StudyMaterialsSharingSystem.Models.Software", b =>
                {
                    b.HasOne("StudyMaterialsSharingSystem.Models.SoftwareType", "SoftwareType")
                        .WithMany("Softwares")
                        .HasForeignKey("SoftwareTypeID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("StudyMaterialsSharingSystem.Models.SubCategory", b =>
                {
                    b.HasOne("StudyMaterialsSharingSystem.Models.Category", "Category")
                        .WithMany("SubCategories")
                        .HasForeignKey("CategoryID")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
