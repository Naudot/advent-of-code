namespace AdventOfCode.Year2024
{
	public enum Warehouse
	{
		GROUND,
		ROBOT,
		BOX,
		WALL,
		BOX_LEFT,
		BOX_RIGHT
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

						// Move the robot
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
			int mapHeight = input[0].Length;
			int mapWidth = mapHeight * 2;
			Warehouse[,] warehouse = new Warehouse[mapWidth, mapHeight];
			(int x, int y) robotPosition = (-1, -1);

			for (int y = 0; y < mapHeight; y++)
			{
				for (int x = 0; x < mapWidth; x += 2)
				{
					Warehouse type = GetType(input[y][x / 2]);

					if (type == Warehouse.BOX)
						type = Warehouse.BOX_LEFT;

					warehouse[x, y] = type;

					if (type != Warehouse.ROBOT)
					{
						if (type == Warehouse.BOX_LEFT)
							type = Warehouse.BOX_RIGHT;

						warehouse[x + 1, y] = type;
					}
					else
					{
						robotPosition = (x, y);
					}
				}
			}

			List<Direction> moves = new();
			for (int i = mapHeight + 1; i < input.Length; i++)
				moves.AddRange(input[i].Select(symbol => StaticBank.GetDirectionOfArrow(symbol)));

			for (int i = 0; i < moves.Count; i++)
			{
				// Work with left
				HashSet<(int x, int y)> boxesLeftToPush = new();
				(int x, int y) directionToPush = StaticBank.GetValueOfDirection(moves[i]);

				(int x, int y) nextDirection = (robotPosition.x + directionToPush.x, robotPosition.y + directionToPush.y);
				HashSet<(int x, int y)> nextObjs = new() { robotPosition };

				//WriteMap(warehouse, mapWidth, mapHeight);

				//Console.WriteLine("Will move to " + StaticBank.GetSymbolOfDirection(directionToPush));

				do
				{
					nextObjs = GetNextObjs(nextObjs, directionToPush, warehouse);

					if (IsMadeOnlyOfGround(nextObjs, warehouse))
					{
						List<(int x, int y)> boxes = boxesLeftToPush.ToList();

						// On déplace les plus loins en premières pour pouvoir clean derrière elles facilement
						if (directionToPush == (0, -1))
							boxes = boxes.OrderBy(pos => pos.y).ToList();
						if (directionToPush == (0, 1))
							boxes = boxes.OrderByDescending(pos => pos.y).ToList();

						foreach ((int x, int y) boxLeft in boxes)
						{
							// TODO Cleaner les anciens char de boxes
							warehouse[boxLeft.x + directionToPush.x, boxLeft.y + directionToPush.y] = Warehouse.BOX_LEFT;
							warehouse[(boxLeft.x + 1) + directionToPush.x, boxLeft.y + directionToPush.y] = Warehouse.BOX_RIGHT;

							if (directionToPush == (0, -1) || directionToPush == (0, 1))
							{
								warehouse[boxLeft.x, boxLeft.y] = Warehouse.GROUND;
								warehouse[(boxLeft.x + 1), boxLeft.y] = Warehouse.GROUND;
							}
						}

						// Move the robot
						warehouse[robotPosition.x, robotPosition.y] = Warehouse.GROUND;
						robotPosition = (robotPosition.x + directionToPush.x, robotPosition.y + directionToPush.y);
						warehouse[robotPosition.x, robotPosition.y] = Warehouse.ROBOT;

						break;
					}
					else if (IsMadeOnlyOfBoxesOrGround(nextObjs, warehouse))
					{
						nextObjs
							.Where(obj => warehouse[obj.x, obj.y] == Warehouse.BOX_LEFT).ToList()
							.ForEach(boxLeft => boxesLeftToPush.Add((boxLeft.x, boxLeft.y)));
					}

					nextDirection = (nextDirection.x + directionToPush.x, nextDirection.y + directionToPush.y);

				} while (!ContainsWall(nextObjs, warehouse));
			}

			WriteMap(warehouse, mapWidth, mapHeight);

			long sum = 0;
			for (int y = 0; y < mapHeight; y++)
				for (int x = 0; x < mapWidth; x++)
					if (warehouse[x, y] == Warehouse.BOX_LEFT)
						sum += 100 * y + x;
			return sum;
		}

		private bool IsMadeOnlyOfGround(HashSet<(int x, int y)> positions, Warehouse[,] warehouse)
		{
			return positions.Count(pos => warehouse[pos.x, pos.y] == Warehouse.GROUND) == positions.Count();
		}

		private bool IsMadeOnlyOfBoxesOrGround(HashSet<(int x, int y)> positions, Warehouse[,] warehouse)
		{
			return positions.Count(pos => warehouse[pos.x, pos.y] == Warehouse.BOX_LEFT || warehouse[pos.x, pos.y] == Warehouse.BOX_RIGHT || warehouse[pos.x, pos.y] == Warehouse.GROUND) == positions.Count();
		}

		private bool ContainsWall(HashSet<(int x, int y)> positions, Warehouse[,] warehouse)
		{
			return positions.Count(pos => warehouse[pos.x, pos.y] == Warehouse.WALL) > 0;
		}

		private HashSet<(int x, int y)> GetNextObjs(HashSet<(int x, int y)> currentObjs, (int x, int y) direction, Warehouse[,] map)
		{
			HashSet<(int x, int y)> nextObjs = new();

			foreach ((int x, int y) currentObj in currentObjs)
			{
				Warehouse current = map[currentObj.x, currentObj.y];

				if (current == Warehouse.GROUND || current == Warehouse.WALL)
					continue;

				(int x, int y) next = (currentObj.x + direction.x, currentObj.y + direction.y);
				Warehouse nextObj = map[next.x, next.y];

				if (current == Warehouse.BOX_LEFT && nextObj == Warehouse.BOX_RIGHT && direction == (1, 0))
					continue;
				if (current == Warehouse.BOX_RIGHT && nextObj == Warehouse.BOX_LEFT && direction == (-1, 0))
					continue;

				nextObjs.Add(next);

				if (nextObj == Warehouse.BOX_LEFT)
					nextObjs.Add((next.x + 1, next.y));
				else if (nextObj == Warehouse.BOX_RIGHT)
					nextObjs.Add((next.x - 1, next.y));
			}

			return nextObjs;
		}

		private void WriteMap(Warehouse[,] warehouse, int mapSizeX, int mapSizeY)
		{
			for (int y = 0; y < mapSizeY; y++)
			{
				for (int x = 0; x < mapSizeX; x++)
				{
					Console.Write(GetSymbol(warehouse[x, y]));
				}
				Console.WriteLine();
			}
		}

		private Warehouse GetType(char symbole)
		{
			return symbole switch
			{
				'#' => Warehouse.WALL,
				'O' => Warehouse.BOX,
				'@' => Warehouse.ROBOT,
				'[' => Warehouse.BOX_LEFT,
				']' => Warehouse.BOX_RIGHT,
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
				Warehouse.BOX_LEFT => '[',
				Warehouse.BOX_RIGHT => ']',
				_ => '.'
			};
		}
	}
}
