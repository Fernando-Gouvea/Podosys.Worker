using MassTransit;
using Podosys.Worker.Domain.Services;
using PodoSys.Services.ServiceModels;

namespace Podosys.Worker.Api.Queues
{
    public class QueueWhatsappSendMessageConsumer : IConsumer<WhatsappSendMessageModel>
    {
        private readonly ISendMessage _sendMessage;
        public QueueWhatsappSendMessageConsumer(ISendMessage sendMessage)
        {
            _sendMessage = sendMessage;

        }

        public async Task Consume(ConsumeContext<WhatsappSendMessageModel> context)
        {
            await _sendMessage.SendMessageAsync(context.Message.Number, context.Message.Message);
        }
    }

    public class QueueWhatsappSendMessageConsumerDefinition : ConsumerDefinition<QueueWhatsappSendMessageConsumer>
    {
        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<QueueWhatsappSendMessageConsumer> consumerConfigurator, IRegistrationContext context)
        {
            consumerConfigurator.UseMessageRetry(retry => retry.Interval(3, TimeSpan.FromSeconds(30)));
        }
    }
}

