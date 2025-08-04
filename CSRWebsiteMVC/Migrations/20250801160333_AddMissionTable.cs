using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CSRWebsiteMVC.Migrations
{
    /// <inheritdoc />
    public partial class AddMissionTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MissionThemes_Missions_MissionId",
                table: "MissionThemes");

            migrationBuilder.DropIndex(
                name: "IX_MissionThemes_MissionId",
                table: "MissionThemes");

            migrationBuilder.DropColumn(
                name: "MissionId",
                table: "MissionThemes");

            migrationBuilder.RenameColumn(
                name: "AvailableSeats",
                table: "Missions",
                newName: "TotalSeats");

            migrationBuilder.AddColumn<int>(
                name: "MissionThemeId",
                table: "Missions",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Missions_MissionThemeId",
                table: "Missions",
                column: "MissionThemeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Missions_MissionThemes_MissionThemeId",
                table: "Missions",
                column: "MissionThemeId",
                principalTable: "MissionThemes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Missions_MissionThemes_MissionThemeId",
                table: "Missions");

            migrationBuilder.DropIndex(
                name: "IX_Missions_MissionThemeId",
                table: "Missions");

            migrationBuilder.DropColumn(
                name: "MissionThemeId",
                table: "Missions");

            migrationBuilder.RenameColumn(
                name: "TotalSeats",
                table: "Missions",
                newName: "AvailableSeats");

            migrationBuilder.AddColumn<int>(
                name: "MissionId",
                table: "MissionThemes",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MissionThemes_MissionId",
                table: "MissionThemes",
                column: "MissionId");

            migrationBuilder.AddForeignKey(
                name: "FK_MissionThemes_Missions_MissionId",
                table: "MissionThemes",
                column: "MissionId",
                principalTable: "Missions",
                principalColumn: "Id");
        }
    }
}
