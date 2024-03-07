using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Chatter.PostgreMigrations.Migrations
{
    /// <inheritdoc />
    public partial class ChangeRoomChatterIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "pk_room_chatter_user",
                table: "room_chatter_user");

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "room_chatter_user",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "pk_room_chatter_user",
                table: "room_chatter_user",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "ix_room_chatter_user_chatter_user_id_room_id",
                table: "room_chatter_user",
                columns: new[] { "chatter_user_id", "room_id" },
                filter: "is_deleted = false");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "pk_room_chatter_user",
                table: "room_chatter_user");

            migrationBuilder.DropIndex(
                name: "ix_room_chatter_user_chatter_user_id_room_id",
                table: "room_chatter_user");

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "room_chatter_user",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "pk_room_chatter_user",
                table: "room_chatter_user",
                columns: new[] { "chatter_user_id", "room_id" });
        }
    }
}
