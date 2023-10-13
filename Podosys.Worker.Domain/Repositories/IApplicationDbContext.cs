using Microsoft.EntityFrameworkCore;
using Podosys.Worker.Domain.Models.Reports;

namespace Podosys.Worker.Domain.Repositories
{
    public interface IApplicationDbContext
    {
        DbSet<AgeGroup> AgeGroups { get; set; }
        DbSet<Procedure> Procedures { get; set; }
        DbSet<Profit> Profit { get; set; }
        DbSet<ProcedurePerformed> ProcedurePerformed { get; set; }
        DbSet<ProfitProfessional> ProfitProfessional { get; set; }
        DbSet<RegisteredPacient> RegisteredPacients { get; set; }
        int SaveChanges();
    }
}
