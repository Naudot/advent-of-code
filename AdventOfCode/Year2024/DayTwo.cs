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
				safeReportsCount -= IsReportSafe(levels) ? 0 : 1;
			}

			return safeReportsCount;
		}

		protected override object ResolveSecondPart(string[] input)
		{
			int safeReportsCount = input.Length;

			for (int i = 0; i < input.Length; i++)
			{
				int[] levels = input[i].Split(' ').Select(level => int.Parse(level)).ToArray();
				bool isSafe = IsReportSafe(levels);

				if (!isSafe)
				{
					bool isOneReportSafeAmongTrimmedLevels = false;
					for (int j = 0; j < levels.Length; j++)
					{
						int[] trimmedLevels = levels.Where((val, index) => index != j).ToArray();
						isOneReportSafeAmongTrimmedLevels |= IsReportSafe(trimmedLevels);
					}

					if (!isOneReportSafeAmongTrimmedLevels)
						safeReportsCount --;
				}
			}

			return safeReportsCount;
		}

		private bool IsReportSafe(int[] levels)
		{
			bool isIncreasing = (levels[0] - levels[1]) < 0;
			for (int j = 0; j < levels.Length - 1; j++)
			{
				int diff = levels[j] - levels[j + 1];
				if (Math.Abs(diff) < 1 || Math.Abs(diff) > 3 || (isIncreasing ? diff > 0 : diff < 0))
					return false;
			}

			return true;
		}
	}
}
