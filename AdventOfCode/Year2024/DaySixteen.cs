namespace AdventOfCode.Year2024
{
	public class DaySixteen : Day2024
	{
		public enum Labyrinth
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

			public float Value = float.MaxValue;
			public bool HasReachedEnd;
		}

		protected override bool DeactivateJIT => true;

		protected override object ResolveFirstPart(string[] input)
		{
			return GetBestPath(input, false).bestPath.Value;
		}

		protected override object ResolveSecondPart(string[] input)
		{
			return GetBestPath(input, true).distinctPositionsCount;
		}

		private (Node bestPath, int distinctPositionsCount) GetBestPath(string[] input, bool isSecondPart)
		{
			(Labyrinth[,] labyrinth, Node startNode) = GetLabyrinth(input);
			Node bestPath = new() { Value = float.MaxValue };

			// My second part consist in comparing the known best path to optimize my list of current paths
			if (isSecondPart)
				bestPath = GetBestPath(input, false).bestPath;

			HashSet<(int, int)> distinctPositions = new();
			List<Node> currentPaths = new() { startNode };
			while (currentPaths.Any(path => !path.HasReachedEnd))
			{
				List<Node> newPaths = new();

				for (int i = 0; i < currentPaths.Count; i++)
				{
					Node current = currentPaths[i];
					List<(int x, int y, int dirX, int dirY)> nextPaths = new();

					for (int j = 0; j < StaticBank.Directions.Count; j++)
					{
						(int x, int y) direction = StaticBank.Directions[j];
						(int x, int y) nextPosition = (current.Position.x + direction.x, current.Position.y + direction.y);
						Labyrinth next = labyrinth[nextPosition.x, nextPosition.y];

						if (next == Labyrinth.WALL)
							continue;
						if (current.PreviousPositions.Contains(nextPosition))
							continue;

						nextPaths.Add((nextPosition.x, nextPosition.y, direction.x, direction.y));
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
							Value = current.Value + ((nextPaths[j].dirX, nextPaths[j].dirY) == current.Direction ? 1 : 1001),
							ValuedPositions = new(current.ValuedPositions)
						};

						newNode.ValuedPositions.Add(newNode.Value);

						if (labyrinth[newNode.Position.x, newNode.Position.y] == Labyrinth.END)
						{
							newNode.HasReachedEnd = true;

							// So during the second part, we do not replace the best path, since we already fetched it
							if (!isSecondPart && bestPath.Value > newNode.Value)
								bestPath = newNode;

							if (newNode.Value == bestPath.Value)
								newNode.PreviousPositions.ToList().ForEach(pos => distinctPositions.Add(pos));
						}
						else
						{
							if (bestPath.Value < newNode.Value)
								newNode.HasReachedEnd = true;

							// Second part optimizations
							// If my current node has a weight value which is diverging too much from the best path value, I discard it
							// And the magic value 2000 perfectly works, which is WTF :)
							if (isSecondPart && 
								(bestPath.PreviousPositions.Count <= newNode.PreviousPositions.Count || 
								 newNode.Value - bestPath.ValuedPositions[newNode.PreviousPositions.Count - 1] > 2000))
							{
								newNode.HasReachedEnd = true;
							}
						}

						if (!newNode.HasReachedEnd)
							newPaths.Add(newNode);
					}
				}

				currentPaths = newPaths;
				Console.WriteLine("Processing " + currentPaths.Count);

				// First part optimizations
				// I remove paths if similar paths are better
				if (!isSecondPart)
				{
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
			}

			return (bestPath, distinctPositions.Count + 1);
		}

		private (Labyrinth[,] labyrinth, Node startNode) GetLabyrinth(string[] input)
		{
			Labyrinth[,] labyrinth = new Labyrinth[input.Length, input[0].Length];

			Node startNode = new()
			{
				Direction = StaticBank.GetValueOfDirection(Direction.EAST),
				Value = 0
			};

			for (int y = 0; y < input.Length; y++)
			{
				for (int x = 0; x < input[0].Length; x++)
				{
					labyrinth[x, y] = input[y][x] == '#' ? Labyrinth.WALL : (input[y][x] == 'E' ? Labyrinth.END : (input[y][x] == 'S' ? Labyrinth.START : Labyrinth.GROUND));
					if (labyrinth[x, y] == Labyrinth.START)
						startNode.Position = (x, y);
				}
			}

			return (labyrinth, startNode);
		}
	}
}
