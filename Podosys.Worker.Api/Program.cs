using Microsoft.EntityFrameworkCore;
using Podosys.Worker.Api.Extensions;
using Podosys.Worker.Api.Workers;
using Podosys.Worker.Domain.Repositories;
using Podosys.Worker.Domain.Services;
using Podosys.Worker.Persistence.Context;
using Podosys.Worker.Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<IApplicationDbContext, ApplicationDbContext>(options => options.UseSqlServer("name=ConnectionStrings:DefaultConnection"));

var timeZoneBrasilia = TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");

builder.Services.AddTransient<IReportRepository, ReportRepository>();
builder.Services.AddTransient<IPodosysRepository, PodosysRepository>();
builder.Services.AddTransient<IUpdateReport, UpdateReport>();

builder.Services.AddCronJob<TimerUpdateReportLastDay>(c => { c.CronExpression = "0 6 * * *"; c.TimeZoneInfo = timeZoneBrasilia; });
builder.Services.AddCronJob<TimerUpdateReportCurrentDay>(c => { c.CronExpression = "0 10,12,14,16,18,20,22 * * *"; c.TimeZoneInfo = timeZoneBrasilia; });
//builder.Services.AddCronJob<TimerNoHibernate>(c => { c.CronExpression = "*/10 * * * *"; c.TimeZoneInfo = timeZoneBrasilia; });

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
