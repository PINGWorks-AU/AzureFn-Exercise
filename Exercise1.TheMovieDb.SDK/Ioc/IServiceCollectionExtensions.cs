using Exercise1.Abstractions;
using Exercise1.TheMovieDb.SDK;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class IServiceCollectionExtensions
    {
        public const string TmdbBaseUrl = "https://api.themoviedb.org";

        public static IServiceCollection AddTheMovieDb( this IServiceCollection services, string apiKey )
        {
            services.AddSingleton( new TmdbOptions { ApiKey = apiKey } )
                    .AddTransient<IMovieRepository, TmdbClient>()
                    .AddHttpClient<IMovieRepository, TmdbClient>(
                        http => http.BaseAddress = new Uri( TmdbBaseUrl )
                    );

            return services;
        }
    }
}
