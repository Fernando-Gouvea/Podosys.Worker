using Podosys.Worker.Domain.Models.Reports;

namespace Podosys.Worker.Domain.Repositories
{
    public interface IReportRepository
    {
        Task AddProfitAsync(Profit profit);
        Task<IEnumerable<Profit>> GetAll();
        Task AddProcedurePerformedAsync(ProcedurePerformed procedurePerformed);
        Task AddProcedureReportAsync(IEnumerable<Procedure> procedure);
    }
}
