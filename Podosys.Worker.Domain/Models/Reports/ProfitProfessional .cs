namespace Podosys.Worker.Domain.Models.Reports
{
    public class ProfitProfessional
    {
        public DateTime Date { get; set; }
        public string? Professional { get; set; }
        public string? Procedure { get; set; }
        public decimal Value { get; set; }
    }
}
