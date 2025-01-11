using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace new_chess_server.Migrations
{
    /// <inheritdoc />
    public partial class AddMoveIndexToModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MoveIndex",
                table: "MoveHistoryItems",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MoveIndex",
                table: "MoveHistoryItems");
        }
    }
}
