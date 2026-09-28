using Assessment2.Enums;
using Assessment2.Models;

namespace Assessment2.Views
{
    internal class MainView
    {
        private readonly ConsoleIO _consoleIO;

        public MainView(ConsoleIO consoleIO)
        {
            this._consoleIO = consoleIO;
        }

        public void DisplayDashboard(
            BoilerStatus status,
            BoilerPhase phase,
            InterlockState interlock,
            int remainingSeconds)
        {
            _consoleIO.ClearScreen();
            _consoleIO.DisplayHeading("BOILER CONTROLLER");
            _consoleIO.DisplayStatus(status, interlock);
            _consoleIO.DisplayTimer(phase, remainingSeconds);
            _consoleIO.DisplayMenu();
            _consoleIO.DisplayInputPrompt();
        }

        public ConsoleKey ReadMenuKey()
        {
            return Console.ReadKey(true).Key;
        }

        public void DisplayNotification(NotificationEventArgs eventArgs)
        {
            _consoleIO.DisplayNotification(eventArgs.Message, eventArgs.Type);
        }

        public void DisplayStatusChanged(StatusChangedEventArgs eventArgs)
        {
            _consoleIO.DisplayStatus(eventArgs.CurrentStatus, eventArgs.Interlock);
        }

        public void DisplayTimer(BoilerPhase phase, int remainingSeconds)
        {
            _consoleIO.DisplayTimer(phase, remainingSeconds);
        }

        public void DisplayInvalidInput()
        {
            _consoleIO.DisplayNotification("Invalid option. Please select a menu key from 1 to 7.", LogType.Warning);
        }

    }
}
