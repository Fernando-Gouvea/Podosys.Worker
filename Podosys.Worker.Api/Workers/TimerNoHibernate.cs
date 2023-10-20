using Podosys.Worker.Api.Extensions;
using Podosys.Worker.Domain.Services;
using RestSharp;

namespace Podosys.Worker.Api.Workers
{
    public class TimerNoHibernate : CronJobExtensions
    {

        public TimerNoHibernate(IScheduleConfig<TimerNoHibernate> config, IServiceProvider serviceProvider)
            : base(config.CronExpression, config.TimeZoneInfo, serviceProvider, nameof(TimerNoHibernate))
        {

        }
        public override async Task<Task> DoWork(IServiceScope scope, CancellationToken cancellationToken)
        {
            try
            {
                var client = new RestClient("http://fernandogouvea-001-site2.gtempurl.com");

                var requests = new RestRequest("/WeatherForecast", Method.Get);

                RestResponse response = client.Execute(requests);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message, $"Erro em {nameof(TimerNoHibernate)}");
            }

            return Task.CompletedTask;
        }
    }
}
