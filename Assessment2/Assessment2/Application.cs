using Assessment2.Views;

namespace Assessment2
{
    internal class Application
    {

        private readonly MainView _mainView;

        public Application()
        {
            ConsoleIO consoleOperations = new();
            _mainView = new MainView(consoleOperations);
        }
        public void Run()
        {
            DisplayDashboard();
        }
        private void DisplayDashboard()
        {
            _mainView.DisplayDashboard();
        }
    }
}
