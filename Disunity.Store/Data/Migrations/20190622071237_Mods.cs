using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Disunity.Store.Migrations
{
    public partial class Mods : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ModVersion",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    ModId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    IsActive = table.Column<bool>(nullable: false, defaultValue: true),
                    Downloads = table.Column<int>(nullable: false, defaultValue: 0),
                    VersionNumber = table.Column<string>(maxLength: 16, nullable: false),
                    WebsiteUrl = table.Column<string>(maxLength: 1024, nullable: false),
                    Description = table.Column<string>(maxLength: 256, nullable: false),
                    FileURL = table.Column<string>(maxLength: 1024, nullable: false),
                    IconURL = table.Column<string>(maxLength: 1024, nullable: false),
                    ModVersionID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModVersion", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ModVersion_ModVersion_ModVersionID",
                        column: x => x.ModVersionID,
                        principalTable: "ModVersion",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Mod",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    OwnerId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    IsActive = table.Column<bool>(nullable: false, defaultValue: true),
                    IsDeprecated = table.Column<bool>(nullable: false, defaultValue: false),
                    IsPinned = table.Column<bool>(nullable: false, defaultValue: false),
                    LatestId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mod", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Mod_ModVersion_LatestId",
                        column: x => x.LatestId,
                        principalTable: "ModVersion",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Mod_Orgs_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Orgs",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ModVersionDownloadEvent",
                columns: table => new
                {
                    ModVersionId = table.Column<int>(nullable: false),
                    SourceIP = table.Column<string>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    TotalDownloads = table.Column<int>(nullable: false, defaultValue: 1),
                    CountedDownloads = table.Column<int>(nullable: false, defaultValue: 1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModVersionDownloadEvent", x => new { x.SourceIP, x.ModVersionId });
                    table.ForeignKey(
                        name: "FK_ModVersionDownloadEvent_ModVersion_ModVersionId",
                        column: x => x.ModVersionId,
                        principalTable: "ModVersion",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Mod_LatestId",
                table: "Mod",
                column: "LatestId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Mod_OwnerId",
                table: "Mod",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_ModVersion_ModId",
                table: "ModVersion",
                column: "ModId");

            migrationBuilder.CreateIndex(
                name: "IX_ModVersion_ModVersionID",
                table: "ModVersion",
                column: "ModVersionID");

            migrationBuilder.CreateIndex(
                name: "IX_ModVersionDownloadEvent_ModVersionId",
                table: "ModVersionDownloadEvent",
                column: "ModVersionId");

            migrationBuilder.AddForeignKey(
                name: "FK_ModVersion_Mod_ModId",
                table: "ModVersion",
                column: "ModId",
                principalTable: "Mod",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Mod_ModVersion_LatestId",
                table: "Mod");

            migrationBuilder.DropTable(
                name: "ModVersionDownloadEvent");

            migrationBuilder.DropTable(
                name: "ModVersion");

            migrationBuilder.DropTable(
                name: "Mod");
        }
    }
}
