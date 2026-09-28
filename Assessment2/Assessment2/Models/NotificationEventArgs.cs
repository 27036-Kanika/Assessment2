using Assessment2.Enums;

namespace Assessment2.Models
{
    internal class NotificationEventArgs
    {
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
