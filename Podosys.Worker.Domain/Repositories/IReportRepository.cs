using Podosys.Worker.Domain.Models.Reports;

namespace Podosys.Worker.Domain.Repositories
{
    public interface IReportRepository
    {
        Task AddProfitAsync(Profit profit);
        Task<IEnumerable<Profit>> GetAll();
        Task AddProcedurePerformedAsync(ProcedurePerformed procedurePerformed);
        Task AddProcedureReportAsync(IEnumerable<ProcedureReport> procedure);
        Task AddRegisterPacientReportAsync(RegisteredPacient registeredPacient);
        Task AddAgeGroupReportAsync(AgeGroup ageGroup);
        Task AddAddressReportAsync(List<AddressReport> addressReport);
        Task AddProfitProfessionalReportAsync(IEnumerable<ProfitProfessional> profit);
        Task AddCommunicationChannelReportAsync(IEnumerable<CommunicationChannel> channels);
        Task AddOperacionalCostAsync(List<OperationalCostReport> operacionalCost);
        Task AddSaleOffAsync(SaleOffReport saleoff);
    }
}
