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
				Dictionary<int, int> previousPages = GetPreviousPagesCount(input, pageUpdate);
				bool isPageUpdateProperlyOrdered = IsPageUpdateProperlyOrdered(pageUpdate, previousPages);

				// P1
				if (isPageUpdateProperlyOrdered && !isPartTwo)
					result += pageUpdate[pageUpdate.Length / 2];

				// P2
				if (!isPageUpdateProperlyOrdered && isPartTwo)
				{
					int[] orderedValues = previousPages.OrderBy(node => node.Value).Select(node => node.Key).ToArray();
					result += orderedValues[orderedValues.Length / 2];
				}
			}

			return result;
		}

		private Dictionary<int, int> GetPreviousPagesCount(string[] input, int[] wantedPageNumbers)
		{
			Dictionary<int, int> previousPagesCount = new();

			for (int i = 0; i < input.Length; i++)
			{
				if (input[i] == string.Empty)
					break;

				string[] values = input[i].Split('|');
				int leftPage = int.Parse(values[0]);
				int rightPage = int.Parse(values[1]);

				if (!wantedPageNumbers.Contains(leftPage) || !wantedPageNumbers.Contains(rightPage))
					continue;

				if (!previousPagesCount.ContainsKey(leftPage))
					previousPagesCount.Add(leftPage, 0);
				if (!previousPagesCount.ContainsKey(rightPage))
					previousPagesCount.Add(rightPage, 0);

				previousPagesCount[rightPage]++;
			}

			return previousPagesCount;
		}

		private bool IsPageUpdateProperlyOrdered(int[] pageUpdate, Dictionary<int, int> pagesBeforeCount)
		{
			for (int i = 0; i < pageUpdate.Length; i++)
				if (pagesBeforeCount[pageUpdate[i]] != i)
					return false;
			return true;
		}
	}
}
