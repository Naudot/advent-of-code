using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Year2023
{
	public class DaySix : Day2023
	{
		protected override object ResolveFirstPart(string[] input)
		{
			ulong[] times = Regex.Matches(input[0], @"(\d+)").Cast<Match>().Select(match => ulong.Parse(match.Value)).ToArray();
			ulong[] distances = Regex.Matches(input[1], @"(\d+)").Cast<Match>().Select(match => ulong.Parse(match.Value)).ToArray();

			return times.Select((value, index) => GetWaysToBeat(value, distances[index]))
						.Aggregate((ulong)1, (x, y) => x * y);
		}

		protected override object ResolveSecondPart(string[] input)
		{
			ulong time = ulong.Parse(Regex.Match(input[0].Replace(" ", string.Empty), @"(\d+)").Value);
			ulong distance = ulong.Parse(Regex.Match(input[1].Replace(" ", string.Empty), @"(\d+)").Value);
			return GetWaysToBeat(time, distance);
		}

		private ulong GetWaysToBeat(ulong time, ulong distance)
		{
			// Best solution :)
			ulong waysToBeat = 0;

			for (ulong j = 0; j < time; j++)
			{
				if (j * (time - j) > distance)
				{
					waysToBeat++;
				}
			}

			return waysToBeat;

			// Dirty and slow LINQ solution
			//return 
			//	(ulong)Enumerable
			//	.Range(0, (int)time)
			//	.Where(value => (ulong)value * (time - (ulong)value) > distance)
			//	.Count();
		}
	}
}
