using Microsoft.Extensions.Configuration;

namespace LibreTranslate.DotNet.IntegrationTests;

public class LibreTranslateClientIntegrationTests
{
    private readonly LibreTranslateSettings _settings;

    public LibreTranslateClientIntegrationTests()
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .AddJsonFile("appsettings.local.json", optional: true)
            .Build();
        _settings = configuration.GetSection("LibreTranslate").Get<LibreTranslateSettings>()!;
    }

    [Fact]
    public async Task TranslateAsync_CooperCasaApi_ReturnsExpectedTranslation()
    {
        using var httpClient = new HttpClient { BaseAddress = new Uri(_settings.ApiUrl) };
        var client = new LibreTranslateClient(httpClient, _settings.ApiKey);
        var result = await client.TranslateAsync(
            text: "Bonjour",
            sourceLang: "auto",
            targetLang: "en",
            alternatives: 3
        );

        Assert.NotNull(result.TranslatedText);
        Assert.True(result.TranslatedText.Length > 0);
        Assert.Equal("Hello", result.TranslatedText[0]);

        Assert.NotNull(result.DetectedLanguage);
        Assert.True(result.DetectedLanguage.Length > 0);
        Assert.Equal("fr", result.DetectedLanguage[0].Language);
        Assert.Equal(90, result.DetectedLanguage[0].Confidence);

        Assert.NotNull(result.Alternatives);
        Assert.True(result.Alternatives.Length > 0);
        Assert.Contains("Good morning", result.Alternatives[0]);
        Assert.Contains("Morning", result.Alternatives[0]);
    }

    [Fact]
    public async Task TranslateBatchAsync_CooperCasaApi_ReturnsExpectedBatchTranslation()
    {
        using var httpClient = new HttpClient { BaseAddress = new Uri(_settings.ApiUrl) };
        var client = new LibreTranslateClient(httpClient, _settings.ApiKey);
        var inputTexts = new List<string> { "Hello", "world" };
        var result = await client.TranslateBatchAsync(
            texts: inputTexts,
            sourceLang: "en",
            targetLang: "es",
            alternatives: 3
        );

        Assert.NotNull(result);
        Assert.Equal(2, result.TranslatedText.Length);
        Assert.Equal("Hola", result.TranslatedText[0]);
        Assert.Equal("mundo", result.TranslatedText[1]);

        Assert.NotNull(result.Alternatives);
        Assert.Equal(2, result.Alternatives.Length);
        Assert.Equal(["¿Hola", "- Hola"], result.Alternatives[0]);
        Assert.Equal(["mundo mundial", "mundial", "del mundo"], result.Alternatives[1]);
    }
}
