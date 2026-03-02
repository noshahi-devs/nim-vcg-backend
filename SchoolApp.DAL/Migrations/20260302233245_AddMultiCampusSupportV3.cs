using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolApp.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddMultiCampusSupportV3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Campus",
                table: "GeneralIncome");

            migrationBuilder.DropColumn(
                name: "Campus",
                table: "GeneralExpense");

            migrationBuilder.AddColumn<int>(
                name: "AcademicYearId",
                table: "Student",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CampusId",
                table: "Student",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CampusId",
                table: "Standard",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CampusId",
                table: "Staff",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CampusId",
                table: "Section",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StandardId",
                table: "Section",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CampusId",
                table: "OthersPayment",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CampusId",
                table: "MonthlyPayment",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CampusId",
                table: "GeneralIncome",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CampusId",
                table: "GeneralExpense",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AcademicYearId",
                table: "ExamSchedule",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CampusId",
                table: "ExamSchedule",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CampusId",
                table: "Department",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CampusId",
                table: "BankAccounts",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CampusId",
                table: "AcademicYear",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Campus",
                columns: table => new
                {
                    CampusId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CampusName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CampusCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContactNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Campus", x => x.CampusId);
                });

            migrationBuilder.UpdateData(
                table: "AcademicYear",
                keyColumn: "AcademicYearId",
                keyValue: 1,
                column: "CampusId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "AcademicYear",
                keyColumn: "AcademicYearId",
                keyValue: 2,
                column: "CampusId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "AcademicYear",
                keyColumn: "AcademicYearId",
                keyValue: 3,
                column: "CampusId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "AcademicYear",
                keyColumn: "AcademicYearId",
                keyValue: 4,
                column: "CampusId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "AcademicYear",
                keyColumn: "AcademicYearId",
                keyValue: 5,
                column: "CampusId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "AcademicYear",
                keyColumn: "AcademicYearId",
                keyValue: 6,
                column: "CampusId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "AcademicYear",
                keyColumn: "AcademicYearId",
                keyValue: 7,
                column: "CampusId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "AcademicYear",
                keyColumn: "AcademicYearId",
                keyValue: 8,
                column: "CampusId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "AcademicYear",
                keyColumn: "AcademicYearId",
                keyValue: 9,
                column: "CampusId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "AcademicYear",
                keyColumn: "AcademicYearId",
                keyValue: 10,
                column: "CampusId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "AcademicYear",
                keyColumn: "AcademicYearId",
                keyValue: 11,
                column: "CampusId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "AcademicYear",
                keyColumn: "AcademicYearId",
                keyValue: 12,
                column: "CampusId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "AcademicYear",
                keyColumn: "AcademicYearId",
                keyValue: 13,
                column: "CampusId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "AcademicYear",
                keyColumn: "AcademicYearId",
                keyValue: 14,
                column: "CampusId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "AcademicYear",
                keyColumn: "AcademicYearId",
                keyValue: 15,
                column: "CampusId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "AcademicYear",
                keyColumn: "AcademicYearId",
                keyValue: 16,
                column: "CampusId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "AcademicYear",
                keyColumn: "AcademicYearId",
                keyValue: 17,
                column: "CampusId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "AcademicYear",
                keyColumn: "AcademicYearId",
                keyValue: 18,
                column: "CampusId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "AcademicYear",
                keyColumn: "AcademicYearId",
                keyValue: 19,
                column: "CampusId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "AcademicYear",
                keyColumn: "AcademicYearId",
                keyValue: 20,
                column: "CampusId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "AcademicYear",
                keyColumn: "AcademicYearId",
                keyValue: 21,
                column: "CampusId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "AcademicYear",
                keyColumn: "AcademicYearId",
                keyValue: 22,
                column: "CampusId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "AcademicYear",
                keyColumn: "AcademicYearId",
                keyValue: 23,
                column: "CampusId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "AcademicYear",
                keyColumn: "AcademicYearId",
                keyValue: 24,
                column: "CampusId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "AcademicYear",
                keyColumn: "AcademicYearId",
                keyValue: 25,
                column: "CampusId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "AcademicYear",
                keyColumn: "AcademicYearId",
                keyValue: 26,
                column: "CampusId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "AcademicYear",
                keyColumn: "AcademicYearId",
                keyValue: 27,
                column: "CampusId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "AcademicYear",
                keyColumn: "AcademicYearId",
                keyValue: 28,
                column: "CampusId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "AcademicYear",
                keyColumn: "AcademicYearId",
                keyValue: 29,
                column: "CampusId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "AcademicYear",
                keyColumn: "AcademicYearId",
                keyValue: 30,
                column: "CampusId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "AcademicYear",
                keyColumn: "AcademicYearId",
                keyValue: 31,
                column: "CampusId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "AcademicYear",
                keyColumn: "AcademicYearId",
                keyValue: 32,
                column: "CampusId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "AcademicYear",
                keyColumn: "AcademicYearId",
                keyValue: 33,
                column: "CampusId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "AcademicYear",
                keyColumn: "AcademicYearId",
                keyValue: 34,
                column: "CampusId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "AcademicYear",
                keyColumn: "AcademicYearId",
                keyValue: 35,
                column: "CampusId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "AcademicYear",
                keyColumn: "AcademicYearId",
                keyValue: 36,
                column: "CampusId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "AcademicYear",
                keyColumn: "AcademicYearId",
                keyValue: 37,
                column: "CampusId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "AcademicYear",
                keyColumn: "AcademicYearId",
                keyValue: 38,
                column: "CampusId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "AcademicYear",
                keyColumn: "AcademicYearId",
                keyValue: 39,
                column: "CampusId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "AcademicYear",
                keyColumn: "AcademicYearId",
                keyValue: 40,
                column: "CampusId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "AcademicYear",
                keyColumn: "AcademicYearId",
                keyValue: 41,
                column: "CampusId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "AcademicYear",
                keyColumn: "AcademicYearId",
                keyValue: 42,
                column: "CampusId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "AcademicYear",
                keyColumn: "AcademicYearId",
                keyValue: 43,
                column: "CampusId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "AcademicYear",
                keyColumn: "AcademicYearId",
                keyValue: 44,
                column: "CampusId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "AcademicYear",
                keyColumn: "AcademicYearId",
                keyValue: 45,
                column: "CampusId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "AcademicYear",
                keyColumn: "AcademicYearId",
                keyValue: 46,
                column: "CampusId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "AcademicYear",
                keyColumn: "AcademicYearId",
                keyValue: 47,
                column: "CampusId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "AcademicYear",
                keyColumn: "AcademicYearId",
                keyValue: 48,
                column: "CampusId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "AcademicYear",
                keyColumn: "AcademicYearId",
                keyValue: 49,
                column: "CampusId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "AcademicYear",
                keyColumn: "AcademicYearId",
                keyValue: 50,
                column: "CampusId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "AcademicYear",
                keyColumn: "AcademicYearId",
                keyValue: 51,
                column: "CampusId",
                value: 1);

            migrationBuilder.InsertData(
                table: "Campus",
                columns: new[] { "CampusId", "Address", "CampusCode", "CampusName", "ContactNumber", "CreatedAt", "Email", "IsActive" },
                values: new object[] { 1, null, "MAIN", "Main Campus", null, new DateTime(2026, 3, 3, 4, 32, 39, 307, DateTimeKind.Local).AddTicks(805), null, true });

            migrationBuilder.UpdateData(
                table: "Department",
                keyColumn: "DepartmentId",
                keyValue: 1,
                column: "CampusId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Department",
                keyColumn: "DepartmentId",
                keyValue: 2,
                column: "CampusId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Department",
                keyColumn: "DepartmentId",
                keyValue: 3,
                column: "CampusId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Department",
                keyColumn: "DepartmentId",
                keyValue: 4,
                column: "CampusId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Department",
                keyColumn: "DepartmentId",
                keyValue: 5,
                column: "CampusId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Department",
                keyColumn: "DepartmentId",
                keyValue: 6,
                column: "CampusId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Department",
                keyColumn: "DepartmentId",
                keyValue: 7,
                column: "CampusId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Department",
                keyColumn: "DepartmentId",
                keyValue: 8,
                column: "CampusId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "ExamSchedule",
                keyColumn: "ExamScheduleId",
                keyValue: 1,
                columns: new[] { "AcademicYearId", "CampusId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "ExamSchedule",
                keyColumn: "ExamScheduleId",
                keyValue: 2,
                columns: new[] { "AcademicYearId", "CampusId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "ExamSchedule",
                keyColumn: "ExamScheduleId",
                keyValue: 3,
                columns: new[] { "AcademicYearId", "CampusId" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Mark",
                keyColumn: "MarkId",
                keyValue: 1,
                column: "MarkEntryDate",
                value: new DateTime(2026, 3, 3, 4, 32, 39, 307, DateTimeKind.Local).AddTicks(2560));

            migrationBuilder.UpdateData(
                table: "Mark",
                keyColumn: "MarkId",
                keyValue: 2,
                column: "MarkEntryDate",
                value: new DateTime(2026, 3, 3, 4, 32, 39, 307, DateTimeKind.Local).AddTicks(2565));

            migrationBuilder.UpdateData(
                table: "Mark",
                keyColumn: "MarkId",
                keyValue: 3,
                column: "MarkEntryDate",
                value: new DateTime(2026, 3, 3, 4, 32, 39, 307, DateTimeKind.Local).AddTicks(2568));

            migrationBuilder.UpdateData(
                table: "Staff",
                keyColumn: "StaffId",
                keyValue: 1,
                column: "CampusId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Staff",
                keyColumn: "StaffId",
                keyValue: 2,
                column: "CampusId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Staff",
                keyColumn: "StaffId",
                keyValue: 3,
                column: "CampusId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Standard",
                keyColumn: "StandardId",
                keyValue: 1,
                columns: new[] { "CampusId", "CreatedAt" },
                values: new object[] { null, new DateTime(2026, 3, 3, 4, 32, 39, 307, DateTimeKind.Local).AddTicks(2667) });

            migrationBuilder.UpdateData(
                table: "Standard",
                keyColumn: "StandardId",
                keyValue: 2,
                columns: new[] { "CampusId", "CreatedAt" },
                values: new object[] { null, new DateTime(2026, 3, 3, 4, 32, 39, 307, DateTimeKind.Local).AddTicks(2670) });

            migrationBuilder.UpdateData(
                table: "Standard",
                keyColumn: "StandardId",
                keyValue: 3,
                columns: new[] { "CampusId", "CreatedAt" },
                values: new object[] { null, new DateTime(2026, 3, 3, 4, 32, 39, 307, DateTimeKind.Local).AddTicks(2672) });

            migrationBuilder.UpdateData(
                table: "Standard",
                keyColumn: "StandardId",
                keyValue: 4,
                columns: new[] { "CampusId", "CreatedAt" },
                values: new object[] { null, new DateTime(2026, 3, 3, 4, 32, 39, 307, DateTimeKind.Local).AddTicks(2674) });

            migrationBuilder.UpdateData(
                table: "Standard",
                keyColumn: "StandardId",
                keyValue: 5,
                columns: new[] { "CampusId", "CreatedAt" },
                values: new object[] { null, new DateTime(2026, 3, 3, 4, 32, 39, 307, DateTimeKind.Local).AddTicks(2675) });

            migrationBuilder.UpdateData(
                table: "Standard",
                keyColumn: "StandardId",
                keyValue: 6,
                columns: new[] { "CampusId", "CreatedAt" },
                values: new object[] { null, new DateTime(2026, 3, 3, 4, 32, 39, 307, DateTimeKind.Local).AddTicks(2677) });

            migrationBuilder.UpdateData(
                table: "Standard",
                keyColumn: "StandardId",
                keyValue: 7,
                columns: new[] { "CampusId", "CreatedAt" },
                values: new object[] { null, new DateTime(2026, 3, 3, 4, 32, 39, 307, DateTimeKind.Local).AddTicks(2679) });

            migrationBuilder.UpdateData(
                table: "Standard",
                keyColumn: "StandardId",
                keyValue: 8,
                columns: new[] { "CampusId", "CreatedAt" },
                values: new object[] { null, new DateTime(2026, 3, 3, 4, 32, 39, 307, DateTimeKind.Local).AddTicks(2680) });

            migrationBuilder.UpdateData(
                table: "Standard",
                keyColumn: "StandardId",
                keyValue: 9,
                columns: new[] { "CampusId", "CreatedAt" },
                values: new object[] { null, new DateTime(2026, 3, 3, 4, 32, 39, 307, DateTimeKind.Local).AddTicks(2682) });

            migrationBuilder.UpdateData(
                table: "Standard",
                keyColumn: "StandardId",
                keyValue: 10,
                columns: new[] { "CampusId", "CreatedAt" },
                values: new object[] { null, new DateTime(2026, 3, 3, 4, 32, 39, 307, DateTimeKind.Local).AddTicks(2683) });

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 1,
                columns: new[] { "AcademicYearId", "CampusId", "CreatedAt" },
                values: new object[] { null, null, new DateTime(2026, 3, 3, 4, 32, 39, 307, DateTimeKind.Local).AddTicks(2720) });

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 2,
                columns: new[] { "AcademicYearId", "CampusId", "CreatedAt" },
                values: new object[] { null, null, new DateTime(2026, 3, 3, 4, 32, 39, 307, DateTimeKind.Local).AddTicks(2733) });

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 3,
                columns: new[] { "AcademicYearId", "CampusId", "CreatedAt" },
                values: new object[] { null, null, new DateTime(2026, 3, 3, 4, 32, 39, 307, DateTimeKind.Local).AddTicks(2848) });

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 4,
                columns: new[] { "AcademicYearId", "CampusId", "CreatedAt" },
                values: new object[] { null, null, new DateTime(2026, 3, 3, 4, 32, 39, 307, DateTimeKind.Local).AddTicks(2854) });

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 5,
                columns: new[] { "AcademicYearId", "CampusId", "CreatedAt" },
                values: new object[] { null, null, new DateTime(2026, 3, 3, 4, 32, 39, 307, DateTimeKind.Local).AddTicks(2858) });

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 6,
                columns: new[] { "AcademicYearId", "CampusId", "CreatedAt" },
                values: new object[] { null, null, new DateTime(2026, 3, 3, 4, 32, 39, 307, DateTimeKind.Local).AddTicks(2862) });

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 7,
                columns: new[] { "AcademicYearId", "CampusId", "CreatedAt" },
                values: new object[] { null, null, new DateTime(2026, 3, 3, 4, 32, 39, 307, DateTimeKind.Local).AddTicks(2866) });

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 8,
                columns: new[] { "AcademicYearId", "CampusId", "CreatedAt" },
                values: new object[] { null, null, new DateTime(2026, 3, 3, 4, 32, 39, 307, DateTimeKind.Local).AddTicks(2870) });

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 9,
                columns: new[] { "AcademicYearId", "CampusId", "CreatedAt" },
                values: new object[] { null, null, new DateTime(2026, 3, 3, 4, 32, 39, 307, DateTimeKind.Local).AddTicks(2875) });

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 10,
                columns: new[] { "AcademicYearId", "CampusId", "CreatedAt" },
                values: new object[] { null, null, new DateTime(2026, 3, 3, 4, 32, 39, 307, DateTimeKind.Local).AddTicks(2880) });

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 11,
                columns: new[] { "AcademicYearId", "CampusId", "CreatedAt" },
                values: new object[] { null, null, new DateTime(2026, 3, 3, 4, 32, 39, 307, DateTimeKind.Local).AddTicks(2884) });

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 12,
                columns: new[] { "AcademicYearId", "CampusId", "CreatedAt" },
                values: new object[] { null, null, new DateTime(2026, 3, 3, 4, 32, 39, 307, DateTimeKind.Local).AddTicks(2888) });

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 13,
                columns: new[] { "AcademicYearId", "CampusId", "CreatedAt" },
                values: new object[] { null, null, new DateTime(2026, 3, 3, 4, 32, 39, 307, DateTimeKind.Local).AddTicks(2900) });

            migrationBuilder.CreateIndex(
                name: "IX_Student_AcademicYearId",
                table: "Student",
                column: "AcademicYearId");

            migrationBuilder.CreateIndex(
                name: "IX_Student_CampusId",
                table: "Student",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_Standard_CampusId",
                table: "Standard",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_Staff_CampusId",
                table: "Staff",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_Section_CampusId",
                table: "Section",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_Section_StandardId",
                table: "Section",
                column: "StandardId");

            migrationBuilder.CreateIndex(
                name: "IX_OthersPayment_CampusId",
                table: "OthersPayment",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_MonthlyPayment_CampusId",
                table: "MonthlyPayment",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_GeneralIncome_CampusId",
                table: "GeneralIncome",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_GeneralExpense_CampusId",
                table: "GeneralExpense",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_ExamSchedule_AcademicYearId",
                table: "ExamSchedule",
                column: "AcademicYearId");

            migrationBuilder.CreateIndex(
                name: "IX_ExamSchedule_CampusId",
                table: "ExamSchedule",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_Department_CampusId",
                table: "Department",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_BankAccounts_CampusId",
                table: "BankAccounts",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_AcademicYear_CampusId",
                table: "AcademicYear",
                column: "CampusId");

            migrationBuilder.AddForeignKey(
                name: "FK_AcademicYear_Campus_CampusId",
                table: "AcademicYear",
                column: "CampusId",
                principalTable: "Campus",
                principalColumn: "CampusId");

            migrationBuilder.AddForeignKey(
                name: "FK_BankAccounts_Campus_CampusId",
                table: "BankAccounts",
                column: "CampusId",
                principalTable: "Campus",
                principalColumn: "CampusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Department_Campus_CampusId",
                table: "Department",
                column: "CampusId",
                principalTable: "Campus",
                principalColumn: "CampusId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExamSchedule_AcademicYear_AcademicYearId",
                table: "ExamSchedule",
                column: "AcademicYearId",
                principalTable: "AcademicYear",
                principalColumn: "AcademicYearId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExamSchedule_Campus_CampusId",
                table: "ExamSchedule",
                column: "CampusId",
                principalTable: "Campus",
                principalColumn: "CampusId");

            migrationBuilder.AddForeignKey(
                name: "FK_GeneralExpense_Campus_CampusId",
                table: "GeneralExpense",
                column: "CampusId",
                principalTable: "Campus",
                principalColumn: "CampusId");

            migrationBuilder.AddForeignKey(
                name: "FK_GeneralIncome_Campus_CampusId",
                table: "GeneralIncome",
                column: "CampusId",
                principalTable: "Campus",
                principalColumn: "CampusId");

            migrationBuilder.AddForeignKey(
                name: "FK_MonthlyPayment_Campus_CampusId",
                table: "MonthlyPayment",
                column: "CampusId",
                principalTable: "Campus",
                principalColumn: "CampusId");

            migrationBuilder.AddForeignKey(
                name: "FK_OthersPayment_Campus_CampusId",
                table: "OthersPayment",
                column: "CampusId",
                principalTable: "Campus",
                principalColumn: "CampusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Section_Campus_CampusId",
                table: "Section",
                column: "CampusId",
                principalTable: "Campus",
                principalColumn: "CampusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Section_Standard_StandardId",
                table: "Section",
                column: "StandardId",
                principalTable: "Standard",
                principalColumn: "StandardId");

            migrationBuilder.AddForeignKey(
                name: "FK_Staff_Campus_CampusId",
                table: "Staff",
                column: "CampusId",
                principalTable: "Campus",
                principalColumn: "CampusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Standard_Campus_CampusId",
                table: "Standard",
                column: "CampusId",
                principalTable: "Campus",
                principalColumn: "CampusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Student_AcademicYear_AcademicYearId",
                table: "Student",
                column: "AcademicYearId",
                principalTable: "AcademicYear",
                principalColumn: "AcademicYearId");

            migrationBuilder.AddForeignKey(
                name: "FK_Student_Campus_CampusId",
                table: "Student",
                column: "CampusId",
                principalTable: "Campus",
                principalColumn: "CampusId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AcademicYear_Campus_CampusId",
                table: "AcademicYear");

            migrationBuilder.DropForeignKey(
                name: "FK_BankAccounts_Campus_CampusId",
                table: "BankAccounts");

            migrationBuilder.DropForeignKey(
                name: "FK_Department_Campus_CampusId",
                table: "Department");

            migrationBuilder.DropForeignKey(
                name: "FK_ExamSchedule_AcademicYear_AcademicYearId",
                table: "ExamSchedule");

            migrationBuilder.DropForeignKey(
                name: "FK_ExamSchedule_Campus_CampusId",
                table: "ExamSchedule");

            migrationBuilder.DropForeignKey(
                name: "FK_GeneralExpense_Campus_CampusId",
                table: "GeneralExpense");

            migrationBuilder.DropForeignKey(
                name: "FK_GeneralIncome_Campus_CampusId",
                table: "GeneralIncome");

            migrationBuilder.DropForeignKey(
                name: "FK_MonthlyPayment_Campus_CampusId",
                table: "MonthlyPayment");

            migrationBuilder.DropForeignKey(
                name: "FK_OthersPayment_Campus_CampusId",
                table: "OthersPayment");

            migrationBuilder.DropForeignKey(
                name: "FK_Section_Campus_CampusId",
                table: "Section");

            migrationBuilder.DropForeignKey(
                name: "FK_Section_Standard_StandardId",
                table: "Section");

            migrationBuilder.DropForeignKey(
                name: "FK_Staff_Campus_CampusId",
                table: "Staff");

            migrationBuilder.DropForeignKey(
                name: "FK_Standard_Campus_CampusId",
                table: "Standard");

            migrationBuilder.DropForeignKey(
                name: "FK_Student_AcademicYear_AcademicYearId",
                table: "Student");

            migrationBuilder.DropForeignKey(
                name: "FK_Student_Campus_CampusId",
                table: "Student");

            migrationBuilder.DropTable(
                name: "Campus");

            migrationBuilder.DropIndex(
                name: "IX_Student_AcademicYearId",
                table: "Student");

            migrationBuilder.DropIndex(
                name: "IX_Student_CampusId",
                table: "Student");

            migrationBuilder.DropIndex(
                name: "IX_Standard_CampusId",
                table: "Standard");

            migrationBuilder.DropIndex(
                name: "IX_Staff_CampusId",
                table: "Staff");

            migrationBuilder.DropIndex(
                name: "IX_Section_CampusId",
                table: "Section");

            migrationBuilder.DropIndex(
                name: "IX_Section_StandardId",
                table: "Section");

            migrationBuilder.DropIndex(
                name: "IX_OthersPayment_CampusId",
                table: "OthersPayment");

            migrationBuilder.DropIndex(
                name: "IX_MonthlyPayment_CampusId",
                table: "MonthlyPayment");

            migrationBuilder.DropIndex(
                name: "IX_GeneralIncome_CampusId",
                table: "GeneralIncome");

            migrationBuilder.DropIndex(
                name: "IX_GeneralExpense_CampusId",
                table: "GeneralExpense");

            migrationBuilder.DropIndex(
                name: "IX_ExamSchedule_AcademicYearId",
                table: "ExamSchedule");

            migrationBuilder.DropIndex(
                name: "IX_ExamSchedule_CampusId",
                table: "ExamSchedule");

            migrationBuilder.DropIndex(
                name: "IX_Department_CampusId",
                table: "Department");

            migrationBuilder.DropIndex(
                name: "IX_BankAccounts_CampusId",
                table: "BankAccounts");

            migrationBuilder.DropIndex(
                name: "IX_AcademicYear_CampusId",
                table: "AcademicYear");

            migrationBuilder.DropColumn(
                name: "AcademicYearId",
                table: "Student");

            migrationBuilder.DropColumn(
                name: "CampusId",
                table: "Student");

            migrationBuilder.DropColumn(
                name: "CampusId",
                table: "Standard");

            migrationBuilder.DropColumn(
                name: "CampusId",
                table: "Staff");

            migrationBuilder.DropColumn(
                name: "CampusId",
                table: "Section");

            migrationBuilder.DropColumn(
                name: "StandardId",
                table: "Section");

            migrationBuilder.DropColumn(
                name: "CampusId",
                table: "OthersPayment");

            migrationBuilder.DropColumn(
                name: "CampusId",
                table: "MonthlyPayment");

            migrationBuilder.DropColumn(
                name: "CampusId",
                table: "GeneralIncome");

            migrationBuilder.DropColumn(
                name: "CampusId",
                table: "GeneralExpense");

            migrationBuilder.DropColumn(
                name: "AcademicYearId",
                table: "ExamSchedule");

            migrationBuilder.DropColumn(
                name: "CampusId",
                table: "ExamSchedule");

            migrationBuilder.DropColumn(
                name: "CampusId",
                table: "Department");

            migrationBuilder.DropColumn(
                name: "CampusId",
                table: "BankAccounts");

            migrationBuilder.DropColumn(
                name: "CampusId",
                table: "AcademicYear");

            migrationBuilder.AddColumn<string>(
                name: "Campus",
                table: "GeneralIncome",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Campus",
                table: "GeneralExpense",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Mark",
                keyColumn: "MarkId",
                keyValue: 1,
                column: "MarkEntryDate",
                value: new DateTime(2026, 3, 3, 1, 50, 49, 305, DateTimeKind.Local).AddTicks(1917));

            migrationBuilder.UpdateData(
                table: "Mark",
                keyColumn: "MarkId",
                keyValue: 2,
                column: "MarkEntryDate",
                value: new DateTime(2026, 3, 3, 1, 50, 49, 305, DateTimeKind.Local).AddTicks(1926));

            migrationBuilder.UpdateData(
                table: "Mark",
                keyColumn: "MarkId",
                keyValue: 3,
                column: "MarkEntryDate",
                value: new DateTime(2026, 3, 3, 1, 50, 49, 305, DateTimeKind.Local).AddTicks(1933));

            migrationBuilder.UpdateData(
                table: "Standard",
                keyColumn: "StandardId",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 3, 1, 50, 49, 305, DateTimeKind.Local).AddTicks(2160));

            migrationBuilder.UpdateData(
                table: "Standard",
                keyColumn: "StandardId",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 3, 1, 50, 49, 305, DateTimeKind.Local).AddTicks(2166));

            migrationBuilder.UpdateData(
                table: "Standard",
                keyColumn: "StandardId",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 3, 1, 50, 49, 305, DateTimeKind.Local).AddTicks(2171));

            migrationBuilder.UpdateData(
                table: "Standard",
                keyColumn: "StandardId",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 3, 1, 50, 49, 305, DateTimeKind.Local).AddTicks(2174));

            migrationBuilder.UpdateData(
                table: "Standard",
                keyColumn: "StandardId",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 3, 1, 50, 49, 305, DateTimeKind.Local).AddTicks(2177));

            migrationBuilder.UpdateData(
                table: "Standard",
                keyColumn: "StandardId",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 3, 1, 50, 49, 305, DateTimeKind.Local).AddTicks(2181));

            migrationBuilder.UpdateData(
                table: "Standard",
                keyColumn: "StandardId",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 3, 1, 50, 49, 305, DateTimeKind.Local).AddTicks(2185));

            migrationBuilder.UpdateData(
                table: "Standard",
                keyColumn: "StandardId",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 3, 1, 50, 49, 305, DateTimeKind.Local).AddTicks(2188));

            migrationBuilder.UpdateData(
                table: "Standard",
                keyColumn: "StandardId",
                keyValue: 9,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 3, 1, 50, 49, 305, DateTimeKind.Local).AddTicks(2191));

            migrationBuilder.UpdateData(
                table: "Standard",
                keyColumn: "StandardId",
                keyValue: 10,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 3, 1, 50, 49, 305, DateTimeKind.Local).AddTicks(2194));

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 3, 1, 50, 49, 305, DateTimeKind.Local).AddTicks(2280));

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 3, 1, 50, 49, 305, DateTimeKind.Local).AddTicks(2430));

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 3, 1, 50, 49, 305, DateTimeKind.Local).AddTicks(2446));

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 3, 1, 50, 49, 305, DateTimeKind.Local).AddTicks(2454));

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 3, 1, 50, 49, 305, DateTimeKind.Local).AddTicks(2463));

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 3, 1, 50, 49, 305, DateTimeKind.Local).AddTicks(2473));

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 3, 1, 50, 49, 305, DateTimeKind.Local).AddTicks(2481));

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 3, 1, 50, 49, 305, DateTimeKind.Local).AddTicks(2489));

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 9,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 3, 1, 50, 49, 305, DateTimeKind.Local).AddTicks(2497));

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 10,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 3, 1, 50, 49, 305, DateTimeKind.Local).AddTicks(2505));

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 11,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 3, 1, 50, 49, 305, DateTimeKind.Local).AddTicks(2529));

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 12,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 3, 1, 50, 49, 305, DateTimeKind.Local).AddTicks(2538));

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 13,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 3, 1, 50, 49, 305, DateTimeKind.Local).AddTicks(2548));
        }
    }
}
