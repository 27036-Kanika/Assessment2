using Assessment2.Enums;

namespace Assessment2.Helpers
{
    internal class Validator
    {
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

        public bool CanStart(BoilerStatus status, InterlockState interlock)
        {
            return status == BoilerStatus.Ready && interlock == InterlockState.Closed;
        }

        public bool CanStop(BoilerStatus status)
        {
            if (status == BoilerStatus.Running || status == BoilerStatus.Operational)
            {
                return true;
            }

            return false;
        }

        public bool CanSimulateError(BoilerStatus status)
        {
            return status == BoilerStatus.Operational;
        }

        public bool CanReset(BoilerStatus status, InterlockState interlock)
        {
            return status == BoilerStatus.Lockout && interlock == InterlockState.Closed;
        }

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
