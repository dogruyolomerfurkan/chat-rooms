using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Chatter.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class JTableChanged : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JTable_RoomBlockedUsers");

            migrationBuilder.DropTable(
                name: "JTable_RoomUsers");

            migrationBuilder.CreateTable(
                name: "RoomChatterUser",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoomId = table.Column<int>(type: "int", nullable: false),
                    ChatterUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IsBlocked = table.Column<bool>(type: "bit", nullable: false),
                    ChatterUserId1 = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    RoomId1 = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomChatterUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoomChatterUser_AspNetUsers_ChatterUserId",
                        column: x => x.ChatterUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoomChatterUser_AspNetUsers_ChatterUserId1",
                        column: x => x.ChatterUserId1,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RoomChatterUser_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoomChatterUser_Rooms_RoomId1",
                        column: x => x.RoomId1,
                        principalTable: "Rooms",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_RoomChatterUser_ChatterUserId",
                table: "RoomChatterUser",
                column: "ChatterUserId");

            migrationBuilder.CreateIndex(
                name: "IX_RoomChatterUser_ChatterUserId1",
                table: "RoomChatterUser",
                column: "ChatterUserId1");

            migrationBuilder.CreateIndex(
                name: "IX_RoomChatterUser_RoomId",
                table: "RoomChatterUser",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_RoomChatterUser_RoomId1",
                table: "RoomChatterUser",
                column: "RoomId1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoomChatterUser");

            migrationBuilder.CreateTable(
                name: "JTable_RoomBlockedUsers",
                columns: table => new
                {
                    BlockedRoomsId = table.Column<int>(type: "int", nullable: false),
                    BlockedUsersId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JTable_RoomBlockedUsers", x => new { x.BlockedRoomsId, x.BlockedUsersId });
                    table.ForeignKey(
                        name: "FK_JTable_RoomBlockedUsers_AspNetUsers_BlockedUsersId",
                        column: x => x.BlockedUsersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_JTable_RoomBlockedUsers_Rooms_BlockedRoomsId",
                        column: x => x.BlockedRoomsId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JTable_RoomUsers",
                columns: table => new
                {
                    ChatRoomsId = table.Column<int>(type: "int", nullable: false),
                    UsersId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JTable_RoomUsers", x => new { x.ChatRoomsId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_JTable_RoomUsers_AspNetUsers_UsersId",
                        column: x => x.UsersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_JTable_RoomUsers_Rooms_ChatRoomsId",
                        column: x => x.ChatRoomsId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_JTable_RoomBlockedUsers_BlockedUsersId",
                table: "JTable_RoomBlockedUsers",
                column: "BlockedUsersId");

            migrationBuilder.CreateIndex(
                name: "IX_JTable_RoomUsers_UsersId",
                table: "JTable_RoomUsers",
                column: "UsersId");
        }
    }
}
