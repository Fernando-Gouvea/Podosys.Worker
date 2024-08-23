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

        public async Task AddProcedureReportAsync(IEnumerable<ProcedureReport> procedure)
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

        public async Task AddCommunicationChannelReportAsync(IEnumerable<CommunicationChannel> channels)
        {
            var oldReports = await _context.CommunicationChannels
                            .Where(x => x.Date.Date == channels.FirstOrDefault().Date.Date)
                            .ToListAsync();

            if (oldReports.Any())
                _context.CommunicationChannels.RemoveRange(oldReports);

            await _context.CommunicationChannels.AddRangeAsync(channels);

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

        public async Task AddOperacionalCostAsync(List<OperationalCostReport> operacionalCost)
        {
            var oldReports = await _context.OperationalCostReport
                            .Where(x => x.Date.Date == operacionalCost.FirstOrDefault().Date.Date)
                            .ToListAsync();

            if (oldReports.Any())
                _context.OperationalCostReport.RemoveRange(oldReports);

            await _context.OperationalCostReport.AddRangeAsync(operacionalCost);

            _context.SaveChanges();
        }

        public async Task AddAddressReportAsync(List<AddressReport> addressReport)
        {
            var oldReports = await _context.AddressReport
                            .Where(x => x.Date.Date == addressReport.FirstOrDefault().Date.Date)
                            .ToListAsync();

            if (oldReports.Any())
                _context.AddressReport.RemoveRange(oldReports);

            await _context.AddressReport.AddRangeAsync(addressReport);

            _context.SaveChanges();
        }


        public async Task AddSaleOffAsync(SaleOffReport saleoff)
        {
            var oldReports = await _context.SaleOffReport
                            .Where(x => x.Date.Date == saleoff.Date.Date)
                            .ToListAsync();

            if (oldReports.Any())
                _context.SaleOffReport.RemoveRange(oldReports);

            await _context.SaleOffReport.AddAsync(saleoff);

            _context.SaveChanges();
        }

        public async Task AddAnnualComparisonReportAsync(AnnualComparisonReport report)
        {
            var oldReports = await _context.AnnualComparisonReport
                            .Where(x => x.Date.Date == report.Date.Date)
                            .ToListAsync();

            if (oldReports.Any())
                _context.AnnualComparisonReport.RemoveRange(oldReports);

            await _context.AnnualComparisonReport.AddAsync(report);

            _context.SaveChanges();
        }

        public async Task AddProcedurePriceReportAsync(IEnumerable<ProcedurePriceReport> report)
        {
            //var oldReports = await _context.ProcedurePriceReport
            //                .Where(x => x.Date.Date == report.Date.Date)
            //                .ToListAsync();

            //if (oldReports.Any())
            //    _context.AnnualComparisonReport.RemoveRange(oldReports);

            //await _context.AnnualComparisonReport.AddAsync(report);

            //_context.SaveChanges();
        }
    }
}
