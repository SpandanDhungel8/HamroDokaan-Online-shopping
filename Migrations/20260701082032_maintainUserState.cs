using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HamroDokaan.Migrations
{
    /// <inheritdoc />
    public partial class maintainUserState : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "User_Id",
                table: "Carts",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "User_Id",
                table: "Carts");
        }
    }
}
