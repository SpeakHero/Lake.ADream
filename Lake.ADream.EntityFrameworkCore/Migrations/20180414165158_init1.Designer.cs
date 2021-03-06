﻿// <auto-generated />
using Lake.ADream.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.Data.EntityFrameworkCore.Storage.Internal;
using System;

namespace Lake.ADream.EntityFrameworkCore.Migrations
{
    [DbContext(typeof(ADreamDbContext))]
    [Migration("20180414165158_init1")]
    partial class init1
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.2-rtm-10011");

            modelBuilder.Entity("Lake.ADream.Models.Entities.Identity.Permission", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Action")
                        .IsRequired();

                    b.Property<string>("AreaName");

                    b.Property<string>("Controller")
                        .IsRequired();

                    b.Property<DateTime>("CreatedTime");

                    b.Property<string>("CretaedUser")
                        .HasMaxLength(50);

                    b.Property<string>("Description");

                    b.Property<string>("EditeUser")
                        .HasMaxLength(50);

                    b.Property<DateTime>("EditedTime");

                    b.Property<bool>("IsAllowAnonymous");

                    b.Property<bool>("IsDelete");

                    b.Property<bool>("IsEnable");

                    b.Property<int>("Level");

                    b.Property<string>("Params");

                    b.Property<int>("ShowSort");

                    b.Property<DateTime>("TimeSpan")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("Id");

                    b.ToTable("Permissions");
                });

            modelBuilder.Entity("Lake.ADream.Models.Entities.Identity.Role", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp");

                    b.Property<DateTime>("CreatedTime");

                    b.Property<string>("CretaedUser")
                        .HasMaxLength(50);

                    b.Property<string>("Description");

                    b.Property<string>("EditeUser")
                        .HasMaxLength(50);

                    b.Property<DateTime>("EditedTime");

                    b.Property<bool>("IsDelete");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("NormalizedName");

                    b.Property<DateTime>("TimeSpan")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("Lake.ADream.Models.Entities.Identity.RoleClaim", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<DateTime>("CreatedTime");

                    b.Property<string>("CretaedUser")
                        .HasMaxLength(50);

                    b.Property<string>("Description");

                    b.Property<string>("EditeUser")
                        .HasMaxLength(50);

                    b.Property<DateTime>("EditedTime");

                    b.Property<bool>("IsDelete");

                    b.Property<string>("RoleId");

                    b.Property<DateTime>("TimeSpan")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("Id");

                    b.ToTable("RoleClaims");
                });

            modelBuilder.Entity("Lake.ADream.Models.Entities.Identity.RolePermission", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ApplicationPermissionId")
                        .IsRequired();

                    b.Property<DateTime>("CreatedTime");

                    b.Property<string>("CretaedUser")
                        .HasMaxLength(50);

                    b.Property<string>("Description");

                    b.Property<string>("EditeUser")
                        .HasMaxLength(50);

                    b.Property<DateTime>("EditedTime");

                    b.Property<bool>("IsDelete");

                    b.Property<bool>("IsEnable");

                    b.Property<string>("PermissionId");

                    b.Property<string>("Regular");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.Property<DateTime>("TimeSpan")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("PermissionId");

                    b.HasIndex("RoleId");

                    b.ToTable("RolePermissions");
                });

            modelBuilder.Entity("Lake.ADream.Models.Entities.Identity.User", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp");

                    b.Property<DateTime>("CreatedTime");

                    b.Property<string>("CretaedUser")
                        .HasMaxLength(50);

                    b.Property<string>("Description");

                    b.Property<string>("EditeUser")
                        .HasMaxLength(50);

                    b.Property<DateTime>("EditedTime");

                    b.Property<string>("Email")
                        .HasMaxLength(50);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("IsDelete");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(50);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityAudit");

                    b.Property<string>("SecurityStamp");

                    b.Property<int>("Sex");

                    b.Property<DateTime>("TimeSpan")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserClaimId");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.HasIndex("UserClaimId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Lake.ADream.Models.Entities.Identity.UserClaim", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<DateTime>("CreatedTime");

                    b.Property<string>("CretaedUser")
                        .HasMaxLength(50);

                    b.Property<string>("Description");

                    b.Property<string>("EditeUser")
                        .HasMaxLength(50);

                    b.Property<DateTime>("EditedTime");

                    b.Property<bool>("IsDelete");

                    b.Property<DateTime>("TimeSpan")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("UserClaims");
                });

            modelBuilder.Entity("Lake.ADream.Models.Entities.Identity.UserLogin", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedTime");

                    b.Property<string>("CretaedUser")
                        .HasMaxLength(50);

                    b.Property<string>("Description");

                    b.Property<string>("EditeUser")
                        .HasMaxLength(50);

                    b.Property<DateTime>("EditedTime");

                    b.Property<bool>("IsDelete");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("ProviderKey");

                    b.Property<DateTime>("TimeSpan")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.ToTable("UserLogins");
                });

            modelBuilder.Entity("Lake.ADream.Models.Entities.Identity.UserRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedTime");

                    b.Property<string>("CretaedUser")
                        .HasMaxLength(50);

                    b.Property<string>("Description");

                    b.Property<string>("EditeUser")
                        .HasMaxLength(50);

                    b.Property<DateTime>("EditedTime");

                    b.Property<bool>("IsDelete");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.Property<DateTime>("TimeSpan")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("UserRoles");
                });

            modelBuilder.Entity("Lake.ADream.Models.Entities.Identity.UserToken", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedTime");

                    b.Property<string>("CretaedUser")
                        .HasMaxLength(50);

                    b.Property<string>("Description");

                    b.Property<string>("EditeUser")
                        .HasMaxLength(50);

                    b.Property<DateTime>("EditedTime");

                    b.Property<bool>("IsDelete");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<DateTime>("TimeSpan")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<string>("UserId");

                    b.Property<string>("Value");

                    b.HasKey("Id");

                    b.ToTable("UserTokens");
                });

            modelBuilder.Entity("Lake.ADream.Models.Entities.Identity.RolePermission", b =>
                {
                    b.HasOne("Lake.ADream.Models.Entities.Identity.Permission")
                        .WithMany("Role")
                        .HasForeignKey("PermissionId");

                    b.HasOne("Lake.ADream.Models.Entities.Identity.Role")
                        .WithMany("Permissions")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Lake.ADream.Models.Entities.Identity.User", b =>
                {
                    b.HasOne("Lake.ADream.Models.Entities.Identity.UserClaim")
                        .WithMany("User")
                        .HasForeignKey("UserClaimId");
                });
#pragma warning restore 612, 618
        }
    }
}
