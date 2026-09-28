using Assessment2.Enums;
using Assessment2.Helpers;
using Assessment2.Models;
using Assessment2.Services;
using Assessment2.Views;

namespace Assessment2
{
    internal class Application
    {

        private readonly IBoilerService _boilerService;
        private readonly MainView _mainView;
        private readonly Validator _validator;
        private readonly CancellationTokenSource _applicationCancellation = new();
        private Task? _boilerOperationTask;

        public Application()
        {
            Validator validator = new();
            Boiler boiler = new();
            _boilerService = new BoilerService(boiler, validator);
            ConsoleIO consoleOperations = new();
            _mainView = new MainView(consoleOperations);
            _validator = validator;
        }
        public async Task Run()
        {
            try
            {
                Console.CursorVisible = false;
                DisplayDashboard();
                await _boilerService.Initialize(_applicationCancellation.Token);
                await RunMenuLoop();
            }
            catch (OperationCanceledException)
            {
                // Application cancellation is handled during shutdown.
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

                case MenuOptions.Exit:
                    return false;
            }

            return true;
        }

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

        private async Task StopBoilerSequence()
        {
            await _boilerService.Stop(_applicationCancellation.Token);
            if (_boilerOperationTask is not null)
            {
                await _boilerOperationTask;
                _boilerOperationTask = null;
            }
        }

        private void DisplayDashboard()
        {
            _mainView.DisplayDashboard(
                _boilerService.GetCurrentStatus(),
                _boilerService.GetCurrentPhase(),
                _boilerService.GetInterlockState(),
                _boilerService.GetRemainingTime());
        }

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

