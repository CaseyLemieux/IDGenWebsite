using Microsoft.EntityFrameworkCore.Migrations;

namespace IDGenWebsite.Migrations.IDGenWebsite
{
    public partial class EmplopyeeTeacherFieldAdds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SchoolID",
                table: "AspNetUsers",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "TeacherID",
                table: "AspNetUsers",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SchoolID",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TeacherID",
                table: "AspNetUsers");
        }
    }
}
