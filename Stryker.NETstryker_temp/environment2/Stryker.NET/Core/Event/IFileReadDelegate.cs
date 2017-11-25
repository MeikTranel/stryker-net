namespace Stryker.NET.Core.Event
{
    public delegate void AllFilesReadDelegate(string[] filePaths);

    public interface IFileReadHandler
    {
        void OnSourceFileRead(string filePath);
        void OnAllSourceFilesRead(string[] filePaths);
    }
}
