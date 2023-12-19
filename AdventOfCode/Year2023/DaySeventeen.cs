﻿namespace AdventOfCode.Year2023
{
	public class DaySeventeen : Day2023
	{
		public class Node
		{
			public (int X, int Y) Position;
			public (int X, int Y) Direction;
			public int TotalCost;
			public int DirectionRepeater;
		}

		protected override object ResolveFirstPart(string[] input)
		{
			// 827 TOO HIGH
			// Right, Down, Left, Up
			List<(int, int)> directions = new() { (1, 0), (0, 1), (-1, 0), (0, -1) };
			PriorityQueue<Node, float> priorityQueue = new();
			Node first = new();
			priorityQueue.Enqueue(first, 0);

			HashSet<(int x, int y, int straight, int prevDx, int prevDy)> visited = new();

			while (priorityQueue.Count > 0)
			{
				Node node = priorityQueue.Dequeue();

				if (!visited.Add((node.Position.X, node.Position.Y, node.DirectionRepeater, node.Direction.X, node.Direction.Y)))
				{
					continue;
				}

				if (node.Position.X == input[0].Length - 1 && node.Position.Y == input.Length - 1)
				{
					return node.TotalCost;
				}

				for (int i = 0; i < directions.Count; i++)
				{
					(int X, int Y) direction = directions[i];

					bool isSameDirection = node.Direction.X == direction.X && node.Direction.Y == direction.Y;
					//bool isStart = node.Direction.X == 0 && node.Direction.Y == 0;

					// Can not go in a straight line more than 3 times
					if (/*!isStart && */isSameDirection && node.DirectionRepeater > 3)
						break;

					// We skip OOB positions
					if (node.Position.X + direction.X < 0 
						|| node.Position.Y + direction.Y < 0 
						|| node.Position.X + direction.X > input[0].Length - 1 
						|| node.Position.Y + direction.Y > input.Length - 1)
						break;

					// We can not go back
					if (-node.Direction.X == direction.X && -node.Direction.Y == direction.Y)
						break;

					int newTotalCost = node.TotalCost + input[node.Position.Y + direction.Y][node.Position.X + direction.X] - '0';

					priorityQueue.Enqueue(new Node()
					{
						Position = (node.Position.X + direction.X, node.Position.Y + direction.Y),
						DirectionRepeater = (isSameDirection ? node.DirectionRepeater + 1 : 0),
						TotalCost = newTotalCost,
						Direction = direction
					}, newTotalCost);
				}
			}

			return -1;
		}

		protected override object ResolveSecondPart(string[] input)
		{
			return string.Empty;
		}
	}
}
