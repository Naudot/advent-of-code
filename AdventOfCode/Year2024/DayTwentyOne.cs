namespace AdventOfCode.Year2024
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

		private static Dictionary<char, (int x, int y)> keypad = new()
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
				result += int.Parse(code.Split('A')[0]) * GetComplexity(code);
			}

			return result;
		}

		protected override object ResolveSecondPart(string[] input)
		{
			return 0;
		}

		private int GetComplexity(string code)
		{
			// When the robot arrives at the numeric keypad, its robotic arm is pointed at the A button in the bottom right corner. 
			char startNumericalChar = 'A';
			char targetNumericalChar = code[0];

			// This loop is moving through the numerical keypad with the first directionnal keypad
			for (int j = 0; j < code.Length; j++)
			{
				List<(int, int)> directionsToGo = GetDirectionsToPosition(3, 4, keypad[startNumericalChar], keypad[targetNumericalChar], keypad[' '], true);
				List<char> toPushByFirstRobot = directionsToGo.Select(dir => directionsChar[dir]).ToList();
				toPushByFirstRobot.Add('A');
				//for (int k = 0; k < toPushByFirstRobot.Count; k++)
				//	Console.Write(toPushByFirstRobot[k]);

				// For each numerical to push, the robot must navigate in a directionnal keypad
				char firstDirStartChar = 'A';
				char firstDirTargetChar = toPushByFirstRobot[0];

				// This loop is moving through the first directionnal keypad with the second directionnal keypad
				for (int k = 0; k < toPushByFirstRobot.Count; k++)
				{
					List<(int, int)> directionsOnFirstDirKeypad = GetDirectionsToPosition(3, 2, dirKeypad[firstDirStartChar], dirKeypad[firstDirTargetChar], dirKeypad[' '], false);
					List<char> toPushBySecondRobot = directionsOnFirstDirKeypad.Select(dir => directionsChar[dir]).ToList();
					toPushBySecondRobot.Add('A');
					//for (int l = 0; l < toPushBySecondRobot.Count; l++)
					//	Console.Write(toPushBySecondRobot[l]);

					if (k + 1 < toPushByFirstRobot.Count)
					{
						firstDirStartChar = toPushByFirstRobot[k];
						firstDirTargetChar = toPushByFirstRobot[k + 1];
					}
				}

				if (j + 1 < code.Length)
				{
					startNumericalChar = code[j];
					targetNumericalChar = code[j + 1];
				}
			}
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
							Positions = new(node.Positions) { nextPos },
							Directions = new(node.Directions)
						};
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
