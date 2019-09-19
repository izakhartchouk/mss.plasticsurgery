using System.Threading.Tasks;
using MSS.PlasticSurgery.Services.Models;

namespace MSS.PlasticSurgery.Services
{
    public interface IEmailService
    {
        Task SendAsync(EmailMessage message);
    }
}
