namespace Podosys.Worker.Domain.Models.Reports
{
    public class Profit
    {
        public DateTime Date { get; set; }
        public decimal TotalValue { get; set; }
        public decimal CurrentAccountValue { get; set; }
        public decimal CashValue { get; set; }
        public decimal Total { get; set; }
    }
}
