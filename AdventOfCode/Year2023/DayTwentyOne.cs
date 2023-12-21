namespace AdventOfCode.Year2023
{
	public class DayTwentyOne : Day2023
	{
		protected override bool DeactivateJIT => true;

		protected override object ResolveFirstPart(string[] input)
		{
			(int X, int Y) start = (input[0].Length / 2, input.Length / 2);
			List<(int, int)> steps = new() { start };
			HashSet<(int, int)> newSteps = new();

			int stepCount = 6;

			for (int i = 0; i < stepCount; i++)
			{
				for (int j = 0; j < steps.Count; j++)
				{
					List<(int, int)> neighbors = GetProperNeighbors(input, steps[j], false);
					for (int k = 0; k < neighbors.Count; k++)
					{
						newSteps.Add(neighbors[k]);
					}
				}

				steps.Clear();
				steps.AddRange(newSteps);
				newSteps.Clear();

				//for (int y = 0; y < input.Length; y++)
				//{
				//	Console.WriteLine();
				//	for (int x = 0; x < input[0].Length; x++)
				//	{
				//		Console.Write(steps.Contains((x, y)) ? 'O' : input[y][x]);
				//	}
				//}
				//Console.WriteLine();
			}

			return steps.Count;
		}

		protected override object ResolveSecondPart(string[] input)
		{
			(int X, int Y) start = (input[0].Length / 2, input.Length / 2);
			List<(int, int)> steps = new() { start };
			HashSet<(int, int)> newSteps = new();
			Dictionary<int, int> diffs = new();

			int stepCount = 11 * 11;
			int test = 4;

			int sizeY = input.Length;
			int sizeX = input[0].Length;
			int totalSize = sizeY * sizeX;
			int wallCount = 0;

			for (int y = 0; y < sizeY; y++)
			{
				for (int x = 0; x < sizeX; x++)
				{
					if (input[y][x] == '#') wallCount++;
				}
			}

			for (int i = 0; i < stepCount; i++)
			{
				for (int j = 0; j < steps.Count; j++)
				{
					List<(int, int)> neighbors = GetProperNeighbors(input, steps[j], true);
					for (int k = 0; k < neighbors.Count; k++)
					{
						newSteps.Add(neighbors[k]);
					}
				}

				steps.Clear();
				steps.AddRange(newSteps);
				newSteps.Clear();

				if (i >= 1)
					test += 5 + ((i - 1) * 2);

				if (i % 11 == 0)
					Console.WriteLine(steps.Count + " " + test);

				//for (int y = 0; y < input.Length; y++)
				//{
				//	Console.WriteLine();
				//	for (int x = 0; x < input[0].Length; x++)
				//	{
				//		Console.Write(steps.Contains((x, y)) ? 'O' : input[y][x]);
				//	}
				//}
				//Console.WriteLine();
			}

			return steps.Count;
		}

		private List<(int, int)> GetProperNeighbors(string[] input, (int X, int Y) coord, bool useInfinite)
		{
			List<(int, int)> neighbors = new();

			int sizeX = input[0].Length - 1;
			int sizeY = input.Length - 1;

			int trueX = coord.X;
			int trueY = coord.Y;

			bool ignoreWalls = true;

			if (useInfinite)
			{
				if (trueX > 0) trueX %= input[0].Length;
				while (trueX < 0) trueX += input[0].Length;
				if (trueY > 0) trueY %= input.Length;
				while (trueY < 0) trueY += input.Length;

				int left = trueX - 1 < 0 ? trueX + sizeX : trueX - 1;
				int right = trueX + 1 > sizeX ? trueX - sizeX : trueX + 1;
				int top = trueY - 1 < 0 ? trueY + sizeY : trueY - 1;
				int bottom = trueY + 1 > sizeY ? trueY - sizeY : trueY + 1;

				if (ignoreWalls || input[trueY][left] != '#')
					neighbors.Add((coord.X - 1, coord.Y));

				if (ignoreWalls || input[trueY][right] != '#')
					neighbors.Add((coord.X + 1, coord.Y));

				if (ignoreWalls || input[top][trueX] != '#')
					neighbors.Add((coord.X, coord.Y - 1));

				if (ignoreWalls || input[bottom][trueX] != '#')
					neighbors.Add((coord.X, coord.Y + 1));
			}
			else
			{
				if (ignoreWalls || trueX > 0 && input[coord.Y][trueX - 1] != '#')
					neighbors.Add((coord.X - 1, coord.Y));

				if (ignoreWalls || trueX < input[0].Length - 1 && input[coord.Y][trueX + 1] != '#')
					neighbors.Add((coord.X + 1, coord.Y));

				if (ignoreWalls || trueY > 0 && input[trueY - 1][coord.X] != '#')
					neighbors.Add((coord.X, coord.Y - 1));

				if (ignoreWalls || trueY < input.Length - 1 && input[trueY + 1][coord.X] != '#')
					neighbors.Add((coord.X, coord.Y + 1));
			}

			return neighbors;
		}

		private bool IsPowerOfTwo(int x)
		{
			return (x & (x - 1)) == 0;
		}
	}
}
