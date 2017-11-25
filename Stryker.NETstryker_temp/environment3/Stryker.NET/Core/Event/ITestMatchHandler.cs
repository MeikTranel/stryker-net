namespace Stryker.NET.Core.Event
{
    public interface ITestMatchHandler
    {
        void OnAllMutantsMatchedWithTests();
    }
}
