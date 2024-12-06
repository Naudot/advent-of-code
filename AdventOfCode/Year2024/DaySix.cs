using System.Numerics;

namespace AdventOfCode.Year2024
{
	public class DaySix : Day2024
	{
		protected override object ResolveFirstPart(string[] input)
		{
			(char[,] map, Vector2 guard) = GetMap(input);
			Vector2 direction = new(0, -1);
			HashSet<Vector2> paths = new() { guard };

			while (true)
			{
				Vector2 nextPos = guard + direction;
				if (nextPos.X < 0 || nextPos.X >= input[0].Length || nextPos.Y < 0 || nextPos.Y >= input.Length)
					break;

				if (map[(int)nextPos.Y, (int)nextPos.X] == '#')
				{
					direction = GetNextDirection(direction);
				}
				else
				{
					guard = nextPos;
					paths.Add(guard);
				}
			}

			return paths.Count;
		}

		protected override object ResolveSecondPart(string[] input)
		{
			(char[,] map, Vector2 originalGuardPosition) = GetMap(input);
			int mapSize = input.Length;

			int loopCount = 0;

			// Bruteforce
			for (int y = 0; y < mapSize; y++)
			{
				for (int x = 0; x < mapSize; x++)
				{
					if (originalGuardPosition.X == x && originalGuardPosition.Y == y)
						continue;

					bool loopHasBegun = false;
					Vector2 loopStartPosition = Vector2.Zero;

					Vector2 delta = new(0, -1);
					Vector2 guard = originalGuardPosition;
					HashSet<Vector2> paths = new() { guard };

					while (true)
					{
						Vector2 nextPos = guard + delta;
						if (nextPos.X < 0 || nextPos.X >= mapSize || nextPos.Y < 0 || nextPos.Y >= mapSize)
							break;

						bool isFakeObstacle = nextPos.X == x && nextPos.Y == y;
						if (isFakeObstacle || map[(int)nextPos.Y, (int)nextPos.X] == '#')
						{
							delta = GetNextDirection(delta);
						}
						else
						{
							guard = nextPos;

							if (loopHasBegun)
							{
								if (!paths.Contains(guard))
								{
									loopHasBegun = false;
								}
								else if (guard == loopStartPosition)
								{
									loopCount++;
									break;
								}
							}
							else if (paths.Contains(guard))
							{
								loopHasBegun = true;
								loopStartPosition = guard;
							}
							
							paths.Add(guard);
						}
					}
				}
			}

			return loopCount;
		}

		private (char[,], Vector2 guard) GetMap(string[] input)
		{
			int mapSize = input.Length;
			char[,] map = new char[input.Length, input[0].Length];
			Vector2 guard = new(-1, -1);

			for (int y = 0; y < input.Length; y++)
				for (int x = 0; x < input[y].Length; x++)
				{
					char current = input[y][x];
					map[y, x] = current;
					if (current == '^')
						guard = new(x, y);
				}

			return (map, guard);
		}

		private Vector2 GetNextDirection(Vector2 direction)
		{
			if (direction == new Vector2(0, -1)) return new(1, 0);
			if (direction == new Vector2(1, 0)) return new(0, 1);
			if (direction == new Vector2(0, 1)) return new(-1, 0);
			if (direction == new Vector2(-1, 0)) return new(0, -1);
			return new(0, 0);
		}
	}
}
