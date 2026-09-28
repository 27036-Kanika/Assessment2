namespace Assessment2.FileHandler
{
    /// <summary>
    /// Handler for CSV file operations, including ensuring the file exists, appending lines, and reading all lines.
    /// </summary>
    internal class CSVFileHandler : IFileHandler
    {
        /// <summary>
        /// Ensures that the specified file exists and contains the provided header
        /// </summary>
        /// <param name="filePath">The path of the file to ensure.</param>
        /// <param name="header">The header line to write if the file is created or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task EnsureFile(string filePath, string header, CancellationToken cancellationToken)
        {
            string? directory = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrWhiteSpace(directory))
            {
                Directory.CreateDirectory(directory);
            }

            if (!File.Exists(filePath) || new FileInfo(filePath).Length == 0)
            {
                await File.WriteAllTextAsync(filePath, header + Environment.NewLine, cancellationToken);
            }
        }

        /// <summary>
        /// Appends a line of text to the specified file
        /// </summary>
        /// <param name="filePath">The path of the file to append the line to.</param>
        /// <param name="line">The line of text to append.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns></returns>
        public async Task AppendLine(string filePath, string line, CancellationToken cancellationToken)
        {
            await File.AppendAllTextAsync(filePath, line + Environment.NewLine, cancellationToken);
        }

        /// <summary>
        /// Reads all lines from the specified file and returns them as a read-only list of strings
        /// </summary>
        /// <param name="filePath">File path</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public async Task<IReadOnlyList<string>> ReadAllLines(string filePath, CancellationToken cancellationToken)
        {
            return await File.ReadAllLinesAsync(filePath, cancellationToken);
        }
    }
}
