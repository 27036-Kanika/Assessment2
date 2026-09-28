using Assessment2.Enums;
using Assessment2.Models;

namespace Assessment2.Services
{
    /// <summary>
    /// Interface for the event log service that provides methods to log events and retrieve logs.
    /// </summary>
    internal interface IEventLogService
    {
        /// <summary>
        /// Initializes the event log service, preparing it for logging events.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        Task InitializeAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Logs an event with the specified type, status, and message.
        /// </summary>
        Task Log(LogType type, BoilerStatus status, string message, CancellationToken cancellationToken);

        /// <summary>
        /// Gets all logged events as a read-only list.
        /// </summary>
        Task<IReadOnlyList<EventLog>> GetAllLogs(CancellationToken cancellationToken);
    }
}
