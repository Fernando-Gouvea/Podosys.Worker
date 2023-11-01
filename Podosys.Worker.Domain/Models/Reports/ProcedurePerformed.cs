namespace Podosys.Worker.Domain.Models.Reports
{
    public class ProcedurePerformed
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int ProcedureAmount { get; set; }
        public int BandAidProcedureAmount { get; set; }
        public int TotalAmount { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
