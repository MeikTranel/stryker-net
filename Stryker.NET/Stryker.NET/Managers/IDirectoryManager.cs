using System;

namespace Stryker.NET.Managers
{
    public interface IDirectoryManager : IDisposable
    {
        void CopyRoot(string source, string destination);
    }
}