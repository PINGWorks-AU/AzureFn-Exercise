using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Exercise1.Abstractions.Models
{
	[JsonConverter( typeof( StringEnumConverter ) )]
	public enum Gender
	{
		Unspecified = 0,
		Female = 1,
		Male = 2
	}
}
