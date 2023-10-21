using Podosys.Worker.Domain.Models.Podosys;

namespace Podosys.Worker.Domain.Repositories
{
    public interface IPodosysRepository
    {
        Task<IEnumerable<Transaction>> GetTransaction(DateTime FirstDate, DateTime LastDate);
        Task<IEnumerable<MedicalRecord>> GetMedicalRecord(IEnumerable<Guid> medicalRecordIds);
        Task<IEnumerable<Pacient>> GetPacient(IEnumerable<Guid> pacientIds);
        Task<IEnumerable<Procedure>> GetProcedure(IEnumerable<Guid> medicalRecordIds);
        Task<IEnumerable<Professional>> GetProfessional(IEnumerable<Guid> professionalIds);
        Task<IEnumerable<Pacient>> GetPacientByDate(DateTime date);
    }
}
