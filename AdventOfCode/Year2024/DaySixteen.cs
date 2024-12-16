﻿namespace AdventOfCode.Year2024
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
			public List<(int x, int y)> PreviousPositions = new();

			public long Value = long.MaxValue;
			public bool HasReachedEnd;
		}

		protected override object ResolveFirstPart(string[] input)
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

			List<Node> currentPaths = new() { startNode };
			long bestPathValue = int.MaxValue;

			while (currentPaths.Any(path => !path.HasReachedEnd))
			{
				List<Node> newPaths = new();

				for (int i = 0; i < currentPaths.Count; i++)
				{
					if (currentPaths[i].HasReachedEnd)
						continue;

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
							Value = current.Value + ((nextPaths[j].dirX, nextPaths[j].dirY) == current.Direction ? 1 : 1001)
						};

						if (labyrinth[newNode.Position.x, newNode.Position.y] == Labyrinth.END)
						{
							newNode.HasReachedEnd = true;
							if (bestPathValue > newNode.Value)
								bestPathValue = newNode.Value;
						}
						else
						{
							if (bestPathValue <newNode.Value)
								newNode.HasReachedEnd = true;
						}
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

			return bestPathValue;
		}

		protected override object ResolveSecondPart(string[] input)
		{
			return 0;
		}
	}
}
