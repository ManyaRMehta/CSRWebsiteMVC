using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CSRWebsiteMVC.Migrations
{
    /// <inheritdoc />
    public partial class AddMissionSkillTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MissionMissionSkill");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "MissionSkills",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MissionId",
                table: "MissionSkills",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MissionSkills_MissionId",
                table: "MissionSkills",
                column: "MissionId");

            migrationBuilder.AddForeignKey(
                name: "FK_MissionSkills_Missions_MissionId",
                table: "MissionSkills",
                column: "MissionId",
                principalTable: "Missions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MissionSkills_Missions_MissionId",
                table: "MissionSkills");

            migrationBuilder.DropIndex(
                name: "IX_MissionSkills_MissionId",
                table: "MissionSkills");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "MissionSkills");

            migrationBuilder.DropColumn(
                name: "MissionId",
                table: "MissionSkills");

            migrationBuilder.CreateTable(
                name: "MissionMissionSkill",
                columns: table => new
                {
                    MissionsId = table.Column<int>(type: "integer", nullable: false),
                    SkillsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MissionMissionSkill", x => new { x.MissionsId, x.SkillsId });
                    table.ForeignKey(
                        name: "FK_MissionMissionSkill_MissionSkills_SkillsId",
                        column: x => x.SkillsId,
                        principalTable: "MissionSkills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MissionMissionSkill_Missions_MissionsId",
                        column: x => x.MissionsId,
                        principalTable: "Missions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MissionMissionSkill_SkillsId",
                table: "MissionMissionSkill",
                column: "SkillsId");
        }
    }
}
