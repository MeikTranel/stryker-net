using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace Stryker.NET.Managers
{
    public class DirectoryManager : IDirectoryManager
    {
        public void CopyRoot(string source, string destination)
        {
            //Now Create all of the directories
            foreach (string dirPath in Directory.GetDirectories(source, "*",
                SearchOption.AllDirectories))
            {
                Directory.CreateDirectory(dirPath.Replace(source, destination));
            }

            //Copy all the files & Replaces any files with the same name
            foreach (string newPath in Directory.GetFiles(source, "*.*",
                SearchOption.AllDirectories).Where(a => !a.StartsWith("Db") && !a.EndsWith(".lock")))
            {
                File.Copy(newPath, newPath.Replace(source, destination), true);
            }
        }

        public IEnumerable<string> GetFiles(string source)
        {
            var options = new StrykerOptions();
            var reflector = new FileReflector(options, source);
            return reflector.GetFilesToMutate();
        }

        public void RemoveDirectory(string directoryPath)
        {
            if (Directory.Exists(directoryPath))
            {
                File.SetAttributes(directoryPath, FileAttributes.Normal);
                Directory.Delete(directoryPath, true);
            }
        }
    }
}
