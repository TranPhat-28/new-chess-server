using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace new_chess_server.Migrations
{
    /// <inheritdoc />
    public partial class GameStatisticChangeFieldName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OnlineVictoryEasy",
                table: "GameStatistics",
                newName: "OnlineVictory");

            migrationBuilder.RenameColumn(
                name: "OnlinePlayedEasy",
                table: "GameStatistics",
                newName: "OnlinePlayed");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OnlineVictory",
                table: "GameStatistics",
                newName: "OnlineVictoryEasy");

            migrationBuilder.RenameColumn(
                name: "OnlinePlayed",
                table: "GameStatistics",
                newName: "OnlinePlayedEasy");
        }
    }
}
