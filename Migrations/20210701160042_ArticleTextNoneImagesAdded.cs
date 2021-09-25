using Microsoft.EntityFrameworkCore.Migrations;

namespace spgnn.Migrations
{
    public partial class ArticleTextNoneImagesAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NoneImagesText",
                table: "Articles",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NoneImagesText",
                table: "Articles");
        }
    }
}
