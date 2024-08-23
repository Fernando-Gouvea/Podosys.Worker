using Podosys.Worker.Domain.Models.Reports;
using Podosys.Worker.Domain.Repositories;

namespace Podosys.Worker.Domain.Services.ProcedurePriceTable
{
    public class ProcedurePriceTableReport : IProcedurePriceTableReport
    {
        private readonly IPodosysRepository _podosysRepository;
        private readonly IReportRepository _reportRepositoty;
        private readonly DateTime _updateDate = DateTime.Now.AddHours(4);

        public ProcedurePriceTableReport(IPodosysRepository podosysRepository,
                                         IReportRepository reportRepositoty)
        {
            _podosysRepository = podosysRepository;
            _reportRepositoty = reportRepositoty;
        }

        public async Task ProcedurePriceReportAsync()
        {

            var procedurePrices = await _podosysRepository.GetProcedurePrices();

            var procedurePriceReport = new List<ProcedurePriceReport>();

            foreach (var procedurePrice in procedurePrices)
            {
                foreach (var value in procedurePrice.ProcedurePricesValues)
                {
                    procedurePriceReport.Add(new ProcedurePriceReport
                    {
                        Date = value.Date,
                        ProcedureName = $"{procedurePrice.Name}({value.Observation})",
                        Value = value.PriceMin,
                        UpdateDate = _updateDate

                    });
                }
            }



        }
    }
}