using Podosys.Worker.Domain.Models.Reports;

namespace Podosys.Worker.Domain.Repositories
{
    public interface IProfitRepository
    {
        void CommitAsync();
        void AddProfitAsync(Profit profit);
        Task<IEnumerable<Profit>> GetAll();
    }
}
