using System.Timers;
using Assessment2.Enums;
using Assessment2.Helpers;
using Assessment2.Models;

namespace Assessment2.Services
{
    /// <summary>
    /// BoilerService class that manages the boiler operations, including starting, stopping, resetting, and simulating errors.
    /// </summary>
    internal class BoilerService : IBoilerService
    {
        private const int PhaseDurationSeconds = 10;

        private readonly Boiler _boiler;
        private readonly IEventLogService _eventLogService;
        private readonly Validator _validator;

        private CancellationTokenSource? _operationCancellation;
        private System.Timers.Timer? _timer;
        private TaskCompletionSource? _phaseCompletionSource;

        /// <summary>
        /// Initializes a new instance of the BoilerService class with the specified boiler, event log service, and validator.
        /// </summary>
        /// <param name="boiler">The boiler instance to be managed by the service.</param>
        /// <param name="eventLogService">The event log service for logging boiler events.</param>
        /// <param name="validator">The validator for validating boiler operations.</param>
        public BoilerService(
            Boiler boiler,
            IEventLogService eventLogService,
            Validator validator)
        {
            _boiler = boiler;
            _eventLogService = eventLogService;
            _validator = validator;
        }

        public event EventHandler<NotificationEventArgs>? NotificationOccurred;
        public event EventHandler<StatusChangedEventArgs>? StatusChanged;
        public event EventHandler? TimerUpdated;

        public async Task Initialize( CancellationToken cancellationToken)
        {
            _boiler.UpdateStatus(BoilerStatus.Lockout);
            _boiler.UpdatePhase(BoilerPhase.None);
            _boiler.SetInterlock(InterlockState.Open);
            _boiler.SetRemainingTime(0);
            await _eventLogService.Log(LogType.Info, _boiler.Status, "Boiler Initialized.", cancellationToken);
            await RaiseNotification("Boiler Controller Initialized", LogType.Info, cancellationToken);
        }

        public async Task Reset(CancellationToken cancellationToken)
        {
            if (!_validator.CanReset(_boiler.Status, _boiler.Interlock))
            {
                await Notify("Run interlock switch must be closed before resetting the boiler", LogType.Warning, cancellationToken);
                return;
            }

            DisposeTimer();
            await ChangeStatus(BoilerStatus.Ready, cancellationToken);
            _boiler.UpdatePhase(BoilerPhase.None);
            _boiler.SetRemainingTime(0);
            await RaiseNotification("Boiler is Ready", LogType.Success, cancellationToken);
        }

        public async Task ToggleInterlock(CancellationToken cancellationToken)
        {
            InterlockState newState = _boiler.Interlock == InterlockState.Open ? InterlockState.Closed : InterlockState.Open;
            _boiler.SetInterlock(newState);
            string message = $"Interlock Switch toggled to {newState}.";
            BoilerStatus boilerStatus = _boiler.Status;
            StatusChanged?.Invoke(
                this,
                new StatusChangedEventArgs(
                    boilerStatus,
                    boilerStatus,
                    _boiler.Phase,
                    _boiler.Interlock));
            await Notify(message, LogType.Info, cancellationToken);
        }

        public async Task Start(CancellationToken cancellationToken)
        {
            if (!_validator.CanStart(_boiler.Status, _boiler.Interlock))
            {
                string message = _validator.GetInvalidMessage(
                    MenuOptions.Start,
                    _boiler.Status,
                    _boiler.Interlock);
                await Notify(message, LogType.Warning, cancellationToken);
                return;
            }

            _operationCancellation?.Dispose();
            _operationCancellation = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

            try
            {
                await ChangeStatus(BoilerStatus.Running, _operationCancellation.Token);
                await RunPhase( BoilerPhase.PrePurge, _operationCancellation.Token);
                await RunPhase(BoilerPhase.Ignition, _operationCancellation.Token);
                _boiler.UpdatePhase(BoilerPhase.None);
                _boiler.SetRemainingTime(0);
                _boiler.UpdatePhase(BoilerPhase.Operational);
                await ChangeStatus(BoilerStatus.Operational, _operationCancellation.Token);
                await Notify(
                    "Boiler is now operational.",
                    LogType.Success,
                    _operationCancellation.Token);
                //await RunPhase(BoilerPhase.Operational, _operationCancellation.Token);
                //await Notify(
                //    "Boiler sequence stopped.",
                //    LogType.Success,
                //    _operationCancellation.Token);
                // await ChangeStatus(BoilerStatus.Lockout, _operationCancellation.Token);
            }
            catch (OperationCanceledException)
            {
                _boiler.UpdatePhase(BoilerPhase.None);
                _boiler.SetRemainingTime(0);
                await ChangeStatus(BoilerStatus.Lockout, CancellationToken.None);

                await Notify(
                    "Boiler sequence stopped.",
                    LogType.Warning,
                    CancellationToken.None);
            }
            catch (Exception exception)
            {
                _boiler.UpdatePhase(BoilerPhase.None);
                _boiler.SetRemainingTime(0);
                await ChangeStatus(BoilerStatus.Lockout, CancellationToken.None);

                await Notify(
                    $"Error: {exception.Message}. System in Lockout.",
                    LogType.Error,
                    CancellationToken.None);
            }
            finally
            {
                DisposeTimer();
                _operationCancellation?.Dispose();
                _operationCancellation = null;
            }
        }

        public async Task Stop(CancellationToken cancellationToken)
        {
            if (!_validator.CanStop(_boiler.Status))
            {
                await Notify(
                    "Boiler is not running.",
                    LogType.Warning,
                    cancellationToken);
                return;
            }

            _operationCancellation?.Cancel();
            await ChangeStatus(BoilerStatus.Lockout, CancellationToken.None);
        }

        public async Task SimulateError(CancellationToken cancellationToken)
        {
            if (!_validator.CanSimulateError(_boiler.Status))
            {
                string message = _validator.GetInvalidMessage(
                    MenuOptions.SimulateError,
                    _boiler.Status,
                    _boiler.Interlock);

                await Notify(
                    message,
                    LogType.Warning,
                    cancellationToken);
                return;
            }

            _operationCancellation?.Cancel();
            DisposeTimer();

            await ChangeStatus(BoilerStatus.Lockout, CancellationToken.None);

            await Notify(
                "Error: Simulated boiler failure. System in Lockout.",
                LogType.Error,
                CancellationToken.None);
        }

        public async Task Notify(
            string message,
            LogType type,
            CancellationToken cancellationToken)
        {
            await _eventLogService.Log(type, _boiler.Status, message, cancellationToken);
            await RaiseNotification(message, type, cancellationToken);
        }

        public BoilerStatus GetCurrentStatus() => _boiler.Status;

        public BoilerPhase GetCurrentPhase() => _boiler.Phase;

        public InterlockState GetInterlockState() => _boiler.Interlock;

        public int GetRemainingTime() => _boiler.RemainingTime;

        public bool IsRunning() => _boiler.Status == BoilerStatus.Running;

        private async Task RunPhase(
            BoilerPhase phase,
            CancellationToken cancellationToken)
        {
            _boiler.UpdatePhase(phase);
            _boiler.SetRemainingTime(PhaseDurationSeconds);

            await Notify($"{phase} phase started.", LogType.Info, cancellationToken);

            TaskCompletionSource phaseCompletion = new(TaskCreationOptions.RunContinuationsAsynchronously);

            _timer = new System.Timers.Timer(1000)
            {
                AutoReset = true
            };

            _phaseCompletionSource = phaseCompletion;
            _timer.Elapsed += OnTimerElapsed;
            _timer.Start();

            using CancellationTokenRegistration registration = cancellationToken.Register(() => phaseCompletion.TrySetCanceled(cancellationToken));

            try
            {
                await phaseCompletion.Task;
            }
            finally
            {
                DisposeTimer();
                _phaseCompletionSource = null;
            }

            await Notify(
                $"{phase} phase completed.",
                LogType.Success,
                cancellationToken);
        }

        private void OnTimerElapsed(
            object? sender,
            ElapsedEventArgs eventArgs)
        {
            if (_boiler.RemainingTime <= 0)
            {
                return;
            }

            _boiler.SetRemainingTime(_boiler.RemainingTime - 1);

            TimerUpdated?.Invoke(this, EventArgs.Empty);
            if (_boiler.RemainingTime == 0)
            {
                _phaseCompletionSource?.TrySetResult();
            }
        }

        private async Task ChangeStatus(BoilerStatus newStatus, CancellationToken cancellationToken)
        {
            BoilerStatus previousStatus = _boiler.Status;
            if (previousStatus == newStatus)
            {
                return;
            }

            _boiler.UpdateStatus(newStatus);
            StatusChanged?.Invoke(
                this,
                new StatusChangedEventArgs(
                    previousStatus,
                    newStatus,
                    _boiler.Phase,
                    _boiler.Interlock));
            await _eventLogService.Log(
                LogType.Info,
                newStatus,
                $"Boiler Status changed from {previousStatus} to {newStatus}.",
                cancellationToken);
        }

        private async Task RaiseNotification(
            string message,
            LogType type,
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            NotificationOccurred?.Invoke(
                this,
                new NotificationEventArgs(message, type));

            await Task.CompletedTask;
        }

        private void DisposeTimer()
        {
            if (_timer is null)
            {
                return;
            }

            _timer.Stop();
            _timer.Dispose();
            _timer = null;
        }
    }
}
