using LotSystem.Logger.API;
using System.Threading;
using System.Threading.Tasks;

namespace LotSystem.Repositories;

internal sealed partial class Repository
{
    public Repository(ILogger logger)
    {
        _logger = logger;
        _context = new Database.DatabaseContext(logger);
    }

    private bool _isLocked;
    private readonly ILogger _logger;
    private readonly Database.DatabaseContext _context;

    private async Task EnableLock(CancellationToken cancellationToken)
    {
        while (_isLocked)
            await Task.Delay(10, cancellationToken);

        _isLocked = true;
    }
}

