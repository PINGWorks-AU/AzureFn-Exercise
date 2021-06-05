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
						.AddSingleton<ISearchService, SearchService>()
						.AddTransient<ITmdbClient,TmdbClient>()
						.AddHttpClient<ITmdbClient,TmdbClient>(
							http => {
								http.BaseAddress = new Uri( "https://api.themoviedb.org" );
							}
						);
		}
	}
}
