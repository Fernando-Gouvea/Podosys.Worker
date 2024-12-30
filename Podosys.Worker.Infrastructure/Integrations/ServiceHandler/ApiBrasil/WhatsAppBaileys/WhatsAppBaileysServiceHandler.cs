using Microsoft.Extensions.Configuration;
using Podosys.Worker.Infrastructure.Integrations.Interfaces.ApiBrasil.WhatsAppBaileys;
using Podosys.Worker.Infrastructure.Integrations.Services.ApiBrasil.WhatsAppBaileys;
using RestSharp;
using System.Text.RegularExpressions;

namespace Podosys.Worker.Infrastructure.Integrations.ServiceHandler.ApiBrasil.WhatsAppBaileys
{
    public class WhatsAppBaileysServiceHandler : IWhatsAppBaileysServiceHandler
    {
        private readonly IConfiguration _configuration;
        public WhatsAppBaileysServiceHandler(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<Tuple<bool, string>> PostMessageAsync(WhatsAppBaileysServiceRequest request)
        {
            var whatsappServiceResponse = new WhatsAppBaileysServiceResponse();

            RestResponse response = new RestResponse();

            if (!string.IsNullOrEmpty(request.Number))
            {
                request.Number = "55" + Regex.Replace(request.Number, @"[-_/.(),;:]", "");

                var client = new RestClient(_configuration["apiBrasilSettings:url"]);

                var requests = new RestRequest("api/v2/evolution/message/sendText", Method.Post);
                requests.AddHeader("Authorization", "Bearer " + _configuration["apiBrasilSettings:subscriptionKey"]);
                requests.AddHeader("DeviceToken", _configuration["apiBrasilSettings:deviceKey"]);
                requests.AddHeader("Content-Type", "application/json");
                requests.AddParameter("application/json", request, ParameterType.RequestBody);
                requests.Timeout = 30000;

                response = client.Execute(requests);
            }

            return new Tuple<bool, string>(response.StatusCode == System.Net.HttpStatusCode.OK || response.StatusCode == System.Net.HttpStatusCode.Created, response?.Content?.ToString());
        }
    }
}