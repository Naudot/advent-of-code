namespace AdventOfCode.Year2021
{
	public class DayFifteen : Day2021
	{
		protected override object ResolveFirstPart(string[] input)
		{
			int[,] graph = new int[input.Length, input[0].Length];

			for (int i = 0; i < input.Length; i++)
			{
				for (int j = 0; j < input[0].Length; j++)
				{
					graph[i, j] = int.Parse(input[i][j].ToString());
				}
			}

			return 0;
		}

		protected override object ResolveSecondPart(string[] input)
		{
			return 0;
		}

		public void Dijkstra(int[,] graph, int source)
		{
		}
	}
}
