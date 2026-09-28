using Assessment2.Enums;
using Assessment2.Models;

namespace Assessment2.Views
{
    /// <summary>
    /// Handles user Dashboard for boiler system
    /// </summary>
    internal class MainView
    {
        private readonly ConsoleIO _consoleIO;

        /// <summary>
        /// Initializes a new instance of the MainView class with the specified ConsoleIO for user interaction.
        /// </summary>
        /// <param name="consoleIO">ConsoleIO</param>
        public MainView(ConsoleIO consoleIO)
        {
            this._consoleIO = consoleIO;
        }

        /// <summary>
        /// Displays dashboard
        /// </summary>
        /// <param name="status">status</param>
        /// <param name="phase">phase</param>
        /// <param name="interlock">interlock</param>
        /// <param name="remainingSeconds">remainingSeconds</param>
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

        /// <summary>
        /// Reads a key input from the user for menu selection.
        /// </summary>
        public ConsoleKey ReadMenuKey()
        {
            return Console.ReadKey(true).Key;
        }

        /// <summary>
        /// Displays a notification message to the user based on the provided NotificationEventArgs.
        /// </summary>
        public void DisplayNotification(NotificationEventArgs eventArgs)
        {
            _consoleIO.DisplayNotification(eventArgs.Message, eventArgs.Type);
        }
        
        /// <summary>
        /// Displays the current status and interlock state based on the provided StatusChangedEventArgs.
        /// </summary>
        public void DisplayStatusChanged(StatusChangedEventArgs eventArgs)
        {
            _consoleIO.DisplayStatus(eventArgs.CurrentStatus, eventArgs.Interlock);
        }

        /// <summary>
        /// Displays the current phase and remaining seconds based on the provided PhaseChangedEventArgs.
        /// </summary>
        /// <param name="phase">phase</param>
        /// <param name="remainingSeconds">remainingSeconds</param>
        public void DisplayTimer(BoilerPhase phase, int remainingSeconds)
        {
            _consoleIO.DisplayTimer(phase, remainingSeconds);
        }

        /// <summary>
        /// Displays an invalid input message to the user when an unrecognized menu option is selected.
        /// </summary>
        public void DisplayInvalidInput()
        {
            _consoleIO.DisplayNotification("Invalid option. Please select a menu key from 1 to 7.", LogType.Warning);
        }

    }
}
