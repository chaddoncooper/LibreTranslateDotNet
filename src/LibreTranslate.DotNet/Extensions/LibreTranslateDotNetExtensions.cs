using Microsoft.Extensions.DependencyInjection;

namespace LibreTranslate.DotNet.Extensions;

public static class LibreTranslateDotNetExtensions
{
    public static IHttpClientBuilder AddMyApiClient(this IServiceCollection services)
    {
        return services.AddHttpClient<ILibreTranslateClient, LibreTranslateClient>();
    }
}
