using System.Threading.Tasks;

namespace Podosys.Worker.Domain.Services
{
    public interface ISendMessage
    {
        Task SendMessageAsync(string number, string message);
    }
}
