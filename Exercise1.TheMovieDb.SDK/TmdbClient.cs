using Exercise1.Abstractions;
using Exercise1.Abstractions.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Exercise1.TheMovieDb.SDK
{
	internal partial class TmdbClient : IMovieRepository
	{
		private readonly HttpClient Http;
		private readonly string ApiKey;

		public TmdbClient( HttpClient http, TmdbOptions options )
		{
			Http = http;
			ApiKey = options.ApiKey;
		}

		public async Task<IEnumerable<MovieCreditsRec>> GetCreditsForMovie( int movieId )
		{
			var resp = await Http.GetAsync( $"/3/movie/{movieId}/credits?api_key={ApiKey}" );

			if ( !resp.IsSuccessStatusCode )
				return Enumerable.Empty<MovieCreditsRec>();

			var ret = JsonConvert.DeserializeObject<CreditsResult>( await resp.Content.ReadAsStringAsync() );
			return ret?.Cast
						?.Where( i => i.Dept == "Acting" )
						?.Select( i => new MovieCreditsRec { Character = i.Character, Gender = i.Gender, Name = i.Name } )
						?? Enumerable.Empty<MovieCreditsRec>();
		}

		public async Task<IEnumerable<MovieRec>> GetMoviesFromList()
		{
			var resp = await Http.GetAsync( $"/3/list/7097389?api_key={ApiKey}" );

			if ( !resp.IsSuccessStatusCode )
				return Enumerable.Empty<MovieRec>();

			var ret = JsonConvert.DeserializeObject<ListResult>( await resp.Content.ReadAsStringAsync() );
			return ret?.Items?.Select( i => new MovieRec { Id = i.Id, Title = i.Title } ) ?? Enumerable.Empty<MovieRec>();
		}
	}
}
