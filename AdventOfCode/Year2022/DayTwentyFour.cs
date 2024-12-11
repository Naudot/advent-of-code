namespace AdventOfCode.Year2022
{
	public class DayTwentyFour : Day2022
	{
		public enum TileType
		{
			WALL,
			GROUND,
			BLIZZARD
		}

		public enum Direction
		{
			NORTH,
			EAST,
			SOUTH,
			WEST
		}

		public class Blizzard
		{
			public (int, int) Pos;
			public Direction Direction;
		}

		protected override bool DeactivateJIT
		{
			get
			{
				return true;
			}
		}

		private bool mDraw = false;
		private List<Blizzard> mBlizzards = new List<Blizzard>();
		private Dictionary<(int, int), List<Blizzard>> mBlizzardPositions = new Dictionary<(int, int), List<Blizzard>>();
		private (int, int) mDestination = (-1, -1);
		private int mMinX = 1;
		private int mMinY = 1;
		private int mMaxX = -1;
		private int mMaxY = -1;

		protected override object ResolveFirstPart(string[] input)
		{
			mBlizzards.Clear();
			mBlizzardPositions.Clear();
			mDestination = (-1, -1);
			mMaxX = input[0].Length - 2;
			mMaxY = input.Length - 2;

			(int, int) ourPos = (-1, -1);

			for (int i = 0; i < input.Length; i++)
			{
				for (int j = 0; j < input[i].Length; j++)
				{
					if (i == 0)
					{
						if (input[i][j] == '.')
						{
							ourPos = (i, j);
						}
					}
					else if (i == input.Length - 1)
					{
						if (input[i][j] == '.')
						{
							mDestination = (i, j);
						}
					}

					Blizzard blizzard = null;
					if (input[i][j] == '^')
					{
						blizzard = new Blizzard() { Pos = (i, j), Direction = Direction.NORTH };
					}
					if (input[i][j] == '>')
					{
						blizzard = new Blizzard() { Pos = (i, j), Direction = Direction.EAST };
					}
					if (input[i][j] == 'v')
					{
						blizzard = new Blizzard() { Pos = (i, j), Direction = Direction.SOUTH };
					}
					if (input[i][j] == '<')
					{
						blizzard = new Blizzard() { Pos = (i, j), Direction = Direction.WEST };
					}

					if (blizzard != null)
					{
						mBlizzards.Add(blizzard);
						UpdateBlizzardsDic((i, j), blizzard, false);
					}
				}
			}

			HashSet<(int, int)> reachedPositions = new HashSet<(int, int)>();
			reachedPositions.Add(ourPos);
			int currentMinute = 1;
			while (!reachedPositions.Contains(mDestination))
			{
				Draw(currentMinute, reachedPositions);
				UpdateBlizzardsPos();
				ProcessPositions(reachedPositions, mDestination);
				currentMinute++;
			}

			return currentMinute - 1;
		}

		protected override object ResolveSecondPart(string[] input)
		{
			mBlizzards.Clear();
			mBlizzardPositions.Clear();
			mDestination = (-1, -1);
			mMaxX = input[0].Length - 2;
			mMaxY = input.Length - 2;

			(int, int) ourPos = (-1, -1);

			for (int i = 0; i < input.Length; i++)
			{
				for (int j = 0; j < input[i].Length; j++)
				{
					if (i == 0)
					{
						if (input[i][j] == '.')
						{
							ourPos = (i, j);
						}
					}
					else if (i == input.Length - 1)
					{
						if (input[i][j] == '.')
						{
							mDestination = (i, j);
						}
					}

					Blizzard blizzard = null;
					if (input[i][j] == '^')
					{
						blizzard = new Blizzard() { Pos = (i, j), Direction = Direction.NORTH };
					}
					if (input[i][j] == '>')
					{
						blizzard = new Blizzard() { Pos = (i, j), Direction = Direction.EAST };
					}
					if (input[i][j] == 'v')
					{
						blizzard = new Blizzard() { Pos = (i, j), Direction = Direction.SOUTH };
					}
					if (input[i][j] == '<')
					{
						blizzard = new Blizzard() { Pos = (i, j), Direction = Direction.WEST };
					}

					if (blizzard != null)
					{
						mBlizzards.Add(blizzard);
						UpdateBlizzardsDic((i, j), blizzard, false);
					}
				}
			}

			HashSet<(int, int)> reachedPositions = new HashSet<(int, int)>();
			int currentMinute = 1;

			reachedPositions.Add(ourPos);
			while (!reachedPositions.Contains(mDestination))
			{
				Draw(currentMinute, reachedPositions);
				UpdateBlizzardsPos();
				ProcessPositions(reachedPositions, mDestination);
				currentMinute++;
			}

			reachedPositions.Clear();
			reachedPositions.Add(mDestination);
			while (!reachedPositions.Contains(ourPos))
			{
				Draw(currentMinute, reachedPositions);
				UpdateBlizzardsPos();
				ProcessPositions(reachedPositions, ourPos);
				currentMinute++;
			}

			reachedPositions.Clear();
			reachedPositions.Add(ourPos);
			while (!reachedPositions.Contains(mDestination))
			{
				Draw(currentMinute, reachedPositions);
				UpdateBlizzardsPos();
				ProcessPositions(reachedPositions, mDestination);
				currentMinute++;
			}

			return currentMinute - 1;
		}

		private void ProcessPositions(HashSet<(int, int)> reachedPosition, (int, int) exception)
		{
			List<(int, int)> posToAdd = new List<(int, int)>();
			foreach ((int, int) position in reachedPosition)
			{
				List<(int, int)> freePositions = GetFreePositions(position, exception);
				posToAdd.AddRange(freePositions);
			}

			reachedPosition.Clear();
			for (int i = 0; i < posToAdd.Count; i++)
			{
				reachedPosition.Add(posToAdd[i]);
			}
		}

		private List<(int, int)> GetFreePositions((int, int) pos, (int, int) exception)
		{
			(int, int) north = (pos.Item1 - 1, pos.Item2);
			(int, int) east = (pos.Item1, pos.Item2 + 1);
			(int, int) south = (pos.Item1 + 1, pos.Item2);
			(int, int) west = (pos.Item1, pos.Item2 - 1);

			List<(int, int)> freePositions = new List<(int, int)>();

			if ((!mBlizzardPositions.ContainsKey(south) && south.Item1 <= mMaxY && south.Item2 >= mMinX && south.Item2 <= mMaxX) || south == exception)
			{
				freePositions.Add(south);
			}
			if ((!mBlizzardPositions.ContainsKey(east) && east.Item2 <= mMaxX && east.Item1 >= mMinY && east.Item1 <= mMaxY) || east == exception)
			{
				freePositions.Add(east);
			}
			if ((!mBlizzardPositions.ContainsKey(north) && north.Item1 >= mMinY && north.Item2 >= mMinX && north.Item2 <= mMaxX) || north == exception)
			{
				freePositions.Add(north);
			}
			if ((!mBlizzardPositions.ContainsKey(west) && west.Item2 >= mMinX && west.Item1 >= mMinY && west.Item1 <= mMaxY) || west == exception)
			{
				freePositions.Add(west);
			}
			if (!mBlizzardPositions.ContainsKey(pos))
			{
				freePositions.Add(pos);
			}

			return freePositions;
		}

		private void UpdateBlizzardsPos()
		{
			for (int i = 0; i < mBlizzards.Count; i++)
			{
				Blizzard blizzard = mBlizzards[i];

				(int, int) oldPos = blizzard.Pos;
				(int, int) newPos = blizzard.Pos;

				if (blizzard.Direction == Direction.NORTH)
				{
					newPos.Item1--;
					if (newPos.Item1 < mMinY)
					{
						newPos.Item1 = mMaxY;
					}
				}
				else if (blizzard.Direction == Direction.EAST)
				{
					newPos.Item2++;
					if (newPos.Item2 > mMaxX)
					{
						newPos.Item2 = mMinX;
					}
				}
				else if (blizzard.Direction == Direction.SOUTH)
				{
					newPos.Item1++;
					if (newPos.Item1 > mMaxY)
					{
						newPos.Item1 = mMinY;
					}
				}
				else if (blizzard.Direction == Direction.WEST)
				{
					newPos.Item2--;
					if (newPos.Item2 < mMinX)
					{
						newPos.Item2 = mMaxX;
					}
				}

				blizzard.Pos = newPos;
				UpdateBlizzardsDic(oldPos, blizzard, true);
				UpdateBlizzardsDic(newPos, blizzard, false);
			}
		}

		private void UpdateBlizzardsDic((int, int) pos, Blizzard blizzard, bool remove)
		{
			if (!remove)
			{
				if (mBlizzardPositions.ContainsKey(pos))
				{
					mBlizzardPositions[pos].Add(blizzard);
				}
				else
				{
					mBlizzardPositions.Add(pos, new List<Blizzard>() { blizzard });
				}
			}
			else
			{
				if (mBlizzardPositions.ContainsKey(pos))
				{
					mBlizzardPositions[pos].Remove(blizzard);
					if (mBlizzardPositions[pos].Count == 0)
					{
						mBlizzardPositions.Remove(pos);
					}
				}
			}
		}

		private void Draw(int minute, HashSet<(int, int)> possiblePositions)
		{
			if (!mDraw)
			{
				return;
			}

			Console.WriteLine();
			Console.WriteLine();
			Console.Write("Minute " + minute);
			for (int i = mMinY - 1; i < mMaxY + 2; i++)
			{
				Console.WriteLine();
				for (int j = mMinX - 1; j < mMaxX + 2; j++)
				{
					if (possiblePositions.Contains((i, j)))
					{
						Console.ForegroundColor = ConsoleColor.Red;
						Console.Write('E');
						Console.ForegroundColor = ConsoleColor.White;
					}
					else if (mBlizzardPositions.ContainsKey((i, j)))
					{
						if (mBlizzardPositions[(i, j)].Count == 1)
						{
							Console.Write(mBlizzardPositions[(i, j)][0].Direction == Direction.NORTH ? '^' : (
								mBlizzardPositions[(i, j)][0].Direction == Direction.EAST ? '>' : (
								mBlizzardPositions[(i, j)][0].Direction == Direction.SOUTH ? 'v' : '<')));
						}
						else
						{
							Console.Write(mBlizzardPositions[(i, j)].Count.ToString());
						}
					}
					else if (i > 0 && i < mMaxY + 1 && j > 0 && j < mMaxX + 1)
					{
						Console.Write('.');
					}
					else if ((i, j) == mDestination)
					{
						Console.Write('.');
					}
					else
					{
						Console.Write('#');
					}
				}
			}
		}
	}
}
