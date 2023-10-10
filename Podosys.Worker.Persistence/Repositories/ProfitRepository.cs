using Microsoft.EntityFrameworkCore;
using Podosys.Worker.Domain.Models.Reports;
using Podosys.Worker.Domain.Repositories;
using Podosys.Worker.Persistence.Context;

namespace Podosys.Worker.Persistence.Repositories
{
    public class ProfitRepository : IProfitRepository
    {
        private readonly ApplicationDbContext _context;

        public ProfitRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async void CommitAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async void AddProfitAsync(Profit profit)
        {
            var oldProfit = await _context.Profit
                            .Where(x => x.Date.Date == profit.Date.Date)
                            .FirstOrDefaultAsync();

            if (oldProfit != null)
                await _context.AddAsync(profit);

            else
            {
                oldProfit.CashValue = profit.CashValue;
                oldProfit.CurrentAccountValue = profit.CurrentAccountValue;
                oldProfit.TotalValue = profit.TotalValue;

                _context.Update(oldProfit);
            }

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Profit>> GetAll()
        {
            return await _context.Profit.AsNoTracking().ToListAsync();
        }
    }
}
