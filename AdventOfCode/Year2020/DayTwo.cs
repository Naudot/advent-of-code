using System.Text.RegularExpressions;

namespace AdventOfCode.Year2020
{
	public class DayTwo : Day2020
	{
		protected override object ResolveFirstPart()
		{
			string[] input = File.ReadAllLines(GetResourcesPath());

			int result = 0;

			for (int i = 0; i < input.Length; i++)
			{
				Match match = Regex.Match(input[i], @"([\d]*)-([\d]*) ([a-z]): ([a-z]*)");
				int min = int.Parse(match.Groups[1].Value);
				int max = int.Parse(match.Groups[2].Value);
				char letter = char.Parse(match.Groups[3].Value);
				int count = match.Groups[4].Value.Count((arg) => arg == letter);
				if (min <= count && count <= max)
				{
					result++;
				}
			}

			return result;
		}

		protected override object ResolveSecondPart()
		{
			string[] input = File.ReadAllLines(GetResourcesPath());

			int result = 0;

			for (int i = 0; i < input.Length; i++)
			{
				Match match = Regex.Match(input[i], @"([\d]*)-([\d]*) ([a-z]): ([a-z]*)");
				int firstIndex = int.Parse(match.Groups[1].Value);
				int secondIndex = int.Parse(match.Groups[2].Value);
				char letter = char.Parse(match.Groups[3].Value);
				string password = match.Groups[4].Value;
				bool isFirstMatching = password[firstIndex - 1] == letter;
				bool isSecondMatching = password[secondIndex - 1] == letter;
				result += (isFirstMatching ^ isSecondMatching) ? 1 : 0;
			}

			return result;
		}
	}
}
