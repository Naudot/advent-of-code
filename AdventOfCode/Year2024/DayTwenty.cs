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
			public (int x, int y) Direction;
			public (int x, int y) Position;
			public HashSet<(int x, int y)> PreviousPositions = new();
			public List<float> ValuedPositions = new();
			public int AllowedCheatTimeLeft = 0;

			public float Value = float.MaxValue;
			public bool HasReachedEnd;
		}

		protected override bool DeactivateJIT => true;

		protected override object ResolveFirstPart(string[] input)
		{
			(Racetrack[,] racetrack, Node startNode) = GetRacetrack(input);

			int baseTime = GetTime(racetrack, startNode, 0);

			// Time saved, count
			Dictionary<int, int> cheatTimes = new();

			for (int y = 1; y < input.Length - 1; y++)
			{
				for (int x = 1; x < input.Length - 1; x++)
				{
					Console.WriteLine("Processing (" + x + "," + y + ")");
					if (racetrack[x, y] != Racetrack.WALL)
						continue;

					int newTime = GetTime(racetrack, startNode, 1);

					if (newTime < baseTime)
					{
						int timeSaved = baseTime - newTime;
						if (cheatTimes.ContainsKey(timeSaved))
							cheatTimes[timeSaved]++;
						else
							cheatTimes.Add(timeSaved, 1);
					}
				}
			}

			foreach (KeyValuePair<int, int> item in cheatTimes.OrderBy(pair => pair.Key))
				Console.WriteLine("- There are " + item.Value +" cheats that save " + item.Key + " picoseconds.");

			return cheatTimes.Where(pair => pair.Key >= 100).Select(pair => pair.Value).Sum();
		}

		protected override object ResolveSecondPart(string[] input)
		{
			return 0;
		}

		private int GetTime(Racetrack[,] racetrack, Node startNode, int allowedCheatTime)
		{
			Node bestPath = new() { Value = float.MaxValue, AllowedCheatTimeLeft = allowedCheatTime };

			HashSet<(int, int)> distinctPositions = new();
			List<Node> currentPaths = new() { startNode };

			while (currentPaths.Any(path => !path.HasReachedEnd))
			{
				List<Node> newPaths = new();

				for (int i = 0; i < currentPaths.Count; i++)
				{
					Node current = currentPaths[i];
					List<(int x, int y, int dirX, int dirY, bool hasCheated)> nextPaths = new();

					for (int j = 0; j < StaticBank.Directions.Count; j++)
					{
						(int x, int y) direction = StaticBank.Directions[j];
						(int x, int y) nextPosition = (current.Position.x + direction.x, current.Position.y + direction.y);

						if (nextPosition.x < 0 || nextPosition.y < 0 || nextPosition.x >= racetrack.Length || nextPosition.y >= racetrack.Length)
							continue;

						Racetrack next = racetrack[nextPosition.x, nextPosition.y];
						bool hasCheated = false;

						if (next == Racetrack.WALL)
						{
							if (current.AllowedCheatTimeLeft == 0)
								continue;
							else
								hasCheated = true;
						}

						if (current.PreviousPositions.Contains(nextPosition))
							continue;

						nextPaths.Add((nextPosition.x, nextPosition.y, direction.x, direction.y, hasCheated));
					}

					if (nextPaths.Count == 0)
					{
						current.HasReachedEnd = true;
						continue;
					}

					for (int j = 0; j < nextPaths.Count; j++)
					{
						Node newNode = new()
						{
							Position = (nextPaths[j].x, nextPaths[j].y),
							Direction = (nextPaths[j].dirX, nextPaths[j].dirY),
							PreviousPositions = new(current.PreviousPositions)
							{
								current.Position
							},
							Value = current.Value + 1,
							ValuedPositions = new(current.ValuedPositions),
							AllowedCheatTimeLeft = current.AllowedCheatTimeLeft - (nextPaths[j].hasCheated ? -1 : 0)
						};

						newNode.ValuedPositions.Add(newNode.Value);

						if (racetrack[newNode.Position.x, newNode.Position.y] == Racetrack.END)
						{
							newNode.HasReachedEnd = true;

							if (bestPath.Value > newNode.Value)
								bestPath = newNode;

							if (newNode.Value == bestPath.Value)
							{
								foreach ((int x, int y) pos in newNode.PreviousPositions)
									distinctPositions.Add(pos);
							}
						}
						else
						{
							if (bestPath.Value < newNode.Value)
								newNode.HasReachedEnd = true;
						}

						if (!newNode.HasReachedEnd)
							newPaths.Add(newNode);
					}
				}

				currentPaths = newPaths;

				// I remove paths if similar paths are better
				List<Node> toClean = new();
				for (int i = 0; i < currentPaths.Count; i++)
				{
					Node node = currentPaths[i];
					if (currentPaths.Any(path => path.Position == node.Position && path.Value < node.Value))
						toClean.Add(node);
				}
				for (int i = 0; i < toClean.Count; i++)
					currentPaths.Remove(toClean[i]);
			}

			return distinctPositions.Count;
		}

		private (Racetrack[,], Node) GetRacetrack(string[] input)
		{
			Racetrack[,] track = new Racetrack[input.Length, input[0].Length];

			Node startNode = new()
			{
				Direction = StaticBank.GetValueOfDirection(Direction.EAST),
				Value = 0
			};

			for (int y = 0; y < input.Length; y++)
			{
				for (int x = 0; x < input[0].Length; x++)
				{
					track[x, y] = input[y][x] == '#' ? Racetrack.WALL : (input[y][x] == 'E' ? Racetrack.END : (input[y][x] == 'S' ? Racetrack.START : Racetrack.GROUND));
					if (track[x, y] == Racetrack.START)
						startNode.Position = (x, y);
				}
			}

			return (track, startNode);
		}
	}
}
