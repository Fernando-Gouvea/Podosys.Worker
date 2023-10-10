namespace Podosys.Worker.Domain.Models.Reports
{
    public class ProcedurePerformed
    {
        public DateTime Date { get; set; }
        public int Procedure { get; set; }
        public int BandAidProcedure { get; set; }
    }
}
