using Microsoft.Extensions.DependencyInjection;

namespace LibreTranslate.DotNet.Extensions;

public static class LibreTranslateDotNetExtensions
{
    /// <summary>
    /// Registers the LibreTranslateClient as a typed HttpClient.
    /// </summary>
    /// <param name="services">The IServiceCollection.</param>
    /// <param name="apiUrl">The base URL for the LibreTranslate API.</param>
    /// <returns>The IHttpClientBuilder to allow for further configuration.</returns>
    public static IHttpClientBuilder AddLibreTranslateClient(
        this IServiceCollection services,
        string apiUrl)
    {
        // The AddHttpClient method is the key.
        return services.AddHttpClient<LibreTranslateClient>()
            .ConfigureHttpClient(client =>
            {
                client.BaseAddress = new Uri(apiUrl.TrimEnd('/'));
            });
    }
}
