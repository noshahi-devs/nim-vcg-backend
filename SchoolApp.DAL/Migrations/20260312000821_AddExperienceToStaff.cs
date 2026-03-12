using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolApp.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddExperienceToStaff : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CNIC",
                table: "Staff",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Experience",
                table: "Staff",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Campus",
                keyColumn: "CampusId",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 12, 5, 8, 14, 967, DateTimeKind.Local).AddTicks(2646));

            migrationBuilder.UpdateData(
                table: "Mark",
                keyColumn: "MarkId",
                keyValue: 1,
                column: "MarkEntryDate",
                value: new DateTime(2026, 3, 12, 5, 8, 14, 967, DateTimeKind.Local).AddTicks(5581));

            migrationBuilder.UpdateData(
                table: "Mark",
                keyColumn: "MarkId",
                keyValue: 2,
                column: "MarkEntryDate",
                value: new DateTime(2026, 3, 12, 5, 8, 14, 967, DateTimeKind.Local).AddTicks(5587));

            migrationBuilder.UpdateData(
                table: "Mark",
                keyColumn: "MarkId",
                keyValue: 3,
                column: "MarkEntryDate",
                value: new DateTime(2026, 3, 12, 5, 8, 14, 967, DateTimeKind.Local).AddTicks(5592));

            migrationBuilder.UpdateData(
                table: "Staff",
                keyColumn: "StaffId",
                keyValue: 1,
                columns: new[] { "CNIC", "Experience" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Staff",
                keyColumn: "StaffId",
                keyValue: 2,
                columns: new[] { "CNIC", "Experience" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Staff",
                keyColumn: "StaffId",
                keyValue: 3,
                columns: new[] { "CNIC", "Experience" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Standard",
                keyColumn: "StandardId",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 12, 5, 8, 14, 967, DateTimeKind.Local).AddTicks(5802));

            migrationBuilder.UpdateData(
                table: "Standard",
                keyColumn: "StandardId",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 12, 5, 8, 14, 967, DateTimeKind.Local).AddTicks(5808));

            migrationBuilder.UpdateData(
                table: "Standard",
                keyColumn: "StandardId",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 12, 5, 8, 14, 967, DateTimeKind.Local).AddTicks(5811));

            migrationBuilder.UpdateData(
                table: "Standard",
                keyColumn: "StandardId",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 12, 5, 8, 14, 967, DateTimeKind.Local).AddTicks(5814));

            migrationBuilder.UpdateData(
                table: "Standard",
                keyColumn: "StandardId",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 12, 5, 8, 14, 967, DateTimeKind.Local).AddTicks(5817));

            migrationBuilder.UpdateData(
                table: "Standard",
                keyColumn: "StandardId",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 12, 5, 8, 14, 967, DateTimeKind.Local).AddTicks(5883));

            migrationBuilder.UpdateData(
                table: "Standard",
                keyColumn: "StandardId",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 12, 5, 8, 14, 967, DateTimeKind.Local).AddTicks(5887));

            migrationBuilder.UpdateData(
                table: "Standard",
                keyColumn: "StandardId",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 12, 5, 8, 14, 967, DateTimeKind.Local).AddTicks(5889));

            migrationBuilder.UpdateData(
                table: "Standard",
                keyColumn: "StandardId",
                keyValue: 9,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 12, 5, 8, 14, 967, DateTimeKind.Local).AddTicks(5891));

            migrationBuilder.UpdateData(
                table: "Standard",
                keyColumn: "StandardId",
                keyValue: 10,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 12, 5, 8, 14, 967, DateTimeKind.Local).AddTicks(5894));

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 12, 5, 8, 14, 967, DateTimeKind.Local).AddTicks(5976));

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 12, 5, 8, 14, 967, DateTimeKind.Local).AddTicks(5987));

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 12, 5, 8, 14, 967, DateTimeKind.Local).AddTicks(5995));

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 12, 5, 8, 14, 967, DateTimeKind.Local).AddTicks(6001));

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 12, 5, 8, 14, 967, DateTimeKind.Local).AddTicks(6007));

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 12, 5, 8, 14, 967, DateTimeKind.Local).AddTicks(6013));

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 12, 5, 8, 14, 967, DateTimeKind.Local).AddTicks(6018));

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 12, 5, 8, 14, 967, DateTimeKind.Local).AddTicks(6023));

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 9,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 12, 5, 8, 14, 967, DateTimeKind.Local).AddTicks(6030));

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 10,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 12, 5, 8, 14, 967, DateTimeKind.Local).AddTicks(6036));

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 11,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 12, 5, 8, 14, 967, DateTimeKind.Local).AddTicks(6042));

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 12,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 12, 5, 8, 14, 967, DateTimeKind.Local).AddTicks(6050));

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 13,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 12, 5, 8, 14, 967, DateTimeKind.Local).AddTicks(6071));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CNIC",
                table: "Staff");

            migrationBuilder.DropColumn(
                name: "Experience",
                table: "Staff");

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
                column: "CreatedAt",
                value: new DateTime(2026, 3, 7, 22, 14, 15, 438, DateTimeKind.Local).AddTicks(958));

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 7, 22, 14, 15, 438, DateTimeKind.Local).AddTicks(972));

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 7, 22, 14, 15, 438, DateTimeKind.Local).AddTicks(980));

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 7, 22, 14, 15, 438, DateTimeKind.Local).AddTicks(987));

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 7, 22, 14, 15, 438, DateTimeKind.Local).AddTicks(1072));

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 7, 22, 14, 15, 438, DateTimeKind.Local).AddTicks(1081));

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 7, 22, 14, 15, 438, DateTimeKind.Local).AddTicks(1087));

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 7, 22, 14, 15, 438, DateTimeKind.Local).AddTicks(1094));

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 9,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 7, 22, 14, 15, 438, DateTimeKind.Local).AddTicks(1100));

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 10,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 7, 22, 14, 15, 438, DateTimeKind.Local).AddTicks(1107));

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 11,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 7, 22, 14, 15, 438, DateTimeKind.Local).AddTicks(1114));

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 12,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 7, 22, 14, 15, 438, DateTimeKind.Local).AddTicks(1121));

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 13,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 7, 22, 14, 15, 438, DateTimeKind.Local).AddTicks(1138));
        }
    }
}
