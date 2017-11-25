using System;
using System.Collections.Generic;
using System.Text;

namespace Stryker.NET
{
    public interface IFileReflector
    {
        string[] GetFilesToMutate();
    }
}
