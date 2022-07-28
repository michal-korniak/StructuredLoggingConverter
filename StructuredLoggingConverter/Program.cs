using Krip.Logging.Extensions;
using System.Text;
using System.Text.RegularExpressions;

namespace StructuredLoggingConverter
{

    public class Program
    {
        private static readonly string _projectPath = @"C:\Work\Repos\PrinterCore\";
        private static readonly string _interpolatedLogLinePattern = ".*Log(Debug|Error|Information|Trace|Warning|Critical)\\(.*\\$\".*\\{.*\\}.*\".*";

        public static async Task Main()
        {
            IEnumerable<string> csFiles = GetCsFiles();
            IEnumerable<InMemoryFile> files = await LoadFilesIntoMemory(csFiles);
            foreach (InMemoryFile file in files)
            {
                await ProcessFile(file);
            }
        }

        private static IEnumerable<string> GetCsFiles()
        {
            IEnumerable<string> csFiles = Directory.GetFiles(_projectPath, "*.cs", new EnumerationOptions()
            {
                RecurseSubdirectories = true
            });
            return csFiles;
        }

        private static async Task<IEnumerable<InMemoryFile>> LoadFilesIntoMemory(IEnumerable<string> filePaths)
        {
            IEnumerable<Task<InMemoryFile>> createFileContentInstanceForEachFilePathTask =
                filePaths.Select(async filePath => new InMemoryFile(filePath, await File.ReadAllTextAsync(filePath)));
            IEnumerable<InMemoryFile> fileContents = await Task.WhenAll(createFileContentInstanceForEachFilePathTask);
            return fileContents;
        }

        private static async Task ProcessFile(InMemoryFile file)
        {
            IEnumerable<string> interpolatedLogInstructions = GetLogInstructions(file)
                .Where(x => IsInterpolatedLogInstruction(x));

            string newFileContent = file.Content;
            foreach (string interpolatedLogInstruction in interpolatedLogInstructions)
            {
                IEnumerable<string> interpolatedArguments = GetInterpolatedArguments(interpolatedLogInstruction);
                string updatedLogInstruction = InsertArgumentsIntoLogInstruction(interpolatedLogInstruction, interpolatedArguments);
                updatedLogInstruction = RemoveAllOccurencesOfInterpolationCharacter(updatedLogInstruction);
                newFileContent = newFileContent.Replace(interpolatedLogInstruction, updatedLogInstruction);
            }

            if (newFileContent != file.Content)
            {
                await File.WriteAllTextAsync(file.FilePath, newFileContent);
            }
        }

        private static IEnumerable<string> GetLogInstructions(InMemoryFile file)
        {
            Regex logInstructionWithoutCurlyBracketedContentRegex = new(".*Log(Debug|Error|Information|Trace|Warning|Critical)");

            List<string> logInstructions = new List<string>();
            foreach (Match logInstructionWithoutCurlyBracketedContentMatch in logInstructionWithoutCurlyBracketedContentRegex.Matches(file.Content))
            {
                int indexOflastCharacter = logInstructionWithoutCurlyBracketedContentMatch.Index + logInstructionWithoutCurlyBracketedContentMatch.Length;
                string curlyBracketedContent = GetContentOfNextCurlyBracketsBlock(file.Content, indexOflastCharacter);
                string fullLogInstruction = logInstructionWithoutCurlyBracketedContentMatch.Value + curlyBracketedContent;

                logInstructions.Add(fullLogInstruction);
            }

            return logInstructions;
        }

        private static string GetContentOfNextCurlyBracketsBlock(string content, int startIndex)
        {
            char startCharacter = '(';
            char endCharacter = ')';

            int startCharacterCount = 0;
            int endCharacterCount = 0;

            StringBuilder contentBuilder = new();
            for (int currentIndex = startIndex; currentIndex < content.Length; ++currentIndex)
            {
                char currentCharacter = content[currentIndex];

                if (currentCharacter == startCharacter)
                {
                    startCharacterCount += 1;
                }
                if (currentCharacter == endCharacter)
                {
                    endCharacterCount += 1;
                }

                if (startCharacterCount > 0)
                {
                    contentBuilder.Append(currentCharacter);
                }

                if (startCharacterCount == endCharacterCount)
                {
                    break;
                }
            }

            return contentBuilder.ToString();

        }

        private static bool IsInterpolatedLogInstruction(string logInstruction)
        {
            return logInstruction.Contains("$\"");
        }

        private static IEnumerable<string> GetInterpolatedArguments(string logInstruction)
        {
            var regex = new Regex("{.*?}");
            var interpolatedArguments = regex.Matches(logInstruction);

            return interpolatedArguments
                .Select(x => x.Value.Trim('{', '}'))
                .DistinctOrdered();
        }

        private static string InsertArgumentsIntoLogInstruction(string logInstruction, IEnumerable<string> interpolatedArguments)
        {
            if (!interpolatedArguments.Any())
            {
                return logInstruction;
            }

            int indexOfEndBracket = logInstruction.LastIndexOf(")");
            string updatedLogInstruction = logInstruction.Insert(indexOfEndBracket, ", " + string.Join(", ", interpolatedArguments));
            return updatedLogInstruction;
        }

        private static string RemoveAllOccurencesOfInterpolationCharacter(string logInstruction)
        {
            return logInstruction.Replace("$", string.Empty);
        }


    }
}