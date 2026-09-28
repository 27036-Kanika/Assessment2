using Assessment2.Enums;

namespace Assessment2.Models
{
    /// <summary>
    /// NotificationEventArgs class represents the event arguments for a notification event, containing the message, log type, and timestamp.
    /// </summary>
    internal class NotificationEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the NotificationEventArgs class with the specified message and log type, and sets the timestamp to the current time.
        /// </summary>
        /// <param name="message">The notification message.</param>
        /// <param name="type">The type of the log.</param>
        public NotificationEventArgs(string message, LogType type)
        {
            Message = message;
            Type = type;
            Timestamp = DateTime.Now;
        }

        public string Message { get; }

        public LogType Type { get; }

        public DateTime Timestamp { get; }

    }
}
