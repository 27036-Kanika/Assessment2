using Assessment2.Enums;

namespace Assessment2.Models
{
    /// <summary>
    /// StatusChangedEventArgs class represents the event arguments for a status change event in the boiler system.
    /// </summary>
    internal class StatusChangedEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the StatusChangedEventArgs class with the specified previous status, current status, phase, and interlock state.
        /// </summary>
        /// <param name="previousStatus">The previous status of the boiler.</param>
        /// <param name="currentStatus">The current status of the boiler.</param>
        /// <param name="phase">The current phase of the boiler.</param>
        /// <param name="interlock">The current interlock state of the boiler.</param>
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
