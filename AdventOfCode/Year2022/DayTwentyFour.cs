using System;
using System.Collections.Generic;
using System.Linq;

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

		public class Path
		{
			public (int, int) CurrentPos;
			public int CurrentMinute;
			public bool IsStuck = false;

			public int BestPathTime = int.MaxValue;
			public List<Path> Paths = new List<Path>();
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

			Path main = new Path();
			main.CurrentPos = ourPos;
			Draw(0, main.CurrentPos);

			int currentMinute = 1;
			while (main.BestPathTime == int.MaxValue)
			{
				UpdateBlizzardsPos();
				ProcessPath(currentMinute, main);
				currentMinute++;
			}

			return main.BestPathTime;
		}

		private void ProcessPath(int currentMinute, Path current)
		{
			current.CurrentMinute = currentMinute;

			// We separated into multiple paths
			if (current.Paths.Count > 0)
			{
				bool areAllStuck = true;
				for (int i = 0; i < current.Paths.Count; i++)
				{
					Path child = current.Paths[i];
					if (!child.IsStuck)
					{
						ProcessPath(currentMinute, child);
						areAllStuck = false;
					}
				}

				if (areAllStuck)
				{
					current.IsStuck = true;
				}
			}
			// Else we process our path until the destination
			else
			{
				bool isStuck;
				List<(int, int)> freePositions = GetFreePositions(current.CurrentPos, out isStuck);

				if (isStuck)
				{
					if (mDraw)
					{
						Console.Write("\nCan not move and stuck in blizzard, removing a path at minute " + currentMinute);
					}

					current.IsStuck = true;
					return;
				}

				if (freePositions.Count == 1)
				{
					current.CurrentPos = freePositions[0];
					if (current.CurrentPos == mDestination)
					{
						current.BestPathTime = currentMinute;
						Console.Write("\nFound destination in " + currentMinute);
						Console.ReadKey();
					}
					Draw(currentMinute, current.CurrentPos);
				}
				else if (freePositions.Count > 1)
				{
					current.Paths = new List<Path>();
					for (int i = 0; i < freePositions.Count; i++)
					{
						Path child = new Path() { CurrentPos = freePositions[i], CurrentMinute = currentMinute };
						if (child.CurrentPos == mDestination)
						{
							child.BestPathTime = currentMinute;
							Console.Write("\nFound destination in " + currentMinute + " minutes");
							Console.ReadKey();
						}
						Draw(currentMinute, child.CurrentPos);

						current.Paths.Add(child);
					}
				}
				else // Wait
				{
					Draw(currentMinute, current.CurrentPos);
				}
			}
		}

		private List<(int, int)> GetFreePositions((int, int) pos, out bool isStuck)
		{
			(int, int) north = (pos.Item1 - 1, pos.Item2);
			(int, int) east = (pos.Item1, pos.Item2 + 1);
			(int, int) south = (pos.Item1 + 1, pos.Item2);
			(int, int) west = (pos.Item1, pos.Item2 - 1);

			List<(int, int)> freePositions = new List<(int, int)>();

			if ((!mBlizzardPositions.ContainsKey(south) && south.Item1 <= mMaxY && south.Item2 >= mMinX && south.Item2 <= mMaxX) || south == mDestination)
			{
				freePositions.Add(south);
			}
			if ((!mBlizzardPositions.ContainsKey(east) && east.Item2 <= mMaxX && east.Item1 >= mMinY && east.Item1 <= mMaxY) || east == mDestination)
			{
				freePositions.Add(east);
			}
			if ((!mBlizzardPositions.ContainsKey(north) && north.Item1 >= mMinY && north.Item2 >= mMinX && north.Item2 <= mMaxX) || north == mDestination)
			{
				freePositions.Add(north);
			}
			if ((!mBlizzardPositions.ContainsKey(west) && west.Item2 >= mMinX && west.Item1 >= mMinY && west.Item1 <= mMaxY) || west == mDestination)
			{
				freePositions.Add(west);
			}

			if (freePositions.Count == 0)
			{
				if (mBlizzardPositions.ContainsKey(pos))
				{
					isStuck = true;
					return null;
				}
			}

			isStuck = false;
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

		private void Draw(int minute, (int, int) currentPos)
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
					if ((i, j) == currentPos)
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

		protected override object ResolveSecondPart(string[] input)
		{
			return string.Empty;
		}
	}
}
