namespace Exercise1.Abstractions.Models
{
	public class MovieCreditsRec
	{
		public string? Name { get; set; }
		public string? Character { get; set; }
		public Gender Gender { get; set; } = Gender.Unspecified;
	}
}
