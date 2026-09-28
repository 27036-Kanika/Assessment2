namespace Assessment2.Views
{
    internal class MainView
    {
        private readonly ConsoleIO _consoleIO;

        public MainView(ConsoleIO consoleIO)
        {
            this._consoleIO = consoleIO;
        }

        public void DisplayDashboard()
        {
            _consoleIO.DisplayHeading("Boiler Controller");
            _consoleIO.DisplayMenu();
        }

        public ConsoleKey ReadMenuKey()
        {
            return Console.ReadKey(true).Key;
        }

    }
}
