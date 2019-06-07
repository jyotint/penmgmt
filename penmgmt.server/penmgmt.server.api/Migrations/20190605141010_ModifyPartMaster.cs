using Microsoft.EntityFrameworkCore.Migrations;

namespace penmgmt.server.api.Migrations
{
    public partial class ModifyPartMaster : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Committed",
                table: "PartMasters",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Deleted",
                table: "PartMasters",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Committed",
                table: "PartMasters");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "PartMasters");
        }
    }
}
