using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Disunity.Store.Shared.Migrations
{
    public partial class Targets : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TargetVersions",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    TargetId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    VersionNumber = table.Column<string>(maxLength: 16, nullable: false),
                    WebsiteUrl = table.Column<string>(maxLength: 1024, nullable: false),
                    Description = table.Column<string>(maxLength: 256, nullable: false),
                    IconUrl = table.Column<string>(maxLength: 1024, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TargetVersions", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Targets",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    LatestId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Targets", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Targets_TargetVersions_LatestId",
                        column: x => x.LatestId,
                        principalTable: "TargetVersions",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Targets_LatestId",
                table: "Targets",
                column: "LatestId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TargetVersions_TargetId",
                table: "TargetVersions",
                column: "TargetId");

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
                name: "FK_Targets_TargetVersions_LatestId",
                table: "Targets");

            migrationBuilder.DropTable(
                name: "TargetVersions");

            migrationBuilder.DropTable(
                name: "Targets");
        }
    }
}
