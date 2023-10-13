using Microsoft.EntityFrameworkCore;
using Podosys.Worker.Domain.Models.Reports;
using Podosys.Worker.Domain.Repositories;

namespace Podosys.Worker.Persistence.Repositories
{
    public class ReportRepository : IReportRepository
    {
        private readonly IApplicationDbContext _context;

        public ReportRepository(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddProfitAsync(Profit profit)
        {
            var oldReports = await _context.Profit
                            .Where(x => x.Date.Date == profit.Date.Date)
                            .ToListAsync();

            if (oldReports.Any())
                _context.Profit.RemoveRange(oldReports);

            await _context.Profit.AddAsync(profit);

            _context.SaveChanges();
        }

        public async Task<IEnumerable<Profit>> GetAll()
        {
            return await _context.Profit.AsNoTracking().ToListAsync();
        }

        public async Task AddProcedurePerformedAsync(ProcedurePerformed procedurePerformed)
        {
            var oldReports = await _context.ProcedurePerformed
                            .Where(x => x.Date.Date == procedurePerformed.Date.Date)
                            .ToListAsync();

            if (oldReports.Any())
                _context.ProcedurePerformed.RemoveRange(oldReports);

            await _context.ProcedurePerformed.AddAsync(procedurePerformed);

            _context.SaveChanges();
        }

        public async Task AddProcedureReportAsync(IEnumerable<Procedure> procedure)
        {
            var oldReports = await _context.Procedures
                            .Where(x => x.Date.Date == procedure.FirstOrDefault().Date.Date)
                            .ToListAsync();

            if (oldReports.Any())
                _context.Procedures.RemoveRange(oldReports);

            await _context.Procedures.AddRangeAsync(procedure);

            _context.SaveChanges();
        }
    }
}
