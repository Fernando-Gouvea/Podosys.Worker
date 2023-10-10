using System.ComponentModel.DataAnnotations.Schema;

namespace Podosys.Worker.Domain.Models.Reports
{
    public class ProfitProfessional
    {
        public DateTime Date { get; set; }
        public string? Professional { get; set; }
        public string? Procedure { get; set; }

        [Column(TypeName = "Decimal(7,2)")]
        public decimal Value { get; set; }
    }
}
