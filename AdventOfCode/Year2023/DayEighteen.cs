namespace AdventOfCode.Year2023
{
	public class DayEighteen : Day2023
	{
		public enum WallDirection
		{
			RIGHT,
			DOWN,
			LEFT,
			UP
		}

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

		public class Wall
		{
			public long X;
			public long Y;
			public long Size;
			public GroundType GroundType;
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
			int result = 0;

			int sizeY = 450;
			int sizeX = 550;
			GroundType[,] ground = new GroundType[sizeY, sizeX];

			List<(int, int)> dirs = new() { (1, 0), (0, 1), (-1, 0), (0, -1) };
			(int, int) current = (sizeY / 2, sizeX / 2);

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
					}
				}
			}

			// Write the string array to a new file named "WriteLines.txt".
			//string docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
			//using (StreamWriter outputFile = new StreamWriter(Path.Combine(docPath, "WriteLines.txt")))

			// Now raycast to know inside points
			for (int y = 0; y < sizeY; y++)
			{
				//outputFile.WriteLine();
				//Console.WriteLine();
				for (int x = 0; x < sizeX; x++)
				{
					int walls = 0;

					for (int i = 0; i < x + 1; i++)
					{
						GroundType groundType = ground[y, i];
						if (groundType == GroundType.VERTICAL || groundType == GroundType.F || groundType == GroundType.SEVEN)
						{
							walls++;
						}
					}

					if (walls != 0 && walls % 2 == 1 && ground[y, x] == GroundType.EMPTY)
					{
						ground[y, x] = GroundType.LAVA;
						result++;
					}
					//outputFile.Write(GetChar(ground[y, x]));
					//Console.Write(GetChar(ground[y, x]));
				}
			}
			//Console.WriteLine();

			return result;
		}

		protected override object ResolveSecondPart(string[] input)
		{
			long result = 0;

			SortedDictionary<long, List<Wall>> wallDic = new();
			List<(long, long)> dirs = new() { (1, 0), (0, 1), (-1, 0), (0, -1) };
			(long, long) current = (0, 0);

			for (int i = 0; i < input.Length; i++)
			{
				int nextIndex = i == input.Length - 1 ? 0 : i + 1;

				//char direction = input[i].Split(' ')[0][0];
				//char nextDir = input[nextIndex].Split(' ')[0][0];
				char direction = input[i].Split(' ')[2][7];
				char nextDir = input[nextIndex].Split(' ')[2][7];

				//WallDirection wallDir = direction == 'R' ?
				//	WallDirection.RIGHT : (direction == 'D' ? 
				//	WallDirection.DOWN : (direction == 'L' ?
				//	WallDirection.LEFT : 
				//	WallDirection.UP));
				//WallDirection wallNextDir = nextDir == 'R' ?
				//	WallDirection.RIGHT : (nextDir == 'D' ?
				//	WallDirection.DOWN : (nextDir == 'L' ?
				//	WallDirection.LEFT :
				//	WallDirection.UP));
				WallDirection wallDir = direction == '0' ?
					WallDirection.RIGHT : (direction == '1' ?
					WallDirection.DOWN : (direction == '2' ?
					WallDirection.LEFT :
					WallDirection.UP));
				WallDirection wallNextDir = nextDir == '0' ?
					WallDirection.RIGHT : (nextDir == '1' ?
					WallDirection.DOWN : (nextDir == '2' ?
					WallDirection.LEFT :
					WallDirection.UP));

				(long, long) dirValue = wallDir == WallDirection.RIGHT ? dirs[0] : (wallDir == WallDirection.DOWN ? dirs[1] : (wallDir == WallDirection.LEFT ? dirs[2] : dirs[3]));

				//long size = int.Parse(input[i].Split(' ')[1]);
				string hexa = input[i].Split(' ')[2].Replace("(#", string.Empty).Replace(")", string.Empty);
				hexa = hexa.Substring(0, hexa.Length - 1);
				long size = long.Parse(hexa, System.Globalization.NumberStyles.HexNumber);

				current = (current.Item1 + dirValue.Item1 * size, current.Item2 + dirValue.Item2 * size);

				Wall? wall = null;

				// 3 : Up, 2 : Left, 1 : Down, 0 : Right
				if ((wallDir == WallDirection.UP && wallNextDir == WallDirection.RIGHT) 
					|| (wallDir == WallDirection.LEFT && wallNextDir == WallDirection.DOWN))
				{
					wall = new Wall() { GroundType = GroundType.F, Size = size, X = current.Item1, Y = current.Item2 };
					result += size;
				}
				else if ((wallDir == WallDirection.UP && wallNextDir == WallDirection.LEFT) 
					|| (wallDir == WallDirection.RIGHT && wallNextDir == WallDirection.DOWN))
				{
					wall = new Wall() { GroundType = GroundType.SEVEN, Size = size, X = current.Item1, Y = current.Item2 };
					result += size;
				}
				else if ((wallDir == WallDirection.DOWN && wallNextDir == WallDirection.LEFT) 
					|| (wallDir == WallDirection.RIGHT && wallNextDir == WallDirection.UP))
				{
					wall = new Wall() { GroundType = GroundType.J, Size = size, X = current.Item1, Y = current.Item2 };
					result += size;
				}
				else if ((wallDir == WallDirection.DOWN && wallNextDir == WallDirection.RIGHT) 
					|| (wallDir == WallDirection.LEFT && wallNextDir == WallDirection.UP))
				{
					wall = new Wall() { GroundType = GroundType.L, Size = size, X = current.Item1, Y = current.Item2 };
					result += size;
				}

				if (wall == null)
					continue;

				if (!wallDic.ContainsKey(wall.Y))
					wallDic.Add(wall.Y, new List<Wall>() { wall });
				else
					wallDic[wall.Y].Add(wall);
			}

			long minY = wallDic.First().Key;
			long maxY = wallDic.Last().Key;

			for (long y = minY; y < maxY; y++)
			{
				if (y % 1000000 == 0)
				{
					Console.WriteLine("Le million");
				}


				//var test = wallDic
				//	.Where(pair => pair.Key < y)
				//	.Select(wall => wall.Value // List of Walls that are above the current y
				//		.Where(wall => (wall.GroundType == GroundType.F || wall.GroundType == GroundType.SEVEN) && wall.Y + wall.Size))

			}

			// Je sauvegarde chaque wall F et 7
			// Et pour le vertical dès que je trouve un F j'applique le traitement sur tous les 7 en face qui se trouve dans la size du F ?
			// Je regarde chaque wall, si celui ci ouvre la boucle alors je regarde le x du prochain wall - la size du
			// current pour connaître le nombre de tiles entre les deux, puis je passe au next +1
			//foreach (KeyValuePair<long, List<Wall>> pair in wallDic)
			//{
			//	long y = pair.Key;
			//	List<Wall> walls = pair.Value;
			//	int wallCount = 0;

			//	for (int i = 0; i < walls.Count - 1; i++)
			//	{
			//		Wall wall = walls[i];

			//		// We skip other walls than F and Seven
			//		if (wall.GroundType != GroundType.F && wall.GroundType != GroundType.SEVEN)
			//		{
			//			continue;
			//		}

			//		// If next wall is a horizontal wall, we process it
			//		Wall nextWall = walls[i + 1];
			//		if (wall.GroundType != GroundType.F || nextWall.GroundType == GroundType.SEVEN)
			//		{
			//			continue;
			//		}


			//	}
			//}

			//// Write the string array to a new file named "WriteLines.txt".
			////string docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
			////using (StreamWriter outputFile = new StreamWriter(Path.Combine(docPath, "WriteLines.txt")))

			//// Now raycast to know inside points
			//for (int y = 0; y < sizeY; y++)
			//{
			//	//outputFile.WriteLine();
			//	//Console.WriteLine();
			//	for (int x = 0; x < sizeX; x++)
			//	{
			//		int walls = 0;

			//		for (int i = 0; i < x + 1; i++)
			//		{
			//			GroundType groundType = ground[y, i];
			//			if (groundType == GroundType.VERTICAL || groundType == GroundType.F || groundType == GroundType.SEVEN)
			//			{
			//				walls++;
			//			}
			//		}

			//		if (walls != 0 && walls % 2 == 1 && ground[y, x] == GroundType.EMPTY)
			//		{
			//			ground[y, x] = GroundType.LAVA;
			//			result++;
			//		}
			//		//outputFile.Write(GetChar(ground[y, x]));
			//		//Console.Write(GetChar(ground[y, x]));
			//	}
			//}
			////Console.WriteLine();

			return result;
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
