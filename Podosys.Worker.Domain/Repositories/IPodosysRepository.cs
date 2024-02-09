using Podosys.Worker.Domain.Models.Podosys;

namespace Podosys.Worker.Domain.Repositories
{
    public interface IPodosysRepository
    {
        Task<IEnumerable<Transaction>> GetTransaction(DateTime FirstDate);
        Task<IEnumerable<MedicalRecord>> GetMedicalRecord(IEnumerable<Guid> medicalRecordIds);
        Task<IEnumerable<Pacient>> GetPacient(IEnumerable<Guid> pacientIds);
        Task<IEnumerable<Procedure>> GetProcedure(IEnumerable<Guid> medicalRecordIds);
        Task<IEnumerable<Professional>> GetProfessional(IEnumerable<Guid> professionalIds);
        Task<IEnumerable<Address>> GetAddress(IEnumerable<Guid> addressIds);
        Task<IEnumerable<PodosysCommunicationChannel>> GetAllCommunicationChannel();
        Task<IEnumerable<Pacient>> GetPacientByDate(DateTime date);
        Task<IEnumerable<TransactionCategory>> GetAllTransactionCategory();
    }
}
