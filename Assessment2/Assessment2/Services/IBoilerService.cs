using Assessment2.Models;

namespace Assessment2.Services
{
    internal interface IBoilerService
    {
        event EventHandler<StatusChangedEventArgs>? StatusChanged;

        Task Initialize(CancellationToken cancellationToken);

        Task Reset(CancellationToken cancellationToken);

        Task ToggleInterlock(CancellationToken cancellationToken);
    }
}
