namespace Assessment2.Enums
{
    /// <summary>
    /// Represents the status of the boiler
    /// </summary>
    internal enum BoilerStatus
    {
        /// <summary>
        /// Represents the boiler is locked out
        /// </summary>
        Lockout,

        /// <summary>
        /// Represents the boiler is ready to start
        /// </summary>
        Ready,

        /// <summary>
        /// Represents the boiler is running
        /// </summary>
        Running,

        /// <summary>
        /// Represents the boiler is operational
        /// </summary>
        Operational,
    }
}
