using LibreTranslate.DotNet.Models;

namespace LibreTranslate.DotNet;

public interface ILibreTranslateClient
{
    Task<TranslateResponse> TranslateAsync(string text, string sourceLang, string targetLang, int? alternatives = null);
    Task<TranslateResponse> TranslateBatchAsync(List<string> texts, string sourceLang, string targetLang, int? alternatives = null);
}