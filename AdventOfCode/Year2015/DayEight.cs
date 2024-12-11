using System.Text.RegularExpressions;

namespace AdventOfCode.Year2015
{
	public class DayEight : Day2015
	{
		protected override object ResolveFirstPart(string[] input)
		{
			int stringCount = 0;
			int literalCount = 0;

			for (int i = 0; i < input.Length; i++)
			{
				stringCount += input[i].Length;
				input[i] = Regex.Unescape(input[i]);
				literalCount += input[i].Length - 2; // Remove the "
			}

			return stringCount - literalCount;
		}

		protected override object ResolveSecondPart(string[] input)
		{
			int stringCount = 0;
			int literalCount = 0;

			for (int i = 0; i < input.Length; i++)
			{
				stringCount += input[i].Length;

				literalCount +=
					input[i].Length +
					input[i].Count(character => character == '\\') +
					input[i].Count(character => character == '"') + 2;
			}

			return literalCount - stringCount;
		}
	}
}
