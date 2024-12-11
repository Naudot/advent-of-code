namespace AdventOfCode.Year2021
{
	public class DayTwelve : Day2021
	{
		protected override object ResolveFirstPart(string[] input)
		{
			return ProcessCaves(input);
		}

		protected override object ResolveSecondPart(string[] input)
		{
			return ProcessCaves(input, true);
		}

		public int ProcessCaves(string[] input, bool processSmallCaveTwice = false)
		{
			Dictionary<string, Cave> caves = new Dictionary<string, Cave>();

			for (int i = 0; i < input.Length; i++)
			{
				string[] relation = input[i].Split('-');
				string firstCaveName = relation[0];
				string secondCaveName = relation[1];
				Cave firstCaveInfo = new Cave(firstCaveName);
				Cave secondCaveInfo = new Cave(secondCaveName);
				if (caves.ContainsKey(firstCaveName))
				{
					firstCaveInfo = caves[firstCaveName];
				}
				else
				{
					caves.Add(firstCaveName, firstCaveInfo);
				}
				if (caves.ContainsKey(secondCaveName))
				{
					secondCaveInfo = caves[secondCaveName];
				}
				else
				{
					caves.Add(secondCaveName, secondCaveInfo);
				}

				secondCaveInfo.AddConnectedCave(firstCaveInfo);
				firstCaveInfo.AddConnectedCave(secondCaveInfo);
			}

			int pathCount = 0;
			Cave startCave = caves["start"];

			foreach (KeyValuePair<string, Cave> cave in startCave.ConnectedCaves)
			{
				List<Cave> previousExploredCaves = new List<Cave>();
				previousExploredCaves.Add(cave.Value);
				pathCount += ProcessPath(cave.Value, previousExploredCaves, processSmallCaveTwice);
			}

			return pathCount;
		}

		public int ProcessPath(Cave currentCave, List<Cave> previousExploredCaves, bool processSmallCaveTwice)
		{
			int pathCount = 0;

			foreach (KeyValuePair<string, Cave> connectedCaveInfo in currentCave.ConnectedCaves)
			{
				Cave connectedCave = connectedCaveInfo.Value;
				// If we encounter the start, we continue
				if (connectedCave.IsStart)
				{
					continue;
				}
				// If we encounter the end, we return one path found
				else if (connectedCave.IsEnd)
				{
					previousExploredCaves.Add(connectedCave);
					//Console.WriteLine("Path correct found " + Cave.Log(previousExploredCaves));
					pathCount += 1;
				}
				else if (connectedCave.IsSmall)
				{
					// If we encounter the same small cave twice, the path is invalid, except if we can explore one previous small encountered cave
					if (previousExploredCaves.Contains(connectedCave))
					{
						if (!processSmallCaveTwice)
						{
							previousExploredCaves.Add(connectedCave);
							//Console.WriteLine("Path not correct found " + Cave.Log(previousExploredCaves));
							continue;
						}
						else
						{
							// We explored a same small cave twice so we can set the previousExploredCaves to false
							List<Cave> caves = new List<Cave>(previousExploredCaves);
							caves.Add(connectedCave);
							pathCount += ProcessPath(connectedCave, caves, false);
						}
					}
					else
					{
						// We keep going
						List<Cave> caves = new List<Cave>(previousExploredCaves);
						caves.Add(connectedCave);
						pathCount += ProcessPath(connectedCave, caves, processSmallCaveTwice);
					}
				}
				else
				{
					// We keep going
					List<Cave> caves = new List<Cave>(previousExploredCaves);
					caves.Add(connectedCave);
					pathCount += ProcessPath(connectedCave, caves, processSmallCaveTwice);
				}
			}

			return pathCount;
		}

		public class Cave
		{
			public string Name;
			public bool IsSmall;
			public bool IsStart;
			public bool IsEnd;
			public Dictionary<string, Cave> ConnectedCaves = new Dictionary<string, Cave>();

			public Cave(string name)
			{
				Name = name;
				IsEnd = name == "end";
				IsStart = name == "start";
				IsSmall = name[0] >= 97 && name[0] <= 122;
			}

			public void AddConnectedCave(Cave cave)
			{
				if (!ConnectedCaves.ContainsKey(cave.Name))
				{
					ConnectedCaves.Add(cave.Name, cave);
				}
			}

			public static string Log(List<Cave> caves)
			{
				string log = "start";
				for (int i = 0; i < caves.Count; i++)
				{
					log += ", " + caves[i].Name;
				}
				return log;
			}
		}
	}
}
