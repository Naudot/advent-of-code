namespace AdventOfCode.Year2024
{
	public class Node
	{
		public HashSet<int> NodesBefore = new();
		public HashSet<int> NodesAfter = new();
	}

	public class DayFive : Day2024
	{
		protected override object ResolveFirstPart(string[] input)
		{
			return ProcessPageUpdates(input, false);
		}

		protected override object ResolveSecondPart(string[] input)
		{
			return ProcessPageUpdates(input, true);
		}

		private double ProcessPageUpdates(string[] input, bool isPartTwo)
		{
			double result = 0;

			bool isPageUpdates = false;
			for (int i = 0; i < input.Length; i++)
			{
				if (input[i] == string.Empty)
				{
					isPageUpdates = true;
					continue;
				}

				if (isPageUpdates)
				{
					int[] pageUpdate = input[i].Split(',').Select(val => int.Parse(val)).ToArray();
					Dictionary<int, Node> nodes = Parse(input, pageUpdate);

					int index = 0;
					foreach (int pageNumber in pageUpdate)
					{
						if (nodes[pageNumber].NodesBefore.Count != index)
							break;
						index++;
					}

					// P1
					if (index == pageUpdate.Length && !isPartTwo)
						result += pageUpdate[pageUpdate.Length / 2];

					// P2
					if (index != pageUpdate.Length && isPartTwo)
					{
						int[] orderedValues = nodes.OrderBy(node => node.Value.NodesBefore.Count).Select(node => node.Key).ToArray();
						result += orderedValues[orderedValues.Length / 2];
					}
				}
			}

			return result;
		}

		private Dictionary<int, Node> Parse(string[] input, int[] interestedNumbers)
		{
			Dictionary<int, Node> nodes = new();

			for (int i = 0; i < input.Length; i++)
			{
				if (input[i] == string.Empty)
					break;

				string[] values = input[i].Split('|');
				int leftValue = int.Parse(values[0]);
				int rightValue = int.Parse(values[1]);

				if (!interestedNumbers.Contains(leftValue) || !interestedNumbers.Contains(rightValue))
					continue;

				if (!nodes.ContainsKey(leftValue))
					nodes.Add(leftValue, new());
				if (!nodes.ContainsKey(rightValue))
					nodes.Add(rightValue, new());

				nodes[leftValue].NodesAfter.Add(rightValue);
				nodes[rightValue].NodesBefore.Add(leftValue);
			}

			return nodes;
		}
	}
}
