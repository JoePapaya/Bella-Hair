using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BellaHair.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCampaignFieldsToRabat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.RenameColumn(
            //    name: "Code",
            //    table: "Rabatter",
            //    newName: "Navn");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Bookinger_Behandlinger_BehandlingId",
                table: "Bookinger",
                column: "BehandlingId",
                principalTable: "Behandlinger",
                principalColumn: "BehandlingId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookinger_Kunder_KundeId",
                table: "Bookinger",
                column: "KundeId",
                principalTable: "Kunder",
                principalColumn: "KundeId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookinger_Medarbejdere_MedarbejderId",
                table: "Bookinger",
                column: "MedarbejderId",
                principalTable: "Medarbejdere",
                principalColumn: "MedarbejderId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookinger_Behandlinger_BehandlingId",
                table: "Bookinger");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookinger_Kunder_KundeId",
                table: "Bookinger");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookinger_Medarbejdere_MedarbejderId",
                table: "Bookinger");

            migrationBuilder.DropIndex(
                name: "IX_Bookinger_BehandlingId",
                table: "Bookinger");

            migrationBuilder.DropIndex(
                name: "IX_Bookinger_KundeId",
                table: "Bookinger");

            migrationBuilder.DropIndex(
                name: "IX_Bookinger_MedarbejderId",
                table: "Bookinger");

            //migrationBuilder.RenameColumn(
            //    name: "Navn",
            //    table: "Rabatter",
            //    newName: "Code");
        }
    }
}
