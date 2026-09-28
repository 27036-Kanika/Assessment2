using Assessment2.Enums;
using Assessment2.Models;

namespace Assessment2.Services
{
    internal interface IEventLogService
    {
        Task InitializeAsync(CancellationToken cancellationToken);

        Task Log(LogType type, BoilerStatus status, string message, CancellationToken cancellationToken);

        Task<IReadOnlyList<EventLog>> GetAllLogs(CancellationToken cancellationToken);
    }
}
