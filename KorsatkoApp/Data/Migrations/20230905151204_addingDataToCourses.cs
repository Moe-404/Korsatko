using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace KorsatkoApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class addingDataToCourses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Courses",
                columns: new[] { "Id", "AddedOn", "Description", "Name", "Picture", "Prerequisites", "Price" },
                values: new object[,]
                {
                    { 123, new DateTime(2023, 9, 5, 18, 12, 3, 937, DateTimeKind.Local).AddTicks(1889), "C++ Programming Language for Advanced Programmers", "Advanced C++", null, "C Course", 1500.0 },
                    { 124, new DateTime(2023, 9, 5, 18, 12, 3, 937, DateTimeKind.Local).AddTicks(1895), "C Programming Language", "C For Beginners", null, "None", 1200.0 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 123);

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 124);
        }
    }
}
