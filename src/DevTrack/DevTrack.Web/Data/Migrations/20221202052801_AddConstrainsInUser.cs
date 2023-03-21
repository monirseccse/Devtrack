using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevTrack.Web.Data.Migrations
{
    public partial class AddConstrainsInUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ApplicationUserId",
                table: "Activities",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Activities_ApplicationUserId",
                table: "Activities",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_AspNetUsers_ApplicationUserId",
                table: "Activities",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectUsers_AspNetUsers_ApplicationUserId",
                table: "ProjectUsers",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activities_AspNetUsers_ApplicationUserId",
                table: "Activities");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectUsers_AspNetUsers_ApplicationUserId",
                table: "ProjectUsers");

            migrationBuilder.DropIndex(
                name: "IX_Activities_ApplicationUserId",
                table: "Activities");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Activities");
        }
    }
}
