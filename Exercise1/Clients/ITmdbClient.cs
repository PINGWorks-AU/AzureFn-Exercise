using Exercise1.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Exercise1.Clients
{
	public interface ITmdbClient
	{
		Task<IEnumerable<MovieRec>> GetMoviesFromList();
		Task<IEnumerable<MovieCreditsRec>> GetCreditsForMovie( int movieId );
	}
}
