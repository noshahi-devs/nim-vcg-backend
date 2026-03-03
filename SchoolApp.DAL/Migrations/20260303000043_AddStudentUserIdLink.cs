using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolApp.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddStudentUserIdLink : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Student",
                type: "nvarchar(450)",
                nullable: true);

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
                columns: new[] { "CreatedAt", "UserId" },
                values: new object[] { new DateTime(2026, 3, 3, 5, 0, 41, 662, DateTimeKind.Local).AddTicks(2773), null });

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UserId" },
                values: new object[] { new DateTime(2026, 3, 3, 5, 0, 41, 662, DateTimeKind.Local).AddTicks(2781), null });

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UserId" },
                values: new object[] { new DateTime(2026, 3, 3, 5, 0, 41, 662, DateTimeKind.Local).AddTicks(2786), null });

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UserId" },
                values: new object[] { new DateTime(2026, 3, 3, 5, 0, 41, 662, DateTimeKind.Local).AddTicks(2792), null });

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UserId" },
                values: new object[] { new DateTime(2026, 3, 3, 5, 0, 41, 662, DateTimeKind.Local).AddTicks(2796), null });

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UserId" },
                values: new object[] { new DateTime(2026, 3, 3, 5, 0, 41, 662, DateTimeKind.Local).AddTicks(2800), null });

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UserId" },
                values: new object[] { new DateTime(2026, 3, 3, 5, 0, 41, 662, DateTimeKind.Local).AddTicks(2805), null });

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UserId" },
                values: new object[] { new DateTime(2026, 3, 3, 5, 0, 41, 662, DateTimeKind.Local).AddTicks(2809), null });

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UserId" },
                values: new object[] { new DateTime(2026, 3, 3, 5, 0, 41, 662, DateTimeKind.Local).AddTicks(2813), null });

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UserId" },
                values: new object[] { new DateTime(2026, 3, 3, 5, 0, 41, 662, DateTimeKind.Local).AddTicks(2817), null });

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UserId" },
                values: new object[] { new DateTime(2026, 3, 3, 5, 0, 41, 662, DateTimeKind.Local).AddTicks(2821), null });

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 12,
                columns: new[] { "CreatedAt", "UserId" },
                values: new object[] { new DateTime(2026, 3, 3, 5, 0, 41, 662, DateTimeKind.Local).AddTicks(2825), null });

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 13,
                columns: new[] { "CreatedAt", "UserId" },
                values: new object[] { new DateTime(2026, 3, 3, 5, 0, 41, 662, DateTimeKind.Local).AddTicks(2843), null });

            migrationBuilder.CreateIndex(
                name: "IX_Student_UserId",
                table: "Student",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Student_AspNetUsers_UserId",
                table: "Student",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Student_AspNetUsers_UserId",
                table: "Student");

            migrationBuilder.DropIndex(
                name: "IX_Student_UserId",
                table: "Student");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Student");

            migrationBuilder.UpdateData(
                table: "Campus",
                keyColumn: "CampusId",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 3, 4, 32, 39, 307, DateTimeKind.Local).AddTicks(805));

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
                table: "Standard",
                keyColumn: "StandardId",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 3, 4, 32, 39, 307, DateTimeKind.Local).AddTicks(2667));

            migrationBuilder.UpdateData(
                table: "Standard",
                keyColumn: "StandardId",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 3, 4, 32, 39, 307, DateTimeKind.Local).AddTicks(2670));

            migrationBuilder.UpdateData(
                table: "Standard",
                keyColumn: "StandardId",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 3, 4, 32, 39, 307, DateTimeKind.Local).AddTicks(2672));

            migrationBuilder.UpdateData(
                table: "Standard",
                keyColumn: "StandardId",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 3, 4, 32, 39, 307, DateTimeKind.Local).AddTicks(2674));

            migrationBuilder.UpdateData(
                table: "Standard",
                keyColumn: "StandardId",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 3, 4, 32, 39, 307, DateTimeKind.Local).AddTicks(2675));

            migrationBuilder.UpdateData(
                table: "Standard",
                keyColumn: "StandardId",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 3, 4, 32, 39, 307, DateTimeKind.Local).AddTicks(2677));

            migrationBuilder.UpdateData(
                table: "Standard",
                keyColumn: "StandardId",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 3, 4, 32, 39, 307, DateTimeKind.Local).AddTicks(2679));

            migrationBuilder.UpdateData(
                table: "Standard",
                keyColumn: "StandardId",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 3, 4, 32, 39, 307, DateTimeKind.Local).AddTicks(2680));

            migrationBuilder.UpdateData(
                table: "Standard",
                keyColumn: "StandardId",
                keyValue: 9,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 3, 4, 32, 39, 307, DateTimeKind.Local).AddTicks(2682));

            migrationBuilder.UpdateData(
                table: "Standard",
                keyColumn: "StandardId",
                keyValue: 10,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 3, 4, 32, 39, 307, DateTimeKind.Local).AddTicks(2683));

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 3, 4, 32, 39, 307, DateTimeKind.Local).AddTicks(2720));

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 3, 4, 32, 39, 307, DateTimeKind.Local).AddTicks(2733));

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 3, 4, 32, 39, 307, DateTimeKind.Local).AddTicks(2848));

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 3, 4, 32, 39, 307, DateTimeKind.Local).AddTicks(2854));

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 3, 4, 32, 39, 307, DateTimeKind.Local).AddTicks(2858));

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 3, 4, 32, 39, 307, DateTimeKind.Local).AddTicks(2862));

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 3, 4, 32, 39, 307, DateTimeKind.Local).AddTicks(2866));

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 3, 4, 32, 39, 307, DateTimeKind.Local).AddTicks(2870));

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 9,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 3, 4, 32, 39, 307, DateTimeKind.Local).AddTicks(2875));

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 10,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 3, 4, 32, 39, 307, DateTimeKind.Local).AddTicks(2880));

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 11,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 3, 4, 32, 39, 307, DateTimeKind.Local).AddTicks(2884));

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 12,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 3, 4, 32, 39, 307, DateTimeKind.Local).AddTicks(2888));

            migrationBuilder.UpdateData(
                table: "Student",
                keyColumn: "StudentId",
                keyValue: 13,
                column: "CreatedAt",
                value: new DateTime(2026, 3, 3, 4, 32, 39, 307, DateTimeKind.Local).AddTicks(2900));
        }
    }
}
