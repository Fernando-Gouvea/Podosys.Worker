using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Podosys.Worker.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AjustaPRocedurePerformed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Procedure",
                table: "ProcedurePerformed",
                newName: "ProcedureAmount");

            migrationBuilder.RenameColumn(
                name: "BandAidProcedure",
                table: "ProcedurePerformed",
                newName: "BandAidProcedureAmount");

            migrationBuilder.AddColumn<int>(
                name: "TotalAmount",
                table: "ProcedurePerformed",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalAmount",
                table: "ProcedurePerformed");

            migrationBuilder.RenameColumn(
                name: "BandAidProcedureAmount",
                table: "ProcedurePerformed",
                newName: "BandAidProcedure");

            migrationBuilder.RenameColumn(
                name: "ProcedureAmount",
                table: "ProcedurePerformed",
                newName: "Procedure");
        }
    }
}
