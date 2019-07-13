using Microsoft.EntityFrameworkCore.Migrations;

namespace Disunity.Store.Entities.Migrations
{
    public partial class TargetHash : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ModDependencies_ModVersions_DependantId",
                table: "ModDependencies");

            migrationBuilder.DropForeignKey(
                name: "FK_ModVersions_ModVersions_ModVersionId",
                table: "ModVersions");

            migrationBuilder.DropIndex(
                name: "IX_ModVersions_ModVersionId",
                table: "ModVersions");

            migrationBuilder.DropColumn(
                name: "ModVersionId",
                table: "ModVersions");

            migrationBuilder.AddColumn<string>(
                name: "Hash",
                table: "TargetVersions",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DependantId1",
                table: "ModDependencies",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ModDependencies_DependantId1",
                table: "ModDependencies",
                column: "DependantId1");

            migrationBuilder.AddForeignKey(
                name: "FK_ModDependencies_ModVersions_DependantId1",
                table: "ModDependencies",
                column: "DependantId1",
                principalTable: "ModVersions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ModDependencies_ModVersions_DependantId1",
                table: "ModDependencies");

            migrationBuilder.DropIndex(
                name: "IX_ModDependencies_DependantId1",
                table: "ModDependencies");

            migrationBuilder.DropColumn(
                name: "Hash",
                table: "TargetVersions");

            migrationBuilder.DropColumn(
                name: "DependantId1",
                table: "ModDependencies");

            migrationBuilder.AddColumn<int>(
                name: "ModVersionId",
                table: "ModVersions",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ModVersions_ModVersionId",
                table: "ModVersions",
                column: "ModVersionId");

            migrationBuilder.AddForeignKey(
                name: "FK_ModDependencies_ModVersions_DependantId",
                table: "ModDependencies",
                column: "DependantId",
                principalTable: "ModVersions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ModVersions_ModVersions_ModVersionId",
                table: "ModVersions",
                column: "ModVersionId",
                principalTable: "ModVersions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
