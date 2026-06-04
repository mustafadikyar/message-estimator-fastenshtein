namespace MessageEstimator.Enums;

public enum EstimateType
{
    Exact,       // Anahtar ifadeyle birebir eşleşme (güven: 1.0)
    Typo,        // Levenshtein ile yakın eşleşme (güven: 0.85)
    Irrelevant   // Yeterli yakınlık yok (güven: 0.0)
}
