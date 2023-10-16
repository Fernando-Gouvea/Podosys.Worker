using Podosys.Worker.Domain.Enums;
using Podosys.Worker.Domain.Models.Podosys;
using Podosys.Worker.Domain.Models.Reports;
using Podosys.Worker.Domain.Repositories;

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
            var firstdate = DateTime.Now.AddDays(-1);
            var lastdate = DateTime.Now;

            var transactions = await _podosysRepository.GetTransaction(firstdate, lastdate);

            if (!transactions.Any() || !transactions.Any(x => x.MedicalRecordId != null))
                return;

            var medicalRecords = await _podosysRepository.GetMedicalRecord(transactions.Where(x => x.MedicalRecordId != null).Select(x => (Guid)x.MedicalRecordId));

            var professionals = await _podosysRepository.GetProfessional(medicalRecords.Select(x => (Guid)x.UserId).Distinct());

            var pacients = await _podosysRepository.GetPacient(medicalRecords.Where(x => x.PacientId != null).Select(x => (Guid)x.PacientId));

            var procedures = await _podosysRepository.GetProcedure(medicalRecords.Select(x => x.Id));

            var profit = CalculateProfit(transactions);

            await _reportRepositoty.AddProfitAsync(profit);

            var procedureProfit = CalculateProcedurePerformed(procedures, firstdate);

            await _reportRepositoty.AddProcedurePerformedAsync(procedureProfit);

            var procedureReport = CalculateProcedure(procedures, transactions);

            await _reportRepositoty.AddProcedureReportAsync(procedureReport);

            var registerPacient = CalculateRegisteredPacient(pacients, firstdate);

            await _reportRepositoty.AddRegisterPacientReportAsync(registerPacient);

            var AgeReport = CalculateAgeGroup(pacients, procedures, medicalRecords, firstdate);

            await _reportRepositoty.AddAgeGroupReportAsync(AgeReport);

            var professionalReport = CalculateProfitProfissional(professionals, procedures, medicalRecords, transactions);

            await _reportRepositoty.AddUpdateHistoryReportAsync(new UpdateHistory { Date = DateTime.Now});
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

        private RegisteredPacient CalculateRegisteredPacient(IEnumerable<Pacient> pacients, DateTime date)
        {
            return new RegisteredPacient
            {
                Date = date.Date,
                RegisterAmounth = pacients.Where(x => x.RegisterDate.Date == date).Count()
            };
        }

        private AgeGroup CalculateAgeGroup(IEnumerable<Pacient> pacients, IEnumerable<Models.Podosys.Procedure> procedures, IEnumerable<MedicalRecord> medicalRecords, DateTime date)
        {
            var procedureBandaid = MedialRecordProcedureBandaid(procedures);

            var pacientIds = medicalRecords.Where(x => procedureBandaid.Item1.Contains(x.Id)).Select(x => x.PacientId);

            var listPacients = pacients.Where(x => pacientIds.Contains(x.Id));

            return new AgeGroup
            {
                Date = date.Date,
                Baby = listPacients.Where(x => x.Age >= 0 && x.Age < 2).Count(),
                Child = listPacients.Where(x => x.Age >= 2 && x.Age < 12).Count(),
                Teenager = listPacients.Where(x => x.Age >= 12 && x.Age < 18).Count(),
                Young = listPacients.Where(x => x.Age >= 18 && x.Age < 30).Count(),
                Adult = listPacients.Where(x => x.Age >= 30 && x.Age < 60).Count(),
                Elderly = listPacients.Where(x => x.Age >= 60 && x.Age < 150).Count()
            };
        }

        private ProcedurePerformed CalculateProcedurePerformed(IEnumerable<Models.Podosys.Procedure> procedure, DateTime date)
        {
            if (!procedure.Any())
                return new ProcedurePerformed
                {
                    BandAidProcedureAmount = 0,
                    ProcedureAmount = 0,
                    Date = date.Date,
                };

            var procedureBandaid = MedialRecordProcedureBandaid(procedure);

            return new ProcedurePerformed
            {
                ProcedureAmount = procedureBandaid.Item1.Count(),
                BandAidProcedureAmount = procedureBandaid.Item2.Count(),
                TotalAmount = procedureBandaid.Item1.Count() + procedureBandaid.Item2.Count(),
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
                    Amounth = 1,
                    ProcedureName = "Prontuarios Não Fechados",
                    Value = 0
                };

                response.Add(report);
            }

            return response;
        }

        private IEnumerable<ProfitProfessional> CalculateProfitProfissional(IEnumerable<Professional> professionals, IEnumerable<Models.Podosys.Procedure> procedures, IEnumerable<MedicalRecord> medicalRecords, IEnumerable<Transaction> transactions)
        {
            var profit = new List<ProfitProfessional>();

            var medicalRecordBandaid = MedialRecordProcedureBandaid(procedures);

            foreach (var professional in professionals)
            {
                var medicalRecord = medicalRecords.Where(x => x.UserId == professional.Id);

                var transaction = transactions.Where(t => medicalRecord.Any(x => x.Id == t.MedicalRecordId));

                profit.Add(new ProfitProfessional
                {
                    Date = medicalRecord.FirstOrDefault().MedicalRecordDate.Date,
                    Professional = professional.Name,
                    Value = transaction.Sum(x => x.Value),
                    ProcedureAmount = medicalRecord.Where(x => medicalRecordBandaid.Item1.Contains(x.Id)).Count(),
                    BandaidAmount = medicalRecord.Where(x => medicalRecordBandaid.Item2.Contains(x.Id)).Count()
                });
            }

            return profit;
        }

        private Tuple<IEnumerable<Guid>, IEnumerable<Guid>> MedialRecordProcedureBandaid(IEnumerable<Models.Podosys.Procedure> procedures)
        {
            var procedure = procedures.Where(x => x.ProcedureType != null && !x.ProcedureType.Contains(ProcedureEnum.Curativo.ToString())).Select(x => x.MedicalRecordId);
            var bandaid = procedures.Where(x => x.ProcedureType != null && x.ProcedureType.Contains(ProcedureEnum.Curativo.ToString())).Select(x => x.MedicalRecordId);

            return new Tuple<IEnumerable<Guid>, IEnumerable<Guid>>(procedure, bandaid);
        }

    }
}