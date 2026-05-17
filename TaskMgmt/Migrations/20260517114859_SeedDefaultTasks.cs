using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TaskMgmt.Migrations
{
    /// <inheritdoc />
    public partial class SeedDefaultTasks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "Description", "IsCompleted", "Title" },
                values: new object[,]
                {
                    { 1, "Create folders and configure dependencies.", true, "Set up project structure" },
                    { 2, "Plan tables, relationships and migrations.", true, "Design database schema" },
                    { 3, "Implement GET, POST, PUT, DELETE for tasks.", false, "Build REST API endpoints" },
                    { 4, "Connect UI to the API and handle responses.", false, "Integrate Angular frontend" },
                    { 5, "Configure hosting and set up CI/CD pipeline.", false, "Deploy to production" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 5);
        }
    }
}
