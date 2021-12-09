using System;
using System.Collections.Generic;

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
						sizes.Add(GetBassinSize(coord, i, j));
						// Lowest point
					}
				}
			}

			sizes.Sort();

			return sizes[sizes.Count - 1] * sizes[sizes.Count - 2] * sizes[sizes.Count - 3];
		}

		public int GetBassinSize(int[,] tubes, int i, int j)
		{
			int added = 1;
			List<Tuple<int, int>> coordsProcessed = new List<Tuple<int, int>>();
			coordsProcessed.Add(new Tuple<int, int>(i, j));

			do
			{
				added = 0;
				if ((i == 0 || coord[j, i - 1] > num)
						&& (i == (height - 1) || coord[j, i + 1] > num)
						&& (j == 0 || coord[j - 1, i] > num)
						&& (j == (width - 1) || coord[j + 1, i] > num))
				{
					sizes.Add(GetBassinSize(coord, i, j));
					// Lowest point
				}

			} while (added != 0);

			return coordsProcessed.Count;
		}
	}
}
