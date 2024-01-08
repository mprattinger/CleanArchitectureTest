using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanArchitectureTest.Data.Migrations
{
    /// <inheritdoc />
    public partial class todo3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TodoAppointees_Members_TodoId",
                table: "TodoAppointees");

            migrationBuilder.DropForeignKey(
                name: "FK_TodoAppointees_Todos_AppointeeId",
                table: "TodoAppointees");

            migrationBuilder.AddForeignKey(
                name: "FK_TodoAppointees_Members_AppointeeId",
                table: "TodoAppointees",
                column: "AppointeeId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TodoAppointees_Todos_TodoId",
                table: "TodoAppointees",
                column: "TodoId",
                principalTable: "Todos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TodoAppointees_Members_AppointeeId",
                table: "TodoAppointees");

            migrationBuilder.DropForeignKey(
                name: "FK_TodoAppointees_Todos_TodoId",
                table: "TodoAppointees");

            migrationBuilder.AddForeignKey(
                name: "FK_TodoAppointees_Members_TodoId",
                table: "TodoAppointees",
                column: "TodoId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TodoAppointees_Todos_AppointeeId",
                table: "TodoAppointees",
                column: "AppointeeId",
                principalTable: "Todos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
