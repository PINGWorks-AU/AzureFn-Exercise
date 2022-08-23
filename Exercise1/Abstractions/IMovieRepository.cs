using Exercise1.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Exercise1.Abstractions
{
    public interface IMovieRepository
    {
        Task<IEnumerable<MovieRec>> GetMoviesFromList();
        Task<IEnumerable<MovieCreditsRec>> GetCreditsForMovie(int movieId);
    }
}
