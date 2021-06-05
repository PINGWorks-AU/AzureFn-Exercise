using Exercise1.Clients;
using Exercise1.Models;
using Exercise1.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Exercise1.Tests.Services
{
	[TestClass]
	public class SearchServiceTests
	{
		[TestMethod]
		public void SearchService_ImplementsInterface()
			=> Assert.IsInstanceOfType( new SearchService( Mock.Of<ITmdbClient>() ), typeof( ISearchService ) );

		[TestMethod]
		public async Task SearchService_Search_LazyLoadMoviesAndCache()
		{
			var mockCl = new Mock<ITmdbClient>();
			mockCl.Setup( c => c.GetMoviesFromList() )
				.ReturnsAsync( new[] { new MovieRec() } )
				.Verifiable();

			var svc = new SearchService( mockCl.Object );
			await svc.Search( "" );
			mockCl.Verify( c => c.GetMoviesFromList(), Times.Once );

			await svc.Search( "" );
			await svc.Search( "" );
			mockCl.Verify( c => c.GetMoviesFromList(), Times.Once );
		}

		[TestMethod]
		public async Task SearchService_Search_Degenerates()
		{
			var svc = new SearchService( CreateClient().Object );
			Assert.AreEqual( Enumerable.Empty<SearchResult>(), await svc.Search( null ) );
			Assert.AreEqual( Enumerable.Empty<SearchResult>(), await svc.Search( "" ) );
		}

		[TestMethod]
		public async Task SearchService_Search_GetsCastForSearch()
		{
			var mockCl = CreateClient();
			mockCl.Setup( c => c.GetCreditsForMovie( 123 ) )
				.ReturnsAsync( new[] { new MovieCreditsRec { Name = "n", Character = "c", Gender = Gender.Female } } )
				.Verifiable();

			var svc = new SearchService( mockCl.Object );
			var ret = await svc.Search( "potato" );

			Mock.Verify( mockCl );

			Assert.AreEqual( 1, ret.Count() );
			Assert.AreEqual( 123, ret.First().Movie.Id );
			Assert.AreEqual( "POTato", ret.First().Movie.Title );

			Assert.AreEqual( 1, ret.First().Credits.Count() );
			var credit = ret.First().Credits.First();
			Assert.AreEqual( "n", credit.Name );
			Assert.AreEqual( "c", credit.Character );
			Assert.AreEqual( Gender.Female, credit.Gender );
		}

		[TestMethod]
		public void SearchResult_Serialization()
		{
			var result = new SearchResult {
							Movie = new() { Id = 1, Title = "a" },
							Credits = new[] { new SearchResult.CreditInfo { Character = "c", Name = "n", Gender = Gender.Female } } };

			var json = JsonConvert.SerializeObject( result, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() } );

			Assert.AreEqual(
				@"{""movie"":{""id"":1,""title"":""a""},""credits"":[{""gender"":""Female"",""name"":""n"",""character"":""c""}]}",
				json
			);
		}

		private Mock<ITmdbClient> CreateClient()
		{
			var mockCl = new Mock<ITmdbClient>();
			mockCl.Setup( c => c.GetMoviesFromList() )
				.ReturnsAsync( new[] { new MovieRec { Id = 123, Title = "POTato" }, new MovieRec { Title = "tomaTO" } } );

			return mockCl;
		}
	}
}
