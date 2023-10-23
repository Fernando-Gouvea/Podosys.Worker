using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Podosys.Worker.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AdicionaWorkingDays : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WorkingDays",
                table: "Profit",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WorkingDays",
                table: "Profit");
        }
    }
}
