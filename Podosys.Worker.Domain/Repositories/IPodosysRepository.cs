using Podosys.Worker.Domain.Models.Podosys;

namespace Podosys.Worker.Domain.Repositories
{
    public interface IPodosysRepository
    {
        Task<IEnumerable<MedicalRecord>> GetMedicalRecord(DateTime FirstDate, DateTime LastDate);
    }
}
