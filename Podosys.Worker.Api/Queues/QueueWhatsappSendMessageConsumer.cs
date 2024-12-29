using MassTransit;
using MassTransit.Metadata;
using PodoSys.Services.ServiceModels;
using System.Diagnostics;

namespace Podosys.Worker.Api.Queues
{
    public class QueueWhatsappSendMessageConsumer : IConsumer<WhatsappSendMessageModel>
    {
        public async Task Consume(ConsumeContext<WhatsappSendMessageModel> context)
        {
           
        }
    }

    public class QueueWhatsappSendMessageConsumerDefinition : ConsumerDefinition<QueueWhatsappSendMessageConsumer>
    {
        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<QueueWhatsappSendMessageConsumer> consumerConfigurator, IRegistrationContext context)
        {
            consumerConfigurator.UseMessageRetry(retry => retry.Interval(3, TimeSpan.FromSeconds(3)));
        }
    }
}

