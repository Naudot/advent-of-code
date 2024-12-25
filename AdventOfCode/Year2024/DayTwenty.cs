namespace AdventOfCode.Year2024
{
	public class DayTwenty : Day2024
	{
		public enum Racetrack
		{
			GROUND,
			WALL,
			START,
			END
		}

		public class Node
		{
			public (int x, int y) Position;
			public HashSet<(int x, int y)> PreviousPositions = new();
			public int Value = int.MaxValue;
		}

		protected override bool DeactivateJIT => true;

		protected override object ResolveFirstPart(string[] input)
		{
			(Racetrack[,] racetrack, Node startNode) = GetRacetrack(input);

			(int bestTime, Node bestEndNode) = GetBestPath(racetrack, startNode, int.MaxValue);
			Dictionary<int, int> finishTimes = GetCheatTimes(racetrack, startNode, 2, bestTime, bestEndNode);

			foreach (KeyValuePair<int, int> item in finishTimes.OrderBy(pair => (bestTime - pair.Key)))
				Console.WriteLine("- There are " + item.Value +" cheats that save " + (bestTime - item.Key) + " picoseconds.");

			return finishTimes.Where(pair => (bestTime - pair.Key) >= 100).Select(pair => pair.Value).Sum();
		}

		protected override object ResolveSecondPart(string[] input)
		{
			(Racetrack[,] racetrack, Node startNode) = GetRacetrack(input);

			(int bestTime, Node bestEndNode) = GetBestPath(racetrack, startNode, int.MaxValue);
			Dictionary<int, int> finishTimes = GetCheatTimes(racetrack, startNode, 20, bestTime, bestEndNode);

			foreach (KeyValuePair<int, int> item in finishTimes.OrderBy(pair => (bestTime - pair.Key)))
				Console.WriteLine("- There are " + item.Value + " cheats that save " + (bestTime - item.Key) + " picoseconds.");

			return finishTimes.Where(pair => (bestTime - pair.Key) >= 100).Select(pair => pair.Value).Sum();
		}

		private (int,Node) GetBestPath(Racetrack[,] racetrack, Node startNode, int bestPathValue)
		{
			Node endNode = startNode;
			List<Node> currentPaths = new()	{ startNode };

			while (currentPaths.Count != 0)
			{
				List<Node> newPaths = new();

				for (int i = 0; i < currentPaths.Count; i++)
				{
					Node current = currentPaths[i];

					//Console.WriteLine("Processing " + current.Position.x + " " + current.Position.y);

					for (int j = 0; j < StaticBank.Directions.Count; j++)
					{
						(int x, int y) direction = StaticBank.Directions[j];
						(int x, int y) nextPosition = (current.Position.x + direction.x, current.Position.y + direction.y);

						if (nextPosition.x < 0
							|| nextPosition.y < 0
							|| nextPosition.x >= racetrack.GetLength(0)
							|| nextPosition.y >= racetrack.GetLength(1)
							|| current.PreviousPositions.Contains(nextPosition))
							continue;

						Racetrack next = racetrack[nextPosition.x, nextPosition.y];

						// If we reach a wall or if the best path value is better (lower) than our current path, we skip
						if (next == Racetrack.WALL || bestPathValue < current.Value + 1)
							continue;

						Node newNode = new()
						{
							Position = (nextPosition.x, nextPosition.y),
							PreviousPositions = new(current.PreviousPositions) { current.Position },
							Value = current.Value + 1
						};

						// If we reach the END node or the best path value is better (lower) than our current path
						if (racetrack[newNode.Position.x, newNode.Position.y] == Racetrack.END)
						{
							if (bestPathValue > newNode.Value)
							{
								bestPathValue = newNode.Value;
								endNode = newNode;
							}
						}
						else
						{
							newPaths.Add(newNode);
						}
					}
				}

				currentPaths = newPaths;
			}

			return (bestPathValue, endNode);
		}

		private Dictionary<int, int> GetCheatTimes(Racetrack[,] racetrack, Node startNode, int radius, int bestPathValue, Node bestNoCheatEndNode)
		{
			Dictionary<int, int> finishTimes = new();

			foreach ((int x, int y) positionToTest in bestNoCheatEndNode.PreviousPositions)
			{
				bool hasBeenTest = false;
				List<Node> currentPaths = new() { startNode };

				while (currentPaths.Count != 0)
				{
					List<Node> newPaths = new();

					for (int i = 0; i < currentPaths.Count; i++)
					{
						Node current = currentPaths[i];

						//Console.WriteLine("Processing " + current.Position.x + " " + current.Position.y);

						Racetrack currentTrack = racetrack[current.Position.x, current.Position.y];

						// If we reach a wall or if the best path value is better (lower) than our current path, we skip
						if (currentTrack == Racetrack.WALL || bestPathValue < current.Value)
							continue;

						// If we reach the END node or the best path value is better (lower) than our current path
						if (currentTrack == Racetrack.END)
						{
							if (bestPathValue > current.Value)
							{
								int finishTime = current.Value;
								if (finishTimes.ContainsKey(finishTime))
									finishTimes[current.Value]++;
								else
									finishTimes.Add(finishTime, 1);
							}
						}
						else
						{
							if (current.Position == positionToTest && !hasBeenTest)
							{
								hasBeenTest = true;
								newPaths.AddRange(GetAllGroundNodesAround(racetrack, current, radius, bestPathValue));
							}
						}

						// Get next proper directionnal Nodes
						for (int j = 0; j < StaticBank.Directions.Count; j++)
						{
							(int x, int y) direction = StaticBank.Directions[j];
							(int x, int y) nextPosition = (current.Position.x + direction.x, current.Position.y + direction.y);

							if (nextPosition.x < 0
								|| nextPosition.y < 0
								|| nextPosition.x >= racetrack.GetLength(0)
								|| nextPosition.y >= racetrack.GetLength(1)
								|| current.PreviousPositions.Contains(nextPosition))
								continue;

							Node newNode = new()
							{
								Position = (nextPosition.x, nextPosition.y),
								PreviousPositions = new(current.PreviousPositions) { current.Position },
								Value = current.Value + 1
							};

							newPaths.Add(newNode);
						}
					}

					currentPaths = newPaths;
				}
			}

			return finishTimes;
		}

		private (Racetrack[,], Node) GetRacetrack(string[] input)
		{
			Racetrack[,] track = new Racetrack[input.Length, input[0].Length];

			Node startNode = new()
			{
				Value = 0
			};

			for (int y = 0; y < input.Length; y++)
			{
				for (int x = 0; x < input[0].Length; x++)
				{
					track[x, y] = input[y][x] == '#' ? Racetrack.WALL : (input[y][x] == 'E' ? Racetrack.END : (input[y][x] == 'S' ? Racetrack.START : Racetrack.GROUND));
					if (track[x, y] == Racetrack.START)
					{
						startNode.Position = (x, y);
						startNode.PreviousPositions.Add((x, y));
					}
				}
			}

			return (track, startNode);
		}

		private List<Node> GetAllGroundNodesAround(Racetrack[,] track, Node centralNode, int radius, int bestPathValue)
		{
			Console.WriteLine("Asking for " + centralNode.Position.x + " " + centralNode.Position.y);
			List<Node> aroundNodes = new();

			//(int x, int y) topLeft = (centralNode.Position.x - radius, centralNode.Position.y - radius);
			//(int x, int y) bottomRight = (centralNode.Position.x + radius, centralNode.Position.y + radius);

			// E.g. : Radius = 2 -> deltaY from -2 to 2
			for (int deltaY = -radius; deltaY <= radius; deltaY++)
			{
				for (int deltaX = -radius; deltaX <=  radius; deltaX++)
				{
					if (deltaY == 0 && deltaX == 0)
						continue;

					(int x, int y) pos = (centralNode.Position.x + deltaX, centralNode.Position.y + deltaY);

					if (Math.Pow(pos.x - centralNode.Position.x, 2) 
						+ Math.Pow(pos.y - centralNode.Position.y, 2) 
						<= Math.Pow(radius, 2))
					{
						if (pos.x < 0
							|| pos.y < 0
							|| pos.x >= track.GetLength(0)
							|| pos.y >= track.GetLength(1)
							|| centralNode.PreviousPositions.Contains(pos))
							continue;

						Racetrack next = track[pos.x, pos.y];
						int manhattanDistance = Math.Abs(pos.x - centralNode.Position.x) + Math.Abs(pos.y - centralNode.Position.y);

						// If we reach a wall or if the best path value is better (lower) than our current path, we skip
						if (next == Racetrack.WALL || bestPathValue < centralNode.Value + manhattanDistance)
							continue;

						Node newNode = new()
						{
							Position = pos,
							PreviousPositions = new(centralNode.PreviousPositions), // TODO : Peut être un truc bizarre ici
							Value = centralNode.Value + manhattanDistance
						};

						aroundNodes.Add(newNode);
					}
				}
			}

			for (int i = 0; i < aroundNodes.Count; i++)
				Console.WriteLine("Getting " + aroundNodes[i].Position.x + " " + aroundNodes[i].Position.y);

			return aroundNodes;
		}
	}
}
