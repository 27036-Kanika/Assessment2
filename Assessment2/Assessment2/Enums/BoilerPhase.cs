namespace Assessment2.Enums
{
    /// <summary>
    /// Represents the phases of boiler
    /// </summary>
    internal enum BoilerPhase
    {
        /// <summary>
        /// Not running phase
        /// </summary>
        None,

        /// <summary>
        /// Represents pre purging state
        /// </summary>
        PrePurge,

        /// <summary>
        /// Represents ignition stage
        /// </summary>
        Ignition,

        /// <summary>
        /// Represents operational stage
        /// </summary>
        Operational,
    }
}
