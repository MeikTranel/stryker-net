using System;
using System.Collections.Generic;

namespace Stryker.NET.Managers
{
    public interface IDirectoryManager : IDisposable
    {
        void CopyRoot(string source, string destination);
        IEnumerable<string> GetFiles(string source);
    }
}