using System.Collections.Generic;
using System.Linq;

namespace Exercise1.Abstractions.Models
{
	public class SearchResult
	{
		public MovieInfo? Movie { get; set; }
		public IEnumerable<CreditInfo> Credits { get; set; } = Enumerable.Empty<CreditInfo>();

		public class MovieInfo
		{
			public int? Id { get; set; }
			public string? Title { get; set; }
		}

		public class CreditInfo
		{
			public Gender? Gender { get; set; }
			public string? Name { get; set; }
			public string? Character { get; set; }
		}
	}
}
