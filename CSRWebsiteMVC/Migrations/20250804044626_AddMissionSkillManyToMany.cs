using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CSRWebsiteMVC.Migrations
{
    /// <inheritdoc />
    public partial class AddMissionSkillManyToMany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MissionSkills_Missions_MissionId",
                table: "MissionSkills");

            migrationBuilder.DropIndex(
                name: "IX_MissionSkills_MissionId",
                table: "MissionSkills");

            migrationBuilder.DropColumn(
                name: "MissionId",
                table: "MissionSkills");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MissionSkillsMap");

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
    }
}
