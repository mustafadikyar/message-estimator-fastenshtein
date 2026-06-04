using MessageEstimator.Enums;

namespace MessageEstimator.Models;

public class MessageEstimationResponse
{
    public string OriginalMessage { get; set; } = string.Empty;
    public string? Scope { get; set; }
    public string? MatchedKeyword { get; set; }
    public double ConfidenceScore { get; set; }
    public EstimateType EstimateType { get; set; }
}
