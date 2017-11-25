using System;
using System.Collections.Generic;

namespace Stryker.NET.Managers
{
    public interface IDirectoryManager
    {
        void CopyRoot(string source, string destination);
        IEnumerable<string> GetFiles(string source);
        void RemoveDirectory(string directory);
    }
}