using Microsoft.EntityFrameworkCore.Migrations;

namespace BlogProjectMVC.Data.Migrations
{
    public partial class updatingPostStateProperyToStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "State",
                table: "Posts",
                newName: "Status");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Posts",
                newName: "State");
        }
    }
}
