using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolApp.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddAdmissionDateColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "AdmissionDate",
                table: "Student",
                type: "datetime2",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Campus",
                keyColumn: "CampusId",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 17, 3, 34, 51, 745, DateTimeKind.Local).AddTicks(3259));

            migrationBuilder.UpdateData(
                table: "Mark",
                keyColumn: "MarkId",
                keyValue: 1,
                column: "MarkEntryDate",
                value: new DateTime(2026, 3, 17, 3, 34, 51, 745, DateTimeKind.Local).AddTicks(6593));

            migrationBuilder.UpdateData(
                table: "Mark",
                keyColumn: "MarkId",
                keyValue: 2,
                column: "MarkEntryDate",
                value: new DateTime(2026, 3, 17, 3, 34, 51, 745, DateTimeKind.Local).AddTicks(6600));

            migrationBuilder.UpdateData(
                table: "Mark",
                keyColumn: "MarkId",
                keyValue: 3,
                column: "MarkEntryDate",
                value: new DateTime(2026, 3, 17, 3, 34, 51, 745, DateTimeKind.Local).AddTicks(6604));

            migrationBuilder.UpdateData(
                table: "Standard",
                keyColumn: "StandardId",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 17, 3, 34, 51, 745, DateTimeKind.Local).AddTicks(8287));

            migrationBuilder.UpdateData(
                table: "Standard",
                keyColumn: "StandardId",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 17, 3, 34, 51, 745, DateTimeKind.Local).AddTicks(8298));

            migrationBuilder.UpdateData(
                table: "Standard",
                keyColumn: "StandardId",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 17, 3, 34, 51, 745, DateTimeKind.Local).AddTicks(8301));

            migrationBuilder.UpdateData(
                table: "Standard",
                keyColumn: "StandardId",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 17, 3, 34, 51, 745, DateTimeKind.Local).AddTicks(8304));

            migrationBuilder.UpdateData(
                table: "Standard",
                keyColumn: "StandardId",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 17, 3, 34, 51, 745, DateTimeKind.Local).AddTicks(8307));

            migrationBuilder.UpdateData(
                table: "Standard",
                keyColumn: "StandardId",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 17, 3, 34, 51, 745, DateTimeKind.Local).AddTicks(8309));

            migrationBuilder.UpdateData(
                table: "Standard",
                keyColumn: "StandardId",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 17, 3, 34, 51, 745, DateTimeKind.Local).AddTicks(8312));

            migrationBuilder.UpdateData(
                table: "Standard",
                keyColumn: "StandardId",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 17, 3, 34, 51, 745, DateTimeKind.Local).AddTicks(8315));

            migrationBuilder.UpdateData(
                table: "Standard",
                keyColumn: "StandardId",
                keyValue: 9,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 17, 3, 34, 51, 745, DateTimeKind.Local).AddTicks(8318));

            migrationBuilder.UpdateData(
                table: "Standard",
                keyColumn: "StandardId",
                keyValue: 10,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 17, 3, 34, 51, 745, DateTimeKind.Local).AddTicks(8321));

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 1,
                columns: new[] { "AdmissionDate", "CreatedAt" },
                values: new object[] { null, new DateTime(2026, 3, 17, 3, 34, 51, 745, DateTimeKind.Local).AddTicks(8559) });

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 2,
                columns: new[] { "AdmissionDate", "CreatedAt" },
                values: new object[] { null, new DateTime(2026, 3, 17, 3, 34, 51, 745, DateTimeKind.Local).AddTicks(8573) });

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 3,
                columns: new[] { "AdmissionDate", "CreatedAt" },
                values: new object[] { null, new DateTime(2026, 3, 17, 3, 34, 51, 745, DateTimeKind.Local).AddTicks(8580) });

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 4,
                columns: new[] { "AdmissionDate", "CreatedAt" },
                values: new object[] { null, new DateTime(2026, 3, 17, 3, 34, 51, 745, DateTimeKind.Local).AddTicks(8586) });

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 5,
                columns: new[] { "AdmissionDate", "CreatedAt" },
                values: new object[] { null, new DateTime(2026, 3, 17, 3, 34, 51, 745, DateTimeKind.Local).AddTicks(8593) });

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 6,
                columns: new[] { "AdmissionDate", "CreatedAt" },
                values: new object[] { null, new DateTime(2026, 3, 17, 3, 34, 51, 745, DateTimeKind.Local).AddTicks(8600) });

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 7,
                columns: new[] { "AdmissionDate", "CreatedAt" },
                values: new object[] { null, new DateTime(2026, 3, 17, 3, 34, 51, 745, DateTimeKind.Local).AddTicks(8606) });

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 8,
                columns: new[] { "AdmissionDate", "CreatedAt" },
                values: new object[] { null, new DateTime(2026, 3, 17, 3, 34, 51, 745, DateTimeKind.Local).AddTicks(8612) });

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 9,
                columns: new[] { "AdmissionDate", "CreatedAt" },
                values: new object[] { null, new DateTime(2026, 3, 17, 3, 34, 51, 745, DateTimeKind.Local).AddTicks(8618) });

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 10,
                columns: new[] { "AdmissionDate", "CreatedAt" },
                values: new object[] { null, new DateTime(2026, 3, 17, 3, 34, 51, 745, DateTimeKind.Local).AddTicks(8623) });

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 11,
                columns: new[] { "AdmissionDate", "CreatedAt" },
                values: new object[] { null, new DateTime(2026, 3, 17, 3, 34, 51, 745, DateTimeKind.Local).AddTicks(8628) });

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 12,
                columns: new[] { "AdmissionDate", "CreatedAt" },
                values: new object[] { null, new DateTime(2026, 3, 17, 3, 34, 51, 745, DateTimeKind.Local).AddTicks(8635) });

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 13,
                columns: new[] { "AdmissionDate", "CreatedAt" },
                values: new object[] { null, new DateTime(2026, 3, 17, 3, 34, 51, 745, DateTimeKind.Local).AddTicks(8658) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdmissionDate",
                table: "Student");

            migrationBuilder.UpdateData(
                table: "Campus",
                keyColumn: "CampusId",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 12, 7, 17, 44, 935, DateTimeKind.Local).AddTicks(3367));

            migrationBuilder.UpdateData(
                table: "Mark",
                keyColumn: "MarkId",
                keyValue: 1,
                column: "MarkEntryDate",
                value: new DateTime(2026, 3, 12, 7, 17, 44, 935, DateTimeKind.Local).AddTicks(5960));

            migrationBuilder.UpdateData(
                table: "Mark",
                keyColumn: "MarkId",
                keyValue: 2,
                column: "MarkEntryDate",
                value: new DateTime(2026, 3, 12, 7, 17, 44, 935, DateTimeKind.Local).AddTicks(5967));

            migrationBuilder.UpdateData(
                table: "Mark",
                keyColumn: "MarkId",
                keyValue: 3,
                column: "MarkEntryDate",
                value: new DateTime(2026, 3, 12, 7, 17, 44, 935, DateTimeKind.Local).AddTicks(5972));

            migrationBuilder.UpdateData(
                table: "Standard",
                keyColumn: "StandardId",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 12, 7, 17, 44, 935, DateTimeKind.Local).AddTicks(6262));

            migrationBuilder.UpdateData(
                table: "Standard",
                keyColumn: "StandardId",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 12, 7, 17, 44, 935, DateTimeKind.Local).AddTicks(6267));

            migrationBuilder.UpdateData(
                table: "Standard",
                keyColumn: "StandardId",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 12, 7, 17, 44, 935, DateTimeKind.Local).AddTicks(6270));

            migrationBuilder.UpdateData(
                table: "Standard",
                keyColumn: "StandardId",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 12, 7, 17, 44, 935, DateTimeKind.Local).AddTicks(6273));

            migrationBuilder.UpdateData(
                table: "Standard",
                keyColumn: "StandardId",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 12, 7, 17, 44, 935, DateTimeKind.Local).AddTicks(6276));

            migrationBuilder.UpdateData(
                table: "Standard",
                keyColumn: "StandardId",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 12, 7, 17, 44, 935, DateTimeKind.Local).AddTicks(6278));

            migrationBuilder.UpdateData(
                table: "Standard",
                keyColumn: "StandardId",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 12, 7, 17, 44, 935, DateTimeKind.Local).AddTicks(6281));

            migrationBuilder.UpdateData(
                table: "Standard",
                keyColumn: "StandardId",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 12, 7, 17, 44, 935, DateTimeKind.Local).AddTicks(6284));

            migrationBuilder.UpdateData(
                table: "Standard",
                keyColumn: "StandardId",
                keyValue: 9,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 12, 7, 17, 44, 935, DateTimeKind.Local).AddTicks(6286));

            migrationBuilder.UpdateData(
                table: "Standard",
                keyColumn: "StandardId",
                keyValue: 10,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 12, 7, 17, 44, 935, DateTimeKind.Local).AddTicks(6289));

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 12, 7, 17, 44, 935, DateTimeKind.Local).AddTicks(6350));

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 12, 7, 17, 44, 935, DateTimeKind.Local).AddTicks(6364));

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 12, 7, 17, 44, 935, DateTimeKind.Local).AddTicks(6373));

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 12, 7, 17, 44, 935, DateTimeKind.Local).AddTicks(6379));

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 12, 7, 17, 44, 935, DateTimeKind.Local).AddTicks(6386));

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 12, 7, 17, 44, 935, DateTimeKind.Local).AddTicks(6392));

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 12, 7, 17, 44, 935, DateTimeKind.Local).AddTicks(6400));

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 12, 7, 17, 44, 935, DateTimeKind.Local).AddTicks(6409));

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 9,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 12, 7, 17, 44, 935, DateTimeKind.Local).AddTicks(6416));

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 10,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 12, 7, 17, 44, 935, DateTimeKind.Local).AddTicks(6422));

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 11,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 12, 7, 17, 44, 935, DateTimeKind.Local).AddTicks(6432));

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 12,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 12, 7, 17, 44, 935, DateTimeKind.Local).AddTicks(6440));

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 13,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 12, 7, 17, 44, 935, DateTimeKind.Local).AddTicks(6463));
        }
    }
}
