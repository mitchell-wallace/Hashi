using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HashiAPI_1.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "projects",
                columns: table => new
                {
                    pmap_id = table.Column<int>(type: "int", unicode: false, maxLength: 255, nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    project_name = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    jira_id = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    wrike_id = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    created_date = table.Column<DateTime>(type: "datetime", nullable: true),
                    updated_date = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__projects__4A0B0D685F01DFD1", x => x.pmap_id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    umap_id = table.Column<int>(type: "int", unicode: false, maxLength: 255, nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    email = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    first_name = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    last_name = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    jira_id = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    wrike_id = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    created_date = table.Column<DateTime>(type: "datetime", nullable: true),
                    updated_date = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__users__AB6E61650C1582E8", x => x.umap_id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "projects");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
