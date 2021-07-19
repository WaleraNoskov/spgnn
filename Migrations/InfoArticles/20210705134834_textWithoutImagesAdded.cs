using Microsoft.EntityFrameworkCore.Migrations;

namespace spgnn.Migrations.InfoArticles
{
    public partial class textWithoutImagesAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NoneImagesText",
                table: "InfoArticles",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NoneImagesText",
                table: "InfoArticles");
        }
    }
}
