namespace AdventOfCode.Year2024
{
	public class DayTen : Day2024
	{
		public class Node
		{
			public int Value;
			public (int x, int y) Position;
			public HashSet<(int x, int y)> PreviousPositions = new();

			public bool IsEnd()
			{
				return Value == 9;
			}
		}

		protected override object ResolveFirstPart(string[] input)
		{
			return GetValues(input, false);
		}

		protected override object ResolveSecondPart(string[] input)
		{
			return GetValues(input, true);
		}

		private int GetValues(string[] input, bool secondPart)
		{
			int[,] map = new int[input.Length, input[0].Length];
			List<Node> startNodes = new();

			for (int y = 0; y < input.Length; y++)
				for (int x = 0; x < input[y].Length; x++)
				{
					if (input[y][x] == '0')
						startNodes.Add(new() { Position = (x, y), Value = 0 });
					map[x, y] = input[y][x] - '0';
				}

			int sum = 0;

			for (int j = 0; j < startNodes.Count; j++)
			{
				List<Node> nodes = new() { startNodes[j] };

				while (nodes.Any(node => !node.IsEnd()))
				{
					List<Node> nextNodes = new();

					for (int i = 0; i < nodes.Count; i++)
					{
						Node node = nodes[i];
						node.PreviousPositions.Add(node.Position);

						if (GetNode(map, node, 0, -1, input.Length, out Node upNode))
							nextNodes.Add(upNode);
						if (GetNode(map, node, 1, 0, input.Length, out Node rightNode))
							nextNodes.Add(rightNode);
						if (GetNode(map, node, 0, 1, input.Length, out Node downNode))
							nextNodes.Add(downNode);
						if (GetNode(map, node, -1, 0, input.Length, out Node leftNode))
							nextNodes.Add(leftNode);
					}

					nodes = nextNodes;
				}

				if (!secondPart)
					sum += nodes.Where(node => node.IsEnd()).Select(node => node.Position).Distinct().Count();
				else
					sum += nodes.Where(node => node.IsEnd()).Select(node => node.Position).Count();
			}

			return sum;
		}

		private bool GetNode(int[,] map, Node node, int deltaX, int deltaY, int mapSize, out Node newNode)
		{
			(int x, int y) nextPosition = (node.Position.x + deltaX, node.Position.y + deltaY);

			newNode = null;
			if (nextPosition.x >= 0 && nextPosition.x < mapSize && nextPosition.y >= 0 && nextPosition.y < mapSize)
				if (map[nextPosition.x, nextPosition.y] == node.Value + 1 && !node.PreviousPositions.Contains(nextPosition))
					newNode = new() { Value = node.Value + 1, Position = (nextPosition.x, nextPosition.y), PreviousPositions = node.PreviousPositions };

			return newNode != null;
		}
	}
}
