using System.Text;

namespace AdventOfCode.Year2025
{
	public class DayFour : Day2025
	{
		protected override bool DeactivateJIT => true;

		protected override object ResolveFirstPart(string[] input)
		{
			int count = 0;

			for (int y = 0; y < input.Length; y++)
				for (int x = 0; x < input[y].Length; x++)
					if (input[y][x] == '@' && CountAdjacentRoll(input, y, x) < 4)
						count++;

			return count;
		}

		protected override object ResolveSecondPart(string[] input)
		{
			HashSet<(int y, int x)> foundRolls = new();
			int totalCount = 0;

			int count;
			do
			{
				count = 0;
				for (int y = 0; y < input.Length; y++)
					for (int x = 0; x < input[y].Length; x++)
						if (input[y][x] == '@' && CountAdjacentRoll(input, y, x) < 4)
						{
							foundRolls.Add((y, x));
							totalCount++;
							count++;
						}

				foreach ((int y, int x) in foundRolls)
				{
					StringBuilder line = new(input[y]);
					line[x] = '.';
					input[y] = line.ToString();
				}
				foundRolls.Clear();
			} while (count != 0);

			return totalCount;
		}

		private int CountAdjacentRoll(string[] input, int y, int x)
		{
			int count = 0;

			for (int i = 0; i < StaticBank.EightDirections.Count; i++)
			{
				(int x, int y) direction = StaticBank.EightDirections[i];
				if (StaticBank.IsInBoundaries((x + direction.x, y + direction.y), input)
					&& input[y + direction.y][x + direction.x] == '@')
					count++;
			}

			return count;
		}
	}
}
