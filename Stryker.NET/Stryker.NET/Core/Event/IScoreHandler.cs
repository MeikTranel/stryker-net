namespace Stryker.NET.Core.Event
{
    public delegate void ScoreCalculatedDelegate();

    public interface IScoreHandler
    {
        void OnScoreCalculated();
    }
}
