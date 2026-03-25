using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolApp.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddStaffSalaryColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SettingValue",
                table: "SystemSettings",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<DateTime>(
                name: "PaymentDate",
                table: "StaffSalary",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentMonth",
                table: "StaffSalary",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StaffId",
                table: "StaffSalary",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Campus",
                keyColumn: "CampusId",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 25, 2, 38, 32, 548, DateTimeKind.Local).AddTicks(8129));

            migrationBuilder.UpdateData(
                table: "Mark",
                keyColumn: "MarkId",
                keyValue: 1,
                column: "MarkEntryDate",
                value: new DateTime(2026, 3, 25, 2, 38, 32, 549, DateTimeKind.Local).AddTicks(4096));

            migrationBuilder.UpdateData(
                table: "Mark",
                keyColumn: "MarkId",
                keyValue: 2,
                column: "MarkEntryDate",
                value: new DateTime(2026, 3, 25, 2, 38, 32, 549, DateTimeKind.Local).AddTicks(4104));

            migrationBuilder.UpdateData(
                table: "Mark",
                keyColumn: "MarkId",
                keyValue: 3,
                column: "MarkEntryDate",
                value: new DateTime(2026, 3, 25, 2, 38, 32, 549, DateTimeKind.Local).AddTicks(4111));

            migrationBuilder.UpdateData(
                table: "StaffSalary",
                keyColumn: "StaffSalaryId",
                keyValue: 1,
                columns: new[] { "PaymentDate", "PaymentMonth", "StaffId" },
                values: new object[] { new DateTime(2026, 3, 25, 2, 38, 32, 549, DateTimeKind.Local).AddTicks(4213), null, null });

            migrationBuilder.UpdateData(
                table: "StaffSalary",
                keyColumn: "StaffSalaryId",
                keyValue: 2,
                columns: new[] { "PaymentDate", "PaymentMonth", "StaffId" },
                values: new object[] { new DateTime(2026, 3, 25, 2, 38, 32, 549, DateTimeKind.Local).AddTicks(4231), null, null });

            migrationBuilder.UpdateData(
                table: "StaffSalary",
                keyColumn: "StaffSalaryId",
                keyValue: 3,
                columns: new[] { "PaymentDate", "PaymentMonth", "StaffId" },
                values: new object[] { new DateTime(2026, 3, 25, 2, 38, 32, 549, DateTimeKind.Local).AddTicks(4243), null, null });

            migrationBuilder.UpdateData(
                table: "Standard",
                keyColumn: "StandardId",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 25, 2, 38, 32, 549, DateTimeKind.Local).AddTicks(4420));

            migrationBuilder.UpdateData(
                table: "Standard",
                keyColumn: "StandardId",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 25, 2, 38, 32, 549, DateTimeKind.Local).AddTicks(4428));

            migrationBuilder.UpdateData(
                table: "Standard",
                keyColumn: "StandardId",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 25, 2, 38, 32, 549, DateTimeKind.Local).AddTicks(4554));

            migrationBuilder.UpdateData(
                table: "Standard",
                keyColumn: "StandardId",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 25, 2, 38, 32, 549, DateTimeKind.Local).AddTicks(4560));

            migrationBuilder.UpdateData(
                table: "Standard",
                keyColumn: "StandardId",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 25, 2, 38, 32, 549, DateTimeKind.Local).AddTicks(4564));

            migrationBuilder.UpdateData(
                table: "Standard",
                keyColumn: "StandardId",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 25, 2, 38, 32, 549, DateTimeKind.Local).AddTicks(4568));

            migrationBuilder.UpdateData(
                table: "Standard",
                keyColumn: "StandardId",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 25, 2, 38, 32, 549, DateTimeKind.Local).AddTicks(4572));

            migrationBuilder.UpdateData(
                table: "Standard",
                keyColumn: "StandardId",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 25, 2, 38, 32, 549, DateTimeKind.Local).AddTicks(4576));

            migrationBuilder.UpdateData(
                table: "Standard",
                keyColumn: "StandardId",
                keyValue: 9,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 25, 2, 38, 32, 549, DateTimeKind.Local).AddTicks(4580));

            migrationBuilder.UpdateData(
                table: "Standard",
                keyColumn: "StandardId",
                keyValue: 10,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 25, 2, 38, 32, 549, DateTimeKind.Local).AddTicks(4584));

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 25, 2, 38, 32, 549, DateTimeKind.Local).AddTicks(4697));

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 25, 2, 38, 32, 549, DateTimeKind.Local).AddTicks(4714));

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 25, 2, 38, 32, 549, DateTimeKind.Local).AddTicks(4725));

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 25, 2, 38, 32, 549, DateTimeKind.Local).AddTicks(4733));

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 25, 2, 38, 32, 549, DateTimeKind.Local).AddTicks(4741));

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 25, 2, 38, 32, 549, DateTimeKind.Local).AddTicks(4750));

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 25, 2, 38, 32, 549, DateTimeKind.Local).AddTicks(4759));

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 25, 2, 38, 32, 549, DateTimeKind.Local).AddTicks(4773));

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 9,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 25, 2, 38, 32, 549, DateTimeKind.Local).AddTicks(4782));

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 10,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 25, 2, 38, 32, 549, DateTimeKind.Local).AddTicks(4816));

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 11,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 25, 2, 38, 32, 549, DateTimeKind.Local).AddTicks(4843));

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 12,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 25, 2, 38, 32, 549, DateTimeKind.Local).AddTicks(4852));

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 13,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 25, 2, 38, 32, 549, DateTimeKind.Local).AddTicks(4861));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentDate",
                table: "StaffSalary");

            migrationBuilder.DropColumn(
                name: "PaymentMonth",
                table: "StaffSalary");

            migrationBuilder.DropColumn(
                name: "StaffId",
                table: "StaffSalary");

            migrationBuilder.AlterColumn<string>(
                name: "SettingValue",
                table: "SystemSettings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Campus",
                keyColumn: "CampusId",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 18, 3, 15, 2, 254, DateTimeKind.Local).AddTicks(1767));

            migrationBuilder.UpdateData(
                table: "Mark",
                keyColumn: "MarkId",
                keyValue: 1,
                column: "MarkEntryDate",
                value: new DateTime(2026, 3, 18, 3, 15, 2, 254, DateTimeKind.Local).AddTicks(6301));

            migrationBuilder.UpdateData(
                table: "Mark",
                keyColumn: "MarkId",
                keyValue: 2,
                column: "MarkEntryDate",
                value: new DateTime(2026, 3, 18, 3, 15, 2, 254, DateTimeKind.Local).AddTicks(6310));

            migrationBuilder.UpdateData(
                table: "Mark",
                keyColumn: "MarkId",
                keyValue: 3,
                column: "MarkEntryDate",
                value: new DateTime(2026, 3, 18, 3, 15, 2, 254, DateTimeKind.Local).AddTicks(6315));

            migrationBuilder.UpdateData(
                table: "Standard",
                keyColumn: "StandardId",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 18, 3, 15, 2, 254, DateTimeKind.Local).AddTicks(6531));

            migrationBuilder.UpdateData(
                table: "Standard",
                keyColumn: "StandardId",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 18, 3, 15, 2, 254, DateTimeKind.Local).AddTicks(6536));

            migrationBuilder.UpdateData(
                table: "Standard",
                keyColumn: "StandardId",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 18, 3, 15, 2, 254, DateTimeKind.Local).AddTicks(6540));

            migrationBuilder.UpdateData(
                table: "Standard",
                keyColumn: "StandardId",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 18, 3, 15, 2, 254, DateTimeKind.Local).AddTicks(6544));

            migrationBuilder.UpdateData(
                table: "Standard",
                keyColumn: "StandardId",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 18, 3, 15, 2, 254, DateTimeKind.Local).AddTicks(6547));

            migrationBuilder.UpdateData(
                table: "Standard",
                keyColumn: "StandardId",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 18, 3, 15, 2, 254, DateTimeKind.Local).AddTicks(6551));

            migrationBuilder.UpdateData(
                table: "Standard",
                keyColumn: "StandardId",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 18, 3, 15, 2, 254, DateTimeKind.Local).AddTicks(6554));

            migrationBuilder.UpdateData(
                table: "Standard",
                keyColumn: "StandardId",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 18, 3, 15, 2, 254, DateTimeKind.Local).AddTicks(6558));

            migrationBuilder.UpdateData(
                table: "Standard",
                keyColumn: "StandardId",
                keyValue: 9,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 18, 3, 15, 2, 254, DateTimeKind.Local).AddTicks(6561));

            migrationBuilder.UpdateData(
                table: "Standard",
                keyColumn: "StandardId",
                keyValue: 10,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 18, 3, 15, 2, 254, DateTimeKind.Local).AddTicks(6565));

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 18, 3, 15, 2, 254, DateTimeKind.Local).AddTicks(6656));

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 18, 3, 15, 2, 254, DateTimeKind.Local).AddTicks(6672));

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 18, 3, 15, 2, 254, DateTimeKind.Local).AddTicks(6683));

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 18, 3, 15, 2, 254, DateTimeKind.Local).AddTicks(6801));

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 18, 3, 15, 2, 254, DateTimeKind.Local).AddTicks(6813));

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 18, 3, 15, 2, 254, DateTimeKind.Local).AddTicks(6823));

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 18, 3, 15, 2, 254, DateTimeKind.Local).AddTicks(6830));

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 18, 3, 15, 2, 254, DateTimeKind.Local).AddTicks(6838));

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 9,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 18, 3, 15, 2, 254, DateTimeKind.Local).AddTicks(6845));

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 10,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 18, 3, 15, 2, 254, DateTimeKind.Local).AddTicks(6852));

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 11,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 18, 3, 15, 2, 254, DateTimeKind.Local).AddTicks(6860));

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 12,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 18, 3, 15, 2, 254, DateTimeKind.Local).AddTicks(6867));

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 13,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 18, 3, 15, 2, 254, DateTimeKind.Local).AddTicks(6890));
        }
    }
}
