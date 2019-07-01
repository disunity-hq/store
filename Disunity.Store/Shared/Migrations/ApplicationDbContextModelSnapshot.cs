﻿using System;

using Disunity.Store.Areas.Orgs.Models;
using Disunity.Store.Shared.Data;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;


namespace Disunity.Store.Shared.Migrations {

    [DbContext(typeof(ApplicationDbContext))]
    internal partial class ApplicationDbContextModelSnapshot : ModelSnapshot {

        protected override void BuildModel(ModelBuilder modelBuilder) {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:Enum:org_member_role", "owner,member")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("Disunity.Store.Areas.Identity.Models.UserIdentity", b => {
                b.Property<string>("Id")
                 .ValueGeneratedOnAdd();

                b.Property<int>("AccessFailedCount");

                b.Property<string>("ConcurrencyStamp")
                 .IsConcurrencyToken();

                b.Property<string>("Email")
                 .HasMaxLength(256);

                b.Property<bool>("EmailConfirmed");

                b.Property<bool>("LockoutEnabled");

                b.Property<DateTimeOffset?>("LockoutEnd");

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
                 .HasName("UserNameIndex");

                b.ToTable("AspNetUsers");
            });

            modelBuilder.Entity("Disunity.Store.Areas.Mods.Models.Mod", b => {
                b.Property<int>("Id")
                 .ValueGeneratedOnAdd();

                b.Property<DateTime>("CreatedAt");

                b.Property<bool?>("IsActive")
                 .ValueGeneratedOnAdd()
                 .HasDefaultValue(true);

                b.Property<bool?>("IsDeprecated")
                 .ValueGeneratedOnAdd()
                 .HasDefaultValue(false);

                b.Property<bool?>("IsPinned")
                 .ValueGeneratedOnAdd()
                 .HasDefaultValue(false);

                b.Property<int?>("LatestId");

                b.Property<string>("Name")
                 .IsRequired()
                 .HasMaxLength(128);

                b.Property<int?>("OwnerId");

                b.Property<DateTime>("UpdatedAt");

                b.HasKey("Id");

                b.HasAlternateKey("Name");

                b.HasIndex("LatestId");

                b.HasIndex("OwnerId");

                b.ToTable("Mods");
            });

            modelBuilder.Entity("Disunity.Store.Areas.Mods.Models.ModVersion", b => {
                b.Property<int>("Id")
                 .ValueGeneratedOnAdd();

                b.Property<DateTime>("CreatedAt");

                b.Property<string>("Description")
                 .IsRequired()
                 .HasMaxLength(256);

                b.Property<int?>("Downloads")
                 .ValueGeneratedOnAdd()
                 .HasDefaultValue(0);

                b.Property<string>("FileUrl")
                 .IsRequired()
                 .HasMaxLength(1024);

                b.Property<string>("IconUrl")
                 .IsRequired()
                 .HasMaxLength(1024);

                b.Property<bool?>("IsActive")
                 .ValueGeneratedOnAdd()
                 .HasDefaultValue(true);

                b.Property<int>("ModId");

                b.Property<int?>("ModVersionId");

                b.Property<string>("Name")
                 .IsRequired()
                 .HasMaxLength(128);

                b.Property<DateTime>("UpdatedAt");

                b.Property<string>("VersionNumber")
                 .IsRequired()
                 .HasMaxLength(16);

                b.Property<string>("WebsiteUrl")
                 .IsRequired()
                 .HasMaxLength(1024);

                b.HasKey("Id");

                b.HasAlternateKey("Name");

                b.HasAlternateKey("VersionNumber");

                b.HasIndex("ModId");

                b.HasIndex("ModVersionId");

                b.ToTable("ModVersions");
            });

            modelBuilder.Entity("Disunity.Store.Areas.Mods.Models.ModVersionDownloadEvent", b => {
                b.Property<string>("SourceIp");

                b.Property<int>("ModVersionId");

                b.Property<int?>("CountedDownloads")
                 .ValueGeneratedOnAdd()
                 .HasDefaultValue(1);

                b.Property<DateTime>("CreatedAt");

                b.Property<int?>("TotalDownloads")
                 .ValueGeneratedOnAdd()
                 .HasDefaultValue(1);

                b.Property<DateTime>("UpdatedAt");

                b.HasKey("SourceIp", "ModVersionId");

                b.HasIndex("ModVersionId");

                b.ToTable("ModVersionDownloadEvents");
            });

            modelBuilder.Entity("Disunity.Store.Areas.Orgs.Models.Org", b => {
                b.Property<int>("Id")
                 .ValueGeneratedOnAdd();

                b.Property<string>("Name")
                 .IsRequired();

                b.HasKey("Id");

                b.HasAlternateKey("Name");

                b.ToTable("Orgs");
            });

            modelBuilder.Entity("Disunity.Store.Areas.Orgs.Models.OrgMember", b => {
                b.Property<string>("UserId");

                b.Property<int>("OrgId");

                b.Property<OrgMemberRole>("Role");

                b.HasKey("UserId", "OrgId");

                b.HasIndex("OrgId");

                b.ToTable("OrgMembers");
            });

            modelBuilder.Entity("Disunity.Store.Areas.Targets.Models.Target", b => {
                b.Property<int>("ID")
                 .ValueGeneratedOnAdd();

                b.Property<int?>("LatestId");

                b.Property<string>("Name")
                 .IsRequired()
                 .HasMaxLength(128);

                b.HasKey("ID");

                b.HasAlternateKey("Name");

                b.HasIndex("LatestId")
                 .IsUnique();

                b.ToTable("Targets");
            });

            modelBuilder.Entity("Disunity.Store.Areas.Targets.Models.TargetVersion", b => {
                b.Property<int>("ID")
                 .ValueGeneratedOnAdd();

                b.Property<string>("Description")
                 .IsRequired()
                 .HasMaxLength(256);

                b.Property<string>("IconUrl")
                 .IsRequired()
                 .HasMaxLength(1024);

                b.Property<string>("Name")
                 .IsRequired()
                 .HasMaxLength(128);

                b.Property<int>("TargetId");

                b.Property<string>("VersionNumber")
                 .IsRequired()
                 .HasMaxLength(16);

                b.Property<string>("WebsiteUrl")
                 .IsRequired()
                 .HasMaxLength(1024);

                b.HasKey("ID");

                b.HasAlternateKey("Name");

                b.HasAlternateKey("VersionNumber");

                b.HasIndex("TargetId");

                b.ToTable("TargetVersions");
            });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b => {
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
                 .HasName("RoleNameIndex");

                b.ToTable("AspNetRoles");
            });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b => {
                b.Property<int>("Id")
                 .ValueGeneratedOnAdd();

                b.Property<string>("ClaimType");

                b.Property<string>("ClaimValue");

                b.Property<string>("RoleId")
                 .IsRequired();

                b.HasKey("Id");

                b.HasIndex("RoleId");

                b.ToTable("AspNetRoleClaims");
            });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b => {
                b.Property<int>("Id")
                 .ValueGeneratedOnAdd();

                b.Property<string>("ClaimType");

                b.Property<string>("ClaimValue");

                b.Property<string>("UserId")
                 .IsRequired();

                b.HasKey("Id");

                b.HasIndex("UserId");

                b.ToTable("AspNetUserClaims");
            });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b => {
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

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b => {
                b.Property<string>("UserId");

                b.Property<string>("RoleId");

                b.HasKey("UserId", "RoleId");

                b.HasIndex("RoleId");

                b.ToTable("AspNetUserRoles");
            });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b => {
                b.Property<string>("UserId");

                b.Property<string>("LoginProvider")
                 .HasMaxLength(128);

                b.Property<string>("Name")
                 .HasMaxLength(128);

                b.Property<string>("Value");

                b.HasKey("UserId", "LoginProvider", "Name");

                b.ToTable("AspNetUserTokens");
            });

            modelBuilder.Entity("Disunity.Store.Areas.Mods.Models.Mod", b => {
                b.HasOne("Disunity.Store.Areas.Mods.Models.ModVersion", "Latest")
                 .WithMany()
                 .HasForeignKey("LatestId");

                b.HasOne("Disunity.Store.Areas.Orgs.Models.Org", "Owner")
                 .WithMany("Mods")
                 .HasForeignKey("OwnerId")
                 .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity("Disunity.Store.Areas.Mods.Models.ModVersion", b => {
                b.HasOne("Disunity.Store.Areas.Mods.Models.Mod", "Mod")
                 .WithMany("Versions")
                 .HasForeignKey("ModId")
                 .OnDelete(DeleteBehavior.Cascade);

                b.HasOne("Disunity.Store.Areas.Mods.Models.ModVersion")
                 .WithMany("Dependencies")
                 .HasForeignKey("ModVersionId");
            });

            modelBuilder.Entity("Disunity.Store.Areas.Mods.Models.ModVersionDownloadEvent", b => {
                b.HasOne("Disunity.Store.Areas.Mods.Models.ModVersion", "ModVersion")
                 .WithMany()
                 .HasForeignKey("ModVersionId")
                 .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity("Disunity.Store.Areas.Orgs.Models.OrgMember", b => {
                b.HasOne("Disunity.Store.Areas.Orgs.Models.Org", "Org")
                 .WithMany("Members")
                 .HasForeignKey("OrgId")
                 .OnDelete(DeleteBehavior.Cascade);

                b.HasOne("Disunity.Store.Areas.Identity.Models.UserIdentity", "User")
                 .WithMany("Orgs")
                 .HasForeignKey("UserId")
                 .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity("Disunity.Store.Areas.Targets.Models.Target", b => {
                b.HasOne("Disunity.Store.Areas.Targets.Models.TargetVersion", "Latest")
                 .WithOne("Target")
                 .HasForeignKey("Disunity.Store.Areas.Targets.Models.Target", "LatestId");
            });

            modelBuilder.Entity("Disunity.Store.Areas.Targets.Models.TargetVersion", b => {
                b.HasOne("Disunity.Store.Areas.Targets.Models.Target")
                 .WithMany("Versions")
                 .HasForeignKey("TargetId")
                 .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b => {
                b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                 .WithMany()
                 .HasForeignKey("RoleId")
                 .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b => {
                b.HasOne("Disunity.Store.Areas.Identity.Models.UserIdentity")
                 .WithMany()
                 .HasForeignKey("UserId")
                 .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b => {
                b.HasOne("Disunity.Store.Areas.Identity.Models.UserIdentity")
                 .WithMany()
                 .HasForeignKey("UserId")
                 .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b => {
                b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                 .WithMany()
                 .HasForeignKey("RoleId")
                 .OnDelete(DeleteBehavior.Cascade);

                b.HasOne("Disunity.Store.Areas.Identity.Models.UserIdentity")
                 .WithMany()
                 .HasForeignKey("UserId")
                 .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b => {
                b.HasOne("Disunity.Store.Areas.Identity.Models.UserIdentity")
                 .WithMany()
                 .HasForeignKey("UserId")
                 .OnDelete(DeleteBehavior.Cascade);
            });
#pragma warning restore 612, 618
        }

    }

}