namespace Podosys.Worker.Domain.Models.Reports
{
    public class Procedure
    {
        public DateTime Date { get; set; }
        public string? ProcedureName { get; set; }
        public int Amounth { get; set; }
        public decimal Value { get; set; }
    }
}
