using Exercise1.Abstractions;
using Exercise1.Models;
using Exercise1.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Primitives;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise1.Tests
{
	[TestClass]
	public class HttpFunctionTests
	{
		private class QueryColl : IQueryCollection
		{
			private Dictionary<string,StringValues>	Values { get; }

			public QueryColl( Dictionary<string, StringValues> store )
			{
				Values = store;
			}

			public StringValues this[string key] => Values[key];
			public int Count => Values.Count;
			public ICollection<string> Keys => Values.Keys;
			public bool ContainsKey( string key ) => Values.ContainsKey( key );
			public IEnumerator<KeyValuePair<string, StringValues>> GetEnumerator() => Values.GetEnumerator();
			public bool TryGetValue( string key, out StringValues value ) => Values.TryGetValue( key, out value );
			IEnumerator IEnumerable.GetEnumerator() => Values.GetEnumerator();
		}

		[TestMethod]
		public void HttpFunction_FunctionBindings()
		{
			var m = typeof( HttpFunctions ).GetMethod( "Search" );
			var attrFn = m.GetCustomAttributes( typeof( FunctionNameAttribute ), false ).First() as FunctionNameAttribute;
			Assert.AreEqual( "Example-Search", attrFn.Name );

			var reqParam = m.GetParameters().First();
			Assert.IsTrue( reqParam.ParameterType == typeof( HttpRequest ) );
			var attrTrigger = reqParam.GetCustomAttributes( typeof( HttpTriggerAttribute ), false ).First() as HttpTriggerAttribute;
			Assert.AreEqual( AuthorizationLevel.Anonymous, attrTrigger.AuthLevel );
			CollectionAssert.AreEqual( new[] { "post" }, attrTrigger.Methods );
			Assert.AreEqual( "search", attrTrigger.Route );
		}

		[TestMethod]
		public async Task HttpFunction_Returns400IfMissingQueryString()
		{
			var fn = new HttpFunctions( Mock.Of<IMovieSearcher>() );

			// nothing in query so we get a 400
			IActionResult ret = await fn.Search( CreateMockRequest( includeQs: false ) );
			Assert.IsInstanceOfType( ret, typeof( BadRequestResult ) );

			// if we set the header we get a 200 response
			ret = await fn.Search( CreateMockRequest() );
			Assert.IsInstanceOfType( ret, typeof( OkObjectResult ) );
		}

		[TestMethod]
		public async Task HttpFunction_SearchesService()
		{
			var mockSvc = new Mock<IMovieSearcher>();
			mockSvc.Setup( s => s.Search( "search" ) ).ReturnsAsync( new[] { new SearchResult() } ).Verifiable();

			var fn = new HttpFunctions( mockSvc.Object );

			var ret = await fn.Search( CreateMockRequest() ) as OkObjectResult;

			Assert.IsNotNull( ret );
			Assert.IsTrue( ( ret.Value as IEnumerable<SearchResult> ).Any() );
			Mock.Verify( mockSvc );
		}

		private HttpRequest CreateMockRequest( bool includeQs = true )
		{
			var mockReq = new Mock<HttpRequest>();

			// query string requirement
			var dict = new Dictionary<string, StringValues>();
			var qry = new QueryColl( dict );
			mockReq.SetupGet( r => r.Query ).Returns( qry );

			// body
			var ms = new MemoryStream( Encoding.UTF8.GetBytes( @"{ ""query"": ""search"" }" ) );
			mockReq.SetupGet( r => r.Body ).Returns( ms );

			if ( includeQs )
				dict.Add( "today", DateTime.Today.ToString( "yyyy-MM-dd" ) );

			return mockReq.Object;
		}
	}
}
