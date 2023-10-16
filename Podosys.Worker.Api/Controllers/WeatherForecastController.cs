using Microsoft.AspNetCore.Mvc;
using Podosys.Worker.Domain.Services;

namespace Podosys.Worker.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IUpdateReport _updateReport;

        public WeatherForecastController(IUpdateReport updateReport)
        {
            _updateReport = updateReport;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<string> Get()
        {
            var file = "";

            try
            {
                //await _updateReport.UpdateReportAsync();
                var stream = new StreamReader("cronjob-status-info-TimerUpdateReport.txt");

                string? line;

                while ((line = stream.ReadLine()) != null)
                    file += line;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            return file;
        }
    }
}