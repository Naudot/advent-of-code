namespace AdventOfCode.Year2024
{
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
			int pagesIndex = Array.IndexOf(input, string.Empty) + 1;
			double result = 0;

			for (int i = pagesIndex; i < input.Length; i++)
			{
				int[] pageUpdate = input[i].Split(',').Select(val => int.Parse(val)).ToArray();
				Dictionary<int, HashSet<int>> pagesCount = GetPages(input, pageUpdate);

				int index = 0;
				foreach (int pageNumber in pageUpdate)
				{
					if (pagesCount[pageNumber].Count != index)
						break;
					index++;
				}

				// P1
				if (index == pageUpdate.Length && !isPartTwo)
					result += pageUpdate[pageUpdate.Length / 2];

				// P2
				if (index != pageUpdate.Length && isPartTwo)
				{
					int[] orderedValues = pagesCount.OrderBy(node => node.Value.Count).Select(node => node.Key).ToArray();
					result += orderedValues[orderedValues.Length / 2];
				}
			}

			return result;
		}

		private Dictionary<int, HashSet<int>> GetPages(string[] input, int[] wantedPageNumbers)
		{
			Dictionary<int, HashSet<int>> pagesLink = new();

			for (int i = 0; i < input.Length; i++)
			{
				if (input[i] == string.Empty)
					break;

				string[] values = input[i].Split('|');
				int leftPage = int.Parse(values[0]);
				int rightPage = int.Parse(values[1]);

				if (!wantedPageNumbers.Contains(leftPage) || !wantedPageNumbers.Contains(rightPage))
					continue;

				if (!pagesLink.ContainsKey(leftPage))
					pagesLink.Add(leftPage, new());
				if (!pagesLink.ContainsKey(rightPage))
					pagesLink.Add(rightPage, new());

				pagesLink[rightPage].Add(leftPage);
			}

			return pagesLink;
		}

		private bool IsPageUpdateValid(int[] pageUpdate, Dictionary<int, HashSet<int>> pagesBeforeCount)
		{
			int index = 0;

			foreach (int pageNumber in pageUpdate)
			{
				if (pagesBeforeCount[pageNumber].Count != index)
					break;
				index++;
			}

			return index == pageUpdate.Length;
		}
	}
}
