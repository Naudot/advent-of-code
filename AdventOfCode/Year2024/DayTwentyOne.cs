﻿namespace AdventOfCode.Year2024
{
	public class DayTwentyOne : Day2024
	{
		protected override bool DeactivateJIT => true;

		public class Node
		{
			public (int x, int y) Position;
			public HashSet<(int x, int y)> Positions = new();
			public List<(int x, int y)> Directions = new();
		}

		private static Dictionary<char, (int x, int y)> numKeypad = new()
		{
			{ '7', (0, 0) },
			{ '8', (1, 0) },
			{ '9', (2, 0) },
			{ '4', (0, 1) },
			{ '5', (1, 1) },
			{ '6', (2, 1) },
			{ '1', (0, 2) },
			{ '2', (1, 2) },
			{ '3', (2, 2) },
			{ ' ', (0, 3) },
			{ '0', (1, 3) },
			{ 'A', (2, 3) },
		};

		private static Dictionary<char, (int x, int y)> dirKeypad = new()
		{
			{ ' ', (0, 0) },
			{ '^', (1, 0) },
			{ 'A', (2, 0) },
			{ '<', (0, 1) },
			{ 'v', (1, 1) },
			{ '>', (2, 1) }
		};

		private static Dictionary<(int dirX, int dirY), char> directionsChar = new()
		{
			{ (0, -1), '^' },
			{ (-1, 0), '<' },
			{ (0, 1), 'v' },
			{ (1, 0), '>' }
		};

		private Dictionary<((int startX, int startY), (int targetX, int targetY)), List<(int posX, int posY)>> keypadMemoise = new();
		private Dictionary<((int startX, int startY), (int targetX, int targetY)), List<(int posX, int posY)>> directionnalMemoise = new();

		protected override object ResolveFirstPart(string[] input)
		{
			long result = 0;

			string[] codes = input;

			// For each code
			for (int i = 0; i < codes.Length; i++)
			{
				string code = codes[i];
				int complexity = GetComplexity(code.ToList(), 0, 3, 2);
				result += int.Parse(code.Split('A')[0]) * complexity;

				Console.WriteLine();
			}

			return result;
		}

		protected override object ResolveSecondPart(string[] input)
		{
			return 0;
		}

		private int GetComplexity(List<char> toPush, int depth, int stopDepth, int logDepth)
		{
			int complexity = 0;

			// When the robot arrives at the numeric keypad, its robotic arm is pointed at the A button in the bottom right corner. 
			char startChar = 'A';
			char targetChar = toPush[0];

			// This loop is moving through the numerical keypad with the first directionnal keypad
			for (int i = 0; i < toPush.Count; i++)
			{
				bool useNumKeypad = depth == 0;
				int height = useNumKeypad ? 4 : 2;
				Dictionary< char, (int, int)> usedPad = useNumKeypad ? numKeypad : dirKeypad;

				List<(int, int)> directionsToGo = GetDirectionsToPosition(3, height, usedPad[startChar], usedPad[targetChar], usedPad[' '], useNumKeypad);
				List<char> toPushNext = directionsToGo.Select(dir => directionsChar[dir]).ToList();
				toPushNext.Add('A');

				if (depth == logDepth)
					for (int j = 0; j < toPushNext.Count; j++)
						Console.Write(toPushNext[j]);

				if (depth == stopDepth)
					complexity += toPushNext.Count;
				else
					complexity += GetComplexity(toPushNext, depth + 1, stopDepth, logDepth);

				if (i + 1 < toPush.Count)
				{
					startChar = toPush[i];
					targetChar = toPush[i + 1];
				}
			}

			return complexity;
		}

		private List<(int, int)> GetDirectionsToPosition(
			int width,
			int height,
			(int x, int y) start,
			(int x, int y) targetPosition,
			(int x, int y) forbiddenPos,
			bool isNumericalKeypad)
		{
			if (isNumericalKeypad && keypadMemoise.ContainsKey((start, targetPosition)))
				return keypadMemoise[(start, targetPosition)];
			if (!isNumericalKeypad && directionnalMemoise.ContainsKey((start, targetPosition)))
				return directionnalMemoise[(start, targetPosition)];

			Node startNode = new() { Position = start, Positions = new() { start } };
			List<Node> currentNodes = new() { startNode };

			while (!currentNodes.Any(node => node.Position.x == targetPosition.x && node.Position.y == targetPosition.y))
			{
				List<Node> newNodes = new();

				for (int i = 0; i < currentNodes.Count; i++)
				{
					Node node = currentNodes[i];

					for (int j = 0; j < StaticBank.Directions.Count; j++)
					{
						(int x, int y) direction = StaticBank.Directions[j];
						(int x, int y) nextPos = (node.Position.x + direction.x, node.Position.y + direction.y);

						if (nextPos.x < 0 || 
							nextPos.x >= width || 
							nextPos.y < 0 || 
							nextPos.y >= height || 
							(nextPos.x == forbiddenPos.x && nextPos.y == forbiddenPos.y) || 
							node.Positions.Contains(nextPos))
							continue;

						Node newNode = new()
						{
							Position = nextPos,
							Positions = new(node.Positions),
							Directions = new(node.Directions)
						};
						newNode.Positions.Add(nextPos);
						newNode.Directions.Add(direction);
						newNodes.Add(newNode);
					}
				}

				currentNodes = newNodes;
			}

			Node bestPath = currentNodes.First(node => node.Positions.Contains(targetPosition));

			if (isNumericalKeypad)
				keypadMemoise.Add((start, targetPosition), bestPath.Directions);
			else
				directionnalMemoise.Add((start, targetPosition), bestPath.Directions);

			return bestPath.Directions;
		}
	}
}
