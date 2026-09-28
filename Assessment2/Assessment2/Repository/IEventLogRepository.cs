using Assessment2.Models;

namespace Assessment2.Repository
{
    /// <summary>
    /// Interface for the event log repository, providing methods to manage event logs.
    /// </summary>
    internal interface IEventLogRepository
    {
        /// <summary>
        /// Ensures that the event log file exists.
        /// </summary>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        Task EnsureFile(CancellationToken cancellationToken);

        /// <summary>
        /// Adds a new event log entry.
        /// </summary>
        /// <param name="eventLog">The event log entry to add.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        Task Add(EventLog eventLog, CancellationToken cancellationToken);

        /// <summary>
        /// Retrieves all event log entries.
        /// </summary>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A read-only list of all event log entries.</returns>
        Task<IReadOnlyList<EventLog>> GetAll(CancellationToken cancellationToken);
    }
}
