using System.Threading.Tasks;
using MSS.PlasticSurgery.Services.Models;

namespace MSS.PlasticSurgery.Services
{
    public interface IEmailService
    {
        void SendAsync(EmailMessage message);
    }
}
