using LibreTranslate.DotNet.Models;

namespace LibreTranslate.DotNet;

public interface ILibreTranslateClient
{
    Task<TranslateResponse> TranslateAsync(string text, string sourceLang, string targetLang, string format = "text", int? alternatives = null, string apiKey = "");
    Task<TranslateResponse> TranslateBatchAsync(List<string> texts, string sourceLang, string targetLang, string format = "text", int? alternatives = null, string apiKey = "");
}