namespace Stryker.NET.Core.Event
{
    public delegate void WrapUpDelegate();

    public interface IWrapUpHandler
    {
        void OnWrapUp();
    }
}
