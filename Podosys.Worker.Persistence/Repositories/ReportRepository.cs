﻿using Microsoft.EntityFrameworkCore;
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

        public async Task AddRegisterPacientReportAsync(RegisteredPacient registeredPacient)
        {
            var oldReports = await _context.RegisteredPacients
                            .Where(x => x.Date.Date == registeredPacient.Date.Date)
                            .ToListAsync();

            if (oldReports.Any())
                _context.RegisteredPacients.RemoveRange(oldReports);

            await _context.RegisteredPacients.AddAsync(registeredPacient);

            _context.SaveChanges();
        }

        public async Task AddAgeGroupReportAsync(AgeGroup ageGroup)
        {
            var oldReports = await _context.AgeGroups
                            .Where(x => x.Date.Date == ageGroup.Date.Date)
                            .ToListAsync();

            if (oldReports.Any())
                _context.AgeGroups.RemoveRange(oldReports);

            await _context.AgeGroups.AddAsync(ageGroup);

            _context.SaveChanges();
        }

        public async Task AddProfitProfessionalReportAsync(IEnumerable<ProfitProfessional> profit)
        {
            var oldReports = await _context.ProfitProfessional
                            .Where(x => x.Date.Date == profit.FirstOrDefault().Date.Date)
                            .ToListAsync();

            if (oldReports.Any())
                _context.ProfitProfessional.RemoveRange(oldReports);

            await _context.ProfitProfessional.AddRangeAsync(profit);

            _context.SaveChanges();
        }

        public async Task AddUpdateHistoryReportAsync(UpdateHistory updateHistory)
        {
            await _context.UpdateHistories.AddAsync(updateHistory);

            _context.SaveChanges();
        }
    }
}