namespace AdventOfCode.Year2015
{
	public class DayOne : Day2015
	{
		protected override object ResolveFirstPart(string[] input)
		{
			return input[0].Select(character => character == '(' ? 1 : -1).Sum();
		}

		protected override object ResolveSecondPart(string[] input)
		{
			int result = 0;

			for (int i = 0; i < input[0].Length; i++)
			{
				result += input[0][i] == '(' ? 1 : -1;
				if (result == -1)
					return i + 1;
			}

			return 0;
		}
	}
}
