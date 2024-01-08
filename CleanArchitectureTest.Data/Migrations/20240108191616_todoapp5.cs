using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanArchitectureTest.Data.Migrations
{
    /// <inheritdoc />
    public partial class todoapp5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TodoAppointees_Members_MemberId",
                table: "TodoAppointees");

            migrationBuilder.DropForeignKey(
                name: "FK_TodoAppointees_Todos_TodoId",
                table: "TodoAppointees");

            migrationBuilder.DropIndex(
                name: "IX_TodoAppointees_MemberId",
                table: "TodoAppointees");

            migrationBuilder.DropColumn(
                name: "MemberId",
                table: "TodoAppointees");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.AddForeignKey(
                name: "FK_TodoAppointees_Todos_TodoId",
                table: "TodoAppointees",
                column: "TodoId",
                principalTable: "Todos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
