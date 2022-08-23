using System.Collections.Generic;

namespace Exercise1.Models
{
	public class SearchResult
	{
		public MovieInfo Movie { get; set; }
		public IEnumerable<CreditInfo> Credits { get; set; }

		public class MovieInfo
		{
			public int Id { get; set; }
			public string Title { get; set; }
		}

		public class CreditInfo
		{
			public Gender Gender { get; set; }
			public string Name { get; set; }
			public string Character { get; set; }
		}
	}
}
