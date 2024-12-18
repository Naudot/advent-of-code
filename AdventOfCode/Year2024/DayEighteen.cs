namespace AdventOfCode.Year2024
{
	public class DayEighteen : Day2024
	{
		private const long WALL = 100000;

		protected override bool DeactivateJIT
		{
			get
			{
				return true;
			}
		}

		protected override object ResolveFirstPart(string[] input)
		{
			return GetShortestDistanceToEnd(input, 71, 1024);
		}

		protected override object ResolveSecondPart(string[] input)
		{
			int memoryToInit = 2900;
			long distance = -1;
			while (distance < WALL)
				distance = GetShortestDistanceToEnd(input, 71, ++memoryToInit);

			return input[memoryToInit - 1];
		}

		private long GetShortestDistanceToEnd(string[] input, int mapSize, int memoryToInit)
		{
			long[,] memory = new long[mapSize, mapSize];

			for (int y = 0; y < mapSize; y++)
			{
				for (int x = 0; x < mapSize; x++)
				{
					memory[x, y] = 1;
				}
			}

			for (int i = 0; i < memoryToInit; i++)
			{
				string[] coord = input[i].Split(',');
				(long x, long y) bytePos = (long.Parse(coord[0]), long.Parse(coord[1]));
				memory[bytePos.x, bytePos.y] = 100000;
			}

			Graph graph = new(mapSize * mapSize);
			for (int y = 0; y < mapSize; y++)
			{
				for (int x = 0; x < mapSize; x++)
				{
					int node = x + y * mapSize;

					if (y - 1 >= 0)
					{
						int adjUp = x + (y - 1) * mapSize;
						graph.AddEdge(node, adjUp, memory[x, y - 1]);
					}

					if (y + 1 < mapSize)
					{
						int adjDown = x + (y + 1) * mapSize;
						graph.AddEdge(node, adjDown, memory[x, y + 1]);
					}

					if (x - 1 >= 0)
					{
						int adjLeft = (x - 1) + y * mapSize;
						graph.AddEdge(node, adjLeft, memory[x - 1, y]);
					}

					if (x + 1 < mapSize)
					{
						int adjRight = (x + 1) + y * mapSize;
						graph.AddEdge(node, adjRight, memory[x + 1, y]);
					}
				}
			}

			return Dijkstra(graph, 0)[mapSize * mapSize - 1];
		}

		public class Graph
		{
			private List<(long, long)>[] adjacencyList;

			public Graph(long numberOfNodes)
			{
				adjacencyList = new List<(long, long)>[numberOfNodes];

				for (int i = 0; i < numberOfNodes; i++)
				{
					adjacencyList[i] = new List<(long, long)>();
				}
			}
			public void AddEdge(int nodeIndex, int edgeNodeIndex, long weight)
			{
				adjacencyList[nodeIndex].Add(new(edgeNodeIndex, weight));
			}
			public List<(long, long)>[] GetAdjacencyList()
			{
				return adjacencyList;
			}
		}

		public static long[] Dijkstra(Graph graph, long source)
		{
			long vertices = graph.GetAdjacencyList().Length;
			long[] distances = new long[vertices];
			bool[] shortestPathTreeSet = new bool[vertices];

			for (int i = 0; i < vertices; i++)
			{
				distances[i] = long.MaxValue;
				shortestPathTreeSet[i] = false;
			}

			distances[source] = 0;

			for (int count = 0; count < vertices - 1; count++)
			{
				long minDistance = MinimumDistance(distances, shortestPathTreeSet);
				shortestPathTreeSet[minDistance] = true;

				foreach ((long nodeEdgeIndex, long weight) in graph.GetAdjacencyList()[minDistance])
				{
					if (!shortestPathTreeSet[nodeEdgeIndex] && distances[minDistance] != int.MaxValue && distances[minDistance] + weight < distances[v])
					{
						distances[nodeEdgeIndex] = distances[minDistance] + weight;
					}
				}
			}

			return distances;
		}

		private static long MinimumDistance(long[] distances, bool[] shortestPathTreeSet)
		{
			long minIndex = -1;
			long min = long.MaxValue;

			for (int v = 0; v < distances.Length; v++)
			{
				if (!shortestPathTreeSet[v] && distances[v] <= min)
				{
					min = distances[v];
					minIndex = v;
				}
			}

			return minIndex;
		}
	}
}
