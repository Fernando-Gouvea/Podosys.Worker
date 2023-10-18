using Podosys.Worker.Api.Extensions;
using Podosys.Worker.Domain.Services;

namespace Podosys.Worker.Api.Workers
{
    public class TimerUpdateReportLastDay : CronJobExtensions
    {

        public TimerUpdateReportLastDay(IScheduleConfig<TimerUpdateReportLastDay> config, IServiceProvider serviceProvider)
            : base(config.CronExpression, config.TimeZoneInfo, serviceProvider, nameof(TimerUpdateReportLastDay))
        {

        }
        public override async Task<Task> DoWork(IServiceScope scope, CancellationToken cancellationToken)
        {
            try
            {
                var svc = scope.ServiceProvider.GetRequiredService<IUpdateReport>();
                await svc.UpdateReportAsync(DateTime.Now.Date.AddDays(-1), DateTime.Now.Date);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message, $"Erro em {nameof(TimerUpdateReportLastDay)}");
            }

            return Task.CompletedTask;
        }
    }
}
