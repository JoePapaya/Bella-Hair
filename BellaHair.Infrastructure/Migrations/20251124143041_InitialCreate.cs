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
                    VarighedMinutter = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Behandlinger", x => x.BehandlingId);
                });

            migrationBuilder.CreateTable(
                name: "Kunder",
                columns: table => new
                {
                    KundeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Navn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Adresse = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Postnr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Fødselsdag = table.Column<DateOnly>(type: "date", nullable: false),
                    By = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Points = table.Column<int>(type: "int", nullable: false),
                    Telefon = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BesøgAntal = table.Column<int>(type: "int", nullable: false),
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
                    ErFreelancer = table.Column<bool>(type: "bit", nullable: false),
                    Navn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Kompetencer = table.Column<string>(type: "nvarchar(max)", nullable: false)
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
                    Navn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Percentage = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RequiredLoyaltyTier = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rabatter", x => x.RabatId);
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
                    table.ForeignKey(
                        name: "FK_Bookinger_Behandlinger_BehandlingId",
                        column: x => x.BehandlingId,
                        principalTable: "Behandlinger",
                        principalColumn: "BehandlingId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Bookinger_Kunder_KundeId",
                        column: x => x.KundeId,
                        principalTable: "Kunder",
                        principalColumn: "KundeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Bookinger_Medarbejdere_MedarbejderId",
                        column: x => x.MedarbejderId,
                        principalTable: "Medarbejdere",
                        principalColumn: "MedarbejderId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bookinger_BehandlingId",
                table: "Bookinger",
                column: "BehandlingId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookinger_KundeId",
                table: "Bookinger",
                column: "KundeId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookinger_MedarbejderId",
                table: "Bookinger",
                column: "MedarbejderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bookinger");

            migrationBuilder.DropTable(
                name: "Rabatter");

            migrationBuilder.DropTable(
                name: "Behandlinger");

            migrationBuilder.DropTable(
                name: "Kunder");

            migrationBuilder.DropTable(
                name: "Medarbejdere");
        }
    }
}
