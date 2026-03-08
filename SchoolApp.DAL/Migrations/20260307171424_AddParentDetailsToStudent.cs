using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolApp.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddParentDetailsToStudent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ParentEmail",
                table: "Student",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Campus",
                keyColumn: "CampusId",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 7, 22, 14, 15, 437, DateTimeKind.Local).AddTicks(7714));

            migrationBuilder.UpdateData(
                table: "Mark",
                keyColumn: "MarkId",
                keyValue: 1,
                column: "MarkEntryDate",
                value: new DateTime(2026, 3, 7, 22, 14, 15, 438, DateTimeKind.Local).AddTicks(690));

            migrationBuilder.UpdateData(
                table: "Mark",
                keyColumn: "MarkId",
                keyValue: 2,
                column: "MarkEntryDate",
                value: new DateTime(2026, 3, 7, 22, 14, 15, 438, DateTimeKind.Local).AddTicks(697));

            migrationBuilder.UpdateData(
                table: "Mark",
                keyColumn: "MarkId",
                keyValue: 3,
                column: "MarkEntryDate",
                value: new DateTime(2026, 3, 7, 22, 14, 15, 438, DateTimeKind.Local).AddTicks(702));

            migrationBuilder.UpdateData(
                table: "Standard",
                keyColumn: "StandardId",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 7, 22, 14, 15, 438, DateTimeKind.Local).AddTicks(869));

            migrationBuilder.UpdateData(
                table: "Standard",
                keyColumn: "StandardId",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 7, 22, 14, 15, 438, DateTimeKind.Local).AddTicks(873));

            migrationBuilder.UpdateData(
                table: "Standard",
                keyColumn: "StandardId",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 7, 22, 14, 15, 438, DateTimeKind.Local).AddTicks(876));

            migrationBuilder.UpdateData(
                table: "Standard",
                keyColumn: "StandardId",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 7, 22, 14, 15, 438, DateTimeKind.Local).AddTicks(879));

            migrationBuilder.UpdateData(
                table: "Standard",
                keyColumn: "StandardId",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 7, 22, 14, 15, 438, DateTimeKind.Local).AddTicks(881));

            migrationBuilder.UpdateData(
                table: "Standard",
                keyColumn: "StandardId",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 7, 22, 14, 15, 438, DateTimeKind.Local).AddTicks(884));

            migrationBuilder.UpdateData(
                table: "Standard",
                keyColumn: "StandardId",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 7, 22, 14, 15, 438, DateTimeKind.Local).AddTicks(887));

            migrationBuilder.UpdateData(
                table: "Standard",
                keyColumn: "StandardId",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 7, 22, 14, 15, 438, DateTimeKind.Local).AddTicks(889));

            migrationBuilder.UpdateData(
                table: "Standard",
                keyColumn: "StandardId",
                keyValue: 9,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 7, 22, 14, 15, 438, DateTimeKind.Local).AddTicks(892));

            migrationBuilder.UpdateData(
                table: "Standard",
                keyColumn: "StandardId",
                keyValue: 10,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 7, 22, 14, 15, 438, DateTimeKind.Local).AddTicks(895));

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "ParentEmail", "ParentId" },
                values: new object[] { new DateTime(2026, 3, 7, 22, 14, 15, 438, DateTimeKind.Local).AddTicks(958), null, null });

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "ParentEmail", "ParentId" },
                values: new object[] { new DateTime(2026, 3, 7, 22, 14, 15, 438, DateTimeKind.Local).AddTicks(972), null, null });

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 3,
                columns: new[] { "CreatedAt", "ParentEmail", "ParentId" },
                values: new object[] { new DateTime(2026, 3, 7, 22, 14, 15, 438, DateTimeKind.Local).AddTicks(980), null, null });

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 4,
                columns: new[] { "CreatedAt", "ParentEmail", "ParentId" },
                values: new object[] { new DateTime(2026, 3, 7, 22, 14, 15, 438, DateTimeKind.Local).AddTicks(987), null, null });

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 5,
                columns: new[] { "CreatedAt", "ParentEmail", "ParentId" },
                values: new object[] { new DateTime(2026, 3, 7, 22, 14, 15, 438, DateTimeKind.Local).AddTicks(1072), null, null });

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 6,
                columns: new[] { "CreatedAt", "ParentEmail", "ParentId" },
                values: new object[] { new DateTime(2026, 3, 7, 22, 14, 15, 438, DateTimeKind.Local).AddTicks(1081), null, null });

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 7,
                columns: new[] { "CreatedAt", "ParentEmail", "ParentId" },
                values: new object[] { new DateTime(2026, 3, 7, 22, 14, 15, 438, DateTimeKind.Local).AddTicks(1087), null, null });

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 8,
                columns: new[] { "CreatedAt", "ParentEmail", "ParentId" },
                values: new object[] { new DateTime(2026, 3, 7, 22, 14, 15, 438, DateTimeKind.Local).AddTicks(1094), null, null });

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 9,
                columns: new[] { "CreatedAt", "ParentEmail", "ParentId" },
                values: new object[] { new DateTime(2026, 3, 7, 22, 14, 15, 438, DateTimeKind.Local).AddTicks(1100), null, null });

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 10,
                columns: new[] { "CreatedAt", "ParentEmail", "ParentId" },
                values: new object[] { new DateTime(2026, 3, 7, 22, 14, 15, 438, DateTimeKind.Local).AddTicks(1107), null, null });

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 11,
                columns: new[] { "CreatedAt", "ParentEmail", "ParentId" },
                values: new object[] { new DateTime(2026, 3, 7, 22, 14, 15, 438, DateTimeKind.Local).AddTicks(1114), null, null });

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 12,
                columns: new[] { "CreatedAt", "ParentEmail", "ParentId" },
                values: new object[] { new DateTime(2026, 3, 7, 22, 14, 15, 438, DateTimeKind.Local).AddTicks(1121), null, null });

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 13,
                columns: new[] { "CreatedAt", "ParentEmail", "ParentId" },
                values: new object[] { new DateTime(2026, 3, 7, 22, 14, 15, 438, DateTimeKind.Local).AddTicks(1138), null, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Student_Parents_ParentId",
                table: "Student");

            migrationBuilder.DropTable(
                name: "Parents");

            migrationBuilder.DropIndex(
                name: "IX_Student_ParentId",
                table: "Student");

            migrationBuilder.DropColumn(
                name: "ParentEmail",
                table: "Student");



            migrationBuilder.UpdateData(
                table: "Campus",
                keyColumn: "CampusId",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 3, 5, 0, 41, 662, DateTimeKind.Local).AddTicks(804));

            migrationBuilder.UpdateData(
                table: "Mark",
                keyColumn: "MarkId",
                keyValue: 1,
                column: "MarkEntryDate",
                value: new DateTime(2026, 3, 3, 5, 0, 41, 662, DateTimeKind.Local).AddTicks(2556));

            migrationBuilder.UpdateData(
                table: "Mark",
                keyColumn: "MarkId",
                keyValue: 2,
                column: "MarkEntryDate",
                value: new DateTime(2026, 3, 3, 5, 0, 41, 662, DateTimeKind.Local).AddTicks(2562));

            migrationBuilder.UpdateData(
                table: "Mark",
                keyColumn: "MarkId",
                keyValue: 3,
                column: "MarkEntryDate",
                value: new DateTime(2026, 3, 3, 5, 0, 41, 662, DateTimeKind.Local).AddTicks(2565));

            migrationBuilder.UpdateData(
                table: "Standard",
                keyColumn: "StandardId",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 3, 5, 0, 41, 662, DateTimeKind.Local).AddTicks(2674));

            migrationBuilder.UpdateData(
                table: "Standard",
                keyColumn: "StandardId",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 3, 5, 0, 41, 662, DateTimeKind.Local).AddTicks(2677));

            migrationBuilder.UpdateData(
                table: "Standard",
                keyColumn: "StandardId",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 3, 5, 0, 41, 662, DateTimeKind.Local).AddTicks(2715));

            migrationBuilder.UpdateData(
                table: "Standard",
                keyColumn: "StandardId",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 3, 5, 0, 41, 662, DateTimeKind.Local).AddTicks(2717));

            migrationBuilder.UpdateData(
                table: "Standard",
                keyColumn: "StandardId",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 3, 5, 0, 41, 662, DateTimeKind.Local).AddTicks(2720));

            migrationBuilder.UpdateData(
                table: "Standard",
                keyColumn: "StandardId",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 3, 5, 0, 41, 662, DateTimeKind.Local).AddTicks(2722));

            migrationBuilder.UpdateData(
                table: "Standard",
                keyColumn: "StandardId",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 3, 5, 0, 41, 662, DateTimeKind.Local).AddTicks(2724));

            migrationBuilder.UpdateData(
                table: "Standard",
                keyColumn: "StandardId",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 3, 5, 0, 41, 662, DateTimeKind.Local).AddTicks(2726));

            migrationBuilder.UpdateData(
                table: "Standard",
                keyColumn: "StandardId",
                keyValue: 9,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 3, 5, 0, 41, 662, DateTimeKind.Local).AddTicks(2728));

            migrationBuilder.UpdateData(
                table: "Standard",
                keyColumn: "StandardId",
                keyValue: 10,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 3, 5, 0, 41, 662, DateTimeKind.Local).AddTicks(2730));

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 3, 5, 0, 41, 662, DateTimeKind.Local).AddTicks(2773));

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 3, 5, 0, 41, 662, DateTimeKind.Local).AddTicks(2781));

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 3, 5, 0, 41, 662, DateTimeKind.Local).AddTicks(2786));

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 3, 5, 0, 41, 662, DateTimeKind.Local).AddTicks(2792));

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 3, 5, 0, 41, 662, DateTimeKind.Local).AddTicks(2796));

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 3, 5, 0, 41, 662, DateTimeKind.Local).AddTicks(2800));

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 3, 5, 0, 41, 662, DateTimeKind.Local).AddTicks(2805));

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 3, 5, 0, 41, 662, DateTimeKind.Local).AddTicks(2809));

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 9,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 3, 5, 0, 41, 662, DateTimeKind.Local).AddTicks(2813));

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 10,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 3, 5, 0, 41, 662, DateTimeKind.Local).AddTicks(2817));

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 11,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 3, 5, 0, 41, 662, DateTimeKind.Local).AddTicks(2821));

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 12,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 3, 5, 0, 41, 662, DateTimeKind.Local).AddTicks(2825));

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 13,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 3, 5, 0, 41, 662, DateTimeKind.Local).AddTicks(2843));
        }
    }
}
