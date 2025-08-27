using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolApp.DAL.Migrations
{
    /// <inheritdoc />
    public partial class school : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Mark",
                keyColumn: "MarkId",
                keyValue: 1,
                column: "MarkEntryDate",
                value: new DateTime(2024, 6, 4, 15, 44, 20, 185, DateTimeKind.Local).AddTicks(9347));

            migrationBuilder.UpdateData(
                table: "Mark",
                keyColumn: "MarkId",
                keyValue: 2,
                column: "MarkEntryDate",
                value: new DateTime(2024, 6, 4, 15, 44, 20, 185, DateTimeKind.Local).AddTicks(9350));

            migrationBuilder.UpdateData(
                table: "Mark",
                keyColumn: "MarkId",
                keyValue: 3,
                column: "MarkEntryDate",
                value: new DateTime(2024, 6, 4, 15, 44, 20, 185, DateTimeKind.Local).AddTicks(9352));
        }
    }
}
