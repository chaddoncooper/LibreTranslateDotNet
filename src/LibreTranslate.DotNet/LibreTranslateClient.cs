using LibreTranslate.DotNet.Models;

namespace LibreTranslate.DotNet;

public partial class LibreTranslateClient(HttpClient httpClient) : ILibreTranslateClient
{
    public async Task<TranslateResponse> TranslateAsync(
        string text,
        string sourceLang,
        string targetLang,
        string format = "text",
        int? alternatives = null,
        string apiKey = ""
    )
    {
        return await TranslateObjectAsync(text, sourceLang, targetLang, format, alternatives, apiKey);
    }

    public async Task<TranslateResponse> TranslateBatchAsync(
        List<string> texts,
        string sourceLang,
        string targetLang,
        string format = "text",
        int? alternatives = null,
        string apiKey = ""
    )
    {
        return await TranslateObjectAsync(texts, sourceLang, targetLang, format, alternatives, apiKey);
    }

    private async Task<TranslateResponse> TranslateObjectAsync(
        object q,
        string sourceLang,
        string targetLang,
        string format = "text",
        int? alternatives = null,
        string apiKey = ""
    )
    {
        var request = new TranslateRequest
        {
            Q = q,
            Source = sourceLang,
            Target = targetLang,
            Format = format,
            Alternatives = alternatives,
            ApiKey = apiKey
        };
        var json = System.Text.Json.JsonSerializer.Serialize(request);
        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
        var response = await httpClient.PostAsync("translate", content);
        response.EnsureSuccessStatusCode();
        var responseJson = await response.Content.ReadAsStringAsync();
        var result = System.Text.Json.JsonSerializer.Deserialize<TranslateResponse>(
            responseJson
        );
        return result!;
    }
}
