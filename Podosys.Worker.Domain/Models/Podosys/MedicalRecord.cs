namespace Podosys.Worker.Domain.Models.Podosys
{
    public class MedicalRecord
    {
        public Guid Id { get; set; }
        public Guid? PacientId { get; set; }
        public Guid? UserId { get; set; }
        public string? PayType { get; set; }
        public decimal Value { get; set; }
        public string? Observation { get; set; }
        public DateTime MedicalRecordDate { get; set; }
        public int? ProcedurePriceId { get; set; }
        public bool HomeCare { get; set; }
        public bool Enabler { get; set; } = true;
    }
}
