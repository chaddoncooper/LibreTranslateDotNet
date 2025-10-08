using LibreTranslate.DotNet.Models;

namespace LibreTranslate.DotNet;

public partial class LibreTranslateClient(HttpClient httpClient, string apiKey = "") : ILibreTranslateClient
{
    public async Task<TranslateResponse> TranslateAsync(
        string text,
        string sourceLang,
        string targetLang,
        int? alternatives = null
    )
    {
        return await TranslateObjectAsync(text, sourceLang, targetLang, alternatives);
    }

    public async Task<TranslateResponse> TranslateBatchAsync(
        List<string> texts,
        string sourceLang,
        string targetLang,
        int? alternatives = null
    )
    {
        return await TranslateObjectAsync(texts, sourceLang, targetLang, alternatives);
    }

    private async Task<TranslateResponse> TranslateObjectAsync(
        object q,
        string sourceLang,
        string targetLang,
        int? alternatives = null
    )
    {
        var request = new TranslateRequest
        {
            Q = q,
            Source = sourceLang,
            Target = targetLang,
            Format = "text",
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
