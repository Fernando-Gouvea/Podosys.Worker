using System.ComponentModel.DataAnnotations.Schema;

namespace Podosys.Worker.Domain.Models.Reports
{
    public class ProfitProfessional
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string? Professional { get; set; }
        public int ProcedureAmount { get; set; }
        public int BandaidAmount { get; set; }

        [Column(TypeName = "Decimal(7,2)")]
        public decimal Value { get; set; }
    }
}
