using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

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
                    Pris = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
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
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Percentage = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    FixedAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    RequiredLoyaltyTier = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Aktiv = table.Column<bool>(type: "bit", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsKampagne = table.Column<bool>(type: "bit", nullable: false),
                    StartDato = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SlutDato = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MinimumBeløb = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
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

            migrationBuilder.CreateTable(
                name: "Fakturaer",
                columns: table => new
                {
                    FakturaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KundeId = table.Column<int>(type: "int", nullable: false),
                    BookingId = table.Column<int>(type: "int", nullable: false),
                    ErFirmafaktura = table.Column<bool>(type: "bit", nullable: false),
                    Firmanavn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cvr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FakturaDato = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Beløb = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    RabatBeløb = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    TotalBeløb = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    RabatTekst = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fakturaer", x => x.FakturaId);
                    table.ForeignKey(
                        name: "FK_Fakturaer_Bookinger_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Bookinger",
                        principalColumn: "BookingId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Fakturaer_Kunder_KundeId",
                        column: x => x.KundeId,
                        principalTable: "Kunder",
                        principalColumn: "KundeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Behandlinger",
                columns: new[] { "BehandlingId", "Navn", "Pris", "Type", "VarighedMinutter" },
                values: new object[,]
                {
                    { 1, "Standard klip", 100m, "", 0 },
                    { 2, "Herreklip", 80m, "", 0 },
                    { 3, "Farve", 150m, "", 0 },
                    { 4, "Balayage", 250m, "", 0 },
                    { 5, "Kurbehandling", 60m, "", 0 }
                });

            migrationBuilder.InsertData(
                table: "Kunder",
                columns: new[] { "KundeId", "Adresse", "BesøgAntal", "By", "Email", "Fødselsdag", "LoyaltyTier", "Navn", "Points", "Postnr", "Telefon" },
                values: new object[,]
                {
                    { 1, "", 0, "", "", new DateOnly(1, 1, 1), null, "Kendrick", 0, "", "" },
                    { 2, "", 0, "", "", new DateOnly(1, 1, 1), null, "J. Cole", 0, "", "" },
                    { 3, "", 0, "", "", new DateOnly(1, 1, 1), null, "Drake", 0, "", "" }
                });

            migrationBuilder.InsertData(
                table: "Medarbejdere",
                columns: new[] { "MedarbejderId", "ErFreelancer", "Kompetencer", "Navn" },
                values: new object[,]
                {
                    { 1, false, "[]", "Mia" },
                    { 2, false, "[]", "Sara" },
                    { 3, false, "[]", "Jonas" }
                });

            migrationBuilder.InsertData(
                table: "Rabatter",
                columns: new[] { "RabatId", "Aktiv", "Code", "Description", "FixedAmount", "IsKampagne", "MinimumBeløb", "Navn", "Percentage", "RequiredLoyaltyTier", "SlutDato", "StartDato" },
                values: new object[,]
                {
                    { 1001, true, null, "5% rabat til Bronze-stamkunder", null, false, null, "Stamkunde Bronze", 0.05m, "Bronze", null, null },
                    { 1002, true, null, "10% rabat til Sølv-stamkunder", null, false, null, "Stamkunde Sølv", 0.10m, "Sølv", null, null },
                    { 1003, true, null, "15% rabat til Guld-stamkunder", null, false, null, "Stamkunde Guld", 0.15m, "Guld", null, null },
                    { 2001, true, null, "Julekampagne: 50 kr rabat på alle behandlinger", 50m, true, null, "Julekampagne", null, null, new DateTime(2025, 12, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
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

            migrationBuilder.CreateIndex(
                name: "IX_Fakturaer_BookingId",
                table: "Fakturaer",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_Fakturaer_KundeId",
                table: "Fakturaer",
                column: "KundeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Fakturaer");

            migrationBuilder.DropTable(
                name: "Rabatter");

            migrationBuilder.DropTable(
                name: "Bookinger");

            migrationBuilder.DropTable(
                name: "Behandlinger");

            migrationBuilder.DropTable(
                name: "Kunder");

            migrationBuilder.DropTable(
                name: "Medarbejdere");
        }
    }
}
