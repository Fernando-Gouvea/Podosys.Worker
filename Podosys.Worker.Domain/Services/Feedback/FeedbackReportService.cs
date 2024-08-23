using Podosys.Worker.Domain.Models.Podosys;
using Podosys.Worker.Domain.Repositories;

namespace Podosys.Worker.Domain.Services.ProcedurePriceTable
{
    public class FeedbackReportService : IFeedbackReportService
    {
        private readonly IPodosysRepository _podosysRepository;
        private readonly IReportRepository _reportRepositoty;
        private readonly DateTime _updateDate = DateTime.Now.AddHours(4);

        public FeedbackReportService(IPodosysRepository podosysRepository,
                                         IReportRepository reportRepositoty)
        {
            _podosysRepository = podosysRepository;
            _reportRepositoty = reportRepositoty;
        }

        public async Task FeedbackReportAsync()
        {
            var feedback = await _podosysRepository.GetFeedback(DateTime.Now);

            var reports = new List<FeedbackReport>();

            foreach (var item in feedback)
            {
                reports.Add(new FeedbackReport
                {
                    Criticism = item.Criticism?.Type,
                    Date = item.Date,
                    User = item.User?.Name,
                    UpdateDate = _updateDate,

                });
            }



        }
    }
}