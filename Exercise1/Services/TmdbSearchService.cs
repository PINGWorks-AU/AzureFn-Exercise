using Exercise1.Abstractions;
using Exercise1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exercise1.Services
{
	internal class TmdbSearchService : IMovieSearcher
	{
		private readonly IMovieRepository ApiClient;
		private IEnumerable<MovieRec> Movies;

		public TmdbSearchService( IMovieRepository apiClient )
		{
			ApiClient = apiClient;
		}

		public async Task<IEnumerable<SearchResult>> Search( string query )
		{
			Movies ??= await ApiClient.GetMoviesFromList();

			if ( string.IsNullOrEmpty( query ) )
				return Enumerable.Empty<SearchResult>();

			IEnumerable<MovieRec> resultList = GetMatchedMovies( query );
			var creditDict = await GetMovieCredits( resultList );

			return resultList.Select( m => MapSearchResult( m, creditDict ) );
		}

		private static SearchResult MapSearchResult( MovieRec m, Dictionary<int, IEnumerable<MovieCreditsRec>> creditDict )
			=> new SearchResult
				{
					Movie = new() { Id = m.Id, Title = m.Title },
					Credits = creditDict[m.Id].Select(
						c => new SearchResult.CreditInfo
						{
							Name = c.Name,
							Character = c.Character,
							Gender = c.Gender
						}
					)
				};
		private IEnumerable<MovieRec> GetMatchedMovies( string query )
		{
			return Movies.Where( m => m.Title.Contains( query, StringComparison.OrdinalIgnoreCase ) );
		}
		private async Task<Dictionary<int, IEnumerable<MovieCreditsRec>>> GetMovieCredits( IEnumerable<MovieRec> resultList )
		{
			var creditDict = new Dictionary<int, IEnumerable<MovieCreditsRec>>();
			foreach ( var movieId in resultList.Select( m => m.Id ) )
			{
				var credits = await ApiClient.GetCreditsForMovie( movieId );
				creditDict.Add( movieId, credits );
			}

			return creditDict;
		}
	}
}
