using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Year2022
{
	public class DayFour : Day2022
	{
		protected override object ResolveFirstPart(string[] input)
		{
			int overlappingPartnersCount = 0;

			for (int i = 0; i < input.Length; i++)
			{
				string line = input[i];
				MatchCollection matchColl = Regex.Matches(line, @"(\d*-\d*)");

				string[] first = matchColl[0].Groups[0].Value.Split('-');
				int firstLow = int.Parse(first[0]);
				int firstHigh = int.Parse(first[1]);

				string[] second = matchColl[1].Groups[0].Value.Split('-');
				int secondLow = int.Parse(second[0]);
				int secondHigh = int.Parse(second[1]);

				overlappingPartnersCount += IsCompleteOverlapping(firstLow, firstHigh, secondLow, secondHigh) ? 1 : 0;
			}

			return overlappingPartnersCount;
		}

		protected override object ResolveSecondPart(string[] input)
		{
			int partialOverlappingPartnersCount = 0;

			for (int i = 0; i < input.Length; i++)
			{
				string line = input[i];
				MatchCollection matchColl = Regex.Matches(line, @"(\d*-\d*)");

				string[] first = matchColl[0].Groups[0].Value.Split('-');
				int firstLow = int.Parse(first[0]);
				int firstHigh = int.Parse(first[1]);

				string[] second = matchColl[1].Groups[0].Value.Split('-');
				int secondLow = int.Parse(second[0]);
				int secondHigh = int.Parse(second[1]);

				partialOverlappingPartnersCount += IsPartialOverlapping(firstLow, firstHigh, secondLow, secondHigh) ? 1 : 0;
			}

			return partialOverlappingPartnersCount;
		}

		public bool IsCompleteOverlapping(int firstLow, int firstHigh, int secondLow, int secondHigh)
		{
			if (firstLow <= secondLow && firstHigh >= secondHigh)
			{
				return true;
			}

			if (secondLow <= firstLow && secondHigh >= firstHigh)
			{
				return true;
			}

			return false;
		}

		public bool IsPartialOverlapping(int firstLow, int firstHigh, int secondLow, int secondHigh)
		{
			if (firstLow > secondLow && firstLow > secondHigh)
			{
				return false;
			}

			if (firstHigh < secondLow && firstHigh < secondHigh)
			{
				return false;
			}

			if (secondLow > firstLow && secondLow > firstHigh)
			{
				return false;
			}

			if (secondHigh < firstLow && secondHigh < firstHigh)
			{
				return false;
			}

			return true;
		}
	}
}
