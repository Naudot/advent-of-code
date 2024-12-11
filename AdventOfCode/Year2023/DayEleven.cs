namespace AdventOfCode.Year2023
{
	public class DayEleven : Day2023
	{
		protected override object ResolveFirstPart(string[] input)
		{
			return GetDistanceSum(input, 1);
		}

		protected override object ResolveSecondPart(string[] input)
		{
			return GetDistanceSum(input, 1000000);
		}

		private double GetDistanceSum(string[] input, double expansionFactor)
		{
			List<(double, double)> galaxies = new List<(double, double)>();
			for (int y = 0; y < input.Length; y++)
			{
				for (int x = 0; x < input[y].Length; x++)
				{
					if (input[y][x] == '#')
					{
						galaxies.Add((x, y));
					}
				}
			}

			List<string> rows = input.Select(line => line).ToList();
			List<int> rowIndexes = new List<int>();
			for (int i = 0; i < rows.Count; i++)
			{
				if (!rows[i].Where(c => c != '.').Any())
				{
					rowIndexes.Add(i);
				}
			}

			List<int> columnIndexes = new List<int>();
			for (int i = 0; i < input[0].Length; i++)
			{
				if (!input.Select(line => line[i]).Where(c => c != '.').Any())
				{
					columnIndexes.Add(i);
				}
			}

			// No idea why but I need to do this shit
			if (expansionFactor > 1)
			{
				expansionFactor--;
			}

			double distanceSum = 0;

			for (int i = 0; i < galaxies.Count; i++)
			{
				(double, double) galaxie = galaxies[i];
				double galaxieX = galaxie.Item1 + expansionFactor * columnIndexes.Where(index => index < galaxie.Item1).Count();
				double galaxieY = galaxie.Item2 + expansionFactor * rowIndexes.Where(index => index < galaxie.Item2).Count();
				for (int j = i + 1; j < galaxies.Count; j++)
				{
					double otherX = galaxies[j].Item1 + expansionFactor * columnIndexes.Where(index => index < galaxies[j].Item1).Count();
					double otherY = galaxies[j].Item2 + expansionFactor * rowIndexes.Where(index => index < galaxies[j].Item2).Count();
					distanceSum += GetManhattanDistance(galaxieX, galaxieY, otherX, otherY);
				}
			}

			return distanceSum;
		}

		private double GetManhattanDistance(double x1, double y1, double x2, double y2)
		{
			return Math.Abs(x2 - x1) + Math.Abs(y2 - y1);
		}
	}
}
