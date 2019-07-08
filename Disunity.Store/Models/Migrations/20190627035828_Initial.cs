using System;

using Microsoft.EntityFrameworkCore.Migrations;

using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;


namespace Disunity.Store.Models.Migrations {

    public partial class Initial : Migration {

        protected override void Up(MigrationBuilder migrationBuilder) {
            migrationBuilder.AlterDatabase()
                            .Annotation("Npgsql:Enum:org_member_role", "owner,member");

            migrationBuilder.CreateTable(
                "AspNetRoles",
                table => new {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_AspNetRoles", x => x.Id); });

            migrationBuilder.CreateTable(
                "AspNetUsers",
                table => new {
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
                    AccessFailedCount = table.Column<int>(nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_AspNetUsers", x => x.Id); });

            migrationBuilder.CreateTable(
                "Orgs",
                table => new {
                    Id = table.Column<int>(nullable: false)
                              .Annotation("Npgsql:ValueGenerationStrategy",
                                          NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table => {
                    table.PrimaryKey("PK_Orgs", x => x.Id);
                    table.UniqueConstraint("AK_Orgs_Name", x => x.Name);
                });

            migrationBuilder.CreateTable(
                "AspNetRoleClaims",
                table => new {
                    Id = table.Column<int>(nullable: false)
                              .Annotation("Npgsql:ValueGenerationStrategy",
                                          NpgsqlValueGenerationStrategy.SerialColumn),
                    RoleId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table => {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);

                    table.ForeignKey(
                        "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        x => x.RoleId,
                        "AspNetRoles",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "AspNetUserClaims",
                table => new {
                    Id = table.Column<int>(nullable: false)
                              .Annotation("Npgsql:ValueGenerationStrategy",
                                          NpgsqlValueGenerationStrategy.SerialColumn),
                    UserId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table => {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);

                    table.ForeignKey(
                        "FK_AspNetUserClaims_AspNetUsers_UserId",
                        x => x.UserId,
                        "AspNetUsers",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "AspNetUserLogins",
                table => new {
                    LoginProvider = table.Column<string>(maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table => {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new {x.LoginProvider, x.ProviderKey});

                    table.ForeignKey(
                        "FK_AspNetUserLogins_AspNetUsers_UserId",
                        x => x.UserId,
                        "AspNetUsers",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "AspNetUserRoles",
                table => new {
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table => {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new {x.UserId, x.RoleId});

                    table.ForeignKey(
                        "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        x => x.RoleId,
                        "AspNetRoles",
                        "Id",
                        onDelete: ReferentialAction.Cascade);

                    table.ForeignKey(
                        "FK_AspNetUserRoles_AspNetUsers_UserId",
                        x => x.UserId,
                        "AspNetUsers",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "AspNetUserTokens",
                table => new {
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(maxLength: 128, nullable: false),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table => {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new {x.UserId, x.LoginProvider, x.Name});

                    table.ForeignKey(
                        "FK_AspNetUserTokens_AspNetUsers_UserId",
                        x => x.UserId,
                        "AspNetUsers",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "OrgMembers",
                table => new {
                    UserId = table.Column<string>(nullable: false),
                    OrgId = table.Column<int>(nullable: false),
                    Role = table.Column<OrgMemberRole>(nullable: false)
                },
                constraints: table => {
                    table.PrimaryKey("PK_OrgMembers", x => new {x.UserId, x.OrgId});

                    table.ForeignKey(
                        "FK_OrgMembers_Orgs_OrgId",
                        x => x.OrgId,
                        "Orgs",
                        "Id",
                        onDelete: ReferentialAction.Cascade);

                    table.ForeignKey(
                        "FK_OrgMembers_AspNetUsers_UserId",
                        x => x.UserId,
                        "AspNetUsers",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "ModVersions",
                table => new {
                    Id = table.Column<int>(nullable: false)
                              .Annotation("Npgsql:ValueGenerationStrategy",
                                          NpgsqlValueGenerationStrategy.SerialColumn),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    ModId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    IsActive = table.Column<bool>(nullable: true, defaultValue: true),
                    Downloads = table.Column<int>(nullable: true, defaultValue: 0),
                    VersionNumber = table.Column<string>(maxLength: 16, nullable: false),
                    WebsiteUrl = table.Column<string>(maxLength: 1024, nullable: false),
                    Description = table.Column<string>(maxLength: 256, nullable: false),
                    FileUrl = table.Column<string>(maxLength: 1024, nullable: false),
                    IconUrl = table.Column<string>(maxLength: 1024, nullable: false),
                    ModVersionId = table.Column<int>(nullable: true)
                },
                constraints: table => {
                    table.PrimaryKey("PK_ModVersions", x => x.Id);
                    table.UniqueConstraint("AK_ModVersions_Name", x => x.Name);
                    table.UniqueConstraint("AK_ModVersions_VersionNumber", x => x.VersionNumber);

                    table.ForeignKey(
                        "FK_ModVersions_ModVersions_ModVersionId",
                        x => x.ModVersionId,
                        "ModVersions",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                "Mods",
                table => new {
                    Id = table.Column<int>(nullable: false)
                              .Annotation("Npgsql:ValueGenerationStrategy",
                                          NpgsqlValueGenerationStrategy.SerialColumn),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    OwnerId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    IsActive = table.Column<bool>(nullable: true, defaultValue: true),
                    IsDeprecated = table.Column<bool>(nullable: true, defaultValue: false),
                    IsPinned = table.Column<bool>(nullable: true, defaultValue: false),
                    LatestId = table.Column<int>(nullable: true)
                },
                constraints: table => {
                    table.PrimaryKey("PK_Mods", x => x.Id);
                    table.UniqueConstraint("AK_Mods_Name", x => x.Name);

                    table.ForeignKey(
                        "FK_Mods_ModVersions_LatestId",
                        x => x.LatestId,
                        "ModVersions",
                        "Id",
                        onDelete: ReferentialAction.Restrict);

                    table.ForeignKey(
                        "FK_Mods_Orgs_OwnerId",
                        x => x.OwnerId,
                        "Orgs",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                "ModVersionDownloadEvents",
                table => new {
                    ModVersionId = table.Column<int>(nullable: false),
                    SourceIp = table.Column<string>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    TotalDownloads = table.Column<int>(nullable: true, defaultValue: 1),
                    CountedDownloads = table.Column<int>(nullable: true, defaultValue: 1)
                },
                constraints: table => {
                    table.PrimaryKey("PK_ModVersionDownloadEvents", x => new {x.SourceIp, x.ModVersionId});

                    table.ForeignKey(
                        "FK_ModVersionDownloadEvents_ModVersions_ModVersionId",
                        x => x.ModVersionId,
                        "ModVersions",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "TargetVersions",
                table => new {
                    ID = table.Column<int>(nullable: false)
                              .Annotation("Npgsql:ValueGenerationStrategy",
                                          NpgsqlValueGenerationStrategy.SerialColumn),
                    TargetId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    VersionNumber = table.Column<string>(maxLength: 16, nullable: false),
                    WebsiteUrl = table.Column<string>(maxLength: 1024, nullable: false),
                    Description = table.Column<string>(maxLength: 256, nullable: false),
                    IconUrl = table.Column<string>(maxLength: 1024, nullable: false)
                },
                constraints: table => {
                    table.PrimaryKey("PK_TargetVersions", x => x.ID);
                    table.UniqueConstraint("AK_TargetVersions_Name", x => x.Name);
                    table.UniqueConstraint("AK_TargetVersions_VersionNumber", x => x.VersionNumber);
                });

            migrationBuilder.CreateTable(
                "Targets",
                table => new {
                    ID = table.Column<int>(nullable: false)
                              .Annotation("Npgsql:ValueGenerationStrategy",
                                          NpgsqlValueGenerationStrategy.SerialColumn),
                    LatestId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(maxLength: 128, nullable: false)
                },
                constraints: table => {
                    table.PrimaryKey("PK_Targets", x => x.ID);
                    table.UniqueConstraint("AK_Targets_Name", x => x.Name);

                    table.ForeignKey(
                        "FK_Targets_TargetVersions_LatestId",
                        x => x.LatestId,
                        "TargetVersions",
                        "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                "IX_AspNetRoleClaims_RoleId",
                "AspNetRoleClaims",
                "RoleId");

            migrationBuilder.CreateIndex(
                "RoleNameIndex",
                "AspNetRoles",
                "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                "IX_AspNetUserClaims_UserId",
                "AspNetUserClaims",
                "UserId");

            migrationBuilder.CreateIndex(
                "IX_AspNetUserLogins_UserId",
                "AspNetUserLogins",
                "UserId");

            migrationBuilder.CreateIndex(
                "IX_AspNetUserRoles_RoleId",
                "AspNetUserRoles",
                "RoleId");

            migrationBuilder.CreateIndex(
                "EmailIndex",
                "AspNetUsers",
                "NormalizedEmail");

            migrationBuilder.CreateIndex(
                "UserNameIndex",
                "AspNetUsers",
                "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                "IX_Mods_LatestId",
                "Mods",
                "LatestId");

            migrationBuilder.CreateIndex(
                "IX_Mods_OwnerId",
                "Mods",
                "OwnerId");

            migrationBuilder.CreateIndex(
                "IX_ModVersionDownloadEvents_ModVersionId",
                "ModVersionDownloadEvents",
                "ModVersionId");

            migrationBuilder.CreateIndex(
                "IX_ModVersions_ModId",
                "ModVersions",
                "ModId");

            migrationBuilder.CreateIndex(
                "IX_ModVersions_ModVersionId",
                "ModVersions",
                "ModVersionId");

            migrationBuilder.CreateIndex(
                "IX_OrgMembers_OrgId",
                "OrgMembers",
                "OrgId");

            migrationBuilder.CreateIndex(
                "IX_Targets_LatestId",
                "Targets",
                "LatestId",
                unique: true);

            migrationBuilder.CreateIndex(
                "IX_TargetVersions_TargetId",
                "TargetVersions",
                "TargetId");

            migrationBuilder.AddForeignKey(
                "FK_ModVersions_Mods_ModId",
                "ModVersions",
                "ModId",
                "Mods",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                "FK_TargetVersions_Targets_TargetId",
                "TargetVersions",
                "TargetId",
                "Targets",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder) {
            migrationBuilder.DropForeignKey(
                "FK_Mods_ModVersions_LatestId",
                "Mods");

            migrationBuilder.DropForeignKey(
                "FK_Targets_TargetVersions_LatestId",
                "Targets");

            migrationBuilder.DropTable(
                "AspNetRoleClaims");

            migrationBuilder.DropTable(
                "AspNetUserClaims");

            migrationBuilder.DropTable(
                "AspNetUserLogins");

            migrationBuilder.DropTable(
                "AspNetUserRoles");

            migrationBuilder.DropTable(
                "AspNetUserTokens");

            migrationBuilder.DropTable(
                "ModVersionDownloadEvents");

            migrationBuilder.DropTable(
                "OrgMembers");

            migrationBuilder.DropTable(
                "AspNetRoles");

            migrationBuilder.DropTable(
                "AspNetUsers");

            migrationBuilder.DropTable(
                "ModVersions");

            migrationBuilder.DropTable(
                "Mods");

            migrationBuilder.DropTable(
                "Orgs");

            migrationBuilder.DropTable(
                "TargetVersions");

            migrationBuilder.DropTable(
                "Targets");
        }

    }

}