using Microsoft.EntityFrameworkCore;
using Podosys.Worker.Domain.Enums;
using Podosys.Worker.Domain.Models.Podosys;
using Podosys.Worker.Domain.Models.Reports;
using Podosys.Worker.Domain.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace Podosys.Worker.Domain.Services
{
    public class UpdateReport : IUpdateReport
    {
        private readonly IPodosysRepository _podosysRepository;
        private readonly IReportRepository _reportRepositoty;

        public UpdateReport(IPodosysRepository podosysRepository,
                            IReportRepository reportRepositoty)
        {
            _podosysRepository = podosysRepository;
            _reportRepositoty = reportRepositoty;
        }

        public async Task UpdateReportAsync()
        {
            // var firstdate = DateTime.Parse("01-10-2023");
            var lastdate = DateTime.Now;
            var secunddate = DateTime.Now;

            for (var firstdate = DateTime.Parse("01-01-2023"); firstdate.Date < secunddate.Date; firstdate = firstdate.AddDays(1))
            {

                lastdate = firstdate.AddDays(1);

                var transactions = await _podosysRepository.GetTransaction(firstdate, lastdate);

                if (!transactions.Any() || !transactions.Any(x=>x.MedicalRecordId != null))
                    continue;

                var medicalRecords = await _podosysRepository.GetMedicalRecord(transactions.Where(x => x.MedicalRecordId != null).Select(x => (Guid)x.MedicalRecordId));

                var pacients = await _podosysRepository.GetPacient(medicalRecords.Where(x => x.PacientId != null).Select(x => (Guid)x.PacientId));

                var procedures = await _podosysRepository.GetProcedure(medicalRecords.Select(x => x.Id));

                var profit = CalculateProfit(transactions);

                await _reportRepositoty.AddProfitAsync(profit);

                var procedureProfit = CalculateProcedurePerformed(procedures, firstdate);

                await _reportRepositoty.AddProcedurePerformedAsync(procedureProfit);

                var procedureReport = CalculateProcedure(procedures, transactions);

                await _reportRepositoty.AddProcedureReportAsync(procedureReport);
            }
        }

        private Profit CalculateProfit(IEnumerable<Transaction> transactions)
        {
            var cashValue = transactions.Where(x => x.PaymentTypeId == (int)PaymentTypeEnum.Dinheiro &&
                                              (x.MedicalRecordId != null || x.OrderId != null || x.SaleOffId != null)).Sum(x => x.Value);

            var currentAccountValue = transactions.Where(x => x.PaymentTypeId != (int)PaymentTypeEnum.Dinheiro &&
                                                        (x.MedicalRecordId != null || x.OrderId != null || x.SaleOffId != null)).Sum(x => x.Value);

            return new Profit
            {
                CashValue = cashValue,
                CurrentAccountValue = currentAccountValue,
                Date = transactions.FirstOrDefault().Date.Date,
                TotalValue = cashValue + currentAccountValue
            };
        }

        //private ProfitProfessional CalculateProfitProfessional(IEnumerable<Transaction> transactions)
        //{
        //    var cashValue = transactions.Where(x => x.PaymentTypeId == (int)PaymentTypeEnum.Dinheiro &&
        //                                      (x.MedicalRecordId != null || x.OrderId != null || x.SaleOffId != null)).Sum(x => x.Value);

        //    var currentAccountValue = transactions.Where(x => x.PaymentTypeId != (int)PaymentTypeEnum.Dinheiro &&
        //                                                (x.MedicalRecordId != null || x.OrderId != null || x.SaleOffId != null)).Sum(x => x.Value);

        //    return new ProfitProfessional
        //    {
        //        CashValue = cashValue,
        //        CurrentAccountValue = currentAccountValue,
        //        Date = transactions.FirstOrDefault().Date.Date,
        //        TotalValue = cashValue + currentAccountValue
        //    };
        //}

        private ProcedurePerformed CalculateProcedurePerformed(IEnumerable<Models.Podosys.Procedure> procedure, DateTime date)
        {
            if (!procedure.Any())
                return new ProcedurePerformed
                {
                    BandAidProcedureAmount = 0,
                    ProcedureAmount = 0,
                    Date = date.Date,
                };

            var bandAidAmount = procedure.Where(x => x.ProcedureType != null && x.ProcedureType.Contains(ProcedureEnum.Curativo.ToString())).Count();
            var procedureAmount = procedure.Where(x => x.ProcedureType != null && !x.ProcedureType.Contains(ProcedureEnum.Curativo.ToString())).Count();

            return new ProcedurePerformed
            {
                BandAidProcedureAmount = bandAidAmount,
                ProcedureAmount = procedureAmount,
                Date = date.Date,
            };
        }

        private IEnumerable<Models.Reports.Procedure> CalculateProcedure(IEnumerable<Models.Podosys.Procedure> procedures, IEnumerable<Transaction> transactions)
        {
            var listProcedure = procedures.Select(x => x.ProcedureType).Distinct();

            var response = new List<Models.Reports.Procedure>();

            foreach (var procedure in listProcedure)
            {
                var medicalRecords = procedures.Where(x => x.ProcedureType.Equals(procedure)).Select(x => x.MedicalRecordId).ToList();

                var value = transactions.Where(x => x.MedicalRecordId != null && medicalRecords.Contains((Guid)x.MedicalRecordId)).Sum(x => x.Value);

                var report = new Models.Reports.Procedure
                {
                    Date = transactions.FirstOrDefault().Date.Date,
                    Amounth = procedures.Where(x => x.ProcedureType.Equals(procedure)).Count(),
                    ProcedureName = procedure,
                    Value = value
                };

                response.Add(report);
            }

            if (response.Count == 0)
            {
                var report = new Models.Reports.Procedure
                {
                    Date = transactions.FirstOrDefault().Date.Date,
                    Amounth = 0,
                    ProcedureName = "Prontuarios Não Fechados",
                    Value = 0
                };

                response.Add(report);
            }

            return response;
        }

    }
}