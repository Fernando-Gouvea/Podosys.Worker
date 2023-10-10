using Podosys.Worker.Domain.Repositories;

namespace Podosys.Worker.Domain.Services
{
    public class UpdateReport : IUpdateReport
    {
        private readonly IPodosysRepository _podosysRepository;

        public UpdateReport(IPodosysRepository podosysRepository)
        {
            _podosysRepository = podosysRepository;
        }

        public async void UpdateReportAsync()
        {
            var firstdate = DateTime.Now.AddDays(-60);
            var lastdate = DateTime.Now;

            await _podosysRepository.GetMedicalRecord(firstdate, lastdate);

        }

    }
}