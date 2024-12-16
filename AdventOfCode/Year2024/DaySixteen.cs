namespace AdventOfCode.Year2024
{
	public class DaySixteen : Day2024
	{
		protected override bool DeactivateJIT => true;

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

		protected override object ResolveFirstPart(string[] input)
		{
			return GetBestPath(input);
		}

		protected override object ResolveSecondPart(string[] input, object firstPartResult)
		{
			(Labyrinth[,] labyrinth, Node startNode) = GetLabyrinth(input);
			Node bestPath = (Node)firstPartResult;

			List<Node> finishedPaths = new();
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
							finishedPaths.Add(newNode);
						}
						else
						{
							
							if (bestPath.Value < newNode.Value
								// Optimization
								|| bestPath.PreviousPositions.Count <= newNode.PreviousPositions.Count
								|| newNode.Value - bestPath.ValuedPositions[newNode.PreviousPositions.Count - 1] > 2000) // Wtf ça marche parfaitement :)
								newNode.HasReachedEnd = true;
						}

						if (!newNode.HasReachedEnd)
							newPaths.Add(newNode);
					}
				}

				currentPaths = newPaths;
				Console.WriteLine("Processing " + currentPaths.Count);
			}

			HashSet<(int x, int y)> distinctPositions = new();
			for (int i = 0; i < finishedPaths.Count; i++)
				foreach ((int, int) pos in finishedPaths[i].PreviousPositions)
					distinctPositions.Add(pos);
			return distinctPositions.Count + 1;
		}

		private Node GetBestPath(string[] input)
		{
			(Labyrinth[,] labyrinth, Node startNode) = GetLabyrinth(input);
			Node bestPath = new() { Value = float.MaxValue };

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
							if (bestPath.Value > newNode.Value)
								bestPath = newNode;
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

				List<Node> toClean = new();
				for (int i = 0; i < currentPaths.Count; i++)
				{
					Node node = currentPaths[i];
					List<Node> similarPaths = currentPaths.Where(path => path.Position == node.Position).ToList();

					for (int j = 0; j < similarPaths.Count; j++)
					{
						if (similarPaths[j].Value < node.Value)
						{
							toClean.Add(node);
							break;
						}
					}
				}

				for (int i = 0; i < toClean.Count; i++)
					currentPaths.Remove(toClean[i]);
			}

			return bestPath;
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
