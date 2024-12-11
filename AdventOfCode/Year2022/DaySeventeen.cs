namespace AdventOfCode.Year2022
{
	/// <summary>
	/// For this day, my Y axe ascends from bottom to top.
	/// </summary>
	public class DaySeventeen : Day2022
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

		public HoriLine LineShape = new HoriLine();
		public Plus PlusShape = new Plus();
		public RightAngle RightAngleShape = new RightAngle();
		public VertiLine VertiLineShape = new VertiLine();
		public Square SquareShape = new Square();

		protected override object ResolveFirstPart(string[] input)
		{
			return GetChamberHeight(input, false);
		}

		protected override object ResolveSecondPart(string[] input)
		{
			//Chamber chamber = new Chamber();

			//chamber.Rocks.Add((0, 2));
			//chamber.Rocks.Add((0, 3));
			//chamber.Rocks.Add((0, 4));
			//chamber.Rocks.Add((0, 5));
			//chamber.Rocks.Add((1, 4));
			//chamber.Rocks.Add((2, 2));
			//chamber.Rocks.Add((2, 3));
			//chamber.Rocks.Add((2, 4));
			//chamber.Rocks.Add((4, 0));
			//chamber.Rocks.Add((4, 1));
			//chamber.Rocks.Add((4, 2));

			//chamber.Rocks.Add((5, 2));
			//chamber.Rocks.Add((5, 3));
			//chamber.Rocks.Add((5, 4));
			//chamber.Rocks.Add((5, 5));
			//chamber.Rocks.Add((6, 4));
			//chamber.Rocks.Add((7, 2));
			//chamber.Rocks.Add((7, 3));
			//chamber.Rocks.Add((7, 4));
			//chamber.Rocks.Add((9, 0));
			//chamber.Rocks.Add((9, 1));
			//chamber.Rocks.Add((9, 2));

			//chamber.HighestY = 9;

			//return CheckRepetition(chamber);
			return GetChamberHeight(input, true);
		}

		private static long BlocksProcessed = 0;

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

						for (int x = 0; x < left.Count; x++)
						{
							(int, int) trueCoords = left[x];
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

						for (int x = 0; x < right.Count; x++)
						{
							(int, int) trueCoords = right[x];
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

					for (int y = 0; y < bottom.Count; y++)
					{
						(int, int) trueCoords = bottom[y];
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
						int lowestY = int.MaxValue;
						rock.IsStopped = true;
						for (int j = 0; j < rock.Shape.Count; j++)
						{
							if (lowestY > rock.Shape[j].Item1 + rock.PosY)
							{
								lowestY = rock.Shape[j].Item1 + rock.PosY;
							}

							chamber.Rocks.Add((rock.Shape[j].Item1 + rock.PosY, rock.Shape[j].Item2 + rock.PosX));
						}

						(int, int) top = rock.GetTop();
						if (top.Item1 + rock.PosY > chamber.HighestY)
						{
							chamber.HighestY = top.Item1 + rock.PosY;
						}

						//WriteChamber(chamber, rock);

						if (isSecondPart)
						{
							if (!IsRepeatFound)
							{
								BlocksProcessed = i;
								CheckRepetition(chamber);
							}

							if (IsRepeatFound)
							{
								if (((chamber.HighestY + 1)- LineWhereItBegins) % SizeOfRepeatBlock == 0)
								{
									Console.WriteLine("Repeat found. Rocks processed " + (i + 1));

									if (FirstRocksCount == -1)
									{
										FirstRocksCount = (i + 1);
									}
									else if (SecondRocksCount == -1)
									{
										SecondRocksCount = (i + 1);
									}
									else
									{
										//Console.WriteLine("Rocks between blocks " + (SecondRocksCount - FirstRocksCount));
										//Console.ReadKey();
									}
								}

								if (i + 1 == 10615)
								{
									Console.WriteLine("chamber.HighestY " + chamber.HighestY);
								}
								else if (i + 1 == (10615 + 1005))
								{
									Console.WriteLine("chamber.HighestY 1005 blocks later" + chamber.HighestY);
								}

								// FOUND53 at rocks 85 from line 25
								//53 lignes qui se répètent à partir du rocher 

								//tous les 

								//53 de hauteurs qui se répètent pour 85 rochers tombés
								//et cela à partir de la 25 ème ligne

								//FOUND2724 at rocks 3656 from line 264

								//(1 000 000 000 000 (-15 à cause du modulo) / 35) * 53 + 25
								//35 étant le nombre de rocks dans un repeat
								//-15 étant 1 000 000 000 000 mod 35
								//53 étant la taille du repeat
								//25 étant les 25 lignes du début

								//(1 000 000 000 000 - 175) - (1 000 000 000 000 - 175 Mod 1740) / 1740 * 2724 + 264

								//999 999 999 825 - (999 999 999 825 Mod 1740) / 1740 * 2724 + 264

								//999 999 999 825 - (1 005) / 1740 * 2724 + 264

								//574 712 643 * 2724 + 264 = 1 565 517 239 796

								//Quand je trouve mon repeat j'ai process 3655 blocks, sachant qu'il y en a 1740, je dois soustraire aux 1 000 000 000 000 (3655 - 3480) -> 175

								//-> 1 565 517 239 796 FAUX
								//1 565 517 239 796 mais il me reste 1005 rocks à process ??
								//Si je fais la diff avec ces 1005 blocks j'obtiens 18193 - 16607 = 1586
								//1 565 517 239 796 + 1586 = 1 565 517 241 382
							}
						}
					}
				}

				rock.Clear();
			}

			return chamber.HighestY + 1;
		}

		// Repeat informations
		public static bool IsRepeatFound = false;
		public static int LineWhereItBegins = -1;
		public static int SizeOfRepeatBlock = -1;
		public static long FirstRocksCount = -1;
		public static long SecondRocksCount = -1;

		private void CheckRepetition(Chamber chamber)
		{
			for (int k = 0; k < chamber.HighestY; k++)
			{
				int towerHeight = chamber.HighestY + 1 - k;

				bool isNotFound = false;
				for (int i = 0; i < towerHeight / 2; i++)
				{
					for (int j = 0; j < 7; j++)
					{
						(int, int) coord = (i + k, j);
						(int, int) coordRepeat = (i + k + towerHeight / 2, j);

						if ((chamber.Rocks.Contains(coord) && !chamber.Rocks.Contains(coordRepeat))
							|| (!chamber.Rocks.Contains(coord) && chamber.Rocks.Contains(coordRepeat)))
						{
							isNotFound = true;
							break;
						}
					}

					if (isNotFound)
					{
						break;
					}
				}

				if (isNotFound)
				{
					continue;
				}

				if (towerHeight > 10)
				{
					IsRepeatFound = true;
					LineWhereItBegins = k;
					SizeOfRepeatBlock = towerHeight / 2;
					Console.WriteLine("Found at line " + LineWhereItBegins + " with block size " + SizeOfRepeatBlock + " with blocks already processed " + BlocksProcessed);
				}
			}
		}

		private Rock GetNextRock(long index)
		{
			if (index % 5 == 0)
			{
				return LineShape;
			}
			if (index % 5 == 1)
			{
				return PlusShape;
			}
			if (index % 5 == 2)
			{
				return RightAngleShape;
			}
			if (index % 5 == 3)
			{
				return VertiLineShape;
			}

			return SquareShape;
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
