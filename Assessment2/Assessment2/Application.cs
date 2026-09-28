using Assessment2.Enums;
using Assessment2.FileHandler;
using Assessment2.Helpers;
using Assessment2.Models;
using Assessment2.Repository;
using Assessment2.Services;
using Assessment2.Views;

namespace Assessment2
{
    /// <summary>
    /// Application class that manages the boiler control system, event logging, and user interface.
    /// </summary>
    internal class Application
    {

        private readonly IBoilerService _boilerService;
        private readonly IEventLogService _eventLogService;
        private readonly MainView _mainView;
        private readonly EventLogView _eventLogView;
        private readonly Validator _validator;
        private readonly CancellationTokenSource _applicationCancellation = new();
        private Task? _boilerOperationTask;
        private bool _isEventLogVisible;

        /// <summary>
        /// Initializes a new instance of the Application class, setting up services, views, and event subscriptions.
        /// </summary>
        public Application()
        {
            Validator validator = new();
            IFileHandler fileHandler = new CSVFileHandler();
            IEventLogRepository eventLogRepository = new EventLogRepository(fileHandler);
            _eventLogService = new EventLogService(eventLogRepository);
            Boiler boiler = new();
            _boilerService = new BoilerService(boiler, _eventLogService, validator);
            ConsoleIO consoleOperations = new();
            _mainView = new MainView(consoleOperations);
            _eventLogView = new EventLogView(consoleOperations);
            _validator = validator;
            SubscribeToEvents();
        }

        /// <summary>
        /// Runs the main application loop, initializing services and handling user input until the application is terminated.
        /// </summary>
        public async Task Run()
        {
            try
            {
                Console.CursorVisible = false;
                await _eventLogService.InitializeAsync(_applicationCancellation.Token);
                DisplayDashboard();
                await _boilerService.Initialize(_applicationCancellation.Token);
                await RunMenuLoop();
            }
            catch (OperationCanceledException)
            {
            }
            catch (Exception exception)
            {
                Console.Clear();
                Console.WriteLine($"Application error: {exception.Message}");
                Console.WriteLine("Press any key to exit.");
                Console.ReadKey(true);
            }
            finally
            {
                await Stop();
                _applicationCancellation.Dispose();
                Console.CursorVisible = true;
            }
        }

        /// <summary>
        /// Runs the menu loop, reading user input and executing corresponding actions until the application is terminated.
        /// </summary>
        private async Task RunMenuLoop()
        {
            bool isRunning = true;
            while (isRunning)
            {
                ConsoleKey key = _mainView.ReadMenuKey();
                if (!_validator.ValidateMenuOption(key, out MenuOptions option))
                {
                    await _boilerService.Notify("Invalid option. Please select a menu key from 1 to 7.",
                        LogType.Warning,
                        _applicationCancellation.Token);
                    continue;
                }

                isRunning = await ExecuteMenuOption(option);
            }
        }

        /// <summary>
        /// Executes menu option
        /// </summary>
        /// <param name="option">Option to execute</param>
        private async Task<bool> ExecuteMenuOption( MenuOptions option)
        {
            switch (option)
            {
                case MenuOptions.Start:
                    await StartBoilerSequence();
                    break;

                case MenuOptions.Stop:
                    await StopBoilerSequence();
                    break;

                case MenuOptions.SimulateError:
                    await _boilerService.SimulateError(_applicationCancellation.Token);
                    break;

                case MenuOptions.ToggleInterlock:
                    await _boilerService.ToggleInterlock(_applicationCancellation.Token);
                    break;

                case MenuOptions.Reset:
                    await _boilerService.Reset(_applicationCancellation.Token);
                    break;

                case MenuOptions.ViewEventLog:
                    await ShowEventLog();
                    break;

                case MenuOptions.Exit:
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Starts boiler sequence
        /// </summary>
        private async Task StartBoilerSequence()
        {
            if (_boilerOperationTask is { IsCompleted: false })
            {
                await _boilerService.Notify("Boiler sequence is already running.", LogType.Warning, _applicationCancellation.Token);
                return;
            }

            // Task is stored so the operation can be stopped and awaited when shutdown
            _boilerOperationTask = _boilerService.Start(_applicationCancellation.Token);
        }

        /// <summary>
        /// Stops boiler sequence and waits for the operation to complete if it is running.
        /// </summary>
        private async Task StopBoilerSequence()
        {
            await _boilerService.Stop(_applicationCancellation.Token);
            if (_boilerOperationTask is not null)
            {
                await _boilerOperationTask;
                _boilerOperationTask = null;
            }
        }

        /// <summary>
        /// Shows the event log by retrieving logs from the event log service and displaying them in the event log view. 
        /// </summary>
        private async Task ShowEventLog()
        {
            _isEventLogVisible = true;

            try
            {
                IReadOnlyList<EventLog> logs = await _eventLogService.GetAllLogs(_applicationCancellation.Token);
                _eventLogView.DisplayLogs(logs);
                _eventLogView.WaitForEscape();
            }
            finally
            {
                _isEventLogVisible = false;
                DisplayDashboard();
            }
        }

        /// <summary>
        /// Displays dashboard
        /// </summary>
        private void DisplayDashboard()
        {
            _mainView.DisplayDashboard(
                _boilerService.GetCurrentStatus(),
                _boilerService.GetCurrentPhase(),
                _boilerService.GetInterlockState(),
                _boilerService.GetRemainingTime());
        }

        /// <summary>
        /// Subscribes to events from the boiler service to handle notifications, status changes, and timer updates.
        /// </summary>
        private void SubscribeToEvents()
        {
            _boilerService.NotificationOccurred += OnNotificationOccurred;
            _boilerService.StatusChanged += OnStatusChanged;
            _boilerService.TimerUpdated += OnTimerUpdated;
        }

        /// <summary>
        /// Occurs when a notification is raised by the boiler service. If the event log is not visible, it displays the notification in the main view.
        /// </summary>
        private void OnNotificationOccurred(
            object? sender,
            NotificationEventArgs eventArgs)
        {
            if (!_isEventLogVisible)
            {
                _mainView.DisplayNotification(eventArgs);
            }
        }

        /// <summary>
        /// Occurs when status changes
        /// </summary>
        private void OnStatusChanged(
            object? sender,
            StatusChangedEventArgs eventArgs)
        {
            if (!_isEventLogVisible)
            {
                _mainView.DisplayStatusChanged(eventArgs);
            }
        }

        /// <summary>
        /// Occurs when timer updates
        /// </summary>
        private void OnTimerUpdated(
            object? sender,
            EventArgs eventArgs)
        {
            if (!_isEventLogVisible)
            {
                _mainView.DisplayTimer(
                    _boilerService.GetCurrentPhase(),
                    _boilerService.GetRemainingTime());
            }
        }

        /// <summary>
        /// Stops the application
        /// </summary>
        private async Task Stop()
        {
            _applicationCancellation.Cancel();

            if (_boilerOperationTask is not null)
            {
                try
                {
                    await _boilerOperationTask;
                }
                catch (OperationCanceledException)
                {
                }
            }
        }
    }
}

