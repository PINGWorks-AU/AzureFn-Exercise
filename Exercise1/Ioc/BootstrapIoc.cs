using Exercise1.Abstractions;
using Exercise1.Clients;
using Exercise1.Services;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;

[assembly: FunctionsStartup( typeof( Exercise1.Ioc.BootstrapIoc ) )]

namespace Exercise1.Ioc
{
	public class BootstrapIoc : FunctionsStartup
	{
		public override void Configure( IFunctionsHostBuilder builder )
		{

			builder.Services
						.AddSingleton<IMovieSearcher, TmdbSearchService>()
						.AddTransient<IMovieRepository,TmdbClient>()
						.AddHttpClient<IMovieRepository,TmdbClient>(
							http => {
								http.BaseAddress = new Uri( "https://api.themoviedb.org" );
							}
						);
		}
	}
}
