using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assessment2.Views
{
    internal class ConsoleIO
    {
        public void DisplayHeading(string heading)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("===============================");
            Console.WriteLine(heading);
            Console.WriteLine("===============================");
            Console.ResetColor();
        }

        public void DisplayMenu()
        {
            Console.WriteLine("1. Start Boiler Sequence");
            Console.WriteLine("2. Stop Boiler Sequence");
            Console.WriteLine("3. Simulate Boiler Error");
            Console.WriteLine("4. Toggle Run Interlock");
            Console.WriteLine("5. Reset Lockout");
            Console.WriteLine("6. View Event Log");
            Console.WriteLine("7. Exit Application");
            Console.WriteLine("Select an option: ");
        }
    }
}
