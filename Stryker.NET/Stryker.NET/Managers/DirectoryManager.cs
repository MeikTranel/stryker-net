using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Stryker.NET.Managers
{
    public class DirectoryManager :IDirectoryManager
    {
        public void CreateDirectory(string directory)
        {
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }
    }
}
