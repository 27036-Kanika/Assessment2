namespace Assessment2
{
    /// <summary>
    /// Represents the entry point of the application
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// Entry point of the application. Initializes and runs the application.
        /// </summary>
        public static async Task Main()
        {
            Application application = new();
            await application.Run();
        }
    }
}
