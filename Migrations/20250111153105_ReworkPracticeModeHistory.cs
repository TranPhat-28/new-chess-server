using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace new_chess_server.Migrations
{
    /// <inheritdoc />
    public partial class ReworkPracticeModeHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PracticeModeGameHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PracticeModeGameHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PracticeModeGameHistories_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MoveHistoryItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Side = table.Column<string>(type: "text", nullable: false),
                    Move = table.Column<string>(type: "text", nullable: false),
                    PracticeModeGameHistoryId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MoveHistoryItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MoveHistoryItems_PracticeModeGameHistories_PracticeModeGame~",
                        column: x => x.PracticeModeGameHistoryId,
                        principalTable: "PracticeModeGameHistories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MoveHistoryItems_PracticeModeGameHistoryId",
                table: "MoveHistoryItems",
                column: "PracticeModeGameHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_PracticeModeGameHistories_UserId",
                table: "PracticeModeGameHistories",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MoveHistoryItems");

            migrationBuilder.DropTable(
                name: "PracticeModeGameHistories");
        }
    }
}
