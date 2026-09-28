using Assessment2.Enums;

namespace Assessment2.Helpers
{
    /// <summary>
    /// Validates user inputs
    /// </summary>
    internal class Validator
    {
        /// <summary>
        /// Validates the menu option based on the key pressed by the user.
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="option">Option</param>
        /// <returns>True if the menu option is valid; otherwise, false.</returns>
        public bool ValidateMenuOption(ConsoleKey key, out MenuOptions option)
        {
            option = MenuOptions.Exit;
            int value = key switch
            {
                ConsoleKey.D1 or ConsoleKey.NumPad1 => 1,
                ConsoleKey.D2 or ConsoleKey.NumPad2 => 2,
                ConsoleKey.D3 or ConsoleKey.NumPad3 => 3,
                ConsoleKey.D4 or ConsoleKey.NumPad4 => 4,
                ConsoleKey.D5 or ConsoleKey.NumPad5 => 5,
                ConsoleKey.D6 or ConsoleKey.NumPad6 => 6,
                ConsoleKey.D7 or ConsoleKey.NumPad7 => 7,
                _ => -1
            };

            if (value < 0)
            {
                return false;
            }

            if (!Enum.IsDefined(typeof(MenuOptions), value))
            {
                return false;
            }

            option = (MenuOptions)value;
            return true;
        }

        /// <summary>
        /// Validates if the boiler can start based on its current status and interlock state.
        /// </summary>
        /// <param name="status">Status</param>
        /// <param name="interlock">Interlock state</param>
        /// <returns>True if the boiler can start; otherwise, false.</returns>
        public bool CanStart(BoilerStatus status, InterlockState interlock)
        {
            return status == BoilerStatus.Ready && interlock == InterlockState.Closed;
        }

        /// <summary>
        /// Validates if the boiler can stop based on its current status.
        /// </summary>
        /// <param name="status">Status</param>
        /// <returns>True if the boiler can stop; otherwise, false.</returns>
        public bool CanStop(BoilerStatus status)
        {
            if (status == BoilerStatus.Running || status == BoilerStatus.Operational)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// simulates an error in the boiler if it is in the Operational state.
        /// </summary>
        /// <param name="status">Boiler status</param>
        /// <returns>True if the boiler can simulate an error; otherwise, false.</returns>
        public bool CanSimulateError(BoilerStatus status)
        {
            return status == BoilerStatus.Operational;
        }

        /// <summary>
        /// resets the boiler if it is in the Lockout state and the interlock is closed.
        /// </summary>
        /// <param name="status">Boiler status</param>
        /// <param name="interlock">Interlock state</param>
        /// <returns>True if the boiler can be reset; otherwise, false.</returns>
        public bool CanReset(BoilerStatus status, InterlockState interlock)
        {
            return status == BoilerStatus.Lockout && interlock == InterlockState.Closed;
        }

        /// <summary>
        /// Checks if the interlock can be toggled based on the current boiler status. The interlock can be toggled only when the boiler is not running or operational.
        /// </summary>
        public bool CanToggleInterlock(BoilerStatus status, InterlockState interlock)
        {
            return status != BoilerStatus.Running && status != BoilerStatus.Operational;
        }

        /// <summary>
        /// Invalid message for the menu option based on the current status and interlock state.
        /// </summary>
        /// <param name="option">Menu option</param>
        /// <param name="status">Boiler status</param>
        /// <param name="interlock">Interlock state</param>
        /// <returns>Invalid message string</returns>
        public string GetInvalidMessage(MenuOptions option, BoilerStatus status, InterlockState interlock)
        {
            return option switch
            {
                MenuOptions.Start when interlock != InterlockState.Closed => "Close the Run Interlock switch before starting the boiler",
                MenuOptions.Start when status != BoilerStatus.Ready => "Boiler must be in Ready state to start running",
                MenuOptions.Stop when !CanStop(status) => "Boiler is not running right now",
                MenuOptions.SimulateError when !CanSimulateError(status) => "Error simulation is allowed only in Operational state",
                MenuOptions.Reset when interlock != InterlockState.Closed => "Close the Run Interlock before before resetting the boiler",
                MenuOptions.Reset when status != BoilerStatus.Lockout => "Reset option is available only when the boiler is in Lockout",
                _ => "Operation is not available in present state"
            };
        }
    }
}
