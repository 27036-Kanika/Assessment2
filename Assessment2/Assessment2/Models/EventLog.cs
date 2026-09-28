using Assessment2.Enums;

namespace Assessment2.Models
{
    /// <summary>
    /// EventLog class represents a log entry for the boiler system, containing information about the log type, timestamp, and associated message.
    /// </summary>
    internal class EventLog
    {
        /// <summary>
        /// Gets or sets the unique identifier for the log entry.
        /// </summary>
        public int LogId { get; set; }

        /// <summary>
        /// Gets or sets the timestamp of the log entry.
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Gets or sets the type of the log entry.
        /// </summary>
        public LogType Type { get; set; }

        /// <summary>
        /// Gets or sets the previous status of the boiler.
        /// </summary>
        public BoilerStatus FromStatus { get; set; }

        /// <summary>
        /// Gets or sets the message associated with the log entry.
        /// </summary>
        public string Message { get; set; } = string.Empty;
    }
}
