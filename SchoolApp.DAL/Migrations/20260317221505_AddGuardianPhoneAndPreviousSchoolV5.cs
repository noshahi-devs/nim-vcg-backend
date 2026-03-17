using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolApp.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddGuardianPhoneAndPreviousSchoolV5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[Student]') AND name = N'GuardianPhone') ALTER TABLE [Student] ADD [GuardianPhone] nvarchar(max) NULL;");
            migrationBuilder.Sql("IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[Student]') AND name = N'PreviousSchool') ALTER TABLE [Student] ADD [PreviousSchool] nvarchar(max) NULL;");

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
                columns: new[] { "CreatedAt", "GuardianPhone", "PreviousSchool" },
                values: new object[] { new DateTime(2026, 3, 18, 3, 15, 2, 254, DateTimeKind.Local).AddTicks(6656), null, null });

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "GuardianPhone", "PreviousSchool" },
                values: new object[] { new DateTime(2026, 3, 18, 3, 15, 2, 254, DateTimeKind.Local).AddTicks(6672), null, null });

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 3,
                columns: new[] { "CreatedAt", "GuardianPhone", "PreviousSchool" },
                values: new object[] { new DateTime(2026, 3, 18, 3, 15, 2, 254, DateTimeKind.Local).AddTicks(6683), null, null });

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 4,
                columns: new[] { "CreatedAt", "GuardianPhone", "PreviousSchool" },
                values: new object[] { new DateTime(2026, 3, 18, 3, 15, 2, 254, DateTimeKind.Local).AddTicks(6801), null, null });

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 5,
                columns: new[] { "CreatedAt", "GuardianPhone", "PreviousSchool" },
                values: new object[] { new DateTime(2026, 3, 18, 3, 15, 2, 254, DateTimeKind.Local).AddTicks(6813), null, null });

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 6,
                columns: new[] { "CreatedAt", "GuardianPhone", "PreviousSchool" },
                values: new object[] { new DateTime(2026, 3, 18, 3, 15, 2, 254, DateTimeKind.Local).AddTicks(6823), null, null });

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 7,
                columns: new[] { "CreatedAt", "GuardianPhone", "PreviousSchool" },
                values: new object[] { new DateTime(2026, 3, 18, 3, 15, 2, 254, DateTimeKind.Local).AddTicks(6830), null, null });

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 8,
                columns: new[] { "CreatedAt", "GuardianPhone", "PreviousSchool" },
                values: new object[] { new DateTime(2026, 3, 18, 3, 15, 2, 254, DateTimeKind.Local).AddTicks(6838), null, null });

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 9,
                columns: new[] { "CreatedAt", "GuardianPhone", "PreviousSchool" },
                values: new object[] { new DateTime(2026, 3, 18, 3, 15, 2, 254, DateTimeKind.Local).AddTicks(6845), null, null });

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 10,
                columns: new[] { "CreatedAt", "GuardianPhone", "PreviousSchool" },
                values: new object[] { new DateTime(2026, 3, 18, 3, 15, 2, 254, DateTimeKind.Local).AddTicks(6852), null, null });

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 11,
                columns: new[] { "CreatedAt", "GuardianPhone", "PreviousSchool" },
                values: new object[] { new DateTime(2026, 3, 18, 3, 15, 2, 254, DateTimeKind.Local).AddTicks(6860), null, null });

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 12,
                columns: new[] { "CreatedAt", "GuardianPhone", "PreviousSchool" },
                values: new object[] { new DateTime(2026, 3, 18, 3, 15, 2, 254, DateTimeKind.Local).AddTicks(6867), null, null });

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 13,
                columns: new[] { "CreatedAt", "GuardianPhone", "PreviousSchool" },
                values: new object[] { new DateTime(2026, 3, 18, 3, 15, 2, 254, DateTimeKind.Local).AddTicks(6890), null, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GuardianPhone",
                table: "Student");

            migrationBuilder.DropColumn(
                name: "PreviousSchool",
                table: "Student");

            migrationBuilder.UpdateData(
                table: "Campus",
                keyColumn: "CampusId",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 18, 3, 11, 12, 433, DateTimeKind.Local).AddTicks(8627));

            migrationBuilder.UpdateData(
                table: "Mark",
                keyColumn: "MarkId",
                keyValue: 1,
                column: "MarkEntryDate",
                value: new DateTime(2026, 3, 18, 3, 11, 12, 434, DateTimeKind.Local).AddTicks(1005));

            migrationBuilder.UpdateData(
                table: "Mark",
                keyColumn: "MarkId",
                keyValue: 2,
                column: "MarkEntryDate",
                value: new DateTime(2026, 3, 18, 3, 11, 12, 434, DateTimeKind.Local).AddTicks(1011));

            migrationBuilder.UpdateData(
                table: "Mark",
                keyColumn: "MarkId",
                keyValue: 3,
                column: "MarkEntryDate",
                value: new DateTime(2026, 3, 18, 3, 11, 12, 434, DateTimeKind.Local).AddTicks(1016));

            migrationBuilder.UpdateData(
                table: "Standard",
                keyColumn: "StandardId",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 18, 3, 11, 12, 434, DateTimeKind.Local).AddTicks(1150));

            migrationBuilder.UpdateData(
                table: "Standard",
                keyColumn: "StandardId",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 18, 3, 11, 12, 434, DateTimeKind.Local).AddTicks(1153));

            migrationBuilder.UpdateData(
                table: "Standard",
                keyColumn: "StandardId",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 18, 3, 11, 12, 434, DateTimeKind.Local).AddTicks(1156));

            migrationBuilder.UpdateData(
                table: "Standard",
                keyColumn: "StandardId",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 18, 3, 11, 12, 434, DateTimeKind.Local).AddTicks(1159));

            migrationBuilder.UpdateData(
                table: "Standard",
                keyColumn: "StandardId",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 18, 3, 11, 12, 434, DateTimeKind.Local).AddTicks(1258));

            migrationBuilder.UpdateData(
                table: "Standard",
                keyColumn: "StandardId",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 18, 3, 11, 12, 434, DateTimeKind.Local).AddTicks(1262));

            migrationBuilder.UpdateData(
                table: "Standard",
                keyColumn: "StandardId",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 18, 3, 11, 12, 434, DateTimeKind.Local).AddTicks(1264));

            migrationBuilder.UpdateData(
                table: "Standard",
                keyColumn: "StandardId",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 18, 3, 11, 12, 434, DateTimeKind.Local).AddTicks(1266));

            migrationBuilder.UpdateData(
                table: "Standard",
                keyColumn: "StandardId",
                keyValue: 9,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 18, 3, 11, 12, 434, DateTimeKind.Local).AddTicks(1269));

            migrationBuilder.UpdateData(
                table: "Standard",
                keyColumn: "StandardId",
                keyValue: 10,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 18, 3, 11, 12, 434, DateTimeKind.Local).AddTicks(1272));

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 18, 3, 11, 12, 434, DateTimeKind.Local).AddTicks(1333));

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 18, 3, 11, 12, 434, DateTimeKind.Local).AddTicks(1347));

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 18, 3, 11, 12, 434, DateTimeKind.Local).AddTicks(1354));

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 18, 3, 11, 12, 434, DateTimeKind.Local).AddTicks(1361));

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 18, 3, 11, 12, 434, DateTimeKind.Local).AddTicks(1368));

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 18, 3, 11, 12, 434, DateTimeKind.Local).AddTicks(1374));

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 18, 3, 11, 12, 434, DateTimeKind.Local).AddTicks(1380));

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 18, 3, 11, 12, 434, DateTimeKind.Local).AddTicks(1386));

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 9,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 18, 3, 11, 12, 434, DateTimeKind.Local).AddTicks(1392));

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 10,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 18, 3, 11, 12, 434, DateTimeKind.Local).AddTicks(1398));

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 11,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 18, 3, 11, 12, 434, DateTimeKind.Local).AddTicks(1404));

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 12,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 18, 3, 11, 12, 434, DateTimeKind.Local).AddTicks(1410));

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 13,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 18, 3, 11, 12, 434, DateTimeKind.Local).AddTicks(1431));
        }
    }
}
