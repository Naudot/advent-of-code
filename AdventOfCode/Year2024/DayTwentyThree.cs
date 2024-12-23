namespace AdventOfCode.Year2024
{
	public class DayTwentyThree : Day2024
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
			Dictionary<string, List<string>> connections = GetConnections(input);

			HashSet<List<string>> threeNodesConnections = new();

			foreach (KeyValuePair<string, List<string>> originalConnection in connections)
			{
				string computer = originalConnection.Key;

				foreach (string otherComputer in originalConnection.Value)
				{
					if (otherComputer == computer)
						continue;

					foreach (string otherComputerConnection in connections[otherComputer])
					{
						if (otherComputerConnection == computer)
							continue;

						if (originalConnection.Value.Contains(otherComputerConnection))
						{
							if (threeNodesConnections.FirstOrDefault(tnc => tnc.Contains(computer) && tnc.Contains(otherComputer) && tnc.Contains(otherComputerConnection)) == null)
								threeNodesConnections.Add(new() { computer, otherComputer, otherComputerConnection });
						}
					}
				}
			}

			return threeNodesConnections.Count(list => list.Any(val => val.StartsWith('t')));
		}

		protected override object ResolveSecondPart(string[] input)
		{
			Dictionary<string, List<string>> connections = GetConnections(input);
			Dictionary<string, int> biggestSet = new();

			foreach (KeyValuePair<string, List<string>> connection in connections)
			{
				// We start by fetching our computer
				string computer = connection.Key;
				List<string> computerConnections = connection.Value;

				// We use this count to keep track of how much the count of an element in the set should be 
				int processedCount = 1;

				// We automatically process our first computer
				Dictionary<string, int> set = new();
				for (int i = 0; i < computerConnections.Count; i++)
					set.Add(computerConnections[i], processedCount);
				set.Add(computer, processedCount);

				// For each of the connection of our first computer
				for (int i = 0; i < computerConnections.Count; i++)
				{
					string firstDegreeLinkedComputer = computerConnections[i];

					// If the set does not contain anymore the linked computer, we skip it
					if (!set.ContainsKey(firstDegreeLinkedComputer))
						continue;

					// Keep tracks of connection count by looking at the linked computer connections
					set[firstDegreeLinkedComputer]++;
					foreach (string otherConnection in connections[firstDegreeLinkedComputer])
					{
						if (set.ContainsKey(otherConnection))
							set[otherConnection]++;
					}

					processedCount++;

					// If one of the computer of the set has not been found in the linked computer connections
					// (meaning his count has not reached the processedCount), we remove it from the set.
					List<string> keysToRemove = new();
					foreach (KeyValuePair<string, int> linkCount in set)
						if (linkCount.Value != processedCount)
							keysToRemove.Add(linkCount.Key);
					for (int j = 0; j < keysToRemove.Count; j++)
						set.Remove(keysToRemove[j]);
				}

				if (biggestSet.Count < set.Count)
					biggestSet = set;
			}

			string log = string.Empty;
			List<string> orderedValues = biggestSet.Keys.OrderBy(val => val).ToList();
			for (int i = 0; i < orderedValues.Count; i++)
			{
				log += orderedValues[i];
				if (i != orderedValues.Count - 1)
					log += ",";
			}
			return log;
		}

		private Dictionary<string, List<string>> GetConnections(string[] input)
		{
			Dictionary<string, List<string>> connections = new();

			for (int i = 0; i < input.Length; i++)
			{
				string[] computers = input[i].Split('-');
				string left = computers[0];
				string right = computers[1];

				if (connections.ContainsKey(left))
					connections[left].Add(right);
				else
					connections.Add(left, new() { right });

				if (connections.ContainsKey(right))
					connections[right].Add(left);
				else
					connections.Add(right, new() { left });
			}

			return connections;
		}
	}
}
