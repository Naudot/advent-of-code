using System.ComponentModel;

namespace AdventOfCode.Year2024
{
	public enum Warehouse
	{
		ROBOT,
		BOX,
		GROUND,
		WALL
	}

	public class DayFifteen : Day2024
	{
		protected override bool DeactivateJIT
		{
			get
			{
				return true;
			}
		}

		protected override object ResolveFirstPart(string[] input)
		{
			int mapSize = input[0].Length;
			Warehouse[,] warehouse = new Warehouse[mapSize, mapSize];
			(int x, int y) robotPosition = (-1, -1);

			for (int y = 0; y < mapSize; y++)
			{
				for (int x = 0; x < mapSize; x++)
				{
					warehouse[x, y] = GetType(input[y][x]);
					if (warehouse[x, y] == Warehouse.ROBOT)
						robotPosition = (x, y);
				}
			}

			List<Direction> moves = new();
			for (int i = mapSize + 1; i < input.Length; i++)
				moves.AddRange(input[i].Select(symbol => StaticBank.GetDirectionOfArrow(symbol)));

			for (int i = 0; i < moves.Count; i++)
			{
				List<(int x, int y)> boxesToPush = new();
				(int x, int y) directionToPush = StaticBank.GetValueOfDirection(moves[i]);

				(int x, int y) nextDirection = (robotPosition.x + directionToPush.x, robotPosition.y + directionToPush.y);
				Warehouse nextObj;
				do
				{
					nextObj = warehouse[nextDirection.x, nextDirection.y];

					if (nextObj == Warehouse.GROUND)
					{
						for (int j = 0; j < boxesToPush.Count; j++)
						{
							(int x, int y) box = boxesToPush[j];
							warehouse[box.x + directionToPush.x, box.y + directionToPush.y] = Warehouse.BOX;
						}

						warehouse[robotPosition.x, robotPosition.y] = Warehouse.GROUND;
						robotPosition = (robotPosition.x + directionToPush.x, robotPosition.y + directionToPush.y);
						warehouse[robotPosition.x, robotPosition.y] = Warehouse.ROBOT;
					}
					else if (nextObj == Warehouse.BOX)
					{
						boxesToPush.Add((nextDirection.x, nextDirection.y));
					}

					nextDirection = (nextDirection.x + directionToPush.x, nextDirection.y + directionToPush.y);

				} while (nextObj != Warehouse.WALL && nextObj != Warehouse.GROUND);

				//for (int y = 0; y < mapSize; y++)
				//{
				//	for (int x = 0; x < mapSize; x++)
				//	{
				//		Console.Write(GetSymbol(warehouse[x, y]));
				//	}
				//	Console.WriteLine();
				//}
			}

			long sum = 0;
			for (int y = 0; y < mapSize; y++)
				for (int x = 0; x < mapSize; x++)
					if (warehouse[x, y] == Warehouse.BOX)
						sum += 100 * y + x;
			return sum;
		}

		protected override object ResolveSecondPart(string[] input)
		{
			return 0;
		}

		private Warehouse GetType(char symbole)
		{
			return symbole switch
			{
				'#' => Warehouse.WALL,
				'O' => Warehouse.BOX,
				'@' => Warehouse.ROBOT,
				_ => Warehouse.GROUND,
			};
		}

		private char GetSymbol(Warehouse type)
		{
			return type switch
			{
				Warehouse.WALL => '#',
				Warehouse.BOX => 'O',
				Warehouse.ROBOT => '@',
				_ => '.'
			};
		}
	}
}
