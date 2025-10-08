using System.Net;

namespace LibreTranslate.DotNet.Tests;

public class LibreTranslateClientUnitTests
{
    private class MockHttpMessageHandler(Func<HttpRequestMessage, HttpResponseMessage> handler) : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(handler(request));
        }
    }

    [Fact]
    public async Task TranslateAsync_ReturnsTranslatedText_WithResponseModel()
    {
        var handler = new MockHttpMessageHandler(req =>
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(
                    """
                    {
                        "alternatives": [["Good morning", "Morning"]],
                        "detectedLanguage": [{ "confidence": 4, "language": "en" }],
                        "translatedText": ["Bonjour"]
                    }
                    """
                )
            };
            return response;
        });
        var httpClient = new HttpClient(handler);
        var client = new LibreTranslateClient("http://fake", httpClient);
        var result = await client.TranslateAsync("Hello", "en", "fr");
        Assert.NotNull(result.TranslatedText);
        Assert.Equal("Bonjour", result.TranslatedText[0]);
        Assert.NotNull(result.DetectedLanguage);
        Assert.Equal("en", result.DetectedLanguage[0].Language);
        Assert.Equal(4, result.DetectedLanguage[0].Confidence);
        Assert.NotNull(result.Alternatives);
        Assert.Contains("Good morning", result.Alternatives[0]);
        Assert.Contains("Morning", result.Alternatives[0]);
    }

    [Fact]
    public async Task TranslateBatchAsync_ReturnsTranslatedTexts_WithResponseModel()
    {
        var handler = new MockHttpMessageHandler(req =>
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(
                    """
                    {
                        "alternatives": [
                            [],
                            [
                                "mundo mundial",
                                "mundial",
                                "del mundo"
                            ]
                        ],
                        "detectedLanguage": [
                            {
                                "confidence": 7.5,
                                "language": "en"
                            },
                            {
                                "confidence": 7.5,
                                "language": "en"
                            }
                        ],
                        "translatedText": [
                            "hola",
                            "mundo"
                        ]
                    }
                    """
                )
            };
            return response;
        });
        var httpClient = new HttpClient(handler);
        var client = new LibreTranslateClient("http://fake", httpClient);
        var result = await client.TranslateBatchAsync(["Hello", "world"], "en", "es");
        Assert.NotNull(result);
        Assert.Equal(2, result.TranslatedText.Length);
        Assert.Equal("hola", result.TranslatedText[0]);
        Assert.Equal("mundo", result.TranslatedText[1]);
        Assert.Equal(2, result.DetectedLanguage!.Length);
        Assert.All(result.DetectedLanguage, r => Assert.Equal("en", r.Language));
        Assert.All(result.DetectedLanguage, r => Assert.Equal(7.5f, r.Confidence));
        Assert.Equal(2, result.Alternatives!.Length);
        Assert.Empty(result.Alternatives[0]);
        Assert.Contains("mundo mundial", result.Alternatives[1]);
        Assert.Contains("mundial", result.Alternatives[1]);
        Assert.Contains("del mundo", result.Alternatives[1]);
    }
}
