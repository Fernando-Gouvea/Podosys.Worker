using Podosys.Worker.Api.Extensions;
using Podosys.Worker.Domain.Services;

namespace Podosys.Worker.Api.Workers
{
    public class TimerUpdateReportCurrentDay : CronJobExtensions
    {

        public TimerUpdateReportCurrentDay(IScheduleConfig<TimerUpdateReportCurrentDay> config, IServiceProvider serviceProvider)
            : base(config.CronExpression, config.TimeZoneInfo, serviceProvider, nameof(TimerUpdateReportCurrentDay))
        {

        }
        public override async Task<Task> DoWork(IServiceScope scope, CancellationToken cancellationToken)
        {
            try
            {
                var svc = scope.ServiceProvider.GetRequiredService<IUpdateReport>();
                await svc.UpdateReportAsync(DateTime.Now.Date, DateTime.Now.Date.AddDays(1));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message, $"Erro em {nameof(TimerUpdateReportCurrentDay)}");
            }

            return Task.CompletedTask;
        }
    }
}
