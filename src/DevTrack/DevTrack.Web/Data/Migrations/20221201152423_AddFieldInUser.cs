using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevTrack.Web.Data.Migrations
{
    public partial class AddFieldInUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(name: "FK_AspNetUserTokens_AspNetUsers_UserId",
               table: "AspNetUserTokens");
            migrationBuilder.DropForeignKey(name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                table: "AspNetUserRoles");
            migrationBuilder.DropForeignKey(name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                table: "AspNetUserLogins");
            migrationBuilder.DropForeignKey(name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                table: "AspNetUserClaims");
            migrationBuilder.DropForeignKey(name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                table: "AspNetUserRoles");
            migrationBuilder.DropForeignKey(name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                table: "AspNetRoleClaims");
            migrationBuilder.DropPrimaryKey(name: "PK_AspNetUsers", table: "AspNetUsers");
            migrationBuilder.DropPrimaryKey(name: "PK_AspNetUserTokens", table: "AspNetUserTokens");
            migrationBuilder.DropPrimaryKey(name: "PK_AspNetUserRoles", table: "AspNetUserRoles");
            migrationBuilder.DropPrimaryKey(name: "PK_AspNetRoles", table: "AspNetRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_Activities_ApplicationUsers_ApplicationUserId",
                table: "Activities");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectUsers_ApplicationUsers_ApplicationUserId",
                table: "ProjectUsers");

            migrationBuilder.DropTable(
                name: "ApplicationUsers");

            migrationBuilder.DropIndex(
                name: "IX_Activities_ApplicationUserId",
                table: "Activities");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Activities");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "AspNetUserTokens",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<Guid>(
                name: "RoleId",
                table: "AspNetUserRoles",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "AspNetUserRoles",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "AspNetUserLogins",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "AspNetUserClaims",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "AspNetRoles",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<Guid>(
                name: "RoleId",
                table: "AspNetRoleClaims",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddPrimaryKey(name: "PK_AspNetRoles", table: "AspNetRoles", column: "Id");
            migrationBuilder.AddPrimaryKey(name: "PK_AspNetUserRoles", table: "AspNetUserRoles", columns: new[] { "UserId", "RoleId" });
            migrationBuilder.AddPrimaryKey(name: "PK_AspNetUserTokens", table: "AspNetUserTokens", columns: new[] { "UserId", "LoginProvider", "Name" });
            migrationBuilder.AddPrimaryKey(name: "PK_AspNetUsers", table: "AspNetUsers", column: "Id");
            migrationBuilder.AddForeignKey(name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                table: "AspNetRoleClaims", column: "RoleId", principalTable: "AspNetRoles", principalColumn: "Id");
            migrationBuilder.AddForeignKey(name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                table: "AspNetUserRoles", column: "RoleId", principalTable: "AspNetRoles", principalColumn: "Id");
            migrationBuilder.AddForeignKey(name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                table: "AspNetUserClaims", column: "UserId", principalTable: "AspNetUsers", principalColumn: "Id");
            migrationBuilder.AddForeignKey(name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                table: "AspNetUserLogins", column: "UserId", principalTable: "AspNetUsers", principalColumn: "Id");
            migrationBuilder.AddForeignKey(name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                table: "AspNetUserRoles", column: "UserId", principalTable: "AspNetUsers", principalColumn: "Id");
            migrationBuilder.AddForeignKey(name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                table: "AspNetUserTokens", column: "UserId", principalTable: "AspNetUsers", principalColumn: "Id");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(name: "FK_AspNetUserTokens_AspNetUsers_UserId",
               table: "AspNetUserTokens");
            migrationBuilder.DropForeignKey(name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                table: "AspNetUserRoles");
            migrationBuilder.DropForeignKey(name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                table: "AspNetUserLogins");
            migrationBuilder.DropForeignKey(name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                table: "AspNetUserClaims");
            migrationBuilder.DropForeignKey(name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                table: "AspNetUserRoles");
            migrationBuilder.DropForeignKey(name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                table: "AspNetRoleClaims");
            migrationBuilder.DropPrimaryKey(name: "PK_AspNetUsers", table: "AspNetUsers");
            migrationBuilder.DropPrimaryKey(name: "PK_AspNetUserTokens", table: "AspNetUserTokens");
            migrationBuilder.DropPrimaryKey(name: "PK_AspNetUserRoles", table: "AspNetUserRoles");
            migrationBuilder.DropPrimaryKey(name: "PK_AspNetRoles", table: "AspNetRoles");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "AspNetUserTokens",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "AspNetUsers",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<string>(
                name: "RoleId",
                table: "AspNetUserRoles",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "AspNetUserRoles",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "AspNetUserLogins",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "AspNetUserClaims",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "AspNetRoles",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<string>(
                name: "RoleId",
                table: "AspNetRoleClaims",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "ApplicationUserId",
                table: "Activities",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "ApplicationUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUsers", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Activities_ApplicationUserId",
                table: "Activities",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_ApplicationUsers_ApplicationUserId",
                table: "Activities",
                column: "ApplicationUserId",
                principalTable: "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectUsers_ApplicationUsers_ApplicationUserId",
                table: "ProjectUsers",
                column: "ApplicationUserId",
                principalTable: "ApplicationUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddPrimaryKey(name: "PK_AspNetRoles", table: "AspNetRoles", column: "Id");
            migrationBuilder.AddPrimaryKey(name: "PK_AspNetUserRoles", table: "AspNetUserRoles", columns: new[] { "UserId", "RoleId" });
            migrationBuilder.AddPrimaryKey(name: "PK_AspNetUserTokens", table: "AspNetUserTokens", columns: new[] { "UserId", "LoginProvider", "Name" });
            migrationBuilder.AddPrimaryKey(name: "PK_AspNetUsers", table: "AspNetUsers", column: "Id");
            migrationBuilder.AddForeignKey(name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                table: "AspNetRoleClaims", column: "RoleId", principalTable: "AspNetRoles", principalColumn: "Id");
            migrationBuilder.AddForeignKey(name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                table: "AspNetUserRoles", column: "RoleId", principalTable: "AspNetRoles", principalColumn: "Id");
            migrationBuilder.AddForeignKey(name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                table: "AspNetUserClaims", column: "UserId", principalTable: "AspNetUsers", principalColumn: "Id");
            migrationBuilder.AddForeignKey(name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                table: "AspNetUserLogins", column: "UserId", principalTable: "AspNetUsers", principalColumn: "Id");
            migrationBuilder.AddForeignKey(name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                table: "AspNetUserRoles", column: "UserId", principalTable: "AspNetUsers", principalColumn: "Id");
            migrationBuilder.AddForeignKey(name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                table: "AspNetUserTokens", column: "UserId", principalTable: "AspNetUsers", principalColumn: "Id");

        }
    }
}
