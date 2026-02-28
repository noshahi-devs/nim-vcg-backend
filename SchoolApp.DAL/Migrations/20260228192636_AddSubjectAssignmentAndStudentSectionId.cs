using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolApp.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddSubjectAssignmentAndStudentSectionId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Grade",
                table: "StudentMarksDetails",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SectionId",
                table: "Student",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SubjectAssignment",
                columns: table => new
                {
                    SubjectAssignmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StaffId = table.Column<int>(type: "int", nullable: false),
                    SubjectId = table.Column<int>(type: "int", nullable: false),
                    SectionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubjectAssignment", x => x.SubjectAssignmentId);
                    table.ForeignKey(
                        name: "FK_SubjectAssignment_Section_SectionId",
                        column: x => x.SectionId,
                        principalTable: "Section",
                        principalColumn: "SectionId");
                    table.ForeignKey(
                        name: "FK_SubjectAssignment_Staff_StaffId",
                        column: x => x.StaffId,
                        principalTable: "Staff",
                        principalColumn: "StaffId");
                    table.ForeignKey(
                        name: "FK_SubjectAssignment_Subject_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subject",
                        principalColumn: "SubjectId");
                });



            migrationBuilder.UpdateData(
                table: "Mark",
                keyColumn: "MarkId",
                keyValue: 1,
                column: "MarkEntryDate",
                value: new DateTime(2026, 3, 1, 0, 26, 34, 472, DateTimeKind.Local).AddTicks(1663));

            migrationBuilder.UpdateData(
                table: "Mark",
                keyColumn: "MarkId",
                keyValue: 2,
                column: "MarkEntryDate",
                value: new DateTime(2026, 3, 1, 0, 26, 34, 472, DateTimeKind.Local).AddTicks(1668));

            migrationBuilder.UpdateData(
                table: "Mark",
                keyColumn: "MarkId",
                keyValue: 3,
                column: "MarkEntryDate",
                value: new DateTime(2026, 3, 1, 0, 26, 34, 472, DateTimeKind.Local).AddTicks(1671));

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 1,
                column: "SectionId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 2,
                column: "SectionId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 3,
                column: "SectionId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 4,
                column: "SectionId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 5,
                column: "SectionId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 6,
                column: "SectionId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 7,
                column: "SectionId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 8,
                column: "SectionId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 9,
                column: "SectionId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 10,
                column: "SectionId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 11,
                column: "SectionId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 12,
                column: "SectionId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 13,
                column: "SectionId",
                value: null);

            migrationBuilder.CreateIndex(
                name: "IX_Student_SectionId",
                table: "Student",
                column: "SectionId");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectAssignment_SectionId",
                table: "SubjectAssignment",
                column: "SectionId");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectAssignment_StaffId",
                table: "SubjectAssignment",
                column: "StaffId");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectAssignment_SubjectId",
                table: "SubjectAssignment",
                column: "SubjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Student_Section_SectionId",
                table: "Student",
                column: "SectionId",
                principalTable: "Section",
                principalColumn: "SectionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Student_Section_SectionId",
                table: "Student");

            migrationBuilder.DropTable(
                name: "SubjectAssignment");



            migrationBuilder.DropIndex(
                name: "IX_Student_SectionId",
                table: "Student");

            migrationBuilder.DropColumn(
                name: "SectionId",
                table: "Student");

            migrationBuilder.AlterColumn<int>(
                name: "Grade",
                table: "StudentMarksDetails",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

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
    }
}
