using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanArchitectureTest.Data.Migrations
{
    /// <inheritdoc />
    public partial class todoapp3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TodoAppointees_Members_AppointeeId",
                table: "TodoAppointees");

            migrationBuilder.DropIndex(
                name: "IX_TodoAppointees_AppointeeId",
                table: "TodoAppointees");

            migrationBuilder.AddColumn<Guid>(
                name: "MemberId",
                table: "TodoAppointees",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TodoAppointees_MemberId",
                table: "TodoAppointees",
                column: "MemberId");

            migrationBuilder.AddForeignKey(
                name: "FK_TodoAppointees_Members_MemberId",
                table: "TodoAppointees",
                column: "MemberId",
                principalTable: "Members",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TodoAppointees_Members_MemberId",
                table: "TodoAppointees");

            migrationBuilder.DropIndex(
                name: "IX_TodoAppointees_MemberId",
                table: "TodoAppointees");

            migrationBuilder.DropColumn(
                name: "MemberId",
                table: "TodoAppointees");

            migrationBuilder.CreateIndex(
                name: "IX_TodoAppointees_AppointeeId",
                table: "TodoAppointees",
                column: "AppointeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_TodoAppointees_Members_AppointeeId",
                table: "TodoAppointees",
                column: "AppointeeId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
