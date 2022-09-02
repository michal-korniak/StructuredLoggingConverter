using CommandLine;

namespace StructuredLoggingConverter
{
    public class Options
    {
        [Option('p', "path", Required = true, HelpText = "Path")]
        public string Path { get; set; }

        [Option("useDefaultNames", HelpText = "If enabled program will not ask about names for arguments, and will use default ones.")]
        public bool UseDefaultNames { get; set; } = false;

        [Option("generateNewFiles", HelpText = "If enabled program will generate new files instead of replacing old ones.")]
        public bool GenerateNewFiles { get; set; } = false;
    }
}
