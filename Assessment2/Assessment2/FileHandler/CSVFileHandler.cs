namespace Assessment2.FileHandler
{
    internal class CSVFileHandler : IFileHandler
    {
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

        public async Task AppendLine(string filePath, string line, CancellationToken cancellationToken)
        {
            await File.AppendAllTextAsync(filePath, line + Environment.NewLine, cancellationToken);
        }

        public async Task<IReadOnlyList<string>> ReadAllLines(string filePath, CancellationToken cancellationToken)
        {
            return await File.ReadAllLinesAsync(filePath, cancellationToken);
        }
    }
}
