using Exercise1.Clients;
using Exercise1.Ioc;
using Exercise1.Services;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Linq;
using System.Reflection;

namespace Exercise1.Tests.Bootstrap
{
	[TestClass]
	public class StartupTests
	{
		[TestMethod]
		public void _ImplementsBase()
			=> Assert.IsInstanceOfType( new BootstrapIoc(), typeof( FunctionsStartup ) );

		[TestMethod]
		public void _AssemblyRegistration()
		{
			var startups = Assembly.GetAssembly( typeof( BootstrapIoc ) ).GetCustomAttributes<FunctionsStartupAttribute>();
			Assert.IsTrue(
				startups.Any(
					s => s.WebJobsStartupType == typeof( BootstrapIoc )
				)
			);
		}

		[TestMethod]
		public void _RegistersServices()
		{
			var svcs = new ServiceCollection();
			var mockBuilder = new Mock<IFunctionsHostBuilder>();
			mockBuilder.SetupGet( b => b.Services ).Returns( svcs );

			var b = new BootstrapIoc();
			b.Configure( mockBuilder.Object );

			// check registrations
			Assert.IsTrue( svcs.Any( s => s.ServiceType == typeof(ISearchService) && s.ImplementationType == typeof(SearchService) ) );
			Assert.IsTrue( svcs.Any( s => s.ServiceType == typeof(ITmdbClient) && s.ImplementationType == typeof(TmdbClient) ) );
		}
	}
}
