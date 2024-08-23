namespace Podosys.Worker.Domain.Models.Podosys
{
    public class ProcedurePricesValues
    {
        public int Id { get; set; }
        public decimal PriceMin { get; set; }
        public DateTime Date { get; set; }
        public int? ProcedurePricesId { get; set; }
        public string? Observation { get; set; }
        public bool Enabler { get; set; }
    }
}