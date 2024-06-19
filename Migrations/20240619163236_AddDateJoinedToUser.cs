using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace new_chess_server.Migrations
{
    /// <inheritdoc />
    public partial class AddDateJoinedToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DateJoined",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "MyProperty",
                table: "Users",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateJoined",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "MyProperty",
                table: "Users");
        }
    }
}
