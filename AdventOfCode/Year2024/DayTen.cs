namespace AdventOfCode.Year2024
{
	public class DayTen : Day2024
	{
		public class Node
		{
			public int Value;
			public (int x, int y) Position;
			public HashSet<(int x, int y)> PreviousPositions = new();
			public bool HasReachDeadEnd;

			public bool IsEnd()
			{
				return Value == 9 || HasReachDeadEnd;
			}
		}

		protected override bool DeactivateJIT => true;

		protected override object ResolveFirstPart(string[] input)
		{
			int[,] map = new int[input.Length, input[0].Length];
			List<Node> nodes = new();

			for (int y = 0; y < input.Length; y++)
				for (int x = 0; x < input[y].Length; x++)
				{
					if (input[y][x] == '0')
						nodes.Add(new() { Position = (x, y), Value = 0 });
					map[x, y] = input[y][x] - '0';
				}

			int sum = 0;

			for (int j = 0; j < nodes.Count; j++)
			{
				List<Node> tempsNodes = new() { nodes[j] };

				while (tempsNodes.Any(node => !node.IsEnd()))
				{
					List<Node> newNodes = new();

					for (int i = 0; i < tempsNodes.Count; i++)
					{
						Node node = tempsNodes[i];
						node.PreviousPositions.Add(node.Position);

						Node upNode = GetNode(map, node, 0, -1, input.Length);
						if (upNode != null)
							newNodes.Add(upNode);
						Node rightNode = GetNode(map, node, 1, 0, input.Length);
						if (rightNode != null)
							newNodes.Add(rightNode);
						Node downNode = GetNode(map, node, 0, 1, input.Length);
						if (downNode != null)
							newNodes.Add(downNode);
						Node leftNode = GetNode(map, node, -1, 0, input.Length);
						if (leftNode != null)
							newNodes.Add(leftNode);
					}

					tempsNodes = newNodes;
				}

				sum += tempsNodes.Where(node => node.Value == 9).Select(node => node.Position).Distinct().Count();
			}

			return sum;
		}

		protected override object ResolveSecondPart(string[] input)
		{
			int[,] map = new int[input.Length, input[0].Length];
			List<Node> nodes = new();

			for (int y = 0; y < input.Length; y++)
				for (int x = 0; x < input[y].Length; x++)
				{
					if (input[y][x] == '0')
						nodes.Add(new() { Position = (x, y), Value = 0 });
					map[x, y] = input[y][x] - '0';
				}

			int sum = 0;

			for (int j = 0; j < nodes.Count; j++)
			{
				List<Node> tempsNodes = new() { nodes[j] };

				while (tempsNodes.Any(node => !node.IsEnd()))
				{
					List<Node> newNodes = new();

					for (int i = 0; i < tempsNodes.Count; i++)
					{
						Node node = tempsNodes[i];
						node.PreviousPositions.Add(node.Position);

						Node upNode = GetNode(map, node, 0, -1, input.Length);
						if (upNode != null)
							newNodes.Add(upNode);
						Node rightNode = GetNode(map, node, 1, 0, input.Length);
						if (rightNode != null)
							newNodes.Add(rightNode);
						Node downNode = GetNode(map, node, 0, 1, input.Length);
						if (downNode != null)
							newNodes.Add(downNode);
						Node leftNode = GetNode(map, node, -1, 0, input.Length);
						if (leftNode != null)
							newNodes.Add(leftNode);
					}

					tempsNodes = newNodes;
				}

				sum += tempsNodes.Where(node => node.Value == 9).Select(node => node.Position).Count();
			}

			return sum;
		}

		private Node GetNode(int[,] map, Node node, int deltaX, int deltaY, int mapSize)
		{
			(int x, int y) nextPosition = (node.Position.x + deltaX, node.Position.y + deltaY);

			if (nextPosition.x >= 0 && nextPosition.x < mapSize && nextPosition.y >= 0 && nextPosition.y < mapSize)
				if (map[nextPosition.x, nextPosition.y] == node.Value + 1 && !node.PreviousPositions.Contains(nextPosition))
					return new() { Value = node.Value + 1, Position = (nextPosition.x, nextPosition.y), PreviousPositions = node.PreviousPositions };

			return null;
		}
	}
}
