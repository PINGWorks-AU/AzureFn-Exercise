using Exercise1.Abstractions;
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
						.AddTheMovieDb( "51ed0958e22db09811303e357ee72425" )
						.AddSingleton<IMovieSearcher, MovieSearchService>();
		}
	}
}
