﻿// <auto-generated />
using System;
using Disunity.Store.Entities;
using Disunity.Store.Shared.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Disunity.Store.Entities.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:Enum:org_member_role", "owner,admin,member")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("Disunity.Store.Entities.DisunityVersion", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("URL");

                    b.Property<int>("VersionNumberId");

                    b.HasKey("ID");

                    b.HasIndex("VersionNumberId")
                        .IsUnique();

                    b.ToTable("DisunityVersions");
                });

            modelBuilder.Entity("Disunity.Store.Entities.DisunityVersionCompatibility", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("MaxCompatibleVersionId");

                    b.Property<int?>("MinCompatibleVersionId");

                    b.Property<int>("VersionId");

                    b.HasKey("ID");

                    b.HasIndex("MaxCompatibleVersionId");

                    b.HasIndex("MinCompatibleVersionId");

                    b.HasIndex("VersionId")
                        .IsUnique();

                    b.ToTable("DisunityVersionCompatibilities");
                });

            modelBuilder.Entity("Disunity.Store.Entities.Mod", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasMaxLength(128);

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

                    b.Property<int?>("OwnerId")
                        .IsRequired();

                    b.Property<string>("Slug")
                        .IsRequired()
                        .HasMaxLength(128);

                    b.Property<int?>("TargetID");

                    b.HasKey("Id");

                    b.HasAlternateKey("OwnerId", "DisplayName");

                    b.HasAlternateKey("OwnerId", "Slug");

                    b.HasIndex("LatestId");

                    b.HasIndex("TargetID");

                    b.ToTable("Mods");
                });

            modelBuilder.Entity("Disunity.Store.Entities.ModDependency", b =>
                {
                    b.Property<int>("DependantId");

                    b.Property<int>("DependencyId");

                    b.Property<int>("DependencyType");

                    b.Property<int?>("MaxVersionId");

                    b.Property<int?>("MinVersionId");

                    b.HasKey("DependantId", "DependencyId");

                    b.HasIndex("DependencyId");

                    b.HasIndex("MaxVersionId");

                    b.HasIndex("MinVersionId");

                    b.ToTable("ModDependencies");
                });

            modelBuilder.Entity("Disunity.Store.Entities.ModDisunityCompatibility", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("MaxCompatibleVersionId");

                    b.Property<int?>("MinCompatibleVersionId");

                    b.Property<int>("VersionId");

                    b.HasKey("ID");

                    b.HasIndex("MaxCompatibleVersionId");

                    b.HasIndex("MinCompatibleVersionId");

                    b.HasIndex("VersionId")
                        .IsUnique();

                    b.ToTable("ModDisunityCompatibilities");
                });

            modelBuilder.Entity("Disunity.Store.Entities.ModTargetCompatibility", b =>
                {
                    b.Property<int>("VersionId");

                    b.Property<int>("TargetId");

                    b.Property<int?>("MaxCompatibleVersionId");

                    b.Property<int?>("MinCompatibleVersionId");

                    b.HasKey("VersionId", "TargetId");

                    b.HasIndex("MaxCompatibleVersionId");

                    b.HasIndex("MinCompatibleVersionId");

                    b.HasIndex("TargetId");

                    b.ToTable("ModTargetCompatibilities");
                });

            modelBuilder.Entity("Disunity.Store.Entities.ModVersion", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(256);

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasMaxLength(128);

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

                    b.Property<string>("Readme")
                        .IsRequired();

                    b.Property<int>("VersionNumberId");

                    b.Property<string>("WebsiteUrl")
                        .IsRequired()
                        .HasMaxLength(1024);

                    b.HasKey("Id");

                    b.HasIndex("VersionNumberId");

                    b.HasIndex("ModId", "VersionNumberId")
                        .IsUnique();

                    b.ToTable("ModVersions");
                });

            modelBuilder.Entity("Disunity.Store.Entities.ModVersionDownloadEvent", b =>
                {
                    b.Property<string>("SourceIp");

                    b.Property<int>("ModVersionId");

                    b.Property<int?>("CountedDownloads")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(1);

                    b.Property<DateTime>("CreatedAt");

                    b.Property<DateTime>("LatestDownload");

                    b.Property<int?>("TotalDownloads")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(1);

                    b.Property<DateTime>("UpdatedAt");

                    b.HasKey("SourceIp", "ModVersionId");

                    b.HasIndex("ModVersionId");

                    b.ToTable("ModVersionDownloadEvents");
                });

            modelBuilder.Entity("Disunity.Store.Entities.Org", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasMaxLength(128);

                    b.Property<string>("Slug");

                    b.HasKey("Id");

                    b.HasIndex("Slug")
                        .IsUnique();

                    b.ToTable("Orgs");
                });

            modelBuilder.Entity("Disunity.Store.Entities.OrgMember", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<int>("OrgId");

                    b.Property<OrgMemberRole>("Role");

                    b.HasKey("UserId", "OrgId");

                    b.HasIndex("OrgId");

                    b.ToTable("OrgMembers");
                });

            modelBuilder.Entity("Disunity.Store.Entities.Target", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasMaxLength(128);

                    b.Property<int?>("LatestId");

                    b.Property<string>("Slug")
                        .IsRequired()
                        .HasMaxLength(128);

                    b.HasKey("ID");

                    b.HasIndex("LatestId")
                        .IsUnique();

                    b.HasIndex("Slug")
                        .IsUnique();

                    b.ToTable("Targets");
                });

            modelBuilder.Entity("Disunity.Store.Entities.TargetVersion", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(256);

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasMaxLength(128);

                    b.Property<string>("Hash")
                        .HasMaxLength(128);

                    b.Property<string>("IconUrl")
                        .IsRequired()
                        .HasMaxLength(1024);

                    b.Property<int>("TargetId");

                    b.Property<string>("VersionNumber")
                        .IsRequired()
                        .HasMaxLength(16);

                    b.Property<string>("WebsiteUrl")
                        .IsRequired()
                        .HasMaxLength(1024);

                    b.HasKey("ID");

                    b.HasAlternateKey("TargetId", "VersionNumber");

                    b.ToTable("TargetVersions");
                });

            modelBuilder.Entity("Disunity.Store.Entities.TargetVersionCompatibility", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("MaxCompatibleVersionId");

                    b.Property<int?>("MinCompatibleVersionId");

                    b.Property<int>("VersionId");

                    b.HasKey("ID");

                    b.HasIndex("MaxCompatibleVersionId");

                    b.HasIndex("MinCompatibleVersionId");

                    b.HasIndex("VersionId")
                        .IsUnique();

                    b.ToTable("TargetVersionCompatibilities");
                });

            modelBuilder.Entity("Disunity.Store.Entities.UnityVersion", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("VersionNumberId");

                    b.HasKey("ID");

                    b.HasIndex("VersionNumberId")
                        .IsUnique();

                    b.ToTable("UnityVersions");
                });

            modelBuilder.Entity("Disunity.Store.Entities.UserIdentity", b =>
                {
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

                    b.Property<string>("Slug");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.HasIndex("Slug")
                        .IsUnique();

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("Disunity.Store.Entities.VersionNumber", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Major");

                    b.Property<int>("Minor");

                    b.Property<int>("Patch");

                    b.HasKey("ID");

                    b.HasAlternateKey("Major", "Minor", "Patch");

                    b.ToTable("VersionNumbers");
                });

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
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
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

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
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

            modelBuilder.Entity("Disunity.Store.Entities.DisunityVersion", b =>
                {
                    b.HasOne("Disunity.Store.Entities.VersionNumber", "VersionNumber")
                        .WithMany()
                        .HasForeignKey("VersionNumberId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Disunity.Store.Entities.DisunityVersionCompatibility", b =>
                {
                    b.HasOne("Disunity.Store.Entities.UnityVersion", "MaxCompatibleVersion")
                        .WithMany()
                        .HasForeignKey("MaxCompatibleVersionId");

                    b.HasOne("Disunity.Store.Entities.UnityVersion", "MinCompatibleVersion")
                        .WithMany()
                        .HasForeignKey("MinCompatibleVersionId");

                    b.HasOne("Disunity.Store.Entities.DisunityVersion", "Version")
                        .WithOne("CompatibileUnityVersion")
                        .HasForeignKey("Disunity.Store.Entities.DisunityVersionCompatibility", "VersionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Disunity.Store.Entities.Mod", b =>
                {
                    b.HasOne("Disunity.Store.Entities.ModVersion", "Latest")
                        .WithMany()
                        .HasForeignKey("LatestId");

                    b.HasOne("Disunity.Store.Entities.Org", "Owner")
                        .WithMany("Mods")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Disunity.Store.Entities.Target")
                        .WithMany("CompatibleMods")
                        .HasForeignKey("TargetID");
                });

            modelBuilder.Entity("Disunity.Store.Entities.ModDependency", b =>
                {
                    b.HasOne("Disunity.Store.Entities.ModVersion", "Dependant")
                        .WithMany("ModDependencies")
                        .HasForeignKey("DependantId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Disunity.Store.Entities.Mod", "Dependency")
                        .WithMany()
                        .HasForeignKey("DependencyId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Disunity.Store.Entities.ModVersion", "MaxVersion")
                        .WithMany()
                        .HasForeignKey("MaxVersionId");

                    b.HasOne("Disunity.Store.Entities.ModVersion", "MinVersion")
                        .WithMany()
                        .HasForeignKey("MinVersionId");
                });

            modelBuilder.Entity("Disunity.Store.Entities.ModDisunityCompatibility", b =>
                {
                    b.HasOne("Disunity.Store.Entities.DisunityVersion", "MaxCompatibleVersion")
                        .WithMany()
                        .HasForeignKey("MaxCompatibleVersionId");

                    b.HasOne("Disunity.Store.Entities.DisunityVersion", "MinCompatibleVersion")
                        .WithMany()
                        .HasForeignKey("MinCompatibleVersionId");

                    b.HasOne("Disunity.Store.Entities.ModVersion", "Version")
                        .WithMany()
                        .HasForeignKey("VersionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Disunity.Store.Entities.ModTargetCompatibility", b =>
                {
                    b.HasOne("Disunity.Store.Entities.TargetVersion", "MaxCompatibleVersion")
                        .WithMany()
                        .HasForeignKey("MaxCompatibleVersionId");

                    b.HasOne("Disunity.Store.Entities.TargetVersion", "MinCompatibleVersion")
                        .WithMany()
                        .HasForeignKey("MinCompatibleVersionId");

                    b.HasOne("Disunity.Store.Entities.Target", "Target")
                        .WithMany("Compatibilities")
                        .HasForeignKey("TargetId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Disunity.Store.Entities.ModVersion", "Version")
                        .WithMany("TargetCompatibilities")
                        .HasForeignKey("VersionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Disunity.Store.Entities.ModVersion", b =>
                {
                    b.HasOne("Disunity.Store.Entities.Mod", "Mod")
                        .WithMany("Versions")
                        .HasForeignKey("ModId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Disunity.Store.Entities.VersionNumber", "VersionNumber")
                        .WithMany()
                        .HasForeignKey("VersionNumberId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Disunity.Store.Entities.ModVersionDownloadEvent", b =>
                {
                    b.HasOne("Disunity.Store.Entities.ModVersion", "ModVersion")
                        .WithMany()
                        .HasForeignKey("ModVersionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Disunity.Store.Entities.OrgMember", b =>
                {
                    b.HasOne("Disunity.Store.Entities.Org", "Org")
                        .WithMany("Members")
                        .HasForeignKey("OrgId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Disunity.Store.Entities.UserIdentity", "User")
                        .WithMany("Orgs")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Disunity.Store.Entities.Target", b =>
                {
                    b.HasOne("Disunity.Store.Entities.TargetVersion", "Latest")
                        .WithOne("Target")
                        .HasForeignKey("Disunity.Store.Entities.Target", "LatestId");
                });

            modelBuilder.Entity("Disunity.Store.Entities.TargetVersion", b =>
                {
                    b.HasOne("Disunity.Store.Entities.Target")
                        .WithMany("Versions")
                        .HasForeignKey("TargetId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Disunity.Store.Entities.TargetVersionCompatibility", b =>
                {
                    b.HasOne("Disunity.Store.Entities.UnityVersion", "MaxCompatibleVersion")
                        .WithMany()
                        .HasForeignKey("MaxCompatibleVersionId");

                    b.HasOne("Disunity.Store.Entities.UnityVersion", "MinCompatibleVersion")
                        .WithMany()
                        .HasForeignKey("MinCompatibleVersionId");

                    b.HasOne("Disunity.Store.Entities.TargetVersion", "Version")
                        .WithOne("DisunityCompatibility")
                        .HasForeignKey("Disunity.Store.Entities.TargetVersionCompatibility", "VersionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Disunity.Store.Entities.UnityVersion", b =>
                {
                    b.HasOne("Disunity.Store.Entities.VersionNumber", "VersionNumber")
                        .WithMany()
                        .HasForeignKey("VersionNumberId")
                        .OnDelete(DeleteBehavior.Cascade);
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
                    b.HasOne("Disunity.Store.Entities.UserIdentity")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Disunity.Store.Entities.UserIdentity")
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

                    b.HasOne("Disunity.Store.Entities.UserIdentity")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Disunity.Store.Entities.UserIdentity")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
