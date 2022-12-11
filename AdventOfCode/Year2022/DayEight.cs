namespace AdventOfCode.Year2022
{
	public class DayEight : Day2022
	{
		private const int SIZE = 99;

		protected override object ResolveFirstPart(string[] input)
		{
			int[,] trees = new int[SIZE, SIZE];

			// i is height
			// For each line
			for (int i = 0; i < input.Length; i++)
			{
				// j is width
				// For each char
				for (int j = 0; j < input[i].Length; j++)
				{
					trees[i, j] = input[i][j] - 48;
				}
			}

			int count = 0;

			// i is height
			for (int i = 0; i < SIZE; i++)
			{
				// j is width
				for (int j = 0; j < SIZE; j++)
				{
					// j width, i height
					bool isTreeVisible = IsTreeVisible(trees, j, i);

					//if (isTreeVisible)
					//{
					//	Console.ForegroundColor = ConsoleColor.Green;
					//}
					//else
					//{
					//	Console.ForegroundColor = ConsoleColor.White;
					//}

					count += isTreeVisible ? 1 : 0;
					//Console.Write(trees[i, j]);
				}
				//Console.WriteLine();
			}

			return count;
		}

		protected override object ResolveSecondPart(string[] input)
		{
			int[,] trees = new int[SIZE, SIZE];

			// i is height
			// For each line
			for (int i = 0; i < input.Length; i++)
			{
				// j is width
				// For each char
				for (int j = 0; j < input[i].Length; j++)
				{
					trees[i, j] = input[i][j] - 48;
				}
			}

			int bestScenicScore = 0;

			// i is height
			for (int i = 0; i < SIZE; i++)
			{
				// j is width
				for (int j = 0; j < SIZE; j++)
				{
					// j width, i height
					int scenicScore = GetScenicScore(trees, j, i);
					if (scenicScore > bestScenicScore)
					{
						bestScenicScore = scenicScore;
					}
				}
			}

			return bestScenicScore;
		}

		public bool IsTreeVisible(int[,] trees, int x, int y)
		{
			if (x == 0 || x == SIZE - 1 || y == 0 || y == SIZE - 1)
			{
				return true;
			}

			int treeHeight = trees[y, x];

			// From left
			bool isVisibleFromLeft = true;
			for (int i = 0; i < x; i++)
			{
				if (trees[y, i] >= treeHeight)
				{
					isVisibleFromLeft = false;
					break;
				}
			}

			if (isVisibleFromLeft)
			{
				return true;
			}

			// From right
			bool isVisibleFromRight = true;
			for (int i = x + 1; i < SIZE; i++)
			{
				if (trees[y, i] >= treeHeight)
				{
					isVisibleFromRight = false;
					break;
				}
			}

			if (isVisibleFromRight)
			{
				return true;
			}

			// From top
			bool isVisibleFromTop = true;
			for (int i = 0; i < y; i++)
			{
				if (trees[i, x] >= treeHeight)
				{
					isVisibleFromTop = false;
					break;
				}
			}

			if (isVisibleFromTop)
			{
				return true;
			}

			// From bottom
			bool isVisibleFromBottom = true;
			for (int i = y + 1; i < SIZE; i++)
			{
				if (trees[i, x] >= treeHeight)
				{
					isVisibleFromBottom = false;
					break;
				}
			}

			if (isVisibleFromBottom)
			{
				return true;
			}

			return false;
		}

		public int GetScenicScore(int[,] trees, int x, int y)
		{
			int treeHeight = trees[y, x];

			// To left
			int leftDistance = 0;
			for (int i = x - 1; i >= 0; i--)
			{
				leftDistance++;
				if (trees[y, i] >= treeHeight)
				{
					break;
				}
			}

			// To right
			int rightDistance = 0;
			for (int i = x + 1; i < SIZE; i++)
			{
				rightDistance++;
				if (trees[y, i] >= treeHeight)
				{
					break;
				}
			}

			// To top
			int topDistance = 0;
			for (int i = y - 1; i >= 0; i--)
			{
				topDistance++;
				if (trees[i, x] >= treeHeight)
				{
					break;
				}
			}

			// To bottom
			int bottomDistance = 0;
			for (int i = y + 1; i < SIZE; i++)
			{
				bottomDistance++;
				if (trees[i, x] >= treeHeight)
				{
					break;
				}
			}

			return leftDistance * rightDistance * topDistance * bottomDistance;
		}
	}
}
