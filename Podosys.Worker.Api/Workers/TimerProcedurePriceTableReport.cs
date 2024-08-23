using Podosys.Worker.Api.Extensions;
using Podosys.Worker.Domain.Services.ProcedurePriceTable;

namespace Podosys.Worker.Api.Workers
{
    public class TimerProcedurePriceTableReport : CronJobExtensions
    {

        public TimerProcedurePriceTableReport(IScheduleConfig<TimerProcedurePriceTableReport> config, IServiceProvider serviceProvider)
            : base(config.CronExpression, config.TimeZoneInfo, serviceProvider, nameof(TimerProcedurePriceTableReport))
        {

        }
        public override async Task<Task> DoWork(IServiceScope scope, CancellationToken cancellationToken)
        {
            try
            {
                var svc = scope.ServiceProvider.GetRequiredService<IProcedurePriceTableReport>();
                await svc.ProcedurePriceReportAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message, $"Erro em {nameof(TimerProcedurePriceTableReport)}");
            }

            return Task.CompletedTask;
        }
    }
}
