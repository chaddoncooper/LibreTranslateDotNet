using System.Text.Json.Serialization;

namespace LibreTranslate.DotNet.Models;

public class TranslateRequest
{
    [JsonPropertyName("q")]
    public required object Q { get; set; } // string or List<string>

    [JsonPropertyName("source")]
    public string Source { get; set; } = "auto";

    [JsonPropertyName("target")]
    public required string Target { get; set; }

    [JsonPropertyName("format")]
    public required string Format { get; set; }

    [JsonPropertyName("alternatives")]
    public int? Alternatives { get; set; }

    [JsonPropertyName("api_key")]
    public string ApiKey { get; set; } = string.Empty;
}
