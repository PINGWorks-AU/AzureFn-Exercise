using Exercise1.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Exercise1.Services
{
	public interface ISearchService
	{
		Task<IEnumerable<SearchResult>> Search( string query );
	}
}
