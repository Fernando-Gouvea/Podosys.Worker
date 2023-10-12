using Podosys.Worker.Domain.Enums;
using Podosys.Worker.Domain.Models.Reports;
using Podosys.Worker.Domain.Repositories;
using System.Collections.Generic;
using System.Transactions;

namespace Podosys.Worker.Domain.Services
{
    public class UpdateReport : IUpdateReport
    {
        private readonly IPodosysRepository _podosysRepository;
        private readonly IProfitRepository _profitRepository;

        public UpdateReport(IPodosysRepository podosysRepository,
                            IProfitRepository profitRepository)
        {
            _podosysRepository = podosysRepository;
            _profitRepository = profitRepository;
        }

        public async Task UpdateReportAsync()
        {
            var firstdate = DateTime.Now.AddDays(-1);
            var lastdate = DateTime.Now;

            var transactions = await _podosysRepository.GetTransaction(firstdate, lastdate);

            var medicalRecords = await _podosysRepository.GetMedicalRecord(transactions.Where(x => x.MedicalRecordId != null).Select(x => (Guid)x.MedicalRecordId));

            var pacients = await _podosysRepository.GetPacient(medicalRecords.Where(x => x.PacientId != null).Select(x => (Guid)x.PacientId));

            var procedure = await _podosysRepository.GetProcedure(medicalRecords.Select(x => x.Id));

            var profit = CalculateProfit(transactions);

            await _profitRepository.AddProfitAsync(profit);
        }

        private Profit CalculateProfit(IEnumerable<Models.Podosys.Transaction> transactions)
        {
            var cashValue = transactions.Where(x => x.PaymentTypeId == (int)PaymentTypeEnum.Dinheiro).Sum(x => x.Value);
            var currentAccountValue = transactions.Where(x => x.PaymentTypeId != (int)PaymentTypeEnum.Dinheiro).Sum(x => x.Value);

            return new Profit
            {
                CashValue = cashValue,
                CurrentAccountValue = currentAccountValue,
                Date = transactions.FirstOrDefault().Date.Date,
                TotalValue = cashValue + currentAccountValue
            };
        }

    }
}