using Exercise1.Abstractions;
using Exercise1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace Exercise1
{
	public class HttpFunctions
	{
		private readonly IMovieSearcher SearchService;

		public HttpFunctions( IMovieSearcher searchService )
		{
			SearchService = searchService;
		}

		[FunctionName( "Example-Search" )]
		public async Task<IActionResult> Search( [HttpTrigger( AuthorizationLevel.Anonymous, "post", Route = "search" )] HttpRequest req )
		{
			if ( !req.Query.ContainsKey( "today" ) || req.Query["today"] != DateTime.Today.ToString( "yyyy-MM-dd" ) )
				return new BadRequestResult();

			var qry = JsonConvert.DeserializeObject<SearchRequest>( await req.ReadAsStringAsync() );

			return new OkObjectResult( await SearchService.Search( qry.Query ) );
		}
	}
}
