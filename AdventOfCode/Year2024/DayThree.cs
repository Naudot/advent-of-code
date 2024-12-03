using System.Text.RegularExpressions;

namespace AdventOfCode.Year2024
{
	public class DayThree : Day2024
	{
		protected override object ResolveFirstPart(string[] input)
		{
			double result = 0;

			for (int i = 0; i < input.Length; i++)
			{
				MatchCollection matches = Regex.Matches(input[i], @"mul\((\d{1,3}),(\d{1,3})\)");
				for (int j = 0; j < matches.Count; j++)
				{
					int left = int.Parse(matches[j].Groups[1].Value);
					int right = int.Parse(matches[j].Groups[2].Value);
					result += left * right;
				}
			}

			return result;
		}

		protected override object ResolveSecondPart(string[] input)
		{
			return 0;
		}
	}
}
