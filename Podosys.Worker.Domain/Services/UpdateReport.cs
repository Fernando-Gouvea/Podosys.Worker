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
        private IEnumerable<Procedure> _procedures;
        private readonly DateTime _updateDate = DateTime.Now.AddHours(4);

        public UpdateReport(IPodosysRepository podosysRepository,
                            IReportRepository reportRepositoty)
        {
            _podosysRepository = podosysRepository;
            _reportRepositoty = reportRepositoty;
        }

        public async Task UpdateReportAsync(DateTime firstdate, DateTime lastdate)
        {
            //calculo porcentagem aumento e diminuicao
            //((v2-v1)/v1)*100

            //IBGE inpc
            //https://servicodados.ibge.gov.br/api/v3/agregados/7063/periodos/202301|202302|202303|202304|202305|202306|202307|202308|202309/variaveis/44|68?localidades=N1[all]&classificacao=315[7169]
            //Doc https://servicodados.ibge.gov.br/api/docs/agregados?versao=3#api-acervo

            //firstdate = DateTime.Parse("01-11-2023");
            //lastdate = DateTime.Parse("02-11-2023");

            _procedures = await _podosysRepository.GetAllProcedure();

            for (var count = 0; firstdate <= lastdate; firstdate = firstdate.AddDays(1))
            {
                var transactions = await _podosysRepository.GetTransaction(firstdate);

                var medicalRecords = await _podosysRepository.GetMedicalRecord(firstdate);

                if (transactions.Any())
                {
                    var profit = CalculateProfit(transactions);

                    await _reportRepositoty.AddProfitAsync(profit);

                    var saleoff = CalculateSaleOffs(transactions);

                    if (saleoff != null)
                        await _reportRepositoty.AddSaleOffAsync(saleoff);

                    var transactionsCategoty = await _podosysRepository.GetAllTransactionCategory();

                    var operacionalCost = CalculateOperacionalCost(transactions, transactionsCategoty);

                    if (operacionalCost != null)
                        await _reportRepositoty.AddOperacionalCostAsync(operacionalCost);

                    if (medicalRecords.Any())
                    {
                        var professionals = await _podosysRepository.GetProfessional(medicalRecords.Select(x => (Guid)x.UserId).Distinct());

                        var pacients = await _podosysRepository.GetPacient(medicalRecords.Where(x => x.PacientId != null).Select(x => (Guid)x.PacientId));

                        var address = await _podosysRepository.GetAddress(pacients.Where(x => x.AddressId != null).Select(x => x.AddressId));

                        var procedureProfit = CalculateProcedurePerformed(firstdate, medicalRecords);

                        await _reportRepositoty.AddProcedurePerformedAsync(procedureProfit);

                        var procedureReport = CalculateProcedure(transactions, medicalRecords);

                        await _reportRepositoty.AddProcedureReportAsync(procedureReport);

                        var registerPacient = CalculateRegisteredPacient(pacients, firstdate);

                        await _reportRepositoty.AddRegisterPacientReportAsync(registerPacient);

                        var AgeReport = CalculateAgeGroup(pacients, medicalRecords, firstdate);

                        await _reportRepositoty.AddAgeGroupReportAsync(AgeReport);

                        try
                        {
                            var professionalReport = CalculateProfitProfissional(professionals, medicalRecords, transactions);

                            await _reportRepositoty.AddProfitProfessionalReportAsync(professionalReport);
                        }
                        catch (Exception ex)
                        {

                        }

                        var addressReport = address != null ? CalculateCustomersAddress(address, firstdate) : null;

                        if (addressReport != null && addressReport.Any())
                            await _reportRepositoty.AddAddressReportAsync(addressReport);
                    }
                }

                var channels = await CalculateCommunicationChannel(firstdate);

                if (channels.Any())
                    await _reportRepositoty.AddCommunicationChannelReportAsync(channels);
            }
        }

        private List<AddressReport> CalculateCustomersAddress(IEnumerable<Address> address, DateTime date)
        {
            var addresses = new List<AddressReport>();

            foreach (var item in address.Where(x => x.PostalCode != null && x.Latitude != null))
            {
                addresses.Add(new AddressReport
                {
                    Date = date.Date,
                    UpdateDate = _updateDate,
                    Neighborhood = item.Neighborhood,
                    Street = item.Street,
                    City = item.City,
                    State = item.State,
                    PostalCode = item.PostalCode,
                    Latitude = item.Latitude,
                    Longitude = item.Longitude,
                });
            }

            return addresses;
        }

        private Profit CalculateProfit(IEnumerable<Transaction> transactions)
        {
            var cashValue = transactions.Where(x => x.PaymentTypeId == (int)PaymentTypeEnum.Dinheiro &&
                                              (x.MedicalRecordId != null || x.OrderId != null || x.SaleOffId != null)).Sum(x => x.Value);

            var currentAccountValue = transactions.Where(x => x.PaymentTypeId != (int)PaymentTypeEnum.Dinheiro &&
                                                        (x.MedicalRecordId != null || x.OrderId != null || x.SaleOffId != null)).Sum(x => x.Value);

            var operecionalCost = transactions.Where(x => x.TransactionTypeId == (int)TransactionTypeEnum.Saida && x.MedicalRecordId == null && x.OrderId == null && x.SaleOffId == null);

            return new Profit
            {
                CashValue = cashValue,
                CurrentAccountValue = currentAccountValue,
                Date = transactions.FirstOrDefault().Date.Date,
                TotalValue = cashValue + currentAccountValue,
                OperationalCost = operecionalCost.Sum(x => x.Value),
                AccountBalance = currentAccountValue - operecionalCost.Where(x => x.PaymentTypeId != (int)PaymentTypeEnum.Dinheiro).Sum(x => x.Value),
                WorkingDays = WorkingDays(transactions.FirstOrDefault().Date.Date),
                UpdateDate = _updateDate
            };
        }

        private SaleOffReport CalculateSaleOffs(IEnumerable<Transaction> transactions)
        {
            if (!transactions.Where(x => x.SaleOffId != null).Any())
                return null;

            var saleoffValue = transactions.Where(x => x.SaleOffId != null && !x.PaymentTypeId.Equals(PaymentTypeEnum.Pacote)).Sum(x => x.Value);

            return new SaleOffReport
            {
                Amounth = transactions.Where(x => x.SaleOffId != null).Count(),
                Value = saleoffValue,
                Date = transactions.FirstOrDefault().Date.Date,
                UpdateDate = _updateDate
            };
        }

        private List<OperationalCostReport> CalculateOperacionalCost(IEnumerable<Transaction> transactions, IEnumerable<TransactionCategory> category)
        {
            var operacionalCostReport = new List<OperationalCostReport>();

            var operacionalCost = transactions.Where(x => x.TransactionTypeId == (int)TransactionTypeEnum.Saida);

            if (!operacionalCost.Any())
                return null;

            foreach (var cost in operacionalCost)
            {
                operacionalCostReport.Add(
                        new OperationalCostReport
                        {
                            CostName = cost.Description,
                            Value = cost.Value,
                            Date = cost.Date.Date,
                            Category = category.FirstOrDefault(x => x.Id == cost.TransactionCategoryId)?.Name,
                            UpdateDate = _updateDate
                        });
            }

            return operacionalCostReport;
        }

        private static int WorkingDays(DateTime dateNow)
        {
            var workingDays = 0;

            var lastDay = new DateTime(dateNow.Year, dateNow.Month, DateTime.DaysInMonth(dateNow.Year, dateNow.Month));

            for (DateTime data = dateNow; data < lastDay; data = data.AddDays(1))
            {
                if (data.DayOfWeek != DayOfWeek.Sunday)
                    workingDays++;
            }

            return workingDays;
        }

        private RegisteredPacient CalculateRegisteredPacient(IEnumerable<Pacient> pacients, DateTime date)
        {
            return new RegisteredPacient
            {
                Date = date.Date,
                RegisterAmounth = pacients.Where(x => x.RegisterDate.Date == date).Count(),
                UpdateDate = _updateDate
            };
        }

        private async Task<IEnumerable<CommunicationChannel>> CalculateCommunicationChannel(DateTime date)
        {
            var communicationChannels = new List<CommunicationChannel>();

            var pacientsDay = await _podosysRepository.GetPacientByDate(date);

            if (pacientsDay.Any())
            {
                var podosysCommunications = await _podosysRepository.GetAllCommunicationChannel();

                var communicationChannelId = pacientsDay.Where(x => x.CommunicationChannelId != null).Select(x => x.CommunicationChannelId).Distinct();

                foreach (var channel in communicationChannelId)
                {
                    communicationChannels.Add(new CommunicationChannel
                    {
                        Date = date.Date,
                        Channel = podosysCommunications.FirstOrDefault(x => x.Id == channel).Name,
                        Amounth = pacientsDay.Where(x => x.CommunicationChannelId == channel).Count(),
                        UpdateDate = _updateDate
                    });
                }
            }

            return communicationChannels;
        }

        private AgeGroup CalculateAgeGroup(IEnumerable<Pacient> pacients, IEnumerable<MedicalRecord> medicalRecords, DateTime date)
        {
            var procedureBandaid = MedicalRecordProcedureBandaid(medicalRecords);

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
                Elderly = listPacients.Where(x => x.Age >= 60 && x.Age < 150).Count(),
                UpdateDate = _updateDate
            };
        }

        private ProcedurePerformed CalculateProcedurePerformed(DateTime date, IEnumerable<MedicalRecord> medicalRecord)
        {
            var procedureBandaid = MedicalRecordProcedureBandaid(medicalRecord);

            return new ProcedurePerformed
            {
                ProcedureAmount = procedureBandaid.Item1.Count(),
                BandAidProcedureAmount = procedureBandaid.Item2.Count(),
                TotalAmount = procedureBandaid.Item1.Count() + procedureBandaid.Item2.Count(),
                Date = date.Date,
                UpdateDate = _updateDate
            };
        }

        private IEnumerable<ProcedureReport> CalculateProcedure(IEnumerable<Transaction> transactions, IEnumerable<MedicalRecord> medicalRecords)
        {
            var response = new List<ProcedureReport>();

            foreach (var procedure in _procedures)
            {
                var medicalRecord = medicalRecords.Where(x => x.ProcedurePriceId == procedure.Id).Select(x => x.Id).ToList();

                if (medicalRecord is null || !medicalRecord.Any())
                    continue;

                var value = transactions.Where(x => x.MedicalRecordId != null && medicalRecord.Contains((Guid)x.MedicalRecordId)).Sum(x => x.Value);

                var report = new ProcedureReport
                {
                    Date = transactions.FirstOrDefault().Date.Date,
                    Amounth = medicalRecord.Count(),
                    ProcedureName = procedure.Name,
                    Value = value,
                    UpdateDate = _updateDate
                };

                response.Add(report);
            }

            if (response.Count == 0)
            {
                var report = new Models.Reports.ProcedureReport
                {
                    Date = transactions.FirstOrDefault().Date.Date,
                    Amounth = 1,
                    ProcedureName = "Prontuarios Não Fechados",
                    Value = 0,
                    UpdateDate = _updateDate
                };

                response.Add(report);
            }

            return response;
        }

        private IEnumerable<ProfitProfessional> CalculateProfitProfissional(IEnumerable<Professional> professionals, IEnumerable<MedicalRecord> medicalRecords, IEnumerable<Transaction> transactions)
        {
            var profit = new List<ProfitProfessional>();

            var medicalRecordBandaid = MedicalRecordProcedureBandaid(medicalRecords);

            foreach (var professional in professionals)
            {
                var medicalRecord = medicalRecords.Where(x => x.UserId == professional.Id);

                var transactionMedical = transactions.Where(t => medicalRecord.Any(x => x.Id == t.MedicalRecordId));

                var value = transactionMedical.Sum(x => x.Value);

                var saleOff = CalculateTransactionProfissional(transactions, medicalRecord).Result;

                profit.Add(new ProfitProfessional
                {
                    Date = medicalRecord.FirstOrDefault().MedicalRecordDate.Date,
                    Professional = professional.Name,
                    Value = value + saleOff.Item1,
                    SaleOffValue = saleOff.Item1,
                    SaleOffAmount = saleOff.Item2,
                    ProcedureAmount = medicalRecord.Where(x => medicalRecordBandaid.Item1.Contains(x.Id)).Count(),
                    BandaidAmount = medicalRecord.Where(x => medicalRecordBandaid.Item2.Contains(x.Id)).Count(),
                    UpdateDate = _updateDate,
                });
            }

            return profit;
        }

        private async Task<Tuple<decimal, int>> CalculateTransactionProfissional(IEnumerable<Transaction> transactions, IEnumerable<MedicalRecord> medicalRecords)
        {
            var amounth = 0m;

            var transactionMedical = transactions.Where(t => medicalRecords.Any(x => x.Id == t.MedicalRecordId));

            var transactionMedSaleoff = transactionMedical.Where(x => x.PaymentTypeId.Equals((int)PaymentTypeEnum.Pacote));

            var transactionBySaleOff = await _podosysRepository.GetTransactionBySaleOffIds(transactionMedSaleoff.Select(x => x.SaleOffId));

            var saleOffs = await _podosysRepository.GetSaleOffs(transactionBySaleOff?.Where(x => !x.PaymentTypeId.Equals((int)PaymentTypeEnum.Pacote))?.Select(x => x.SaleOffId));

            if (saleOffs is not null)
                foreach (var saleOff in saleOffs)
                {
                    amounth += saleOff.NumberOfSection > 0 ?
                               (transactionBySaleOff.Where(x => x.SaleOffId == saleOff.Id).Sum(x => x.Value) /
                                saleOff.NumberOfSection) : 0;
                }

            return new Tuple<decimal, int>(amounth, saleOffs is null ? 0 : saleOffs.Count());
        }

        private Tuple<IEnumerable<Guid>, IEnumerable<Guid>> MedicalRecordProcedureBandaid(IEnumerable<MedicalRecord> medicalRecords)
        {
            var procedure = medicalRecords.Where(x => x.ProcedurePriceId != (int)ProcedureEnum.CURATIVO).Select(x => x.Id);
            var bandaid = medicalRecords.Where(x => x.ProcedurePriceId == (int)ProcedureEnum.CURATIVO).Select(x => x.Id);

            return new Tuple<IEnumerable<Guid>, IEnumerable<Guid>>(procedure, bandaid);
        }
    }
}