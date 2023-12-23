namespace AdventOfCode.Year2023
{
	public class DayFourteen : Day2023
	{
		public enum Tilting
		{
			NORTH,
			WEST,
			SOUTH,
			EAST
		}

		public enum Type
		{
			EMPTY,
			ROCK,
			BOULDER
		}

		protected override bool DeactivateJIT
		{
			get
			{
				return true;
			}
		}

		protected override object ResolveFirstPart(string[] input)
		{
			int sizeY = input.Length;
			int sizeX = input[0].Length;
			Type[,] tab = new Type[sizeY, sizeX];

			for (int y = 0; y < sizeY; y++)
			{
				for (int x = 0; x < sizeX; x++)
				{
					tab[y, x] = input[y][x] == '.' ? Type.EMPTY : (input[y][x] == '#' ? Type.ROCK : Type.BOULDER);
				}
			}

			//Write(tab, sizeX, sizeY);

			for (int y = 0; y < sizeY; y++)
			{
				for (int x = 0; x < sizeX; x++)
				{
					if (tab[y, x] != Type.EMPTY)
						continue;

					for (int i = y + 1; i < sizeY; i++)
					{
						if (tab[i, x] == Type.ROCK)
							break;

						if (tab[i, x] == Type.BOULDER)
						{
							tab[y, x] = Type.BOULDER;
							tab[i, x] = Type.EMPTY;
							break;
						}
					}
				}
			}

			//Write(tab, sizeX, sizeY);

			int sum = 0;

			for (int y = 0; y < sizeY; y++)
				for (int x = 0; x < sizeX; x++)
					if (tab[y, x] == Type.BOULDER) sum += sizeY - y;

			return sum;
		}

		protected override object ResolveSecondPart(string[] input)
		{
			int sizeY = input.Length;
			int sizeX = input[0].Length;
			Type[,] tab = new Type[sizeY, sizeX];

			for (int y = 0; y < sizeY; y++)
			{
				for (int x = 0; x < sizeX; x++)
				{
					tab[y, x] = input[y][x] == '.' ? Type.EMPTY : (input[y][x] == '#' ? Type.ROCK : Type.BOULDER);
				}
			}

			Console.WriteLine((1000000000 - 149) % 26);
			Dictionary<int, long> sums = new Dictionary<int, long>();
			//Write(tab, sizeX, sizeY);
			// 93718 too low
			// 93751 too high
			// 93736(OK)
			Tilting currentTilting = Tilting.NORTH;
			long currentCycle = 0;
			long cycle = 149 + 19;
			for (long cycleIndex = 0; cycleIndex < cycle * 4; cycleIndex++)
			{
				//if (cycle % 50000000 == 0)
				//	Console.WriteLine("50 millions ok");
				for (int y = 0; y < sizeY; y++)
				{
					for (int x = 0; x < sizeX; x++)
					{
						if (currentTilting == Tilting.SOUTH)
						{
							int backwardY = sizeY - 1 - y;
							if (tab[backwardY, x] != Type.EMPTY)
								continue;
						}
						else if (currentTilting == Tilting.EAST)
						{
							int backwardX = sizeX - 1 - x;
							if (tab[y, backwardX] != Type.EMPTY)
								continue;
						}
						else
						{
							if (tab[y, x] != Type.EMPTY)
								continue;
						}

						if (currentTilting == Tilting.NORTH)
						{
							for (int i = y + 1; i < sizeY; i++)
							{
								if (tab[i, x] == Type.ROCK)
									break;

								if (tab[i, x] == Type.BOULDER)
								{
									tab[y, x] = Type.BOULDER;
									tab[i, x] = Type.EMPTY;
									break;
								}
							}
						}
						else if (currentTilting == Tilting.WEST)
						{
							for (int i = x + 1; i < sizeX; i++)
							{
								if (tab[y, i] == Type.ROCK)
									break;

								if (tab[y, i] == Type.BOULDER)
								{
									tab[y, x] = Type.BOULDER;
									tab[y, i] = Type.EMPTY;
									break;
								}
							}
						}
						else if (currentTilting == Tilting.SOUTH)
						{
							int backwardY = sizeY - 1 - y;
							for (int i = backwardY; i >= 0; i--)
							{
								if (tab[i, x] == Type.ROCK)
									break;

								if (tab[i, x] == Type.BOULDER)
								{
									tab[backwardY, x] = Type.BOULDER;
									tab[i, x] = Type.EMPTY;
									break;
								}
							}
						}
						else if (currentTilting == Tilting.EAST)
						{
							int backwardX = sizeX - 1 - x;
							for (int i = backwardX; i >= 0; i--)
							{
								if (tab[y, i] == Type.ROCK)
									break;

								if (tab[y, i] == Type.BOULDER)
								{
									tab[y, backwardX] = Type.BOULDER;
									tab[y, i] = Type.EMPTY;
									break;
								}
							}
						}
					}
				}

				if (currentTilting == Tilting.NORTH) currentTilting = Tilting.WEST;
				else if (currentTilting == Tilting.WEST) currentTilting = Tilting.SOUTH;
				else if (currentTilting == Tilting.SOUTH) currentTilting = Tilting.EAST;
				else if (currentTilting == Tilting.EAST) 
				{
					currentTilting = Tilting.NORTH;
					int sumTest = 0;

					for (int y = 0; y < sizeY; y++)
						for (int x = 0; x < sizeX; x++)
							if (tab[y, x] == Type.BOULDER) sumTest += sizeY - y;
					//2*7 9*26 23 2*26 3 4*26 2*19 5*26
					// de 26 termes et commence à 149
					// 14 + 234 + 23 + 52 + 3 + 104 + 38 + 130 598
					if (sums.ContainsKey(sumTest))
					{
						Console.WriteLine("Previous rep " + sumTest + " with index diff " + (currentCycle - sums[sumTest]).ToString() + " at index " + currentCycle);
						sums[sumTest] = currentCycle;
					}
					else
					{
						sums.Add(sumTest, currentCycle);
					}
					// On a un cycle de 25 à partir du cycle index 138
					currentCycle++;
					if (cycleIndex == cycle * 4 - 1)
						Console.WriteLine(sumTest);
				};

				//Write(tab, sizeX, sizeY);
			}

			foreach (var item in sums)
			{
				Console.WriteLine(item.Key + " : " + item.Value);
			}

			int sum = 0;

			for (int y = 0; y < sizeY; y++)
				for (int x = 0; x < sizeX; x++)
					if (tab[y, x] == Type.BOULDER) sum += sizeY - y;

			return sum;
		}

		private void Write(Type[,] tab, int sizeX, int sizeY)
		{
			for (int y = 0; y < sizeY; y++)
			{
				Console.WriteLine();
				for (int x = 0; x < sizeX; x++)
				{
					Console.Write(tab[y, x] == Type.EMPTY ? '.' : (tab[y, x] == Type.ROCK ? '#' : 'O'));
				}
			}
			Console.WriteLine();
		}
	}
}
