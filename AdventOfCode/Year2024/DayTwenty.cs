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
			public Dictionary<(int x, int y), int> PositionWeights = new();
			public int Value = int.MaxValue;
		}

		protected override bool DeactivateJIT => true;

		protected override object ResolveFirstPart(string[] input)
		{
			(Racetrack[,] racetrack, Node startNode) = GetRacetrack(input);

			Node bestEndNode = GetBestPath(racetrack, startNode, int.MaxValue);
			Dictionary<int, int> finishTimes = GetCheatTimes(racetrack, 2, 100, bestEndNode);

			//foreach (KeyValuePair<int, int> item in finishTimes.OrderBy(pair => pair.Key))
			//	Console.WriteLine("- There are " + item.Value + " cheats that save " + item.Key + " picoseconds.");

			return finishTimes.Select(pair => pair.Value).Sum();
		}

		protected override object ResolveSecondPart(string[] input)
		{
			(Racetrack[,] racetrack, Node startNode) = GetRacetrack(input);

			Node bestEndNode = GetBestPath(racetrack, startNode, int.MaxValue);
			Dictionary<int, int> finishTimes = GetCheatTimes(racetrack, 20, 100, bestEndNode);

			return finishTimes.Select(pair => pair.Value).Sum();
		}

		private Dictionary<int, int> GetCheatTimes(Racetrack[,] racetrack, int radius, int threshold, Node bestPathNode)
		{
			Dictionary<int, int> finishTimes = new();

			foreach ((int x, int y) bestPathPosition in bestPathNode.PreviousPositions)
			{
				int bestPathPositionValue = bestPathNode.PositionWeights[(bestPathPosition.x, bestPathPosition.y)];
				List<(int x, int y, int manhattanDistance)> radiusNodes = GetAllGroundNodesAround(racetrack, bestPathPosition, radius);

				for (int i = 0; i < radiusNodes.Count; i++)
				{
					(int x, int y, int manhattanDistance) radiusNode = radiusNodes[i];
					int radiusNodeWeight = bestPathNode.PositionWeights[(radiusNode.x, radiusNode.y)];

					int cheatGain = (radiusNodeWeight - bestPathPositionValue) - radiusNode.manhattanDistance;

					if (cheatGain >= threshold)
					{
						if (finishTimes.ContainsKey(cheatGain))
							finishTimes[cheatGain]++;
						else
							finishTimes.Add(cheatGain, 1);

						//Console.WriteLine("Cheat (" + bestPathPosition.x + ", " + bestPathPosition.y + ") to (" + radiusNode.x + ", " + radiusNode.y + ") saving " + test);
					}
				}
			}

			return finishTimes;
		}

		private Node GetBestPath(Racetrack[,] racetrack, Node startNode, int bestPathValue)
		{
			Node endNode = startNode;
			List<Node> currentPaths = new() { startNode };

			while (currentPaths.Count != 0)
			{
				List<Node> newPaths = new();

				for (int i = 0; i < currentPaths.Count; i++)
				{
					Node current = currentPaths[i];

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
							PositionWeights = new(current.PositionWeights),
							Value = current.Value + 1
						};

						newNode.PositionWeights.Add(nextPosition, newNode.Value);

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

			return endNode;
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
						startNode.PositionWeights.Add(startNode.Position, 0);
					}
				}
			}

			return (track, startNode);
		}

		private List<(int x, int y, int cheatDistance)> GetAllGroundNodesAround(Racetrack[,] track, (int x, int y) centralNode, int radius)
		{
			//Console.WriteLine("Asking for " + centralNode.x + " " + centralNode.y);
			List<(int x, int y, int cheatDistance)> aroundNodes = new();

			// E.g. : Radius = 2 -> deltaY from -2 to 2
			for (int deltaY = -radius; deltaY <= radius; deltaY++)
			{
				for (int deltaX = -radius; deltaX <=  radius; deltaX++)
				{
					if (deltaY == 0 && deltaX == 0)
						continue;

					(int x, int y) pos = (centralNode.x + deltaX, centralNode.y + deltaY);

					if (pos.x < 0
						|| pos.y < 0
						|| pos.x >= track.GetLength(0)
						|| pos.y >= track.GetLength(1))
						continue;

					Racetrack next = track[pos.x, pos.y];
					int manhattanDistance = Math.Abs(pos.x - centralNode.x) + Math.Abs(pos.y - centralNode.y);

					// If we reach a wall or if the best path value is better (lower) than our current path, we skip
					if (manhattanDistance > radius || next == Racetrack.WALL)
						continue;

					aroundNodes.Add((pos.x, pos.y, manhattanDistance));
				}
			}

			//for (int i = 0; i < aroundNodes.Count; i++)
			//	Console.WriteLine("Getting " + aroundNodes[i].Position.x + " " + aroundNodes[i].Position.y);

			return aroundNodes;
		}
	}
}
