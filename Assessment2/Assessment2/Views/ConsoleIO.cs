using Assessment2.Enums;

namespace Assessment2.Views
{
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

        public void DisplayHeading(string heading)
        {
            Console.SetCursorPosition(0, 0);
            Console.Write(new string(' ', Console.WindowWidth - 1));
            Console.SetCursorPosition(0, 0);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(heading);
            Console.ResetColor();
        }

        public void DisplayNotification(string message, LogType type)
        {
            Console.SetCursorPosition(NotificationColumn, NotificationRow);
            ClearLine();
            Console.ForegroundColor = GetNotificationColor(type);
            Console.Write(message);
            Console.ResetColor();
        }

        public void DisplayStatus(BoilerStatus status, InterlockState interlock)
        {
            Console.SetCursorPosition(StatusColumn, StatusRow);
            ClearLine();
            Console.Write($"Status : {status}    Interlock: {interlock}");
        }

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

        public void DisplayInputPrompt()
        {
            Console.SetCursorPosition(0, MenuRow + 8);
            ClearLine();
            Console.Write("Select an option: ");
        }

        public void ClearScreen()
        {
            Console.Clear();
        }

        public void ClearLine()
        {
            int width = Math.Max(1, Console.WindowWidth - 1);
            Console.Write(new string(' ', width));
            Console.SetCursorPosition(0, Console.CursorTop);
        }

        public void DisplayLogLine(string message, LogType type)
        {
            Console.ForegroundColor = GetNotificationColor(type);
            Console.WriteLine(message);
            Console.ResetColor();
        }

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
