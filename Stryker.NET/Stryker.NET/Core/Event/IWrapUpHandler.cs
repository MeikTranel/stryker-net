namespace Stryker.NET.Core.Event
{
    public delegate void WrapUpDelegate();

    interface IWrapUpHandler
    {
        void OnWrapUp();
    }
}
