using Exercise1.Abstractions;
using Exercise1.Ioc;
using Exercise1.Services;
using Exercise1.TheMovieDb.SDK;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq;
using System.Reflection;

namespace Exercise1.Tests.Bootstrap
{
	[TestClass]
	public class BootstrapIocTests
	{
		[TestMethod]
		public void BootstrapIoc_ImplementsBase()
			=> Assert.IsInstanceOfType( new BootstrapIoc(), typeof( FunctionsStartup ) );

		[TestMethod]
		public void BootstrapIoc_AssemblyRegistration()
		{
			var startups = Assembly.GetAssembly( typeof( BootstrapIoc ) ).GetCustomAttributes<FunctionsStartupAttribute>();
			Assert.IsTrue(
				startups.Any(
					s => s.WebJobsStartupType == typeof( BootstrapIoc )
				)
			);
		}

		[TestMethod]
		public void BootstrapIoc_RegistersServices()
		{
			var svcs = new ServiceCollection();
			var mockBuilder = new Mock<IFunctionsHostBuilder>();
			mockBuilder.SetupGet( b => b.Services ).Returns( svcs );

			var b = new BootstrapIoc();
			b.Configure( mockBuilder.Object );

			// check registrations
			Assert.IsTrue( svcs.Any( s => s.ServiceType == typeof( IMovieSearcher ) && s.ImplementationType == typeof( MovieSearchService ) ) );
			Assert.IsTrue( svcs.Any( s => s.ServiceType == typeof( IMovieRepository ) && s.ImplementationType == typeof( TmdbClient ) ) );
		}
	}
}
