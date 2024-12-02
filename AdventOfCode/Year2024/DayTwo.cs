namespace AdventOfCode.Year2024
{
	public class DayTwo : Day2024
	{
		protected override object ResolveFirstPart(string[] input)
		{
			return input.Select(line => line.Split(' ')
							.Select(level => int.Parse(level))
							.ToArray())
						.Where(report => IsReportSafe(report))
						.Count();
		}

		protected override object ResolveSecondPart(string[] input)
		{
			return input.Select(line => line.Split(' ')
							.Select(level => int.Parse(level))
							.ToArray())
						.Where(report => IsReportSafe(report) 
							|| report.Select((_, indexToTrim) => IsReportSafe(report.Where((_, i) => i != indexToTrim).ToArray()))
									 .Any(safe => safe))
						.Count();
		}

		private bool IsReportSafe(int[] report)
		{
			bool isIncreasing = (report[0] - report[1]) < 0;
			for (int j = 0; j < report.Length - 1; j++)
			{
				int diff = report[j] - report[j + 1];
				if (Math.Abs(diff) < 1 || Math.Abs(diff) > 3 || (isIncreasing ? diff > 0 : diff < 0))
					return false;
			}

			return true;
		}
	}
}
