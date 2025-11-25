using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BellaHair.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDescriptionToRabat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Percentage",
                table: "Rabatter",
                type: "decimal(5,2)",
                precision: 5,
                scale: 2,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<bool>(
                name: "Aktiv",
                table: "Rabatter",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Rabatter",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "FixedAmount",
                table: "Rabatter",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsKampagne",
                table: "Rabatter",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "MinimumBeløb",
                table: "Rabatter",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "SlutDato",
                table: "Rabatter",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDato",
                table: "Rabatter",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Aktiv",
                table: "Rabatter");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "Rabatter");

            migrationBuilder.DropColumn(
                name: "FixedAmount",
                table: "Rabatter");

            migrationBuilder.DropColumn(
                name: "IsKampagne",
                table: "Rabatter");

            migrationBuilder.DropColumn(
                name: "MinimumBeløb",
                table: "Rabatter");

            migrationBuilder.DropColumn(
                name: "SlutDato",
                table: "Rabatter");

            migrationBuilder.DropColumn(
                name: "StartDato",
                table: "Rabatter");

            migrationBuilder.AlterColumn<decimal>(
                name: "Percentage",
                table: "Rabatter",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,2)",
                oldPrecision: 5,
                oldScale: 2,
                oldNullable: true);
        }
    }
}
