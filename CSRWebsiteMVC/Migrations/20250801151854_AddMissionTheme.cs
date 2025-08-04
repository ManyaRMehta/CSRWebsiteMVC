using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CSRWebsiteMVC.Migrations
{
    /// <inheritdoc />
    public partial class AddMissionTheme : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MissionMissionTheme");

            migrationBuilder.RenameColumn(
                name: "ThemeName",
                table: "MissionThemes",
                newName: "Title");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "MissionThemes",
                type: "text",
                nullable: true);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MissionThemes_Missions_MissionId",
                table: "MissionThemes");

            migrationBuilder.DropIndex(
                name: "IX_MissionThemes_MissionId",
                table: "MissionThemes");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "MissionThemes");

            migrationBuilder.DropColumn(
                name: "MissionId",
                table: "MissionThemes");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "MissionThemes",
                newName: "ThemeName");

            migrationBuilder.CreateTable(
                name: "MissionMissionTheme",
                columns: table => new
                {
                    MissionsId = table.Column<int>(type: "integer", nullable: false),
                    ThemesId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MissionMissionTheme", x => new { x.MissionsId, x.ThemesId });
                    table.ForeignKey(
                        name: "FK_MissionMissionTheme_MissionThemes_ThemesId",
                        column: x => x.ThemesId,
                        principalTable: "MissionThemes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MissionMissionTheme_Missions_MissionsId",
                        column: x => x.MissionsId,
                        principalTable: "Missions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MissionMissionTheme_ThemesId",
                table: "MissionMissionTheme",
                column: "ThemesId");
        }
    }
}
