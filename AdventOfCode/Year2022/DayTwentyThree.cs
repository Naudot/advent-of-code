namespace AdventOfCode.Year2022
{
	public class DayTwentyThree : Day2022
	{
		public enum Direction
		{
			NORTH,
			SOUTH,
			WEST,
			EAST
		}

		public class Elf
		{
			public (int, int) Pos;
			public (int, int) WantedPos;
			public bool WillMove = false;
		}

		protected override object ResolveFirstPart(string[] input)
		{
			return GetResult(input, false);
		}

		protected override object ResolveSecondPart(string[] input)
		{
			return GetResult(input, true);
		}

		private int GetResult(string[] input, bool isSecondPart)
		{

			Direction first = Direction.NORTH;
			Direction second = Direction.SOUTH;
			Direction third = Direction.WEST;
			Direction fourth = Direction.EAST;

			List<Elf> elves = new List<Elf>();
			Dictionary<(int, int), int> wantedPosCount = new Dictionary<(int, int), int>();
			HashSet<(int, int)> elfPositions = new HashSet<(int, int)>();

			for (int i = 0; i < input.Length; i++)
			{
				for (int j = 0; j < input[i].Length; j++)
				{
					if (input[i][j] == '#')
					{
						Elf elf = new Elf() { Pos = (i, j) };
						elves.Add(elf);
						elfPositions.Add((i, j));
					}
				}
			}

			int roundCount = isSecondPart ? int.MaxValue : 10;
			for (int i = 0; i < roundCount; i++)
			{
				for (int j = 0; j < elves.Count; j++)
				{
					Elf elf = elves[j];

					elf.WillMove = false;
					if (!HasElvesAround(elfPositions, elf))
					{
						continue;
					}

					elf.WillMove = true;
					(int, int) newPosition = elf.Pos;
					if (!HasElves(elfPositions, elf, first))
					{
						newPosition = GetNextPos(newPosition, first);
					}
					else if (!HasElves(elfPositions, elf, second))
					{
						newPosition = GetNextPos(newPosition, second);
					}
					else if (!HasElves(elfPositions, elf, third))
					{
						newPosition = GetNextPos(newPosition, third);
					}
					else if (!HasElves(elfPositions, elf, fourth))
					{
						newPosition = GetNextPos(newPosition, fourth);
					}

					if (!wantedPosCount.ContainsKey(newPosition))
					{
						wantedPosCount.Add(newPosition, 1);
					}
					else
					{
						wantedPosCount[newPosition]++;
					}

					elf.WantedPos = newPosition;
				}

				if (isSecondPart && !wantedPosCount.Any())
				{
					return (i + 1);
				}

				for (int j = 0; j < elves.Count; j++)
				{
					Elf elf = elves[j];

					if (!elf.WillMove)
					{
						continue;
					}

					if (wantedPosCount[elf.WantedPos] > 1)
					{
						continue;
					}

					elfPositions.Remove(elf.Pos);
					elf.Pos = elf.WantedPos;
					elfPositions.Add(elf.Pos);
				}

				wantedPosCount.Clear();
				Direction oldFirst = first;
				first = second;
				second = third;
				third = fourth;
				fourth = oldFirst;
			}

			int minY = elfPositions.Select(pos => pos.Item1).Min();
			int maxY = elfPositions.Select(pos => pos.Item1).Max();

			int minX = elfPositions.Select(pos => pos.Item2).Min();
			int maxX = elfPositions.Select(pos => pos.Item2).Max();

			int count = 0;
			for (int i = minY; i < maxY + 1; i++)
			{
				for (int j = minX; j < maxX + 1; j++)
				{
					if (!elfPositions.Contains((i, j)))
					{
						count++;
					}
				}
			}

			return count;
		}

		private bool HasElves(HashSet<(int, int)> elfPositions, Elf elf, Direction direction)
		{
			(int, int) currentPos = elf.Pos;

			if (direction == Direction.NORTH)
			{
				(int, int) northWest = currentPos;
				northWest.Item1--;
				northWest.Item2--;

				(int, int) north = currentPos;
				north.Item1--;

				(int, int) northEast = currentPos;
				northEast.Item1--;
				northEast.Item2++;

				return elfPositions.Contains(northWest) || elfPositions.Contains(north) || elfPositions.Contains(northEast);
			}

			if (direction == Direction.SOUTH)
			{
				(int, int) southWest = currentPos;
				southWest.Item1++;
				southWest.Item2--;

				(int, int) south = currentPos;
				south.Item1++;

				(int, int) southEast = currentPos;
				southEast.Item1++;
				southEast.Item2++;

				return elfPositions.Contains(southWest) || elfPositions.Contains(south) || elfPositions.Contains(southEast);
			}

			if (direction == Direction.EAST)
			{
				(int, int) northEast = currentPos;
				northEast.Item1--;
				northEast.Item2++;

				(int, int) east = currentPos;
				east.Item2++;

				(int, int) southEast = currentPos;
				southEast.Item1++;
				southEast.Item2++;

				return elfPositions.Contains(northEast) || elfPositions.Contains(east) || elfPositions.Contains(southEast);
			}

			if (direction == Direction.WEST)
			{
				(int, int) northWest = currentPos;
				northWest.Item1--;
				northWest.Item2--;

				(int, int) west = currentPos;
				west.Item2--;

				(int, int) southWest = currentPos;
				southWest.Item1++;
				southWest.Item2--;

				return elfPositions.Contains(northWest) || elfPositions.Contains(west) || elfPositions.Contains(southWest);
			}

			return false;
		}

		private bool HasElvesAround(HashSet<(int, int)> elfPositions, Elf elf)
		{
			(int, int) currentPos = elf.Pos;

			(int, int) northWest = currentPos;
			northWest.Item1--;
			northWest.Item2--;

			(int, int) north = currentPos;
			north.Item1--;

			(int, int) northEast = currentPos;
			northEast.Item1--;
			northEast.Item2++;

			(int, int) east = currentPos;
			east.Item2++;

			(int, int) southEast = currentPos;
			southEast.Item1++;
			southEast.Item2++;
			
			(int, int) south = currentPos;
			south.Item1++;

			(int, int) southWest = currentPos;
			southWest.Item1++;
			southWest.Item2--;

			(int, int) west = currentPos;
			west.Item2--;

			return elfPositions.Contains(northWest) 
				|| elfPositions.Contains(north)
				|| elfPositions.Contains(northEast)
				|| elfPositions.Contains(east)
				|| elfPositions.Contains(southEast)
				|| elfPositions.Contains(south)
				|| elfPositions.Contains(southWest)
				|| elfPositions.Contains(west);
		}

		private (int, int) GetNextPos((int, int) position, Direction direction)
		{
			(int, int) newPos = position;

			switch (direction)
			{
				case Direction.NORTH:
					newPos.Item1--;
					break;
				case Direction.SOUTH:
					newPos.Item1++;
					break;
				case Direction.WEST:
					newPos.Item2--;
					break;
				case Direction.EAST:
					newPos.Item2++;
					break;
				default:
					break;
			}

			return newPos;
		}
	}
}
