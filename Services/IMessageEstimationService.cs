using MessageEstimator.Models;

namespace MessageEstimator.Services;

public interface IMessageEstimationService
{
    MessageEstimationResponse Analyze(string message);
    IReadOnlyList<MessageEstimationResponse> AnalyzeBatch(IEnumerable<string> messages);
}
