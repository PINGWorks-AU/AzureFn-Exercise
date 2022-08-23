using Exercise1.Abstractions.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Exercise1.TheMovieDb.SDK
{
    internal partial class TmdbClient
    {
        [SuppressMessage( "Minor Code Smell", "S3459:Unassigned members should be removed", Justification = "Properties are used in deserialisation" )]
        private sealed class ListResult
        {
            public IEnumerable<Item> Items { get; set; } = Enumerable.Empty<Item>();
            public class Item
            {
                public string? Title { get; set; }
                public int Id { get; set; }
            }
        }

        [SuppressMessage( "Minor Code Smell", "S3459:Unassigned members should be removed", Justification = "Properties are used in deserialisation" )]
        private sealed class CreditsResult
        {
            public IEnumerable<Item> Cast { get; set; } = Enumerable.Empty<Item>();
            public class Item
            {
                public string? Name { get; set; }
                public string? Character { get; set; }
                [JsonProperty( "known_for_department" )]
                public string? Dept { get; set; }
                public Gender Gender { get; set; } = Gender.Unspecified;
            }
        }

    }
}
