using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BellaHair.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Behandlinger",
                columns: table => new
                {
                    BehandlingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Navn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Pris = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    VarighedMinutter = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Behandlinger", x => x.BehandlingId);
                });

            migrationBuilder.CreateTable(
                name: "Bookinger",
                columns: table => new
                {
                    BookingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KundeId = table.Column<int>(type: "int", nullable: false),
                    BehandlingId = table.Column<int>(type: "int", nullable: false),
                    MedarbejderId = table.Column<int>(type: "int", nullable: false),
                    Tidspunkt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Varighed = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ValgtRabat = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookinger", x => x.BookingId);
                });

            migrationBuilder.CreateTable(
                name: "Kunder",
                columns: table => new
                {
                    KundeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Navn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LoyaltyTier = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kunder", x => x.KundeId);
                });

            migrationBuilder.CreateTable(
                name: "Medarbejdere",
                columns: table => new
                {
                    MedarbejderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Navn = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medarbejdere", x => x.MedarbejderId);
                });

            migrationBuilder.CreateTable(
                name: "Rabatter",
                columns: table => new
                {
                    RabatId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Percentage = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RequiredLoyaltyTier = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rabatter", x => x.RabatId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Behandlinger");

            migrationBuilder.DropTable(
                name: "Bookinger");

            migrationBuilder.DropTable(
                name: "Kunder");

            migrationBuilder.DropTable(
                name: "Medarbejdere");

            migrationBuilder.DropTable(
                name: "Rabatter");
        }
    }
}
