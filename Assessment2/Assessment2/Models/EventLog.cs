using Assessment2.Enums;

namespace Assessment2.Models
{
    internal class EventLog
    {
        public int LogId { get; set; }

        public DateTime Timestamp { get; set; }

        public LogType Type { get; set; }

        public BoilerStatus FromStatus { get; set; }

        public string Message { get; set; } = string.Empty;
    }
}
