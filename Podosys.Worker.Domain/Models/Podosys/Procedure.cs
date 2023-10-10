namespace Podosys.Worker.Domain.Models.Podosys
{
    public class Procedure
    {
        public int Id { get; set; }

        public string? ProcedureType { get; set; }

        public Guid MedicalRecordId { get; set; }
    }
}