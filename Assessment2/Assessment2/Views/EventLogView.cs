using Assessment2.Models;

namespace Assessment2.Views
{
    /// <summary>
    /// EventLogView is responsible for displaying the event logs in the console.
    /// </summary>
    internal class EventLogView
    {
        private readonly ConsoleIO _consoleOperations;

        /// <summary>
        /// Initializes a new instance of the EventLogView class with the specified console operations.
        /// </summary>
        /// <param name="consoleOperations">The console operations to be used for displaying logs.</param>
        public EventLogView(ConsoleIO consoleOperations)
        {
            _consoleOperations = consoleOperations;
        }

        /// <summary>
        /// Displays the event logs in the console.
        /// </summary>
        /// <param name="logs">Logs</param>
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

        /// <summary>
        /// Waits for the user to press the Escape key before returning to the previous view.
        /// </summary>
        public void WaitForEscape()
        {
            while (Console.ReadKey(true).Key != ConsoleKey.Escape)
            {
            }
        }
    }
}
