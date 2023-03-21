using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevTrack.Web.Data.Migrations
{
    public partial class UpdateDatebase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_WebcamCaptures_ActivityId",
                table: "WebcamCaptures");

            migrationBuilder.DropIndex(
                name: "IX_ScreenCaptures_ActivityId",
                table: "ScreenCaptures");

            migrationBuilder.DropIndex(
                name: "IX_MouseActivities_ActivityId",
                table: "MouseActivities");

            migrationBuilder.DropIndex(
                name: "IX_KeyboardActivities_ActivityId",
                table: "KeyboardActivities");

            migrationBuilder.DropColumn(
                name: "Time",
                table: "WebcamCaptures");

            migrationBuilder.DropColumn(
                name: "Time",
                table: "ScreenCaptures");

            migrationBuilder.DropColumn(
                name: "Time",
                table: "RunningPrograms");

            migrationBuilder.DropColumn(
                name: "IsProcessed",
                table: "Activities");

            migrationBuilder.DropColumn(
                name: "Time",
                table: "ActiveWindows");

            migrationBuilder.CreateIndex(
                name: "IX_WebcamCaptures_ActivityId",
                table: "WebcamCaptures",
                column: "ActivityId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ScreenCaptures_ActivityId",
                table: "ScreenCaptures",
                column: "ActivityId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MouseActivities_ActivityId",
                table: "MouseActivities",
                column: "ActivityId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_KeyboardActivities_ActivityId",
                table: "KeyboardActivities",
                column: "ActivityId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_WebcamCaptures_ActivityId",
                table: "WebcamCaptures");

            migrationBuilder.DropIndex(
                name: "IX_ScreenCaptures_ActivityId",
                table: "ScreenCaptures");

            migrationBuilder.DropIndex(
                name: "IX_MouseActivities_ActivityId",
                table: "MouseActivities");

            migrationBuilder.DropIndex(
                name: "IX_KeyboardActivities_ActivityId",
                table: "KeyboardActivities");

            migrationBuilder.AddColumn<DateTime>(
                name: "Time",
                table: "WebcamCaptures",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Time",
                table: "ScreenCaptures",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Time",
                table: "RunningPrograms",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsProcessed",
                table: "Activities",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "Time",
                table: "ActiveWindows",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_WebcamCaptures_ActivityId",
                table: "WebcamCaptures",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_ScreenCaptures_ActivityId",
                table: "ScreenCaptures",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_MouseActivities_ActivityId",
                table: "MouseActivities",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_KeyboardActivities_ActivityId",
                table: "KeyboardActivities",
                column: "ActivityId");
        }
    }
}
