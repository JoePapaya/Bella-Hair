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
                    By = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Telefon = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Fødselsdag = table.Column<DateOnly>(type: "date", nullable: false),
                    KundeType = table.Column<int>(type: "int", nullable: false),
                    Firmanavn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cvr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Points = table.Column<int>(type: "int", nullable: false),
                    BesøgAntal = table.Column<int>(type: "int", nullable: false),
                    LoyaltyTier = table.Column<int>(type: "int", nullable: false)
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
                    Percentage = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    FixedAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    RequiredLoyaltyTier = table.Column<int>(type: "int", nullable: true),
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
                    KundeNavn = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    KundeEmail = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    KundeTelefon = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    BehandlingNavn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MedarbejderNavn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BookingTidspunkt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FakturaDato = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ErFirmafaktura = table.Column<bool>(type: "bit", nullable: false),
                    Firmanavn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cvr = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    { 1, "Standard klip", 100m, "Klip", 30 },
                    { 2, "Herreklip", 80m, "Klip", 25 },
                    { 3, "Farve", 150m, "Farve", 60 },
                    { 4, "Balayage", 250m, "Farve", 120 },
                    { 5, "Kurbehandling", 60m, "Kur", 30 }
                });

            migrationBuilder.InsertData(
                table: "Kunder",
                columns: new[] { "KundeId", "Adresse", "BesøgAntal", "By", "Cvr", "Email", "Firmanavn", "Fødselsdag", "KundeType", "LoyaltyTier", "Navn", "Points", "Postnr", "Telefon" },
                values: new object[,]
                {
                    { 1, "Comptonvej 1", 0, "København Ø", null, "kendrick@example.com", null, new DateOnly(1987, 6, 17), 0, 0, "Kendrick", 0, "2100", "12345678" },
                    { 2, "Dreamvillegade 2", 0, "Aarhus C", null, "jcole@example.com", null, new DateOnly(1985, 1, 28), 0, 0, "J. Cole", 0, "8000", "22334455" },
                    { 3, "OVO Allé 3", 0, "Odense C", null, "drake@example.com", null, new DateOnly(1986, 10, 24), 0, 0, "Drake", 0, "5000", "99887766" }
                });

            migrationBuilder.InsertData(
                table: "Medarbejdere",
                columns: new[] { "MedarbejderId", "ErFreelancer", "Kompetencer", "Navn" },
                values: new object[,]
                {
                    { 1, false, "[]", "George Pikins" },
                    { 2, false, "[]", "Dak Prescot" },
                    { 3, false, "[]", "CeeDee Lamb" }
                });

            migrationBuilder.InsertData(
                table: "Rabatter",
                columns: new[] { "RabatId", "Aktiv", "Description", "FixedAmount", "IsKampagne", "MinimumBeløb", "Navn", "Percentage", "RequiredLoyaltyTier", "SlutDato", "StartDato" },
                values: new object[,]
                {
                    { 1001, true, "5% rabat stamkunder med 5 til 9 besøg", null, false, null, "Stamkunde Bronze", 0.05m, 1, null, null },
                    { 1002, true, "10% rabat til stamkunder med 10 til 19 besøg", null, false, null, "Stamkunde Sølv", 0.10m, 2, null, null },
                    { 1003, true, "15% rabat til stamkunder med 20+ besøg", null, false, null, "Stamkunde Guld", 0.15m, 3, null, null },
                    { 2001, true, "Julekampagne: 50 kr rabat på alle behandlinger", 50m, true, null, "Julekampagne", null, null, new DateTime(2025, 12, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2002, true, "5% rabat i januar", null, true, null, "Nytårsrabat", 0.05m, null, new DateTime(2026, 1, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
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
