namespace StructuredLoggingConverter
{
    public class InMemoryFile
    {
        public string FilePath { get; }
        public string Content { get; }

        public InMemoryFile(string filePath, string content)
        {
            FilePath = filePath;
            Content = content;
        }
    }
}