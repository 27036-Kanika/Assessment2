using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assessment2.Models;
using Assessment2.Services;
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
