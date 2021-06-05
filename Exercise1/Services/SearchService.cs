using Exercise1.Clients;
using Exercise1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exercise1.Services
{
	public class SearchService : ISearchService
	{
		private readonly ITmdbClient ApiClient;
		private IEnumerable<MovieRec> Movies;

		public SearchService( ITmdbClient apiClient )
		{
			ApiClient = apiClient;
		}

		public async Task<IEnumerable<SearchResult>> Search( string query )
		{
			Movies ??= await ApiClient.GetMoviesFromList();

			if ( string.IsNullOrEmpty( query ) )
				return Enumerable.Empty<SearchResult>();

			var resultList = Movies.Where( m => m.Title.Contains( query, StringComparison.OrdinalIgnoreCase ) );

			var creditDict = new Dictionary<int, IEnumerable<MovieCreditsRec>>();
			foreach ( var movie in resultList )
			{
				var credits = await ApiClient.GetCreditsForMovie( movie.Id );
				creditDict.Add( movie.Id, credits );
			}

			return Movies
						.Where( m => m.Title.Contains( query, StringComparison.OrdinalIgnoreCase ) )
						.Select( m => {
							return new SearchResult {
								Movie = new() {
									Id = m.Id,
									Title = m.Title
								},
								Credits = creditDict[m.Id].Select(
									c => new SearchResult.CreditInfo {
									  Name = c.Name,
									  Character = c.Character,
									  Gender = c.Gender
									}
								)
							};
						} );
		}
	}
}
