using Cronos;
using System.Reflection;

namespace Podosys.Worker.Extensions;

public abstract class CronJobExtensions : BackgroundService
{
    private readonly CronExpression _expression;
    private readonly TimeZoneInfo _timeZoneInfo;
    private readonly IServiceProvider _serviceProvider;
    private readonly string _jobName;

    public DateTimeOffset? DateNextOccurrence { get; private set; }

    protected CronJobExtensions(
        string cronExpression,
        TimeZoneInfo timeZoneInfo,
        IServiceProvider serviceProvider,
        string jobName,
        CronFormat format = CronFormat.Standard
        )
    {
        _expression = CronExpression.Parse(cronExpression, format);
        _timeZoneInfo = timeZoneInfo;
        _serviceProvider = serviceProvider;
        _jobName = jobName;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            var next = _expression.GetNextOccurrence(DateTimeOffset.Now, _timeZoneInfo);

            if (!next.HasValue)
                continue;

            DateNextOccurrence = next;

            await EscreverCronJobStatusInfo("Em espera");

            var delay = next.Value - TimeZoneInfo.ConvertTime(DateTimeOffset.Now, _timeZoneInfo);

            await Task.Delay(delay, cancellationToken);

            if (cancellationToken.IsCancellationRequested)
                continue;

            try
            {
                await EscreverCronJobStatusInfo("Em execução");

                using var scope = _serviceProvider.CreateScope();
                await DoWork(scope, cancellationToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }

    public abstract Task DoWork(IServiceScope scope, CancellationToken cancellationToken);

    [Obsolete("Obsolete")]
    private async Task EscreverCronJobStatusInfo(string status)
    {
        try
        {
            var location = new Uri(Assembly.GetEntryAssembly()?.GetName().CodeBase);

            await using var outputFile = File.CreateText(string.Concat(new FileInfo(location.AbsolutePath).Directory?.FullName, $"\\cronjob-status-info-{_jobName}.txt"));
            await outputFile.WriteAsync($"{status}|{DateNextOccurrence:dd/MM/yyyy HH:mm}");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}

public interface IScheduleConfig<T>
{
    string CronExpression { get; set; }
    TimeZoneInfo TimeZoneInfo { get; set; }
}

public class ScheduleConfig<T> : IScheduleConfig<T>
{
    public string CronExpression { get; set; }
    public TimeZoneInfo TimeZoneInfo { get; set; } = TimeZoneInfo.Local;
}

public static class ScheduledServiceExtensions
{
    public static IServiceCollection AddCronJob<T>(this IServiceCollection services, Action<IScheduleConfig<T>> options) where T : CronJobExtensions
    {
        if (options == null)
        {
            throw new ArgumentNullException(nameof(options), "Please provide Schedule Configurations.");
        }

        var config = new ScheduleConfig<T>();
        options.Invoke(config);

        if (string.IsNullOrWhiteSpace(config.CronExpression))
        {
            throw new ArgumentNullException(nameof(ScheduleConfig<T>.CronExpression), "Empty Cron Expression is not allowed.");
        }

        services.AddSingleton<IScheduleConfig<T>>(config);

        services.AddHostedService<T>();

        return services;
    }
}