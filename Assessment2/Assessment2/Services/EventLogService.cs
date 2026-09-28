using Assessment2.Enums;
using Assessment2.Models;
using Assessment2.Repository;

namespace Assessment2.Services
{
    internal class EventLogService : IEventLogService
    {
        private readonly IEventLogRepository _eventLogRepository;
        private int _nextLogId = 1;

        public EventLogService(IEventLogRepository eventLogRepository)
        {
            _eventLogRepository = eventLogRepository;
        }

        public async Task InitializeAsync(CancellationToken cancellationToken)
        {
            IReadOnlyList<EventLog> logs = await _eventLogRepository.GetAll(cancellationToken);
            _nextLogId = logs.Count == 0 ? 1 : logs.Max(log => log.LogId) + 1;
        }

        public async Task Log(
            LogType type,
            BoilerStatus status,
            string message,
            CancellationToken cancellationToken)
        {
            EventLog eventLog = new()
            {
                LogId = _nextLogId++,
                Timestamp = DateTime.Now,
                Type = type,
                FromStatus = status,
                Message = message
            };

            await _eventLogRepository.Add(eventLog, cancellationToken);
        }

        public async Task<IReadOnlyList<EventLog>> GetAllLogs(CancellationToken cancellationToken)
        {
            return await _eventLogRepository.GetAll(cancellationToken);
        }
    }

}
