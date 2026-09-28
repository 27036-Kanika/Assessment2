using Assessment2.Enums;

namespace Assessment2.Models
{
    internal class StatusChangedEventArgs
    {
        public StatusChangedEventArgs(
        BoilerStatus previousStatus,
        BoilerStatus currentStatus,
        BoilerPhase phase,
        InterlockState interlock)
        {
            PreviousStatus = previousStatus;
            CurrentStatus = currentStatus;
            Phase = phase;
            Interlock = interlock;
            Timestamp = DateTime.Now;
        }

        public BoilerStatus PreviousStatus { get; }

        public BoilerStatus CurrentStatus { get; }

        public BoilerPhase Phase { get; }

        public InterlockState Interlock { get; }

        public DateTime Timestamp { get; }
    }
}
