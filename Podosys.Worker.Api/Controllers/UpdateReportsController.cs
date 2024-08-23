using Microsoft.AspNetCore.Mvc;
using Podosys.Worker.Domain.Services.Update;

namespace Podosys.Worker.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UpdateReportsController : ControllerBase
    {
        private readonly IUpdateReport _updateReport;

        public UpdateReportsController(IUpdateReport updateReport)
        {
            _updateReport = updateReport;
        }

        [HttpGet("WorkerStatus/{id}")]
        public async Task<IActionResult> WorkerStatus(string id)
        {
            if (!id.Equals("6e17f771-dedd-41c9-8fdb-9184e4265959"))
                return Unauthorized();

            var file = "";

            try
            {
                var stream = new StreamReader("cronjob-status-info-TimerUpdateReportCurrentDay.txt");

                string? line;

                while ((line = stream.ReadLine()) != null)
                    file += line;

                file += $"//TimerUpdateReportCurrentDay//--//";

                stream = new StreamReader("cronjob-status-info-TimerUpdateReportLastDay.txt");

                while ((line = stream.ReadLine()) != null)
                    file += line;

                file += $"//TimerUpdateReportCurrentDay//hora servidor(BR): {DateTime.Now.AddHours(4)}";
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }

            return Ok(file);
        }

        [HttpGet("UpdateReportByDate/{id}")]
        public async Task<IActionResult> UpdateReportByDate([FromRoute] string id, DateTime? firstDate, DateTime? lastDate)
        {
            if (!id.Equals("93418540-0d7c-48c9-8076-a5e16bd5be42"))
                return Unauthorized();

            if (firstDate == null || lastDate == null)
            {
                firstDate = DateTime.Now;
                lastDate = DateTime.Now;
            }

            await _updateReport.UpdateReportAsync((DateTime)firstDate, (DateTime)lastDate);

            return Ok();
        }
    }
}