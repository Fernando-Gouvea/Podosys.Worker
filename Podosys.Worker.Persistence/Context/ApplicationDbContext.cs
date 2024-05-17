using Microsoft.EntityFrameworkCore;
using Podosys.Worker.Domain.Models.Reports;
using Podosys.Worker.Domain.Repositories;

namespace Podosys.Worker.Persistence.Context
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
           : base(options)
        {
        }

        public DbSet<AgeGroup> AgeGroups { get; set; }
        public DbSet<ProcedureReport> Procedures { get; set; }
        public DbSet<Profit> Profit { get; set; }
        public DbSet<ProcedurePerformed> ProcedurePerformed { get; set; }
        public DbSet<ProfitProfessional> ProfitProfessional { get; set; }
        public DbSet<RegisteredPacient> RegisteredPacients { get; set; }
        public DbSet<CommunicationChannel> CommunicationChannels { get; set; }
        public DbSet<SaleOffReport> SaleOffReport { get; set; }
        public DbSet<OperationalCostReport> OperationalCostReport { get; set; }
        public DbSet<AddressReport> AddressReport { get; set; }

        public int SaveChangesAsync()
        {
            return SaveChangesAsync();
        }
    }
}