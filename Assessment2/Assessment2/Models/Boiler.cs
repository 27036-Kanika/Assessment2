using Assessment2.Enums;

namespace Assessment2.Models
{
    internal class Boiler
    {
        public Boiler()
        {
            Status = BoilerStatus.Lockout;
            Phase = BoilerPhase.None;
            Interlock = InterlockState.Open;
            RemainingTime = 0;
            IsStopping = false;
        }

        public BoilerStatus Status { get; set; }

        public BoilerPhase Phase { get; set; }

        public InterlockState Interlock { get; set; }

        public int RemainingTime { get; set; }

        public bool IsStopping { get; set; }

        public void UpdateStatus(BoilerStatus status)
        {
            Status = status;
        }

        public void UpdatePhase(BoilerPhase phase)
        {
            Phase = phase;
        }

        public void SetInterlock(InterlockState state)
        {
            Interlock = state;
        }

        public void SetRemainingTime(int seconds)
        {
            RemainingTime = seconds;
        }

        public void SetStopping(bool isStopping)
        {
            IsStopping = isStopping;
        }
    }
}
