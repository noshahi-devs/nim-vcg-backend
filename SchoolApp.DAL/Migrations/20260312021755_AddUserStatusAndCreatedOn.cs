using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolApp.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddUserStatusAndCreatedOn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedOn",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "Active");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "AspNetUsers");

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
    }
}
