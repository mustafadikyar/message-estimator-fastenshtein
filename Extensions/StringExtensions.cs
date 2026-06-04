using System.Text.RegularExpressions;

namespace MessageEstimator.Extensions;

public static partial class StringExtensions
{
    // Karşılaştırma öncesi metni tek forma indirger; büyük/küçük harf ve Türkçe karakter farkları giderilir.
    public static string NormalizeText(this string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return string.Empty;

        var normalized = input.ToLowerInvariant();
        normalized = ReplaceTurkishCharacters(normalized);
        normalized = WhitespaceRegex().Replace(normalized, " ").Trim();
        return normalized;
    }

    // Normalize edilmiş metni noktalama işaretlerinden arındırılmış kelimelere ayırır.
    public static IEnumerable<string> Tokenize(this string normalizedText)
    {
        if (string.IsNullOrWhiteSpace(normalizedText))
            yield break;

        foreach (var token in normalizedText.Split(' ', StringSplitOptions.RemoveEmptyEntries))
        {
            var word = PunctuationRegex().Replace(token, string.Empty);
            if (!string.IsNullOrEmpty(word))
                yield return word;
        }
    }

    private static string ReplaceTurkishCharacters(string text) =>
        text
            .Replace('ç', 'c').Replace('Ç', 'c')
            .Replace('ğ', 'g').Replace('Ğ', 'g')
            .Replace('ı', 'i').Replace('I', 'i')
            .Replace('İ', 'i')
            .Replace('ö', 'o').Replace('Ö', 'o')
            .Replace('ş', 's').Replace('Ş', 's')
            .Replace('ü', 'u').Replace('Ü', 'u');

    [GeneratedRegex(@"\s+")]
    private static partial Regex WhitespaceRegex();

    [GeneratedRegex(@"[^\w]")]
    private static partial Regex PunctuationRegex();
}
