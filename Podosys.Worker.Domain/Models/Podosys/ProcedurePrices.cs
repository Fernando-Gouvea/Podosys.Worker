namespace Podosys.Worker.Domain.Models.Podosys
{
    public class ProcedurePrices
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<ProcedurePricesValues> ProcedurePricesValues { get; set; }
    }
}