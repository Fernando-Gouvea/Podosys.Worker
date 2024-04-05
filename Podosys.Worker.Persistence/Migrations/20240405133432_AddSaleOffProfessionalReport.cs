using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Podosys.Worker.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddSaleOffProfessionalReport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PendingClosingAmount",
                table: "ProfitProfessional",
                newName: "SaleOffAmount");

            migrationBuilder.AddColumn<decimal>(
                name: "SaleOffValue",
                table: "ProfitProfessional",
                type: "Decimal(7,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SaleOffValue",
                table: "ProfitProfessional");

            migrationBuilder.RenameColumn(
                name: "SaleOffAmount",
                table: "ProfitProfessional",
                newName: "PendingClosingAmount");
        }
    }
}
