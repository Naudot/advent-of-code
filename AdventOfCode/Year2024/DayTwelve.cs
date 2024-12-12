namespace AdventOfCode.Year2024
{
	public class DayTwelve : Day2024
	{
		public class Region
		{
			public char Symbol;
			public HashSet<(int, int)> Plots = new();
		}

		protected override bool DeactivateJIT => true;

		protected override object ResolveFirstPart(string[] input)
		{
			return GetRegions(input)
				.Select(region => region.Plots.Count() * GetPerimeter(region))
				.Sum();
		}

		protected override object ResolveSecondPart(string[] input)
		{
			return GetRegions(input)
				.Select(region => region.Plots.Count() * GetSides(region))
				.Sum();
		}

		private List<Region> GetRegions(string[] input)
		{
			List<Region> regions = new();
			HashSet<(int, int)> processedPlots = new();

			for (int y = 0; y < input.Length; y++)
			{
				for (int x = 0; x < input[y].Length; x++)
				{
					if (processedPlots.Contains((x, y)))
						continue;

					char symbol = input[y][x];
					List<(int x, int y)> toProcess = new() { (x, y) };
					Region region = new() { Symbol = symbol, Plots = new() { (x, y) } };

					while (toProcess.Count > 0)
					{
						(int x, int y) position = toProcess[0];
						toProcess.Remove(position);

						(int x, int y) leftPosition = (position.x - 1, position.y);
						(int x, int y) upPosition = (position.x, position.y - 1);
						(int x, int y) rightPosition = (position.x + 1, position.y);
						(int x, int y) downPosition = (position.x, position.y + 1);

						// Left
						if (leftPosition.x >= 0)
							if (input[leftPosition.y][leftPosition.x] == symbol && !region.Plots.Contains(leftPosition))
							{
								region.Plots.Add(leftPosition);
								toProcess.Add(leftPosition);
							}
						// Up
						if (upPosition.y >= 0)
							if (input[upPosition.y][upPosition.x] == symbol && !region.Plots.Contains(upPosition))
							{
								region.Plots.Add(upPosition);
								toProcess.Add(upPosition);
							}
						// Right
						if (rightPosition.x < input[y].Length)
							if (input[rightPosition.y][rightPosition.x] == symbol && !region.Plots.Contains(rightPosition))
							{
								region.Plots.Add(rightPosition);
								toProcess.Add(rightPosition);
							}
						// Down
						if (downPosition.y < input.Length)
							if (input[downPosition.y][downPosition.x] == symbol && !region.Plots.Contains(downPosition))
							{
								region.Plots.Add(downPosition);
								toProcess.Add(downPosition);
							}

						processedPlots.Add(position);
					}

					regions.Add(region);
				}
			}

			return regions;
		}

		private int GetPerimeter(Region region)
		{
			int perimeter = 0;

			foreach ((int x, int y) plot in region.Plots)
			{
				int plotPerimeter = 4;
				if (region.Plots.Contains((plot.x - 1, plot.y)))
					plotPerimeter--;
				if (region.Plots.Contains((plot.x, plot.y - 1)))
					plotPerimeter--;
				if (region.Plots.Contains((plot.x + 1, plot.y)))
					plotPerimeter--;
				if (region.Plots.Contains((plot.x, plot.y + 1)))
					plotPerimeter--;
				perimeter += plotPerimeter;
			}

			//Console.WriteLine($"A region of {region.Symbol} plants with price {region.Plots.Count} * {perimeter} = {region.Plots.Count * perimeter}.");

			return perimeter;
		}

		private int GetSides(Region region)
		{
			int sides = 0;

			return sides;
		}
	}
}
