using Assessment2.Models;

namespace Assessment2.Views
{
    internal class EventLogView
    {
        private readonly ConsoleIO _consoleOperations;

        public EventLogView(ConsoleIO consoleOperations)
        {
            _consoleOperations = consoleOperations;
        }

        public void DisplayLogs(IReadOnlyList<EventLog> logs)
        {
            _consoleOperations.ClearScreen();
            _consoleOperations.DisplayHeading("===== BOILER EVENT LOG =====");

            if (logs.Count == 0)
            {
                Console.WriteLine("No event logs available.");
            }
            else
            {
                foreach (EventLog log in logs)
                {
                    string line =
                        $"[{log.Timestamp:yyyy-MM-dd HH:mm:ss}] " +
                        $"#{log.LogId} | {log.Type,-7} | " +
                        $"{log.FromStatus,-11} | {log.Message}";

                    _consoleOperations.DisplayLogLine(line, log.Type);
                }
            }

            Console.WriteLine();
            Console.WriteLine("Press ESC to return.");
        }

        public void WaitForEscape()
        {
            while (Console.ReadKey(true).Key != ConsoleKey.Escape)
            {
            }
        }
    }
}
