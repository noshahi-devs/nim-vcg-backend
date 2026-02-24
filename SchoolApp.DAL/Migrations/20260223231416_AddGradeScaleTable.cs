using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolApp.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddGradeScaleTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GradeScales",
                columns: table => new
                {
                    GradeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Grade = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    MinPercentage = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MaxPercentage = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    GradePoint = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GradeScales", x => x.GradeId);
                });

            migrationBuilder.UpdateData(
                table: "Mark",
                keyColumn: "MarkId",
                keyValue: 1,
                column: "MarkEntryDate",
                value: new DateTime(2026, 2, 24, 4, 14, 6, 363, DateTimeKind.Local).AddTicks(6141));

            migrationBuilder.UpdateData(
                table: "Mark",
                keyColumn: "MarkId",
                keyValue: 2,
                column: "MarkEntryDate",
                value: new DateTime(2026, 2, 24, 4, 14, 6, 363, DateTimeKind.Local).AddTicks(6152));

            migrationBuilder.UpdateData(
                table: "Mark",
                keyColumn: "MarkId",
                keyValue: 3,
                column: "MarkEntryDate",
                value: new DateTime(2026, 2, 24, 4, 14, 6, 363, DateTimeKind.Local).AddTicks(6159));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GradeScales");

            migrationBuilder.UpdateData(
                table: "Mark",
                keyColumn: "MarkId",
                keyValue: 1,
                column: "MarkEntryDate",
                value: new DateTime(2026, 2, 22, 22, 16, 36, 877, DateTimeKind.Local).AddTicks(1000));

            migrationBuilder.UpdateData(
                table: "Mark",
                keyColumn: "MarkId",
                keyValue: 2,
                column: "MarkEntryDate",
                value: new DateTime(2026, 2, 22, 22, 16, 36, 877, DateTimeKind.Local).AddTicks(1008));

            migrationBuilder.UpdateData(
                table: "Mark",
                keyColumn: "MarkId",
                keyValue: 3,
                column: "MarkEntryDate",
                value: new DateTime(2026, 2, 22, 22, 16, 36, 877, DateTimeKind.Local).AddTicks(1012));
        }
    }
}
