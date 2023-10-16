using Podosys.Worker.Api.Extensions;
using Podosys.Worker.Domain.Services;

namespace Podosys.Worker.Api.Workers
{
    public class TimerUpdateReport : CronJobExtensions
    {

        public TimerUpdateReport(IScheduleConfig<TimerUpdateReport> config, IServiceProvider serviceProvider)
            : base(config.CronExpression, config.TimeZoneInfo, serviceProvider, nameof(TimerUpdateReport))
        {

        }
        public override async Task<Task> DoWork(IServiceScope scope, CancellationToken cancellationToken)
        {
            try
            {
                var svc = scope.ServiceProvider.GetRequiredService<IUpdateReport>();
                await svc.UpdateReportAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message, $"Erro em {nameof(TimerUpdateReport)}");
            }

            return Task.CompletedTask;
        }
    }
}
