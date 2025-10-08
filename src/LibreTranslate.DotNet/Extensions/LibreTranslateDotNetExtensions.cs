using Microsoft.Extensions.DependencyInjection;

namespace LibreTranslate.DotNet.Extensions;

public static class LibreTranslateDotNetExtensions
{
    public static IHttpClientBuilder AddLibreTranslateClient(this IServiceCollection services)
    {
        return services.AddHttpClient<ILibreTranslateClient, LibreTranslateClient>();
    }
}
