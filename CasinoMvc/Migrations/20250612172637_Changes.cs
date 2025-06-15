using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CasinoMvc.Migrations
{
    /// <inheritdoc />
    public partial class Changes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TotalWind",
                table: "Player",
                newName: "TotalWins");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TotalWins",
                table: "Player",
                newName: "TotalWind");
        }
    }
}
