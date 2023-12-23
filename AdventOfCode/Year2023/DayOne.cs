namespace AdventOfCode.Year2023
{
	public class DayOne : Day2023
	{
		protected override object ResolveFirstPart(string[] input)
		{
			return input.Select(line => 
							(line.FirstOrDefault(c => c >= '0' && c <= '9') - 48) * 10
							+ (line.LastOrDefault(c => c >= '0' && c <= '9') - 48))
						.Sum();
		}

		protected override object ResolveSecondPart(string[] input)
		{
			return input.Select(line => line
							.Replace("one", "o1ne")
							.Replace("two", "t2wo")
							.Replace("three", "th3ree")
							.Replace("four", "fo4ur")
							.Replace("five", "fi5ve")
							.Replace("six", "s6ix")
							.Replace("seven", "se7ven")
							.Replace("eight", "ei8ght")
							.Replace("nine", "ni9ne")
							)
						.Select(line =>
							(line.First(c => c >= '0' && c <= '9') - 48) * 10
							+ (line.Last(c => c >= '0' && c <= '9') - 48))
						.Sum();
		}
	}
}
