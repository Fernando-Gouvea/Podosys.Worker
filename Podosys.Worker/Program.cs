using Microsoft.EntityFrameworkCore;
using Podosys.Worker.Domain.Repositories;
using Podosys.Worker.Domain.Services;
using Podosys.Worker.Extensions;
using Podosys.Worker.Persistence.Context;
using Podosys.Worker.Persistence.Repositories;
using Podosys.Worker.Workers;

try
{
    var builder = WebApplication.CreateBuilder(args);

    var timeZoneBrasilia = TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");

    builder.Services.AddDbContext<IApplicationDbContext, ApplicationDbContext>(options => options.UseSqlServer("name=ConnectionStrings:DefaultConnection"));

    builder.Services.AddTransient<IReportRepository, ReportRepository>();
    builder.Services.AddTransient<IPodosysRepository, PodosysRepository>();
    builder.Services.AddTransient<IUpdateReport, UpdateReport>();

    builder.Services.AddCronJob<TimerUpdateReport>(c => { c.CronExpression = "00 05 * * *"; c.TimeZoneInfo = timeZoneBrasilia; });

    var app = builder.Build();

    app.Run();
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}