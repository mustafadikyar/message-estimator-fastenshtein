using Fastenshtein;
using MessageEstimator.Enums;
using MessageEstimator.Extensions;
using MessageEstimator.Models;

namespace MessageEstimator.Services;

// Mesajdaki kelimeleri tanımlı anahtar ifadelerle karşılaştırır;
// birebir eşleşme yoksa Levenshtein mesafesiyle yazım hatası toleransı uygular.
public class MessageEstimationService : IMessageEstimationService
{
    // Bu eşiğin üzerindeki farklar Irrelevant sayılır.
    private const int MaxTypoDistance = 2;
    private const double ExactConfidence = 1.0;
    private const double TypoConfidence = 0.85;

    private static readonly Dictionary<string, string> Scopes = new(StringComparer.OrdinalIgnoreCase)
    {
        ["SIFRE"] = "Hesap erişimi",
        ["ODEME"] = "Ödeme",
        ["KARGO"] = "Teslimat",
        ["DESTEK"] = "Genel destek"
    };

    public MessageEstimationResponse Analyze(string message)
    {
        var normalizedMessage = message.NormalizeText();
        var tokens = normalizedMessage.Tokenize().ToList();

        MatchCandidate? bestMatch = null;

        foreach (var (keyword, scope) in Scopes)
        {
            var comparisonTerms = BuildComparisonTerms(keyword, scope);

            for (var index = 0; index < tokens.Count; index++)
            {
                foreach (var term in comparisonTerms)
                {
                    var candidate = EvaluateToken(tokens[index], term, keyword, scope, index);
                    bestMatch = SelectBetterMatch(bestMatch, candidate);
                }
            }
        }

        if (bestMatch is null)
        {
            return new MessageEstimationResponse
            {
                OriginalMessage = message,
                ConfidenceScore = 0.0,
                EstimateType = EstimateType.Irrelevant
            };
        }

        return new MessageEstimationResponse
        {
            OriginalMessage = message,
            Scope = bestMatch.Scope,
            MatchedKeyword = bestMatch.Keyword,
            ConfidenceScore = bestMatch.EstimateType == EstimateType.Exact ? ExactConfidence : TypoConfidence,
            EstimateType = bestMatch.EstimateType
        };
    }

    public IReadOnlyList<MessageEstimationResponse> AnalyzeBatch(IEnumerable<string> messages) =>
        messages.Select(Analyze).ToList();

    // Anahtar ifadenin yanı sıra kapsam etiketindeki kelimeler de adaydır (ör. "teslimat" → KARGO).
    private static IEnumerable<string> BuildComparisonTerms(string keyword, string scope)
    {
        yield return keyword.NormalizeText();

        foreach (var word in scope.NormalizeText().Tokenize())
            yield return word;
    }

    private static MatchCandidate? EvaluateToken(
        string token,
        string comparisonTerm,
        string keyword,
        string scope,
        int tokenIndex)
    {
        if (token == comparisonTerm)
        {
            return new MatchCandidate(keyword, scope, EstimateType.Exact, 0, tokenIndex);
        }

        var distance = Levenshtein.Distance(token, comparisonTerm);
        if (distance > MaxTypoDistance)
            return null;

        return new MatchCandidate(keyword, scope, EstimateType.Typo, distance, tokenIndex);
    }

    // Öncelik: Exact > Typo > daha düşük mesafe > mesajda daha geç geçen kelime.
    private static MatchCandidate? SelectBetterMatch(MatchCandidate? current, MatchCandidate? candidate)
    {
        if (candidate is null)
            return current;

        if (current is null)
            return candidate;

        if (candidate.EstimateType != current.EstimateType)
            return candidate.EstimateType == EstimateType.Exact ? candidate : current;

        if (candidate.Distance != current.Distance)
            return candidate.Distance < current.Distance ? candidate : current;

        return candidate.TokenIndex >= current.TokenIndex ? candidate : current;
    }

    private sealed record MatchCandidate(
        string Keyword,
        string Scope,
        EstimateType EstimateType,
        int Distance,
        int TokenIndex);
}
