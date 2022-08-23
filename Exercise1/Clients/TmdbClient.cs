using Exercise1.Abstractions;
using Exercise1.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Exercise1.Clients
{
	internal class TmdbClient : IMovieRepository
	{
		private const string ApiKey = "51ed0958e22db09811303e357ee72425";
		private readonly HttpClient Http;

		private sealed class ListResult
		{
			public IEnumerable<Item> Items { get; set; }
			public class Item
			{
				public string Title { get; set; }
				public int Id { get; set; }
			}
		}
		private sealed class CreditsResult
		{
			public IEnumerable<Item> Cast { get; set; }
			public class Item
			{
				public string Name { get; set; }
				public string Character { get; set; }
				[JsonProperty( "known_for_department" )]
				public string Dept { get; set; }
				public Gender Gender { get; set; }
			}
		}

		public TmdbClient( HttpClient http )
		{
			Http = http;
		}

		public async Task<IEnumerable<MovieCreditsRec>> GetCreditsForMovie( int movieId )
		{
			var resp = await Http.GetAsync( $"/3/movie/{movieId}/credits?api_key={ApiKey}" );

			if ( !resp.IsSuccessStatusCode )
				return Enumerable.Empty<MovieCreditsRec>();

			var ret = JsonConvert.DeserializeObject<CreditsResult>( await resp.Content.ReadAsStringAsync() );
			return ret.Cast
						.Where( i => i.Dept == "Acting" )
						.Select( i => new MovieCreditsRec { Character = i.Character, Gender = i.Gender, Name = i.Name } );
		}
		public async Task<IEnumerable<MovieRec>> GetMoviesFromList()
		{
			var resp = await Http.GetAsync( $"/3/list/7097389?api_key={ApiKey}" );

			if ( !resp.IsSuccessStatusCode )
				return Enumerable.Empty<MovieRec>();

			var ret = JsonConvert.DeserializeObject<ListResult>( await resp.Content.ReadAsStringAsync() );
			return ret.Items.Select( i => new MovieRec { Id = i.Id, Title = i.Title } );
		}
	}
}
