using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CSRWebsiteMVC.Migrations
{
    /// <inheritdoc />
    public partial class AddMissionSkillAssignment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MissionSkillsMap");

            migrationBuilder.AddColumn<int>(
                name: "MissionSkillId",
                table: "Missions",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MissionSkillAssignments",
                columns: table => new
                {
                    MissionId = table.Column<int>(type: "integer", nullable: false),
                    MissionSkillId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MissionSkillAssignments", x => new { x.MissionId, x.MissionSkillId });
                    table.ForeignKey(
                        name: "FK_MissionSkillAssignments_MissionSkills_MissionSkillId",
                        column: x => x.MissionSkillId,
                        principalTable: "MissionSkills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MissionSkillAssignments_Missions_MissionId",
                        column: x => x.MissionId,
                        principalTable: "Missions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Missions_MissionSkillId",
                table: "Missions",
                column: "MissionSkillId");

            migrationBuilder.CreateIndex(
                name: "IX_MissionSkillAssignments_MissionSkillId",
                table: "MissionSkillAssignments",
                column: "MissionSkillId");

            migrationBuilder.AddForeignKey(
                name: "FK_Missions_MissionSkills_MissionSkillId",
                table: "Missions",
                column: "MissionSkillId",
                principalTable: "MissionSkills",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Missions_MissionSkills_MissionSkillId",
                table: "Missions");

            migrationBuilder.DropTable(
                name: "MissionSkillAssignments");

            migrationBuilder.DropIndex(
                name: "IX_Missions_MissionSkillId",
                table: "Missions");

            migrationBuilder.DropColumn(
                name: "MissionSkillId",
                table: "Missions");

            migrationBuilder.CreateTable(
                name: "MissionSkillsMap",
                columns: table => new
                {
                    MissionSkillsId = table.Column<int>(type: "integer", nullable: false),
                    MissionsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MissionSkillsMap", x => new { x.MissionSkillsId, x.MissionsId });
                    table.ForeignKey(
                        name: "FK_MissionSkillsMap_MissionSkills_MissionSkillsId",
                        column: x => x.MissionSkillsId,
                        principalTable: "MissionSkills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MissionSkillsMap_Missions_MissionsId",
                        column: x => x.MissionsId,
                        principalTable: "Missions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MissionSkillsMap_MissionsId",
                table: "MissionSkillsMap",
                column: "MissionsId");
        }
    }
}
