using Podosys.Worker.Infrastructure.Integrations.Interfaces.ApiBrasil.WhatsAppBaileys;
using Podosys.Worker.Infrastructure.Integrations.Services.ApiBrasil.WhatsAppBaileys;
using System;
using System.Threading.Tasks;

namespace Podosys.Worker.Domain.Services
{
    public class SendMessage : ISendMessage
    {
        private readonly IWhatsAppBaileysServiceHandler _whatsAppBaileysService;

        public SendMessage(IWhatsAppBaileysServiceHandler whatsAppBaileysServiceHandler)
        {
            _whatsAppBaileysService = whatsAppBaileysServiceHandler;    
        }

        public async Task SendMessageAsync(string number, string message)
        {
            await _whatsAppBaileysService.PostMessageAsync(new WhatsAppBaileysServiceRequest
            {
                Id = Guid.NewGuid(),
                Number = number,
                TextMessage = new TextMessage
                {
                    Text = message
                }
            });


        }
    }
}