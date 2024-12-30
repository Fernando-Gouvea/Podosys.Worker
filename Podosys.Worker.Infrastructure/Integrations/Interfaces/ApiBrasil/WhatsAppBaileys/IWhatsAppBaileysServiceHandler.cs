using Podosys.Worker.Infrastructure.Integrations.Services.ApiBrasil.WhatsAppBaileys;

namespace Podosys.Worker.Infrastructure.Integrations.Interfaces.ApiBrasil.WhatsAppBaileys
{
    public interface IWhatsAppBaileysServiceHandler
    {
        public Task<Tuple<bool, string>> PostMessageAsync(WhatsAppBaileysServiceRequest request);
    }
}
