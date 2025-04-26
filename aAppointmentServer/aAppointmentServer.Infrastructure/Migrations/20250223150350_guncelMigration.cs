using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace aAppointmentServer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class guncelMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FirtName",
                table: "Users",
                newName: "FirstName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FirstName",
                table: "Users",
                newName: "FirtName");
        }
    }
}
