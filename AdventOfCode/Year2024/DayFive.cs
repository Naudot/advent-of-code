namespace AdventOfCode.Year2024
{
	public class Node
	{
		public int Occurence = 1;
	}

	public class DayFive : Day2024
	{
		protected override bool DeactivateJIT => true;

		private Dictionary<int, Node> nodes = new();

		protected override object ResolveFirstPart(string[] input)
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

				if (!isPageUpdates)
				{
					string[] values = input[i].Split('|');
					int leftValue = int.Parse(values[0]);
					int rightValue = int.Parse(values[1]);

					if (!nodes.ContainsKey(leftValue))
						nodes.Add(leftValue, new());
					else
						nodes[leftValue].Occurence++;

					if (!nodes.ContainsKey(rightValue))
						nodes.Add(rightValue, new());
					else
						nodes[rightValue].Occurence++;
				}
				else
				{
					// TODO : Check la page update et prendre son milieu de page en += si elle match les ordering rules
				}
			}

			foreach (KeyValuePair<int, Node> item in nodes)
			{
				Console.WriteLine($"{item.Key} {item.Value.Occurence}");
			}
			Console.WriteLine($"Nombre d'items {nodes.Count}");

			return result;
		}

		protected override object ResolveSecondPart(string[] input)
		{
			return 0;
		}
	}
}
