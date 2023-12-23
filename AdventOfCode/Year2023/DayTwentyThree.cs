namespace AdventOfCode.Year2023
{
	public class DayTwentyThree : Day2023
	{
		public enum ForestType
		{
			EMPTY, // .
			FOREST, // #
			RIGHT_SLOPE, // >,
			DOWN_SLOPE, // v
			LEFT_SLOPE, // <
			UP_SLOPE // ^
		}

		public class Path
		{
			public bool HasReachEnd;
			public bool IsRejected;
			public int Weight = 0;
			public HashSet<(int X, int Y)> Nodes = new();
			public (int X, int Y, int DirX, int DirY, int Weight) CurrentNode;
		}

		protected override bool DeactivateJIT => true;

		protected override object ResolveFirstPart(string[] input)
		{
			return GetLongestPath(input, true);
		}

		protected override object ResolveSecondPart(string[] input)
		{
			// 4610 too low
			return GetLongestPath(input, false);
		}

		// Le problème c'est de croiser un node qui est traversé par un chemin de grosse valeur
		// Si ce chemin plus tard est rejeté je suis mort
		private int GetLongestPath(string[] input, bool isSlippery)
		{
			// Right, Down, Left, Up
			List<(int, int)> directions = new() { (1, 0), (0, 1), (-1, 0), (0, -1) };

			// Ground parsing
			int size = input.Length;
			ForestType[,] forest = new ForestType[size, size];
			(int X, int Y) start = (-1, -1);
			(int X, int Y) end = (-1, -1);

			for (int y = 0; y < input.Length; y++)
				for (int x = 0; x < input[0].Length; x++)
				{
					forest[y, x] =
						input[y][x] == '#' ? ForestType.FOREST :
						input[y][x] == '.' ? ForestType.EMPTY :
						input[y][x] == '>' ? ForestType.RIGHT_SLOPE :
						input[y][x] == 'v' ? ForestType.DOWN_SLOPE :
						input[y][x] == '<' ? ForestType.LEFT_SLOPE :
											 ForestType.UP_SLOPE;

					if (y == 0 && input[y][x] == '.') start = (x, y);
					if (y == input.Length - 1 && input[y][x] == '.') end = (x, y);
				}

			Path first = new();
			first.Nodes.Add((start.X, start.Y));
			first.CurrentNode = (start.X, start.Y, 0, 1, 0);

			List<Path> paths = new();
			paths.Add(first);

			List<Path> endedPaths = new();

			// Use this to check for path crossings
			Dictionary<(int, int), int> weightedNodes = new();

			// As long as our paths has not reached the end
			while (paths.Where(path => !path.HasReachEnd).Any())
			{
				List<Path> pathsToEnd = new();

				// We process the next node in each path
				for (int i = 0; i < paths.Count; i++)
				{
					Path currentPath = paths[i];
					if (currentPath.HasReachEnd) continue;

					(int X, int Y, int DirX, int DirY, int Weight) node = paths[i].CurrentNode;
					List<(int X, int Y, int DirX, int DirY, int Weight)> newNodes = new();

					// Take next nodes
					for (int j = 0; j < directions.Count; j++)
					{
						(int X, int Y) dir = directions[j];

						// Can not go back
						if (-dir.X == node.DirX && -dir.Y == node.DirY) continue;

						// We skip OOB positions
						if (node.X + dir.X < 0
							|| node.Y + dir.Y < 0
							|| node.X + dir.X > input[0].Length - 1
							|| node.Y + dir.Y > input.Length - 1)
							continue;

						// We skip forests
						(int X, int Y) next = (node.X + dir.X, node.Y + dir.Y);

						ForestType type = forest[next.Y, next.X];
						if (type == ForestType.FOREST) continue;

						// We skip slopes
						if (isSlippery && dir == (1, 0) && type == ForestType.LEFT_SLOPE) continue;
						if (isSlippery && dir == (0, 1) && type == ForestType.UP_SLOPE) continue;
						if (isSlippery && dir == (-1, 0) && type == ForestType.RIGHT_SLOPE) continue;
						if (isSlippery && dir == (0, -1) && type == ForestType.DOWN_SLOPE) continue;

						if (!isSlippery)
						{
							// We skip tile that would make the path loop
							if (currentPath.Nodes.Contains(next)) continue;

							// If we encounter an ended path using this next node with a heavier weight, we do not process this node further
							// Useless because every other ended path have less weight than the current path
							if (weightedNodes.ContainsKey(next) && weightedNodes[next] > node.Weight + 1)
								continue;


							//// If we encounter a path using this next node with a heavier weight, we do not process this node further
							//if (!nodesCost.ContainsKey(next))
							//	nodesCost.Add(next, node.Weight + 1);
							//else
							//	if (nodesCost[next] > node.Weight + 1)
							//	continue;
							//else
							//	nodesCost[next] = node.Weight + 1; // Peut pas faire ça car pas sûr que le path se termine sur une loop
						}

						newNodes.Add((next.X, next.Y, dir.X, dir.Y, node.Weight + 1));
					}

					if (newNodes.Count == 0 && !currentPath.HasReachEnd)
					{
						currentPath.IsRejected = true; // Either by looping or meeting nodes which have heavier weights in other paths
						currentPath.HasReachEnd = true;
					}

					for (int j = 1; j < newNodes.Count; j++)
					{
						Path newPath = new();
						paths.Add(newPath);

						newPath.Weight = currentPath.Weight + 1;
						newPath.Nodes.UnionWith(currentPath.Nodes);
						newPath.Nodes.Add((newNodes[j].X, newNodes[j].Y));
						newPath.CurrentNode = newNodes[j];
						newPath.HasReachEnd = newNodes[j].X == end.X && newNodes[j].Y == end.Y;
						if (newPath.HasReachEnd)
						{
							pathsToEnd.Add(currentPath);
						}
					}

					if (newNodes.Count > 0)
					{
						currentPath.Weight++;
						currentPath.Nodes.Add((newNodes[0].X, newNodes[0].Y));
						currentPath.CurrentNode = newNodes[0];
						currentPath.HasReachEnd = newNodes[0].X == end.X && newNodes[0].Y == end.Y;
					}
					if (currentPath.HasReachEnd)
					{
						pathsToEnd.Add(currentPath);
					}
				}

				for (int i = 0; i < pathsToEnd.Count; i++)
				{
					Path endedPath = pathsToEnd[i];

					paths.Remove(endedPath);

					if (!endedPath.IsRejected)
					{
						endedPaths.Add(endedPath);
						List<(int, int)> nodes = endedPath.Nodes.ToList();
						for (int j = 0; j < nodes.Count; j++)
						{
							(int X, int Y) node = nodes[j];

							if (!weightedNodes.ContainsKey(node))
								weightedNodes.Add(node, j);
							else
								if (weightedNodes[node] < j)
									weightedNodes[node] = j;
						}
					}
				}
			}

			for (int i = 0; i < endedPaths.Count; i++)
			{
				//WritePath(forest, endedPaths[i]);
				//Console.WriteLine("Weight : " + endedPaths[i].Weight);
			}

			endedPaths = endedPaths.OrderByDescending(path => path.Weight).ToList();

			WritePath(forest, endedPaths[0]);

			return endedPaths.Select(path => path.Weight).Max();
		}

		private void WritePath(ForestType[,] forest, Path path)
		{
			int size = forest.GetLength(0);

			for (int y = 0; y < size; y++)
			{
				for (int x = 0; x < size; x++)
				{
					if (path.Nodes.Where(node => node.X == x && node.Y == y).Any())
						Console.Write("O");
					else
						Console.Write(forest[y, x] == ForestType.EMPTY ? '.' : (forest[y, x] == ForestType.FOREST ? '#' : 's'));
				}
				Console.WriteLine();
			}

			Console.WriteLine();
		}
	}
}
