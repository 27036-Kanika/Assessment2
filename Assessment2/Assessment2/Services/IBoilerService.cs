using Assessment2.Enums;
using Assessment2.Models;

namespace Assessment2.Services
{
    /// <summary>
    /// Interface for the boiler service, providing methods and events to manage and monitor the boiler's operation.
    /// </summary>
    internal interface IBoilerService
    {
        event EventHandler<NotificationEventArgs>? NotificationOccurred;
        event EventHandler<StatusChangedEventArgs>? StatusChanged;
        event EventHandler? TimerUpdated;

        Task Initialize(CancellationToken cancellationToken);

        Task Start(CancellationToken cancellationToken);

        Task Stop(CancellationToken cancellationToken);

        Task Reset(CancellationToken cancellationToken);

        Task ToggleInterlock(CancellationToken cancellationToken);

        Task SimulateError(CancellationToken cancellationToken);

        Task Notify(string message, LogType type, CancellationToken cancellationToken);

        BoilerStatus GetCurrentStatus();

        BoilerPhase GetCurrentPhase();

        InterlockState GetInterlockState();

        int GetRemainingTime();

        bool IsRunning();
    }
}
