using Podosys.Worker.Api.Extensions;
using Podosys.Worker.Domain.Services.ProcedurePriceTable;

namespace Podosys.Worker.Api.Workers
{
    public class TimerFeedbackReport : CronJobExtensions
    {

        public TimerFeedbackReport(IScheduleConfig<TimerFeedbackReport> config, IServiceProvider serviceProvider)
            : base(config.CronExpression, config.TimeZoneInfo, serviceProvider, nameof(TimerFeedbackReport))
        {

        }
        public override async Task<Task> DoWork(IServiceScope scope, CancellationToken cancellationToken)
        {
            try
            {
                var svc = scope.ServiceProvider.GetRequiredService<IFeedbackReportService>();
                await svc.FeedbackReportAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message, $"Erro em {nameof(TimerFeedbackReport)}");
            }

            return Task.CompletedTask;
        }
    }
}
