using System;

namespace Stryker.NET.Managers
{
    public interface IDirectoryManager : IDisposable
    {
        void CreateDirectory(string directory);
    }
}