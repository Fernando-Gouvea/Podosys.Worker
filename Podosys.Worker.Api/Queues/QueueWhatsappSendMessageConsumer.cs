using MassTransit;
using Podosys.Worker.Domain.Models.Podosys;
using Podosys.Worker.Domain.Services;
using PodoSys.Services.ServiceModels;

namespace Podosys.Worker.Api.Queues
{
    public class QueueWhatsappSendMessageConsumer : IConsumer<WhatsappSendMessageModel>
    {
        public async Task Consume(ConsumeContext<WhatsappSendMessageModel> context)
        {
           // await _sendMessage.SendMessageAsync(context.Message.Number, context.Message.Message);
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

