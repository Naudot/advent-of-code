namespace AdventOfCode.Year2025
{
	public class DayFive : Day2025
	{
		public class Pair
		{
			public long Min;
			public long Max;
		}

		protected override bool DeactivateJIT => true;

		protected override object ResolveFirstPart(string[] input)
		{
			int freshCount = 0;
			HashSet<(long min, long max)> ranges = new();

			for (int i = 0; i < input.Length; i++)
			{
				if (string.IsNullOrEmpty(input[i]))
					continue;

				if (input[i].Contains("-"))
					ranges.Add((long.Parse(input[i].Split('-')[0]), long.Parse(input[i].Split('-')[1])));
				else
				{
					long value = long.Parse(input[i]);

					foreach ((long min, long max) in ranges)
					{
						if (value >= min && value <= max)
						{
							freshCount++;
							break;
						}
					}
				}
			}

			return freshCount;
		}

		protected override object ResolveSecondPart(string[] input)
		{
			List<Pair> ranges = new();

			for (int i = 0; i < input.Length; i++)
				if (!string.IsNullOrEmpty(input[i]) && input[i].Contains('-'))
					ranges.Add(new Pair { Min = long.Parse(input[i].Split('-')[0]), Max = long.Parse(input[i].Split('-')[1]) });
			
			ranges = ranges.OrderBy(l_range => l_range.Max).OrderBy(l_range => l_range.Min).ToList();

			int rangesCount = ranges.Count;
			for (int i = 0; i < rangesCount - 1; i++)
			{
				if ((ranges[i].Max >= ranges[i + 1].Min && ranges[i].Max <= ranges[i + 1].Max))
				{
					ranges[i].Max = ranges[i + 1].Max;
					ranges.Remove(ranges[i + 1]);
				}
				else if (ranges[i].Max >= ranges[i + 1].Max)
				{
					ranges.Remove(ranges[i + 1]);
				}

				if (rangesCount != ranges.Count)
				{
					rangesCount--;
					i = -1;
				}
			}

			long freshCount = 0;
			foreach (Pair pair in ranges)
				freshCount += (pair.Max - pair.Min) + 1;
			return freshCount;
		}
	}
}
