using Podosys.Worker.Domain.Models.Podosys;

namespace Podosys.Worker.Domain.Repositories
{
    public interface IPodosysRepository
    {
        Task<IEnumerable<Transaction>> GetTransaction(DateTime FirstDate);
        Task<IEnumerable<Transaction>> GetTransactionMonth(DateTime date, bool operacionalCost = false);
        Task<IEnumerable<Transaction>> GetTransactionBySaleOffIds(IEnumerable<Guid?>? saleOffIds);
        Task<IEnumerable<MedicalRecord>> GetMedicalRecord(DateTime date, IEnumerable<Guid?> medicalRecordIds = null);
        Task<IEnumerable<Pacient>> GetPacient(IEnumerable<Guid> pacientIds);
        Task<IEnumerable<Procedure>> GetAllProcedure();
        Task<IEnumerable<Professional>> GetProfessional(IEnumerable<Guid> professionalIds);
        Task<IEnumerable<SaleOff>> GetSaleOffs(IEnumerable<Guid?>? saleOffIds);
        Task<IEnumerable<Address>> GetAddress(IEnumerable<Guid?> addressIds);
        Task<IEnumerable<PodosysCommunicationChannel>> GetAllCommunicationChannel();
        Task<IEnumerable<Pacient>> GetPacientByDate(DateTime date);
        Task<IEnumerable<TransactionCategory>> GetAllTransactionCategory();
    }
}
