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

			int mapSizeX = 150;
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
							} while (map[next] == TileType.NOTHING);
						}
						else if (currentDirection == Direction.LEFT)
						{
							do
							{
								next.Item2--;
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
			Dictionary<(int, int), TileType> map = new Dictionary<(int, int), TileType>();

			int mapSizeX = 150;
			int mapSizeY = input.Length - 2;

			// Height
			for (int i = 0; i < mapSizeY; i++)
			{
				// Width
				for (int j = 0; j < mapSizeX; j++)
				{
					if (j < input[i].Length)
					{
						map.Add((i, j), input[i][j] == '.' ? TileType.PATH : TileType.WALL);
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
					else if (currentDirection == Direction.LEFT)
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
						Direction newDirWhenCubeFaceChanged = currentDirection;

						if (currentDirection == Direction.RIGHT)
						{
							next.Item2++;
							// If we go after the red square
							if (next.Item1 < 50 && next.Item2 > 149)
							{
								next.Item1 = 149 - next.Item1; // Up is down and vice versa
								next.Item2 = 99;
								newDirWhenCubeFaceChanged = Direction.LEFT;
							}
							// If we go after the blue square
							else if (next.Item1 >= 50 && next.Item1 < 100 && next.Item2 > 99)
							{
								next.Item2 = 50 + next.Item1;
								next.Item1 = 49;
								newDirWhenCubeFaceChanged = Direction.UP;
							}
							// If we go after the green square
							else if (next.Item1 >= 100 && next.Item1 < 150 && next.Item2 > 99)
							{
								next.Item1 = 149 - next.Item1; // Up is down and vice versa
								next.Item2 = 149;
								newDirWhenCubeFaceChanged = Direction.LEFT;
							}
							// If we go after the grey square
							else if (next.Item1 >= 150 && next.Item1 < 200 && next.Item2 > 49)
							{
								next.Item2 = next.Item1 - 100;
								next.Item1 = 149;
								newDirWhenCubeFaceChanged = Direction.UP;
							}
						}
						else if (currentDirection == Direction.DOWN)
						{
							next.Item1++;
							// If we go down the grey square
							if (next.Item2 < 50 && next.Item1 > 199)
							{
								next.Item1 = 0;
								next.Item2 = next.Item2 + 100;
								newDirWhenCubeFaceChanged = Direction.DOWN;
							}
							// If we go past the green square
							else if (next.Item2 >= 50 && next.Item2 < 100 && next.Item1 > 149)
							{
								next.Item1 = next.Item2 + 100;
								next.Item2 = 49;
								newDirWhenCubeFaceChanged = Direction.LEFT;
							}
							// If we go past the red square
							else if (next.Item2 >= 100 && next.Item2 < 150 && next.Item1 > 49)
							{
								next.Item1 = next.Item2 - 50;
								next.Item2 = 99;
								newDirWhenCubeFaceChanged = Direction.LEFT;
							}
						}
						else if (currentDirection == Direction.LEFT)
						{
							next.Item2--;
							// If we go before the black square
							if (next.Item1 < 50 && next.Item2 < 50)
							{
								next.Item1 = 149 - next.Item1; // Up is down and vice versa
								next.Item2 = 0;
								newDirWhenCubeFaceChanged = Direction.RIGHT;
							}
							// If we go before the blue square
							else if (next.Item1 >= 50 && next.Item1 < 100 && next.Item2 < 50)
							{
								next.Item2 = next.Item1 - 50;
								next.Item1 = 100;
								newDirWhenCubeFaceChanged = Direction.DOWN;
							}
							// If we go before the crimson square
							else if (next.Item1 >= 100 && next.Item1 < 150 && next.Item2 < 0)
							{
								next.Item1 = 149 - next.Item1; // Up is down and vice versa
								next.Item2 = 50;
								newDirWhenCubeFaceChanged = Direction.RIGHT;
							}
							// If we go before the grey square
							else if (next.Item1 >= 150 && next.Item1 < 200 && next.Item2 < 0)
							{
								next.Item2 = next.Item1 - 100;
								next.Item1 = 0;
								newDirWhenCubeFaceChanged = Direction.DOWN;
							}
						}
						else
						{
							next.Item1--;
							// If we go up the crimson square
							if (next.Item2 < 50 && next.Item1 < 100)
							{
								next.Item1 = next.Item2 + 50;
								next.Item2 = 50;
								newDirWhenCubeFaceChanged = Direction.RIGHT;
							}
							// If we go past the black square
							else if (next.Item2 >= 50 && next.Item2 < 100 && next.Item1 < 0)
							{
								next.Item1 = next.Item2 + 100;
								next.Item2 = 0;
								newDirWhenCubeFaceChanged = Direction.RIGHT;
							}
							// If we go up the red square
							else if (next.Item2 >= 100 && next.Item2 < 150 && next.Item1 < 0)
							{
								next.Item1 = 199;
								next.Item2 = next.Item2 - 100;
								newDirWhenCubeFaceChanged = Direction.UP;
							}
						}

						if (map[next] == TileType.PATH)
						{
							currentPos = next;
							currentDirection = newDirWhenCubeFaceChanged;
						}
					}
				}
			}

			return (currentPos.Item1 + 1) * 1000 + (currentPos.Item2 + 1) * 4 + (int)currentDirection;
		}
	}
}
