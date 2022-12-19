using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Year2022
{
	public abstract class Rock
	{
		public List<(int, int)> Shape;

		public bool IsStopped;
		public int PosX;
		public int PosY;

		public abstract List<(int, int)> GetAllLeft();
		public abstract List<(int, int)> GetAllRight();
		public abstract List<(int, int)> GetAllBottom();
		public abstract (int, int) GetTop();

		public void Clear()
		{
			IsStopped = false;
			PosX = 0;
			PosY = 0;
		}
	}

	public class HoriLine : Rock
	{
		public HoriLine()
		{
			Shape = new List<(int, int)>()
				{
					(0, 0),
					(0, 1),
					(0, 2),
					(0, 3)
				};
		}

		public override List<(int, int)> GetAllLeft()
		{
			return new List<(int, int)>() { Shape[0] };
		}

		public override List<(int, int)> GetAllRight()
		{
			return new List<(int, int)>() { Shape[3] };
		}

		public override List<(int, int)> GetAllBottom()
		{
			return new List<(int, int)>() { Shape[0], Shape[1], Shape[2], Shape[3] };
		}

		public override (int, int) GetTop()
		{
			return Shape[0];
		}
	}

	public class Plus : Rock
	{
		public Plus()
		{
			Shape = new List<(int, int)>()
				{
					(0, 1),
					(1, 0),
					(1, 1),
					(1, 2),
					(2, 1)
				};
		}

		public override List<(int, int)> GetAllLeft()
		{
			return new List<(int, int)>() { Shape[0], Shape[1], Shape[4] };
		}

		public override List<(int, int)> GetAllRight()
		{
			return new List<(int, int)>() { Shape[0], Shape[3], Shape[4] };
		}

		public override List<(int, int)> GetAllBottom()
		{
			return new List<(int, int)>() { Shape[0], Shape[1], Shape[3] };
		}

		public override (int, int) GetTop()
		{
			return Shape[4];
		}
	}

	public class RightAngle : Rock
	{
		public RightAngle()
		{
			Shape = new List<(int, int)>()
				{
					(0, 0),
					(0, 1),
					(0, 2),
					(1, 2),
					(2, 2)
				};
		}

		public override List<(int, int)> GetAllLeft()
		{
			return new List<(int, int)>() { Shape[0], Shape[3], Shape[4] };
		}

		public override List<(int, int)> GetAllRight()
		{
			return new List<(int, int)>() { Shape[2], Shape[3], Shape[4] };
		}

		public override List<(int, int)> GetAllBottom()
		{
			return new List<(int, int)>() { Shape[0], Shape[1], Shape[2] };
		}

		public override (int, int) GetTop()
		{
			return Shape[4];
		}
	}

	public class VertiLine : Rock
	{
		public VertiLine()
		{
			Shape = new List<(int, int)>()
				{
					(0, 0),
					(1, 0),
					(2, 0),
					(3, 0)
				};
		}

		public override List<(int, int)> GetAllLeft()
		{
			return new List<(int, int)>() { Shape[0], Shape[1], Shape[2], Shape[3] };
		}

		public override List<(int, int)> GetAllRight()
		{
			return new List<(int, int)>() { Shape[0], Shape[1], Shape[2], Shape[3] };
		}

		public override List<(int, int)> GetAllBottom()
		{
			return new List<(int, int)>() { Shape[0] };
		}

		public override (int, int) GetTop()
		{
			return Shape[3];
		}
	}

	public class Square : Rock
	{
		public Square()
		{
			Shape = new List<(int, int)>()
				{
					(0, 0),
					(0, 1),
					(1, 0),
					(1, 1)
				};
		}

		public override List<(int, int)> GetAllLeft()
		{
			return new List<(int, int)>() { Shape[0], Shape[2] };
		}

		public override List<(int, int)> GetAllRight()
		{
			return new List<(int, int)>() { Shape[1], Shape[3] };
		}

		public override List<(int, int)> GetAllBottom()
		{
			return new List<(int, int)>() { Shape[0], Shape[1] };
		}

		public override (int, int) GetTop()
		{
			return Shape[2];
		}
	}

	public class Chamber
	{
		public HashSet<(int, int)> Rocks = new HashSet<(int, int)>();

		public int HighestY = -1;
	}

	/// <summary>
	/// For this day, my Y axe ascends from bottom to top.
	/// </summary>
	public class DaySeventeen : Day2022
	{
		public HoriLine HoriLine = new HoriLine();
		public Plus Plus = new Plus();
		public RightAngle RightAngle = new RightAngle();
		public VertiLine VertiLine = new VertiLine();
		public Square Square = new Square();

		protected override object ResolveFirstPart(string[] input)
		{
			return GetChamberHeight(input, false);
		}

		protected override object ResolveSecondPart(string[] input)
		{
			Chamber chamber = new Chamber();

			chamber.Rocks.Add((0, 2));
			chamber.Rocks.Add((0, 3));
			chamber.Rocks.Add((0, 4));
			chamber.Rocks.Add((0, 5));
			chamber.Rocks.Add((1, 4));
			chamber.Rocks.Add((2, 2));
			chamber.Rocks.Add((2, 3));
			chamber.Rocks.Add((2, 4));
			chamber.Rocks.Add((4, 0));
			chamber.Rocks.Add((4, 1));
			chamber.Rocks.Add((4, 2));

			chamber.Rocks.Add((5, 2));
			chamber.Rocks.Add((5, 3));
			chamber.Rocks.Add((5, 4));
			chamber.Rocks.Add((5, 5));
			chamber.Rocks.Add((6, 4));
			chamber.Rocks.Add((7, 2));
			chamber.Rocks.Add((7, 3));
			chamber.Rocks.Add((7, 4));
			chamber.Rocks.Add((9, 0));
			chamber.Rocks.Add((9, 1));
			chamber.Rocks.Add((9, 2));

			chamber.HighestY = 9;

			return CheckRepetition(chamber);
			//return GetChamberHeight(input, true);
		}

		private int GetChamberHeight(string[] input, bool isSecondPart)
		{
			Chamber chamber = new Chamber();
			int inputReadIndex = 0;

			long rocksToProcess = isSecondPart ? 1000000000000 : 2022;
			for (long i = 0; i < rocksToProcess; i++)
			{
				Rock rock = GetNextRock(i);

				rock.PosX = 2;
				rock.PosY = chamber.HighestY + 4;

				while (!rock.IsStopped)
				{
					//WriteChamber(chamber, rock);

					char nextMovement = input[0][inputReadIndex];
					inputReadIndex++;
					if (inputReadIndex >= input[0].Length)
					{
						inputReadIndex = 0;
					}

					bool canAllMove = true;

					if (nextMovement == '<')
					{
						List<(int, int)> left = rock.GetAllLeft();

						for (int j = 0; j < left.Count; j++)
						{
							(int, int) trueCoords = left[j];
							trueCoords.Item1 += rock.PosY;
							trueCoords.Item2 += rock.PosX;

							if (trueCoords.Item2 - 1 < 0 || chamber.Rocks.Contains((trueCoords.Item1, trueCoords.Item2 - 1)))
							{
								canAllMove = false;
								break;
							}
						}

						if (canAllMove)
						{
							rock.PosX--;
						}
					}
					else
					{
						List<(int, int)> right = rock.GetAllRight();

						for (int j = 0; j < right.Count; j++)
						{
							(int, int) trueCoords = right[j];
							trueCoords.Item1 += rock.PosY;
							trueCoords.Item2 += rock.PosX;

							if (trueCoords.Item2 + 1 > 6 || chamber.Rocks.Contains((trueCoords.Item1, trueCoords.Item2 + 1)))
							{
								canAllMove = false;
								break;
							}
						}

						if (canAllMove)
						{
							rock.PosX++;
						}
					}

					bool canMoveDown = true;
					List<(int, int)> bottom = rock.GetAllBottom();

					for (int j = 0; j < bottom.Count; j++)
					{
						(int, int) trueCoords = bottom[j];
						trueCoords.Item1 += rock.PosY;
						trueCoords.Item2 += rock.PosX;

						if (trueCoords.Item1 - 1 < 0 || chamber.Rocks.Contains((trueCoords.Item1 - 1, trueCoords.Item2)))
						{
							canMoveDown = false;
							break;
						}
					}

					if (canMoveDown)
					{
						rock.PosY--;
					}
					else
					{
						rock.IsStopped = true;
						for (int j = 0; j < rock.Shape.Count; j++)
						{
							chamber.Rocks.Add((rock.Shape[j].Item1 + rock.PosY, rock.Shape[j].Item2 + rock.PosX));
						}

						(int, int) top = rock.GetTop();
						if (top.Item1 + rock.PosY > chamber.HighestY)
						{
							chamber.HighestY = top.Item1 + rock.PosY;
						}

						if (isSecondPart)
						{
							int repetitionHeight = CheckRepetition(chamber);
							if (repetitionHeight > -1)
							{
								Console.WriteLine("FOUND" + repetitionHeight + " at rocks " + (i + 1));
								Console.ReadKey();
							}
						}
					}
				}

				rock.Clear();
			}

			return chamber.HighestY + 1;
		}

		private int CheckRepetition(Chamber chamber)
		{
			int towerHeight = chamber.HighestY + 1;

			// Means we have an impair number of filled rock lines
			if (towerHeight % 2 == 1)
			{
				return -1;
			}

			for (int i = 0; i < towerHeight / 2; i++)
			{
				for (int j = 0; j < 7; j++)
				{
					(int, int) coord = (i, j);
					(int, int) coordRepeat = (i + towerHeight / 2, j);

					if (chamber.Rocks.Contains(coord) && !chamber.Rocks.Contains(coordRepeat))
					{
						return -1;
					}
					if (!chamber.Rocks.Contains(coord) && chamber.Rocks.Contains(coordRepeat))
					{
						return -1;
					}
				}
			}

			return towerHeight / 2;
		}

		private Rock GetNextRock(long index)
		{
			if (index % 5 == 0)
			{
				return HoriLine;
			}
			if (index % 5 == 1)
			{
				return Plus;
			}
			if (index % 5 == 2)
			{
				return RightAngle;
			}
			if (index % 5 == 3)
			{
				return VertiLine;
			}

			return Square;
		}

		private void WriteChamber(Chamber chamber, Rock currentRock)
		{
			Console.WriteLine();

			// We draw from top to bottom
			// 8 is 4 for the highest piece + 4 for the delta
			for (int i = chamber.HighestY - 1 + 8; i >= 0; i--)
			{
				Console.Write('|');

				for (int j = 0; j < 7; j++)
				{
					(int, int) coord = (i, j);
					(int, int) rockCoord = (coord.Item1 - currentRock.PosY, coord.Item2 - currentRock.PosX);

					if (chamber.Rocks.Contains(coord))
					{
						Console.Write('#');
					}
					else if (currentRock.Shape.Contains(rockCoord))
					{
						Console.Write('@');
					}
					else
					{
						Console.Write('.');
					}
				}

				Console.Write('|');
				Console.WriteLine();
			}

			Console.WriteLine("+-------+");
			Console.WriteLine();
			Console.ReadKey();
		}
	}
}
