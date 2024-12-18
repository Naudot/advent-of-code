namespace AdventOfCode.Year2024
{
	public class DayEighteen : Day2024
	{
		protected override bool DeactivateJIT
		{
			get
			{
				return true;
			}
		}

		protected override object ResolveFirstPart(string[] input)
		{
			int mapSize = 71;
			int memoryToInit = 1024;

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
				memory[bytePos.x, bytePos.y] = 500;
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

			long[] distances = Dijkstra(graph, 0);

			Console.WriteLine("Vertex\tDistance from Source");
			for (int i = 0; i < distances.Length; i++)
			{
				Console.WriteLine($"{i}\t{distances[i]}");
			}

			return 0;
		}

		protected override object ResolveSecondPart(string[] input)
		{
			return 0;
		}

		public class Graph
		{
			private long vertices;
			private List<(long, long)>[] adjacencyList;

			public Graph(long vertices)
			{
				this.vertices = vertices;
				adjacencyList = new List<(long, long)>[vertices];

				for (int i = 0; i < vertices; i++)
				{
					adjacencyList[i] = new List<(long, long)>();
				}
			}
			public void AddEdge(int u, int v, long weight)
			{
				adjacencyList[u].Add(new(v, weight));
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
				long u = MinimumDistance(distances, shortestPathTreeSet);
				shortestPathTreeSet[u] = true;

				foreach (var neighbor in graph.GetAdjacencyList()[u])
				{
					long v = neighbor.Item1;
					long weight = neighbor.Item2;

					if (!shortestPathTreeSet[v] && distances[u] != int.MaxValue && distances[u] + weight < distances[v])
					{
						distances[v] = distances[u] + weight;
					}
				}
			}

			return distances;
		}

		private static long MinimumDistance(long[] distances, bool[] shortestPathTreeSet)
		{
			long min = long.MaxValue, minIndex = -1;

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
