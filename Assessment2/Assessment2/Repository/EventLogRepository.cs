using System.Globalization;
using Assessment2.Enums;
using Assessment2.FileHandler;
using Assessment2.Models;

namespace Assessment2.Repository
{
    internal class EventLogRepository : IEventLogRepository
    {
        private const string FileName = "BoilerLog.txt";
        private const string Header = "LogId,Timestamp,LogType,LogFromStatus,LogMessage";

        private readonly IFileHandler _fileHandler;

        public EventLogRepository(IFileHandler fileHandler)
        {
            _fileHandler = fileHandler;
        }

        public async Task EnsureFile(CancellationToken cancellationToken)
        {
            await _fileHandler.EnsureFile(FileName, Header, cancellationToken);
        }

        public async Task Add(EventLog eventLog, CancellationToken cancellationToken)
        {
            await EnsureFile(cancellationToken);
            string line = string.Join(",",
                eventLog.LogId.ToString(CultureInfo.InvariantCulture),
                eventLog.Timestamp.ToString("O", CultureInfo.InvariantCulture),
                eventLog.Type,
                eventLog.FromStatus,
                EscapeCsvValue(eventLog.Message));
            await _fileHandler.AppendLine(FileName, line, cancellationToken);
        }

        public async Task<IReadOnlyList<EventLog>> GetAll(CancellationToken cancellationToken)
        {
            await EnsureFile(cancellationToken);
            IReadOnlyList<string> lines = await _fileHandler.ReadAllLines(FileName, cancellationToken);
            List<EventLog> logs = [];

            foreach (string line in lines.Skip(1))
            {
                if (!string.IsNullOrWhiteSpace(line))
                {
                    logs.Add(ParseLog(line));
                }
            }

            return logs;
        }

        private static EventLog ParseLog(string line)
        {
            string[] values = ParseCsvLine(line);
            if (values.Length < 5)
            {
                throw new FormatException("Invalid event log format.");
            }

            return new EventLog
            {
                LogId = int.Parse(values[0], CultureInfo.InvariantCulture),
                Timestamp = DateTime.Parse(
                    values[1],
                    CultureInfo.InvariantCulture),
                Type = Enum.Parse<LogType>(values[2]),
                FromStatus = Enum.Parse<BoilerStatus>(values[3]),
                Message = values[4]
            };
        }

        private static string EscapeCsvValue(string value)
        {
            string escapedValue = value.Replace("\"", "\"\"");
            return $"\"{escapedValue}\"";
        }

        private static string[] ParseCsvLine(string line)
        {
            List<string> values = [];
            List<char> currentValue = [];
            bool insideQuotes = false;
            for (int index = 0; index < line.Length; index++)
            {
                char character = line[index];
                if (character == '"')
                {
                    if (insideQuotes && index + 1 < line.Length && line[index + 1] == '"')
                    {
                        currentValue.Add('"');
                        index++;
                    }
                    else
                    {
                        insideQuotes = !insideQuotes;
                    }

                    continue;
                }

                if (character == ',' && !insideQuotes)
                {
                    values.Add(new string(currentValue.ToArray()));
                    currentValue.Clear();
                    continue;
                }

                currentValue.Add(character);
            }

            values.Add(new string(currentValue.ToArray()));
            return values.ToArray();
        }
    }
}
