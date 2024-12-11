using System.Text.RegularExpressions;

namespace AdventOfCode.Year2016
{
	public class DayOne : Day2016
	{
		protected override object ResolveFirstPart()
		{
			string[] input = File.ReadAllText(GetResourcesPath()).Trim().Split(',');

			int north = 0;
			int east = 0;
			Direction currentDirection = Direction.NORTH;

			for (int i = 0; i < input.Length; i++)
			{
				Match coord = Regex.Match(input[i], @"([A-Z])([\d]*)");
				string direction = coord.Groups[1].Value;
				int move = int.Parse(coord.Groups[2].Value);
				if (direction == "R")
				{
					if (currentDirection == Direction.WEST)
					{
						currentDirection = Direction.NORTH;
					}
					else
					{
						currentDirection += 1;
					}
				}
				else
				{
					if (currentDirection == Direction.NORTH)
					{
						currentDirection = Direction.WEST;
					}
					else
					{
						currentDirection -= 1;
					}
				}

				if (currentDirection == Direction.NORTH)
				{
					north += move;
				}
				if (currentDirection == Direction.SOUTH)
				{
					north -= move;
				}
				if (currentDirection == Direction.EAST)
				{
					east += move;
				}
				if (currentDirection == Direction.WEST)
				{
					east -= move;
				}
			}

			return north + east;
		}

		protected override object ResolveSecondPart()
		{
			string[] input = File.ReadAllText(GetResourcesPath()).Trim().Split(',');

			int north = 0;
			int east = 0;
			Direction currentDirection = Direction.NORTH;

			HashSet<Tuple<int, int>> cases = new HashSet<Tuple<int, int>>();

			for (int i = 0; i < input.Length; i++)
			{
				Match coord = Regex.Match(input[i], @"([A-Z])([\d]*)");

				string direction = coord.Groups[1].Value;
				int move = int.Parse(coord.Groups[2].Value);
				if (direction == "R")
				{
					if (currentDirection == Direction.WEST)
					{
						currentDirection = Direction.NORTH;
					}
					else
					{
						currentDirection += 1;
					}
				}
				else
				{
					if (currentDirection == Direction.NORTH)
					{
						currentDirection = Direction.WEST;
					}
					else
					{
						currentDirection -= 1;
					}
				}

				for (int j = 0; j < move; j++)
				{
					if (currentDirection == Direction.NORTH)
					{
						north += 1;
					}
					if (currentDirection == Direction.SOUTH)
					{
						north -= 1;
					}
					if (currentDirection == Direction.EAST)
					{
						east += 1;
					}
					if (currentDirection == Direction.WEST)
					{
						east -= 1;
					}

					if (cases.FirstOrDefault(t => t.Item1 == north && t.Item2 == east) != null)
					{
						return Math.Abs(north + east);
					}
					else
					{
						cases.Add(new Tuple<int, int>(north, east));
					}
				}
			}

			return 0;
		}
	}
}
