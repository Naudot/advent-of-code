using System.Text.RegularExpressions;

namespace AdventOfCode.Year2024
{
	public class DayFour : Day2024
	{
		protected override object ResolveFirstPart(string[] input)
		{
			int xMasCount = 0;
			int length = input[0].Length;
			int height = input.Length;

			for (int y = 0; y < input.Length; y++)
			{
				for (int x = 0; x < input[y].Length; x++)
				{
					char center = input[y][x];

					if (center != 'X')
						continue;

					// Left
					if (x >= 3 && input[y][x - 1] == 'M' && input[y][x - 2] == 'A' && input[y][x - 3] == 'S')
						xMasCount++;

					// Top Left
					if (x >= 3 && y >= 3 && input[y - 1][x - 1] == 'M' && input[y - 2][x - 2] == 'A' && input[y - 3][x - 3] == 'S')
						xMasCount++;

					// Top
					if (y >= 3 && input[y - 1][x] == 'M' && input[y - 2][x] == 'A' && input[y - 3][x] == 'S')
						xMasCount++;

					// Top Right
					if (x < length - 3 && y >= 3 && input[y - 1][x + 1] == 'M' && input[y - 2][x + 2] == 'A' && input[y - 3][x + 3] == 'S')
						xMasCount++;

					// Right
					if (x < length - 3 && input[y][x + 1] == 'M' && input[y][x + 2] == 'A' && input[y][x + 3] == 'S')
						xMasCount++;

					// Bottom Right
					if (x < length - 3 && y < height - 3 && input[y + 1][x + 1] == 'M' && input[y + 2][x + 2] == 'A' && input[y + 3][x + 3] == 'S')
						xMasCount++;

					// Bottom
					if (y < height - 3 && input[y + 1][x] == 'M' && input[y + 2][x] == 'A' && input[y + 3][x] == 'S')
						xMasCount++;

					// Bottom Left
					if (x >= 3 && y < height - 3 && input[y + 1][x - 1] == 'M' && input[y + 2][x - 2] == 'A' && input[y + 3][x - 3] == 'S')
						xMasCount++;
				}
			}

			return xMasCount;
		}

		protected override object ResolveSecondPart(string[] input)
		{
			int trueXMasCount = 0;

			for (int y = 1; y < input.Length - 1; y++)
			{
				for (int x = 1; x < input[y].Length - 1; x++)
				{
					char center = input[y][x];

					if (center != 'A')
						continue;

					int crossCount = 0;

					// Top Left to Bottom Right
					if (input[y - 1][x - 1] == 'M' && input[y + 1][x + 1] == 'S')
						crossCount++;

					// Bottom Right to Top Left
					if (input[y + 1][x + 1] == 'M' && input[y - 1][x - 1] == 'S')
						crossCount++;

					// Bottom Left to Top Right
					if (input[y + 1][x - 1] == 'M' && input[y - 1][x + 1] == 'S')
						crossCount++;

					// Top Right to Bottom Right
					if (input[y - 1][x + 1] == 'M' && input[y + 1][x - 1] == 'S')
						crossCount++;

					if (crossCount >= 2)
						trueXMasCount++;
				}
			}

			return trueXMasCount;
		}
	}
}
