using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolApp.DAL.Migrations
{
    /// <inheritdoc />
    public partial class dll2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Mark",
                keyColumn: "MarkId",
                keyValue: 1,
                column: "MarkEntryDate",
                value: new DateTime(2025, 8, 8, 23, 43, 13, 213, DateTimeKind.Local).AddTicks(9702));

            migrationBuilder.UpdateData(
                table: "Mark",
                keyColumn: "MarkId",
                keyValue: 2,
                column: "MarkEntryDate",
                value: new DateTime(2025, 8, 8, 23, 43, 13, 213, DateTimeKind.Local).AddTicks(9707));

            migrationBuilder.UpdateData(
                table: "Mark",
                keyColumn: "MarkId",
                keyValue: 3,
                column: "MarkEntryDate",
                value: new DateTime(2025, 8, 8, 23, 43, 13, 213, DateTimeKind.Local).AddTicks(9710));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Mark",
                keyColumn: "MarkId",
                keyValue: 1,
                column: "MarkEntryDate",
                value: new DateTime(2025, 8, 8, 23, 38, 25, 494, DateTimeKind.Local).AddTicks(5642));

            migrationBuilder.UpdateData(
                table: "Mark",
                keyColumn: "MarkId",
                keyValue: 2,
                column: "MarkEntryDate",
                value: new DateTime(2025, 8, 8, 23, 38, 25, 494, DateTimeKind.Local).AddTicks(5682));

            migrationBuilder.UpdateData(
                table: "Mark",
                keyColumn: "MarkId",
                keyValue: 3,
                column: "MarkEntryDate",
                value: new DateTime(2025, 8, 8, 23, 38, 25, 494, DateTimeKind.Local).AddTicks(5685));
        }
    }
}
