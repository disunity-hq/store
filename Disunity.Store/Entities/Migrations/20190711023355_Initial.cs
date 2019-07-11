using System;
using Disunity.Store.Entities;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Disunity.Store.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:org_member_role", "owner,admin,member");

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    Slug = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DisunityVersions",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    URL = table.Column<string>(nullable: true),
                    Version = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DisunityVersions", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Orgs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    DisplayName = table.Column<string>(maxLength: 128, nullable: false),
                    Slug = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orgs", x => x.Id);
                    table.UniqueConstraint("AK_Orgs_DisplayName", x => x.DisplayName);
                });

            migrationBuilder.CreateTable(
                name: "UnityVersion",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Version = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnityVersion", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    RoleId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    UserId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(maxLength: 128, nullable: false),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrgMembers",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    OrgId = table.Column<int>(nullable: false),
                    Role = table.Column<OrgMemberRole>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrgMembers", x => new { x.UserId, x.OrgId });
                    table.ForeignKey(
                        name: "FK_OrgMembers_Orgs_OrgId",
                        column: x => x.OrgId,
                        principalTable: "Orgs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrgMembers_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DisunityVersionCompatibilities",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    VersionId = table.Column<int>(nullable: false),
                    MinCompatibleVersionId = table.Column<int>(nullable: true),
                    MaxCompatibleVersionId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DisunityVersionCompatibilities", x => x.ID);
                    table.ForeignKey(
                        name: "FK_DisunityVersionCompatibilities_UnityVersion_MaxCompatibleVe~",
                        column: x => x.MaxCompatibleVersionId,
                        principalTable: "UnityVersion",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DisunityVersionCompatibilities_UnityVersion_MinCompatibleVe~",
                        column: x => x.MinCompatibleVersionId,
                        principalTable: "UnityVersion",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DisunityVersionCompatibilities_DisunityVersions_VersionId",
                        column: x => x.VersionId,
                        principalTable: "DisunityVersions",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ModDisunityCompatibilities",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    VersionId = table.Column<int>(nullable: false),
                    MinCompatibleVersionId = table.Column<int>(nullable: true),
                    MaxCompatibleVersionId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModDisunityCompatibilities", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ModDisunityCompatibilities_DisunityVersions_MaxCompatibleVe~",
                        column: x => x.MaxCompatibleVersionId,
                        principalTable: "DisunityVersions",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ModDisunityCompatibilities_DisunityVersions_MinCompatibleVe~",
                        column: x => x.MinCompatibleVersionId,
                        principalTable: "DisunityVersions",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ModDependencies",
                columns: table => new
                {
                    DependantId = table.Column<int>(nullable: false),
                    DependencyId = table.Column<int>(nullable: false),
                    MinVersionId = table.Column<int>(nullable: true),
                    MaxVersionId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModDependencies", x => new { x.DependantId, x.DependencyId });
                });

            migrationBuilder.CreateTable(
                name: "ModVersions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    ModId = table.Column<int>(nullable: false),
                    DisplayName = table.Column<string>(maxLength: 128, nullable: false),
                    IsActive = table.Column<bool>(nullable: true, defaultValue: true),
                    Downloads = table.Column<int>(nullable: true, defaultValue: 0),
                    VersionNumber = table.Column<string>(maxLength: 16, nullable: false),
                    WebsiteUrl = table.Column<string>(maxLength: 1024, nullable: false),
                    Description = table.Column<string>(maxLength: 256, nullable: false),
                    Readme = table.Column<string>(nullable: false),
                    FileUrl = table.Column<string>(maxLength: 1024, nullable: false),
                    IconUrl = table.Column<string>(maxLength: 1024, nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ModVersionId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModVersions", x => x.Id);
                    table.UniqueConstraint("AK_ModVersions_DisplayName", x => x.DisplayName);
                    table.UniqueConstraint("AK_ModVersions_VersionNumber", x => x.VersionNumber);
                    table.ForeignKey(
                        name: "FK_ModVersions_ModVersions_ModVersionId",
                        column: x => x.ModVersionId,
                        principalTable: "ModVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Mods",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    OwnerId = table.Column<int>(nullable: false),
                    DisplayName = table.Column<string>(maxLength: 128, nullable: false),
                    Slug = table.Column<string>(maxLength: 128, nullable: false),
                    IsActive = table.Column<bool>(nullable: true, defaultValue: true),
                    IsDeprecated = table.Column<bool>(nullable: true, defaultValue: false),
                    IsPinned = table.Column<bool>(nullable: true, defaultValue: false),
                    LatestId = table.Column<int>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mods", x => x.Id);
                    table.UniqueConstraint("AK_Mods_OwnerId_DisplayName", x => new { x.OwnerId, x.DisplayName });
                    table.UniqueConstraint("AK_Mods_OwnerId_Slug", x => new { x.OwnerId, x.Slug });
                    table.ForeignKey(
                        name: "FK_Mods_ModVersions_LatestId",
                        column: x => x.LatestId,
                        principalTable: "ModVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Mods_Orgs_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Orgs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ModVersionDownloadEvents",
                columns: table => new
                {
                    ModVersionId = table.Column<int>(nullable: false),
                    SourceIp = table.Column<string>(nullable: false),
                    LatestDownload = table.Column<DateTime>(nullable: false),
                    TotalDownloads = table.Column<int>(nullable: true, defaultValue: 1),
                    CountedDownloads = table.Column<int>(nullable: true, defaultValue: 1),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModVersionDownloadEvents", x => new { x.SourceIp, x.ModVersionId });
                    table.ForeignKey(
                        name: "FK_ModVersionDownloadEvents_ModVersions_ModVersionId",
                        column: x => x.ModVersionId,
                        principalTable: "ModVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ModTargetCompatibilities",
                columns: table => new
                {
                    VersionId = table.Column<int>(nullable: false),
                    TargetId = table.Column<int>(nullable: false),
                    MinCompatibleVersionId = table.Column<int>(nullable: true),
                    MaxCompatibleVersionId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModTargetCompatibilities", x => new { x.VersionId, x.TargetId });
                    table.ForeignKey(
                        name: "FK_ModTargetCompatibilities_ModVersions_VersionId",
                        column: x => x.VersionId,
                        principalTable: "ModVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TargetVersions",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    TargetId = table.Column<int>(nullable: false),
                    DisplayName = table.Column<string>(maxLength: 128, nullable: false),
                    VersionNumber = table.Column<string>(maxLength: 16, nullable: false),
                    WebsiteUrl = table.Column<string>(maxLength: 1024, nullable: false),
                    Description = table.Column<string>(maxLength: 256, nullable: false),
                    IconUrl = table.Column<string>(maxLength: 1024, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TargetVersions", x => x.ID);
                    table.UniqueConstraint("AK_TargetVersions_DisplayName", x => x.DisplayName);
                    table.UniqueConstraint("AK_TargetVersions_VersionNumber", x => x.VersionNumber);
                });

            migrationBuilder.CreateTable(
                name: "Targets",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    LatestId = table.Column<int>(nullable: true),
                    DisplayName = table.Column<string>(maxLength: 128, nullable: false),
                    Slug = table.Column<string>(maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Targets", x => x.ID);
                    table.UniqueConstraint("AK_Targets_DisplayName", x => x.DisplayName);
                    table.ForeignKey(
                        name: "FK_Targets_TargetVersions_LatestId",
                        column: x => x.LatestId,
                        principalTable: "TargetVersions",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TargetVersionCompatibilities",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    VersionId = table.Column<int>(nullable: false),
                    MinCompatibleVersionId = table.Column<int>(nullable: true),
                    MaxCompatibleVersionId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TargetVersionCompatibilities", x => x.ID);
                    table.ForeignKey(
                        name: "FK_TargetVersionCompatibilities_UnityVersion_MaxCompatibleVers~",
                        column: x => x.MaxCompatibleVersionId,
                        principalTable: "UnityVersion",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TargetVersionCompatibilities_UnityVersion_MinCompatibleVers~",
                        column: x => x.MinCompatibleVersionId,
                        principalTable: "UnityVersion",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TargetVersionCompatibilities_TargetVersions_VersionId",
                        column: x => x.VersionId,
                        principalTable: "TargetVersions",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_Slug",
                table: "AspNetUsers",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DisunityVersionCompatibilities_MaxCompatibleVersionId",
                table: "DisunityVersionCompatibilities",
                column: "MaxCompatibleVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_DisunityVersionCompatibilities_MinCompatibleVersionId",
                table: "DisunityVersionCompatibilities",
                column: "MinCompatibleVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_DisunityVersionCompatibilities_VersionId",
                table: "DisunityVersionCompatibilities",
                column: "VersionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DisunityVersions_Version",
                table: "DisunityVersions",
                column: "Version",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ModDependencies_DependencyId",
                table: "ModDependencies",
                column: "DependencyId");

            migrationBuilder.CreateIndex(
                name: "IX_ModDependencies_MaxVersionId",
                table: "ModDependencies",
                column: "MaxVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_ModDependencies_MinVersionId",
                table: "ModDependencies",
                column: "MinVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_ModDisunityCompatibilities_MaxCompatibleVersionId",
                table: "ModDisunityCompatibilities",
                column: "MaxCompatibleVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_ModDisunityCompatibilities_MinCompatibleVersionId",
                table: "ModDisunityCompatibilities",
                column: "MinCompatibleVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_ModDisunityCompatibilities_VersionId",
                table: "ModDisunityCompatibilities",
                column: "VersionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Mods_LatestId",
                table: "Mods",
                column: "LatestId");

            migrationBuilder.CreateIndex(
                name: "IX_ModTargetCompatibilities_MaxCompatibleVersionId",
                table: "ModTargetCompatibilities",
                column: "MaxCompatibleVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_ModTargetCompatibilities_MinCompatibleVersionId",
                table: "ModTargetCompatibilities",
                column: "MinCompatibleVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_ModTargetCompatibilities_TargetId",
                table: "ModTargetCompatibilities",
                column: "TargetId");

            migrationBuilder.CreateIndex(
                name: "IX_ModVersionDownloadEvents_ModVersionId",
                table: "ModVersionDownloadEvents",
                column: "ModVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_ModVersions_ModId",
                table: "ModVersions",
                column: "ModId");

            migrationBuilder.CreateIndex(
                name: "IX_ModVersions_ModVersionId",
                table: "ModVersions",
                column: "ModVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_OrgMembers_OrgId",
                table: "OrgMembers",
                column: "OrgId");

            migrationBuilder.CreateIndex(
                name: "IX_Orgs_Slug",
                table: "Orgs",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Targets_LatestId",
                table: "Targets",
                column: "LatestId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TargetVersionCompatibilities_MaxCompatibleVersionId",
                table: "TargetVersionCompatibilities",
                column: "MaxCompatibleVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_TargetVersionCompatibilities_MinCompatibleVersionId",
                table: "TargetVersionCompatibilities",
                column: "MinCompatibleVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_TargetVersionCompatibilities_VersionId",
                table: "TargetVersionCompatibilities",
                column: "VersionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TargetVersions_TargetId",
                table: "TargetVersions",
                column: "TargetId");

            migrationBuilder.CreateIndex(
                name: "IX_UnityVersion_Version",
                table: "UnityVersion",
                column: "Version",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ModDisunityCompatibilities_ModVersions_VersionId",
                table: "ModDisunityCompatibilities",
                column: "VersionId",
                principalTable: "ModVersions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ModDependencies_ModVersions_DependantId",
                table: "ModDependencies",
                column: "DependantId",
                principalTable: "ModVersions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ModDependencies_ModVersions_MaxVersionId",
                table: "ModDependencies",
                column: "MaxVersionId",
                principalTable: "ModVersions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ModDependencies_ModVersions_MinVersionId",
                table: "ModDependencies",
                column: "MinVersionId",
                principalTable: "ModVersions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ModDependencies_Mods_DependencyId",
                table: "ModDependencies",
                column: "DependencyId",
                principalTable: "Mods",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ModVersions_Mods_ModId",
                table: "ModVersions",
                column: "ModId",
                principalTable: "Mods",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ModTargetCompatibilities_TargetVersions_MaxCompatibleVersio~",
                table: "ModTargetCompatibilities",
                column: "MaxCompatibleVersionId",
                principalTable: "TargetVersions",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ModTargetCompatibilities_TargetVersions_MinCompatibleVersio~",
                table: "ModTargetCompatibilities",
                column: "MinCompatibleVersionId",
                principalTable: "TargetVersions",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ModTargetCompatibilities_Targets_TargetId",
                table: "ModTargetCompatibilities",
                column: "TargetId",
                principalTable: "Targets",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TargetVersions_Targets_TargetId",
                table: "TargetVersions",
                column: "TargetId",
                principalTable: "Targets",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Mods_ModVersions_LatestId",
                table: "Mods");

            migrationBuilder.DropForeignKey(
                name: "FK_Targets_TargetVersions_LatestId",
                table: "Targets");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "DisunityVersionCompatibilities");

            migrationBuilder.DropTable(
                name: "ModDependencies");

            migrationBuilder.DropTable(
                name: "ModDisunityCompatibilities");

            migrationBuilder.DropTable(
                name: "ModTargetCompatibilities");

            migrationBuilder.DropTable(
                name: "ModVersionDownloadEvents");

            migrationBuilder.DropTable(
                name: "OrgMembers");

            migrationBuilder.DropTable(
                name: "TargetVersionCompatibilities");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "DisunityVersions");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "UnityVersion");

            migrationBuilder.DropTable(
                name: "ModVersions");

            migrationBuilder.DropTable(
                name: "Mods");

            migrationBuilder.DropTable(
                name: "Orgs");

            migrationBuilder.DropTable(
                name: "TargetVersions");

            migrationBuilder.DropTable(
                name: "Targets");
        }
    }
}
