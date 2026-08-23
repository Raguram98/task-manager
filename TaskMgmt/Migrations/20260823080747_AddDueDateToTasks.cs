using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskMgmt.Migrations
{
    /// <inheritdoc />
    public partial class AddDueDateToTasks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateOnly>(
                name: "DueDate",
                table: "Tasks",
                type: "date",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DueDate",
                table: "Tasks");
        }
    }
}
