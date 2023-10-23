using System.ComponentModel.DataAnnotations.Schema;

namespace Podosys.Worker.Domain.Models.Reports
{
    public class Profit
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }

        [Column(TypeName = "Decimal(7,2)")]
        public decimal TotalValue { get; set; }

        [Column(TypeName = "Decimal(7,2)")]
        public decimal CurrentAccountValue { get; set; }

        [Column(TypeName = "Decimal(7,2)")]
        public decimal CashValue { get; set; }
        public int WorkingDays { get; set; }
    }
}
