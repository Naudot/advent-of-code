using System.Text.RegularExpressions;

namespace AdventOfCode.Year2015
{
	public class DayTwelve : Day2015
	{
		protected override object ResolveFirstPart(string[] input)
		{
			return Regex.Matches(input[0], @"\d+|-\d+").Sum(match => int.Parse(match.Value));
		}

		protected override object ResolveSecondPart(string[] input)
		{
			return 0;
		}
	}
}
