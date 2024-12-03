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
					result += int.Parse(matches[j].Groups[1].Value) * int.Parse(matches[j].Groups[2].Value);
			}

			return result;
		}

		protected override object ResolveSecondPart(string[] input)
		{
			double result = 0;
			bool enabled = true;

			for (int i = 0; i < input.Length; i++)
			{
				MatchCollection matches = Regex.Matches(input[i], @"mul\((\d{1,3}),(\d{1,3})\)|do\(\)|don't\(\)");
				for (int j = 0; j < matches.Count; j++)
				{
					if (matches[j].Value == "do()" || matches[j].Value == "don't()")
						enabled = matches[j].Value == "do()";
					else if (enabled)
						result += int.Parse(matches[j].Groups[1].Value) * int.Parse(matches[j].Groups[2].Value);
				}
			}

			return result;
		}
	}
}
