namespace AdventOfCode.Year2021
{
	public class DayNine : Day2021
	{
		protected override object ResolveFirstPart(string[] input)
		{
			int height = input.Length;
			int width = input[0].Length;

			int[,] coord = new int[width, height];

			for (int i = 0; i < height; i++)
			{
				string line = input[i];
				for (int j = 0; j < width; j++)
				{
					coord[j, i] = int.Parse(line[j].ToString());
				}
			}

			int sumRisk = 0;

			for (int i = 0; i < height; i++)
			{
				for (int j = 0; j < width; j++)
				{
					int num = coord[j , i];

					if ((i == 0 || coord[j, i - 1] > num) 
						&& (i == (height - 1) || coord[j, i + 1] > num)
						&& (j == 0 || coord[j - 1, i] > num) 
						&& (j == (width - 1) || coord[j + 1, i] > num))
					{
						sumRisk += (num + 1);
					}
				}
			}

			return sumRisk;
		}

		protected override object ResolveSecondPart(string[] input)
		{
			int height = input.Length;
			int width = input[0].Length;

			int[,] coord = new int[width, height];

			for (int i = 0; i < height; i++)
			{
				string line = input[i];
				for (int j = 0; j < width; j++)
				{
					coord[j, i] = int.Parse(line[j].ToString());
				}
			}

			List<int> sizes = new List<int>();

			for (int i = 0; i < height; i++)
			{
				for (int j = 0; j < width; j++)
				{
					int num = coord[j , i];

					if ((i == 0 || coord[j, i - 1] > num)
						&& (i == (height - 1) || coord[j, i + 1] > num)
						&& (j == 0 || coord[j - 1, i] > num)
						&& (j == (width - 1) || coord[j + 1, i] > num))
					{
						sizes.Add(GetBassinSize(height, width, coord, i, j));
					}
				}
			}

			sizes.Sort();

			return sizes[sizes.Count - 1] * sizes[sizes.Count - 2] * sizes[sizes.Count - 3];
		}

		public int GetBassinSize(int maxHeight, int maxWidth, int[,] tubes, int beginHeight, int beginWidth)
		{
			List<Tuple<int, int>> coordsProcessed = new List<Tuple<int, int>> { new Tuple<int, int>(beginWidth, beginHeight) };
			List<Tuple<int, int>> coordsToProcess = new List<Tuple<int, int>> { new Tuple<int, int>(beginWidth, beginHeight) };

			while (coordsToProcess.Count != 0)
			{
				// We remove the tuple from the list to process
				Tuple<int, int> toProcess = coordsToProcess[0];
				coordsToProcess.Remove(toProcess);

				int height = toProcess.Item2;
				int width = toProcess.Item1;

				// Check top
				if (height != 0 && tubes[width, height - 1] < 9)
				{
					Tuple<int, int> coordTop = new Tuple<int, int>(width, height - 1);
					if (!HasTuple(coordsProcessed, coordTop))
					{
						coordsToProcess.Add(coordTop);
						coordsProcessed.Add(coordTop);
					}
				}
				// Check bottom
				if (height != (maxHeight - 1) && tubes[width, height + 1] < 9)
				{
					Tuple<int, int> coordBottom = new Tuple<int, int>(width, height + 1);
					if (!HasTuple(coordsProcessed, coordBottom))
					{
						coordsToProcess.Add(coordBottom);
						coordsProcessed.Add(coordBottom);
					}
				}
				// Check left
				if (width != 0 && tubes[toProcess.Item1 - 1, height] < 9)
				{
					Tuple<int, int> coordLeft = new Tuple<int, int>(width - 1, height);
					if (!HasTuple(coordsProcessed, coordLeft))
					{
						coordsToProcess.Add(coordLeft);
						coordsProcessed.Add(coordLeft);
					}
				}
				// Check right
				if (width != (maxWidth - 1) && tubes[width + 1, height] < 9)
				{
					Tuple<int, int> coordRight = new Tuple<int, int>(width + 1, height);
					if (!HasTuple(coordsProcessed, coordRight))
					{
						coordsToProcess.Add(coordRight);
						coordsProcessed.Add(coordRight);
					}
				}
			}

			return coordsProcessed.Count;
		}

		public bool HasTuple(List<Tuple<int, int>> coords, Tuple<int, int> coord)
		{
			return coords.Contains(coord);
		}
	}
}
