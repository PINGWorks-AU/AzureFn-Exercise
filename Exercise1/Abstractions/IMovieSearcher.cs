using Exercise1.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Exercise1.Abstractions
{
    public interface IMovieSearcher
    {
        Task<IEnumerable<SearchResult>> Search(string query);
    }
}
