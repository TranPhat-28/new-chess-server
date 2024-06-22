using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace new_chess_server.Migrations
{
    /// <inheritdoc />
    public partial class AddGameStatisticModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GameStatistics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Ranking = table.Column<int>(type: "integer", nullable: false),
                    PracticePlayedEasy = table.Column<int>(type: "integer", nullable: false),
                    PracticeVictoryEasy = table.Column<int>(type: "integer", nullable: false),
                    PracticePlayedMedium = table.Column<int>(type: "integer", nullable: false),
                    PracticeVictoryMedium = table.Column<int>(type: "integer", nullable: false),
                    PracticePlayedHard = table.Column<int>(type: "integer", nullable: false),
                    PracticeVictoryHard = table.Column<int>(type: "integer", nullable: false),
                    OnlinePlayedEasy = table.Column<int>(type: "integer", nullable: false),
                    OnlineVictoryEasy = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameStatistics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GameStatistics_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GameStatistics_UserId",
                table: "GameStatistics",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameStatistics");
        }
    }
}
