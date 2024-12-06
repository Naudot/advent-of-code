using System.Numerics;

namespace AdventOfCode.Year2024
{
	public class DaySix : Day2024
	{
		protected override object ResolveFirstPart(string[] input)
		{
			int mapSize = input.Length;
			char[,] map = new char[mapSize, mapSize];
			Vector2 player = new(-1, -1);
			Direction direction = Direction.NORTH;

			for (int y = 0; y < input.Length; y++)
			{
				for (int x = 0; x < input[y].Length; x++)
				{
					char current = input[y][x];
					map[y, x] = current;
					if (current == '^')
						player = new(x, y);
				}
			}

			HashSet<Vector2> paths = new() { player };

			while (true)
			{
				Vector2 delta = GetDelta(direction);
				Vector2 nextPos = player + delta;

				if (nextPos.X < 0 || nextPos.X >= mapSize || nextPos.Y < 0 || nextPos.Y >= mapSize)
					break;

				if (map[(int)nextPos.Y, (int)nextPos.X] == '#')
				{
					direction = GetNextDirection(direction);
				}
				else
				{
					player = nextPos;
					paths.Add(player);
				}
			}

			return paths.Count();
		}

		protected override object ResolveSecondPart(string[] input)
		{
			return 0;
		}

		private Vector2 GetDelta(Direction direction)
		{
			return direction switch
			{
				Direction.NORTH => new(0, -1),
				Direction.EAST => new(1, 0),
				Direction.SOUTH => new(0, 1),
				Direction.WEST => new(-1, 0),
				_ => new(0, 0),
			};
		}

		private Direction GetNextDirection(Direction direction)
		{
			return direction switch
			{
				Direction.NORTH => Direction.EAST,
				Direction.EAST => Direction.SOUTH,
				Direction.SOUTH => Direction.WEST,
				Direction.WEST => Direction.NORTH,
				_ => Direction.NORTH,
			};
		}
	}
}
