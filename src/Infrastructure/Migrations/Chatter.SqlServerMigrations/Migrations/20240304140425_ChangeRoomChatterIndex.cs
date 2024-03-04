using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Chatter.SqlServerMigrations.Migrations
{
    /// <inheritdoc />
    public partial class ChangeRoomChatterIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_RoomChatterUser",
                table: "RoomChatterUser");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "RoomChatterUser",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RoomChatterUser",
                table: "RoomChatterUser",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_RoomChatterUser_ChatterUserId_RoomId",
                table: "RoomChatterUser",
                columns: new[] { "ChatterUserId", "RoomId" },
                filter: "IsDeleted = 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_RoomChatterUser",
                table: "RoomChatterUser");

            migrationBuilder.DropIndex(
                name: "IX_RoomChatterUser_ChatterUserId_RoomId",
                table: "RoomChatterUser");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "RoomChatterUser",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RoomChatterUser",
                table: "RoomChatterUser",
                columns: new[] { "ChatterUserId", "RoomId" });
        }
    }
}
