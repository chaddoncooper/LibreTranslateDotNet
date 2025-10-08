using Microsoft.Extensions.DependencyInjection;

namespace LibreTranslate.DotNet.Extensions;

public static class LibreTranslateDotNetExtensions
{
    /// <summary>
    /// Registers the LibreTranslateClient as a typed HttpClient.
    /// </summary>
    /// <param name="services">The IServiceCollection.</param>
    /// <param name="apiUrl">The base URL for the LibreTranslate API.</param>
    /// <param name="apiKey">The API key for LibreTranslate.</param>
    /// <returns>The IHttpClientBuilder to allow for further configuration.</returns>
    public static IHttpClientBuilder AddLibreTranslateHttpClient(
        this IServiceCollection services,
        string apiUrl,
        string apiKey = ""
    )
    {
        return services.AddHttpClient<LibreTranslateClient>(client =>
        {
            client.BaseAddress = new Uri(apiUrl.TrimEnd('/'));
        })
        .AddTypedClient((httpClient, _) => new LibreTranslateClient(httpClient, apiKey));
    }
}
