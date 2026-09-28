using Assessment2.Models;

namespace Assessment2.Repository
{
    internal interface IEventLogRepository
    {
        Task EnsureFile(CancellationToken cancellationToken);

        Task Add(EventLog eventLog, CancellationToken cancellationToken);

        Task<IReadOnlyList<EventLog>> GetAll(CancellationToken cancellationToken);
    }
}
