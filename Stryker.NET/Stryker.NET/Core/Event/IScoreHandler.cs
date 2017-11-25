using Stryker.NET.Report;

namespace Stryker.NET.Core.Event
{
    public delegate void ScoreCalculatedDelegate(ScoreResult result);

    public interface IScoreHandler
    {
        void OnScoreCalculated(ScoreResult result);
    }
}
