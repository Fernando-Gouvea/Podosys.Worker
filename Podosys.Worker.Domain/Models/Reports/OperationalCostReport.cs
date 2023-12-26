using System.ComponentModel.DataAnnotations.Schema;

namespace Podosys.Worker.Domain.Models.Reports
{
    public class OperationalCostReport
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string? CostName { get; set; }

        [Column(TypeName = "Decimal(7,2)")]
        public decimal Value { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
