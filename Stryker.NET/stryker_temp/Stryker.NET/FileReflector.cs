using System.IO;

namespace Stryker.NET
{
    public class FileReflector : IFileReflector
    {
        private IStykerOptions _options;
        private string _rootFolder;

        public FileReflector(IStykerOptions options, string rootFolder)
        {
            _options = options;
            _rootFolder = rootFolder;
        }

        public string[] GetFilesToMutate()
        {
            var files = Directory.GetFiles(_rootFolder, _options.FileFilter, SearchOption.AllDirectories);
            return files;
        }
    }
}
