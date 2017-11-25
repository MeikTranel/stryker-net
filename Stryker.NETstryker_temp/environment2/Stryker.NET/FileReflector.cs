using System.IO;
using Stryker.NET.Core.Event;

namespace Stryker.NET
{
    public class FileReflector : IFileReflector
    {
        private IStykerOptions _options;
        private string _rootFolder;

        private event AllFilesReadDelegate AllFilesRead;

        public FileReflector(IStykerOptions options, string rootFolder, IFileReadHandler fileReadHandler)
        {
            _options = options;
            _rootFolder = rootFolder;

            AllFilesRead += fileReadHandler.OnAllSourceFilesRead;
        }

        public string[] GetFilesToMutate()
        {
            var files = Directory.GetFiles(_rootFolder, _options.FileFilter, SearchOption.AllDirectories);
            
            //notify 'all files read' observers
            AllFilesRead(files);

            return files;
        }
    }
}
