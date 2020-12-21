using Microsoft.EntityFrameworkCore.Migrations;

namespace StudyMaterialsSharingSystem.Data.Migrations
{
    public partial class AddProductReviews : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Message",
                table: "Requests");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Message",
                table: "Requests",
                nullable: true);
        }
    }
}
