using Assessment2.Enums;

namespace Assessment2.Views
{
    /// <summary>
    /// Handles the user interaction and console operations
    /// </summary>
    internal class ConsoleIO
    {
        private const int NotificationColumn = 0;
        private const int StatusColumn = 0;
        private const int TimerColumn = 0;
        private const int MenuColumn = 0;

        private const int NotificationRow = 1;
        private const int StatusRow = 4;
        private const int TimerRow = 6;
        private const int MenuRow = 9;

        /// <summary>
        /// Displays heading
        /// </summary>
        /// <param name="heading">Heading</param>
        public void DisplayHeading(string heading)
        {
            Console.SetCursorPosition(0, 0);
            Console.Write(new string(' ', Console.WindowWidth - 1));
            Console.SetCursorPosition(0, 0);
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine(heading);
            Console.ResetColor();
        }

        /// <summary>
        /// Displays a notification message at the top of the console window with appropriate color based on the log type
        /// </summary>
        public void DisplayNotification(string message, LogType type)
        {
            Console.SetCursorPosition(NotificationColumn, NotificationRow);
            ClearLine();
            Console.ForegroundColor = GetNotificationColor(type);
            Console.Write(message);
            Console.ResetColor();
        }

        /// <summary>
        /// Displays the current status of the boiler and the interlock state at a specific position in the console window
        /// </summary>
        /// <param name="status">The current status of the boiler.</param>
        /// <param name="interlock">The current interlock state of the boiler.</param>
        public void DisplayStatus(BoilerStatus status, InterlockState interlock)
        {
            Console.SetCursorPosition(StatusColumn, StatusRow);
            ClearLine();
            Console.Write($"Status : {status}    Interlock: {interlock}");
        }

        /// <summary>
        /// Displays the current phase of the boiler and the remaining time for that phase at a specific position in the console window
        /// </summary>
        /// <param name="phase">phase</param>
        /// <param name="remainingSeconds">seconds</param>
        public void DisplayTimer(BoilerPhase phase, int remainingSeconds)
        {
            Console.SetCursorPosition(TimerColumn, TimerRow);
            ClearLine();
            if (phase == BoilerPhase.None)
            {
                Console.Write("Timer    : --");
                return;
            }

            if (phase == BoilerPhase.Operational)
            {
                Console.Write("Operational phase");
                return;
            }

            Console.Write($"Timer : {phase} - {remainingSeconds:00} sec remaining");
        }

        /// <summary>
        /// Displays the main menu.
        /// </summary>
        public void DisplayMenu()
        {
            Console.SetCursorPosition(MenuColumn, MenuRow);
            ClearLine();
            Console.WriteLine("1. Start Boiler Sequence");
            Console.SetCursorPosition(MenuColumn, MenuRow + 1);
            Console.WriteLine("2. Stop Boiler Sequence");
            Console.SetCursorPosition(MenuColumn, MenuRow + 2);
            Console.WriteLine("3. Simulate Boiler Error");
            Console.SetCursorPosition(MenuColumn, MenuRow + 3);
            Console.WriteLine("4. Toggle Run Interlock");
            Console.SetCursorPosition(MenuColumn, MenuRow + 4);
            Console.WriteLine("5. Reset Lockout");
            Console.SetCursorPosition(MenuColumn, MenuRow + 5);
            Console.WriteLine("6. View Event Log");
            Console.SetCursorPosition(MenuColumn, MenuRow + 6);
            Console.WriteLine("7. Exit Application");
        }

        /// <summary>
        /// Displays the input prompt for the user to select a menu option.
        /// </summary>
        public void DisplayInputPrompt()
        {
            Console.SetCursorPosition(0, MenuRow + 8);
            ClearLine();
            Console.Write("Select an option: ");
        }

        /// <summary>
        /// Clears the entire console screen and resets the cursor position to the top-left corner.
        /// </summary>
        public void ClearScreen()
        {
            Console.Clear();
        }

        /// <summary>
        /// Clears the current line in the console by overwriting it with spaces and resetting the cursor position to the beginning of the line.
        /// </summary>
        public void ClearLine()
        {
            int width = Math.Max(1, Console.WindowWidth - 1);
            Console.Write(new string(' ', width));
            Console.SetCursorPosition(0, Console.CursorTop);
        }

        /// <summary>
        /// Displays a log message in the console with appropriate color based on the log type.
        /// </summary>
        public void DisplayLogLine(string message, LogType type)
        {
            Console.ForegroundColor = GetNotificationColor(type);
            Console.WriteLine(message);
            Console.ResetColor();
        }

        /// <summary>
        /// Gets the appropriate console color based on the log type for notifications and log messages.
        /// </summary>
        private static ConsoleColor GetNotificationColor(LogType type)
        {
            return type switch
            {
                LogType.Success => ConsoleColor.Green,
                LogType.Warning => ConsoleColor.Yellow,
                LogType.Error => ConsoleColor.Red,
                _ => ConsoleColor.Cyan
            };
        }
    }
}
