namespace Assessment2.FileHandler
{
    /// <summary>
    /// Interface for file handling operations
    /// </summary>
    internal interface IFileHandler
    {
        /// <summary>
        /// Ensures that the specified file exists and contains the provided header
        /// </summary>
        Task EnsureFile(string filePath, string header, CancellationToken cancellationToken);

        /// <summary>
        /// Appends a line of text to the specified file
        /// </summary>
        Task AppendLine(string filePath, string line, CancellationToken cancellationToken);

        /// <summary>
        /// Reads all lines from the specified file and returns them as a read-only list of strings
        /// </summary>
        Task<IReadOnlyList<string>> ReadAllLines(string filePath, CancellationToken cancellationToken);
    }
}
