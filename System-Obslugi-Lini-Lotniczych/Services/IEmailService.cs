using System.Threading;
using System.Threading.Tasks;

namespace LotSystem.Services;

public interface IEmailService
{
    Task SendEmail(string email, string title, string content, CancellationToken cancellationToken = default);
}