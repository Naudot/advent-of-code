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

		private Dictionary<(string, int depth), long> complexityMemoise = new();

		protected override object ResolveFirstPart(string[] input)
		{
			long result = 0;

			string[] codes = input;

			// For each code
			for (int i = 0; i < codes.Length; i++)
			{
				string code = codes[i];
				long complexity = GetComplexity(code.ToList(), 0, 2, -1);
				long val = long.Parse(code.Split('A')[0]);
				result += complexity * val;
			}

			return result;
		}

		protected override object ResolveSecondPart(string[] input)
		{
			complexityMemoise.Clear();

			long result = 0;

			string[] codes = input;

			// For each code
			for (int i = 0; i < codes.Length; i++)
			{
				// 348675369134078 : too high
				// 139292403700976 : too low

				string code = codes[i];
				long complexity = GetComplexity(code.ToList(), 0, 25, 3);
				long val = long.Parse(code.Split('A')[0]);

				//Console.WriteLine(complexity + " * " + val);

				result += complexity * val;
			}

			return result;
		}

		private long GetComplexity(List<char> toPush, int depth, int stopDepth, int logDepth)
		{
			if (complexityMemoise.ContainsKey((new string(toPush.ToArray()), depth)))
				return complexityMemoise[(new string(toPush.ToArray()), depth)];

			long complexity = 0;

			char startChar = 'A';
			char targetChar = toPush[0];

			for (int i = 0; i < toPush.Count; i++)
			{
				bool useNumKeypad = depth == 0;
				int height = useNumKeypad ? 4 : 2;
				Dictionary<char, (int, int)> usedPad = useNumKeypad ? numKeypad : dirKeypad;
				(int x, int y) startPos = usedPad[startChar];
				(int x, int y) endPos = usedPad[targetChar];
				(int x, int y) forbiddenPos = usedPad[' '];

				List<(int, int)> directionsToGo = GetDirectionsToPosition(3, height, startPos, endPos, forbiddenPos, useNumKeypad);
				List<char> toPushNext = directionsToGo.Select(dir => directionsChar[dir]).ToList();

				// For each different char, I check if there is a valid sorted solution
				// Indeed, arriving at the target can be done in any order
				// but the best order is when the same chars must be pushed in a row to avoid
				// useless movements later
				List<List<char>> toPushNexts = new();
				List<char> distinctChars = toPushNext.Distinct().ToList();
				for (int j = 0; j < distinctChars.Count; j++)
				{
					List<char> toPushSorted = toPushNext.OrderBy(c => c == distinctChars[j]).ToList();
					(int x, int y) tempPos = startPos;
					bool isReachingForbiddenChar = false;
					for (int k = 0; k < toPushSorted.Count; k++)
					{
						if (tempPos == forbiddenPos ||
							tempPos.x < 0 ||
							tempPos.x >= 3 ||
							tempPos.y < 0 ||
							tempPos.y >= height)
							isReachingForbiddenChar = true;
						(int x, int y) charDirection = directionsChar.First(pair => pair.Value == toPushSorted[k]).Key;
						tempPos = (tempPos.x + charDirection.x, tempPos.y + charDirection.y);
					}
					if (!isReachingForbiddenChar)
					{
						toPushNexts.Add(toPushSorted);
					}
				}

				//if (toPushNexts.Count == 0)
				//	toPushNexts.Add(new());
				//toPushNexts[0].Add('A');
				//if (depth == stopDepth)
				//	complexity += toPushNexts[0].Count;
				//else
				//	complexity += GetComplexity(toPushNexts[0], depth + 1, stopDepth, logDepth);

				if (toPushNexts.Count == 0)
					toPushNexts.Add(new());

				List<long> complexities = new();
				for (int j = 0; j < toPushNexts.Count; j++)
				{
					toPushNexts[j].Add('A');
					
					if (depth == stopDepth)
						complexities.Add(toPushNexts[j].Count);
					else
						complexities.Add(GetComplexity(toPushNexts[j], depth + 1, stopDepth, logDepth));
				}
				complexity += complexities.Min();

				if (i + 1 < toPush.Count)
				{
					startChar = toPush[i];
					targetChar = toPush[i + 1];
				}
			}

			if (!complexityMemoise.ContainsKey((new string(toPush.ToArray()), depth)))
				complexityMemoise.Add((new string(toPush.ToArray()), depth), complexity);

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
