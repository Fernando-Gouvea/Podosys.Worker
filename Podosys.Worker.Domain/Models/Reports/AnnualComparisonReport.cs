using System.ComponentModel.DataAnnotations.Schema;

namespace Podosys.Worker.Domain.Models.Reports
{
    public class AnnualComparisonReport
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }

        [Column(TypeName = "Decimal(7,2)")]
        public decimal TotalValue { get; set; }

        [Column(TypeName = "Decimal(7,2)")]
        public decimal TotalValueLastYear { get; set; }

        [Column(TypeName = "Decimal(7,2)")]
        public decimal TotalCostValue { get; set; }

        [Column(TypeName = "Decimal(7,2)")]
        public decimal TotalCostValueLastYear { get; set; }

        public DateTime UpdateDate { get; set; }
    }
}
