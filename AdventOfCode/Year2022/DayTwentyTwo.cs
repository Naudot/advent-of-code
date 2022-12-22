using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Year2022
{
	public class DayTwentyTwo : Day2022
	{
		public enum TileType
		{
			NOTHING,
			PATH,
			WALL
		}

		public enum Direction
		{
			RIGHT,
			DOWN,
			LEFT,
			UP
		}

		protected override object ResolveFirstPart(string[] input)
		{
			Dictionary<(int, int), TileType> map = new Dictionary<(int, int), TileType>();

			int mapSizeX = 200;
			int mapSizeY = input.Length - 2;

			// Height
			for (int i = 0; i < mapSizeY; i++)
			{
				// Width
				for (int j = 0; j < mapSizeX; j++)
				{
					if (j < input[i].Length)
					{
						map.Add((i, j), input[i][j] == ' ' ? TileType.NOTHING : (input[i][j] == '.' ? TileType.PATH : TileType.WALL));
					}
					else
					{
						map.Add((i, j), TileType.NOTHING);
					}
				}
			}

			string instructions = input[mapSizeY + 1];

			Direction currentDirection = Direction.RIGHT;
			(int, int) currentPos = map.Where(coord => coord.Key.Item1 == 0 && coord.Value == TileType.PATH).First().Key;

			for (int i = 0; i < instructions.Length; i++)
			{
				string fullInstruction = string.Empty;

				if (instructions[i] == 'R')
				{
					if (currentDirection == Direction.RIGHT)
					{
						currentDirection = Direction.DOWN;
					}
					else if (currentDirection == Direction.DOWN)
					{
						currentDirection = Direction.LEFT;
					}
					else if(currentDirection == Direction.LEFT)
					{
						currentDirection = Direction.UP;
					}
					else
					{
						currentDirection = Direction.RIGHT;
					}
				}
				else if (instructions[i] == 'L')
				{
					if (currentDirection == Direction.RIGHT)
					{
						currentDirection = Direction.UP;
					}
					else if (currentDirection == Direction.UP)
					{
						currentDirection = Direction.LEFT;
					}
					else if (currentDirection == Direction.LEFT)
					{
						currentDirection = Direction.DOWN;
					}
					else
					{
						currentDirection = Direction.RIGHT;
					}
				}
				else
				{
					while (i < instructions.Length && instructions[i] != 'R' && instructions[i] != 'L')
					{
						fullInstruction += instructions[i];
						i++;
					}
					i--;

					int number = int.Parse(fullInstruction);

					for (int j = 0; j < number; j++)
					{
						(int, int) next = currentPos;

						if (currentDirection == Direction.RIGHT)
						{
							do
							{
								next.Item2++;
								if (next.Item2 > mapSizeX - 1)
								{
									next.Item2 = 0;
								}
								if (next.Item2 < 0)
								{
									next.Item2 = mapSizeX - 1;
								}
							} while (map[next] == TileType.NOTHING);
						}
						else if (currentDirection == Direction.DOWN)
						{
							do
							{
								next.Item1++;
								if (next.Item1 > mapSizeY - 1)
								{
									next.Item1 = 0;
								}
								if (next.Item1 < 0)
								{
									next.Item1 = mapSizeY - 1;
								}
							} while (map[next] == TileType.NOTHING);
						}
						else if (currentDirection == Direction.LEFT)
						{
							do
							{
								next.Item2--;
								if (next.Item2 > mapSizeX - 1)
								{
									next.Item2 = 0;
								}
								if (next.Item2 < 0)
								{
									next.Item2 = mapSizeX - 1;
								}
							} while (map[next] == TileType.NOTHING);
						}
						else
						{
							do
							{
								next.Item1--;
								if (next.Item1 > mapSizeY - 1)
								{
									next.Item1 = 0;
								}
								if (next.Item1 < 0)
								{
									next.Item1 = mapSizeY - 1;
								}
							} while (map[next] == TileType.NOTHING);
						}

						if (map[next] == TileType.PATH)
						{
							currentPos = next;
						}
					}
				}
			}

			return (currentPos.Item1 + 1) * 1000 + (currentPos.Item2 + 1) * 4 + (int)currentDirection;
		}

		protected override object ResolveSecondPart(string[] input)
		{
			return string.Empty;
		}
	}
}
