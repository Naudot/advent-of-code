namespace AdventOfCode.Year2023
{
	// Type à déterminer une fois que la loop est complète
	public enum GroundType
	{
		EMPTY,
		START,
		HORIZONTAL,
		VERTICAL,
		F,
		SEVEN,
		J,
		L,
		LAVA
	}

	public class DayEighteen : Day2023
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
			int result = 0;

			// Y, X
			int sizeY = 450;
			int sizeX = 550;
			GroundType[,] ground = new GroundType[sizeY, sizeX];

			List<(int, int)> dirs = new() { (1, 0), (0, 1), (-1, 0), (0, -1) };
			(int, int) current = (sizeY / 2, sizeX / 2);

			// TODO : Déterminer le type
			ground[current.Item2, current.Item1] = GroundType.START;

			for (int i = 0; i < input.Length; i++)
			{
				char dir = input[i].Split(' ')[0][0];
				(int, int) dirValue = dir == 'R' ? dirs[0] : (dir == 'D' ? dirs[1] : (dir == 'L' ? dirs[2] : dirs[3]));
				int val = int.Parse(input[i].Split(' ')[1]);

				for (int j = 0; j < val; j++)
				{
					current = (current.Item1 + dirValue.Item1, current.Item2 + dirValue.Item2);

					if (j != val - 1)
					{
						if (dir == 'R' || dir == 'L')
						{
							ground[current.Item2, current.Item1] = GroundType.HORIZONTAL;
							result++;
						}
						else if (dir == 'D' || dir == 'U')
						{
							ground[current.Item2, current.Item1] = GroundType.VERTICAL;
							result++;
						}
						else
						{
							Console.Write("Wtf");
						}
					}
					else 
					{
						int nextIndex = i == input.Length - 1 ? 0 : i + 1;
						char next = input[nextIndex].Split(' ')[0][0];

						if ((dir == 'U' && next == 'R') || (dir == 'L' && next == 'D'))
						{
							ground[current.Item2, current.Item1] = GroundType.F;
							result++;
						}
						else if ((dir == 'U' && next == 'L') || (dir == 'R' && next == 'D'))
						{
							ground[current.Item2, current.Item1] = GroundType.SEVEN;
							result++;
						}
						else if ((dir == 'D' && next == 'L') || (dir == 'R' && next == 'U'))
						{
							ground[current.Item2, current.Item1] = GroundType.J;
							result++;
						}
						else if ((dir == 'D' && next == 'R') || (dir == 'L' && next == 'U'))
						{
							ground[current.Item2, current.Item1] = GroundType.L;
							result++;
						}
						else if (dir == next)
						{
							Console.Write("Wtf");
						}
					}
				}
			}

			Console.Write("Total wall here " + result);
			string docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

			// Write the string array to a new file named "WriteLines.txt".
			using (StreamWriter outputFile = new StreamWriter(Path.Combine(docPath, "WriteLines.txt")))

			// Now raycast to know inside points
			for (int y = 0; y < sizeY; y++)
			{
				outputFile.WriteLine();
				//Console.WriteLine();
				for (int x = 0; x < sizeX; x++)
				{
					int walls = 0;

					for (int i = 0; i < x + 1; i++)
					{
						GroundType groundType = ground[y, i];

						// TODO : Déterminer si c'est un mur, ou un F ou un 7
						if (groundType != GroundType.EMPTY)
						{
							if (groundType == GroundType.VERTICAL || groundType == GroundType.F || groundType == GroundType.SEVEN)
							{
								walls++;
							}
						}
					}

					if (walls != 0 && walls % 2 == 1 && ground[y, x] == GroundType.EMPTY)
					{
						ground[y, x] = GroundType.LAVA;
						result++;
					}
					outputFile.Write(GetChar(ground[y, x]));
					//Console.Write(GetChar(ground[y, x]));
				}
			}
			//Console.WriteLine();

			// 38572 too high

			return result;
		}

		protected override object ResolveSecondPart(string[] input)
		{
			return string.Empty;
		}

		private char GetChar(GroundType type)
		{
			if (type == GroundType.VERTICAL)
				return '|';
			if (type == GroundType.HORIZONTAL)
				return '-';
			if (type == GroundType.L)
				return 'L';
			if (type == GroundType.J)
				return 'J';
			if (type == GroundType.SEVEN)
				return '7';
			if (type == GroundType.F)
				return 'F';
			if (type == GroundType.EMPTY)
				return '.';
			if (type == GroundType.LAVA)
				return '#';

			return '?';
		}
	}
}
