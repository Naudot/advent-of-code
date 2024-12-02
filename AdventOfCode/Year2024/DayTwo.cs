namespace AdventOfCode.Year2024
{
	public class DayTwo : Day2024
	{
		protected override object ResolveFirstPart(string[] input)
		{
			int safeReportsCount = input.Length;

			for (int i = 0; i < input.Length; i++)
			{
				int[] levels = input[i].Split(' ').Select(level => int.Parse(level)).ToArray();

				bool isIncreasing = (levels[0] - levels[1]) < 0;
				for (int j = 0; j < levels.Length - 1; j++)
				{
					int diff = levels[j] - levels[j + 1];
					if (Math.Abs(diff) < 1 || Math.Abs(diff) > 3 || (isIncreasing ? diff > 0 : diff < 0))
					{
						safeReportsCount--;
						break;
					}
				}
			}

			return safeReportsCount;
		}

		protected override object ResolveSecondPart(string[] input)
		{
			return 0;
		}
	}
}
