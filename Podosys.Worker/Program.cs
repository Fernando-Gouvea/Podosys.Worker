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


    builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer("name=ConnectionStrings:DefaultConnection"));

    builder.Services.AddTransient<IProfitRepository, PodosysRepository>();
    builder.Services.AddTransient<IUpdateReport, UpdateReport>();

    builder.Services.AddCronJob<TimerUpdateReport>(c => { c.CronExpression = "08 10 * * *"; c.TimeZoneInfo = timeZoneBrasilia; });

    var app = builder.Build();

    app.Run();
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}