using Microsoft.EntityFrameworkCore;
using Podosys.Worker.Domain.Models.Reports;

namespace Podosys.Worker.Persistence.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
           : base(options)
        {
        }
        public DbSet<AgeGroup> AgeGroups { get; set; }
        public DbSet<Procedure> Procedures { get; set; }
        public DbSet<Profit> Profit { get; set; }
        public DbSet<ProcedurePerformed> ProcedurePerformed { get; set; }
        public DbSet<ProfitProfessional> ProfitProfessional { get; set; }
        public DbSet<RegisteredPacient> RegisteredPacients { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AgeGroup>().HasNoKey();
            modelBuilder.Entity<Procedure>().HasNoKey();
            modelBuilder.Entity<Profit>().HasNoKey();
            modelBuilder.Entity<ProcedurePerformed>().HasNoKey();
            modelBuilder.Entity<ProfitProfessional>().HasNoKey();
            modelBuilder.Entity<RegisteredPacient>().HasNoKey();
        }
    }
}