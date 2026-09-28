namespace Assessment2.FileHandler
{
    internal interface IFileHandler
    {
        Task EnsureFile(string filePath, string header, CancellationToken cancellationToken);

        Task AppendLine(string filePath, string line, CancellationToken cancellationToken);

        Task<IReadOnlyList<string>> ReadAllLines(string filePath, CancellationToken cancellationToken);
    }
}
