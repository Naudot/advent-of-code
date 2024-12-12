namespace AdventOfCode.Year2024
{
	public class DayTwelve : Day2024
	{
		public class Region
		{
			public char Symbol;
			public HashSet<(int x, int y)> Plots = new();
		}

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
					(int x, int y) position = (x, y);

					if (processedPlots.Contains(position))
						continue;

					char symbol = input[y][x];
					List<(int x, int y)> toProcess = new() { position };
					Region region = new() { Symbol = symbol, Plots = new() { position } };

					do
					{
						position = toProcess[0];
						toProcess.Remove(position);

						for (int i = 0; i < StaticBank.Directions.Count; i++)
						{
							(int x, int y) direction = (position.x + StaticBank.Directions[i].x, position.y + StaticBank.Directions[i].y);
							if (StaticBank.IsInBoundaries(direction, input))
								if (input[direction.y][direction.x] == symbol && !region.Plots.Contains(direction))
								{
									region.Plots.Add(direction);
									toProcess.Add(direction);
								}
						}

						processedPlots.Add(position);
					} while (toProcess.Count > 0);

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

			int minX = region.Plots.Select(plot => plot.x).Min();
			int minY = region.Plots.Select(plot => plot.y).Min();
			int maxX = region.Plots.Select(plot => plot.x).Max();
			int maxY = region.Plots.Select(plot => plot.y).Max();

			for (int y = minY; y <= maxY; y++)
			{
				bool hasUpSideBegun = false;
				bool hasDownSideBegun = false;
				for (int x = minX; x <= maxX; x++)
				{
					(int x, int y) position = (x, y);

					(int x, int y) upPosition = (x, y - 1);
					if (region.Plots.Contains(position) && !region.Plots.Contains(upPosition))
						hasUpSideBegun = true;
					else if (!region.Plots.Contains(position) || region.Plots.Contains(upPosition))
					{
						if (hasUpSideBegun)
							sides++;
						hasUpSideBegun = false;
					}

					(int x, int y) downPosition = (x, y + 1);
					if (region.Plots.Contains(position) && !region.Plots.Contains(downPosition))
						hasDownSideBegun = true;
					else if (!region.Plots.Contains(position) || region.Plots.Contains(downPosition))
					{
						if (hasDownSideBegun)
							sides++;
						hasDownSideBegun = false;
					}
				}
				if (hasUpSideBegun)
					sides++;
				if (hasDownSideBegun)
					sides++;
			}

			for (int x = minX; x <= maxX; x++)
			{
				bool hasLeftSideBegun = false;
				bool hasRightSideBegun = false;
				for (int y = minY; y <= maxY; y++)
				{
					(int x, int y) position = (x, y);

					(int x, int y) leftPosition = (x - 1, y);
					if (region.Plots.Contains(position) && !region.Plots.Contains(leftPosition))
						hasLeftSideBegun = true;
					else if (!region.Plots.Contains(position) || region.Plots.Contains(leftPosition))
					{
						if (hasLeftSideBegun)
							sides++;
						hasLeftSideBegun = false;
					}

					(int x, int y) rightPosition = (x + 1, y);
					if (region.Plots.Contains(position) && !region.Plots.Contains(rightPosition))
						hasRightSideBegun = true;
					else if (!region.Plots.Contains(position) || region.Plots.Contains(rightPosition))
					{
						if (hasRightSideBegun)
							sides++;
						hasRightSideBegun = false;
					}
				}
				if (hasLeftSideBegun)
					sides++;
				if (hasRightSideBegun)
					sides++;
			}

			//Console.WriteLine($"A region of {region.Symbol} plants with price {region.Plots.Count} * {sides} = {region.Plots.Count * sides}.");

			return sides;
		}
	}
}
