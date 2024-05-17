using Microsoft.EntityFrameworkCore;
using Podosys.Worker.Domain.Models.Reports;

namespace Podosys.Worker.Domain.Repositories
{
    public interface IApplicationDbContext
    {
        DbSet<AgeGroup> AgeGroups { get; set; }
        DbSet<ProcedureReport> Procedures { get; set; }
        DbSet<Profit> Profit { get; set; }
        DbSet<ProcedurePerformed> ProcedurePerformed { get; set; }
        DbSet<ProfitProfessional> ProfitProfessional { get; set; }
        DbSet<RegisteredPacient> RegisteredPacients { get; set; }
        DbSet<CommunicationChannel> CommunicationChannels { get; set; }
        DbSet<SaleOffReport> SaleOffReport { get; set; }
        DbSet<OperationalCostReport> OperationalCostReport { get; set; }
        DbSet<AddressReport> AddressReport { get; set; }
        int SaveChanges();
    }
}
