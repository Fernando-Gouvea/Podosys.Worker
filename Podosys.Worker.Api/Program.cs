using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Podosys.Worker.Api.Extensions;
using Podosys.Worker.Api.Queues;
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
builder.Services.AddCronJob<TimerUpdateReportCurrentDay>(c => { c.CronExpression = "0 10,11,12,13,14,15,16,17,18,19,20,21,22,23 * * *"; c.TimeZoneInfo = timeZoneBrasilia; });
//builder.Services.AddCronJob<TimerUpdateReportCurrentDay>(c => { c.CronExpression = "* * * * *"; c.TimeZoneInfo = timeZoneBrasilia; });
//builder.Services.AddCronJob<TimerNoHibernate>(c => { c.CronExpression = "*/10 * * * *"; c.TimeZoneInfo = timeZoneBrasilia; });

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure(x =>x);

builder.Services.AddMassTransit(x =>
{
    x.AddDelayedMessageScheduler();
    x.AddConsumer<QueueWhatsappSendMessageConsumer>(typeof(QueueWhatsappSendMessageConsumerDefinition));

    x.SetKebabCaseEndpointNameFormatter();

    x.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host(Configuration.GetConnectionString("RabbitMq"));
        cfg.UseDelayedMessageScheduler();
        cfg.ServiceInstance(instance =>
        {
            instance.ConfigureJobServiceEndpoints();
            instance.ConfigureEndpoints(ctx, new KebabCaseEndpointNameFormatter("dev", false));

        });
    });
});

// builder.Services.AddDbContext<IApplicationDbContext, ApplicationDbContext>((context, collection) =>
//var host = Host.CreateDefaultBuilder(args)



//      .ConfigureServices((context, collection) =>
//      {
//          collection.AddHttpContextAccessor();

//          collection.AddMassTransit(x =>
//          {
//              x.AddDelayedMessageScheduler();
//              x.AddConsumer<QueueWhatsappSendMessageConsumer>(typeof(QueueWhatsappSendMessageConsumerDefinition));

//              x.SetKebabCaseEndpointNameFormatter();

//              x.UsingRabbitMq((ctx, cfg) =>
//              {
//                  cfg.Host(context.Configuration.GetConnectionString("RabbitMq"));
//                  cfg.UseDelayedMessageScheduler();
//                  cfg.ServiceInstance(instance =>
//                  {
//                      instance.ConfigureJobServiceEndpoints();
//                      instance.ConfigureEndpoints(ctx, new KebabCaseEndpointNameFormatter("dev", false));

//                  });
//              });
//          });
//      }).Build();








var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
