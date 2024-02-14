using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Chatter.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddedRoomPermissionType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RoomPermissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoomId = table.Column<int>(type: "int", nullable: false),
                    ChatterUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PermissionType = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomPermissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoomPermissions_AspNetUsers_ChatterUserId",
                        column: x => x.ChatterUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoomPermissions_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RoomPermissions_ChatterUserId",
                table: "RoomPermissions",
                column: "ChatterUserId");

            migrationBuilder.CreateIndex(
                name: "IX_RoomPermissions_RoomId",
                table: "RoomPermissions",
                column: "RoomId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoomPermissions");
        }
    }
}
