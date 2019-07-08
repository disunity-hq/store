using System;

using Microsoft.EntityFrameworkCore.Migrations;


namespace Disunity.Store.Models.Migrations {

    public partial class test : Migration {

        protected override void Up(MigrationBuilder migrationBuilder) {
            migrationBuilder.AddColumn<DateTime>(
                "CreatedAt",
                "Orgs",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                "UpdatedAt",
                "Orgs",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder) {
            migrationBuilder.DropColumn(
                "CreatedAt",
                "Orgs");

            migrationBuilder.DropColumn(
                "UpdatedAt",
                "Orgs");
        }

    }

}