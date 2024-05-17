using System.ComponentModel.DataAnnotations.Schema;

namespace Podosys.Worker.Domain.Models.Reports
{
    public class ProcedureReport
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string? ProcedureName { get; set; }
        public int Amounth { get; set; }

        [Column(TypeName = "Decimal(7,2)")]
        public decimal Value { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
