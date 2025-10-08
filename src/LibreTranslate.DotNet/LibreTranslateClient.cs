using LibreTranslate.DotNet.Models;

namespace LibreTranslate.DotNet;

public partial class LibreTranslateClient(string apiUrl, HttpClient httpClient) : ILibreTranslateClient
{
    private readonly string _apiUrl = apiUrl.TrimEnd('/');

    public async Task<TranslateResponse> TranslateAsync(
        string text,
        string sourceLang,
        string targetLang,
        string format = "text",
        int? alternatives = null,
        string apiKey = ""
    )
    {
        var request = new TranslateRequest
        {
            Q = text,
            Source = sourceLang,
            Target = targetLang,
            Format = format,
            Alternatives = alternatives,
            ApiKey = apiKey
        };
        var json = System.Text.Json.JsonSerializer.Serialize(request);
        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
        var response = await httpClient.PostAsync(_apiUrl + "/translate", content);
        response.EnsureSuccessStatusCode();
        var responseJson = await response.Content.ReadAsStringAsync();
        var result = System.Text.Json.JsonSerializer.Deserialize<TranslateResponse>(
            responseJson
        );
        return result!;
    }

    public async Task<TranslateResponse> TranslateBatchAsync(
        List<string> texts,
        string sourceLang,
        string targetLang,
        string format = "text", int? alternatives = null,
        string apiKey = ""
    )
    {
        var request = new TranslateRequest
        {
            Q = texts,
            Source = sourceLang,
            Target = targetLang,
            Format = format,
            Alternatives = alternatives,
            ApiKey = apiKey
        };
        var json = System.Text.Json.JsonSerializer.Serialize(request);
        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
        var response = await httpClient.PostAsync(_apiUrl + "/translate", content);
        response.EnsureSuccessStatusCode();
        var responseJson = await response.Content.ReadAsStringAsync();
        var result = System.Text.Json.JsonSerializer.Deserialize<TranslateResponse>(
            responseJson
        );
        return result!;
    }
}
