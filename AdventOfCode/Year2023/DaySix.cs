using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Year2023
{
	public class DaySix : Day2023
	{
		protected override object ResolveFirstPart(string[] input)
		{
			ulong result = 1;

			ulong[] times = Regex.Matches(input[0], @"(\d+)").Cast<Match>().Select(match => ulong.Parse(match.Value)).ToArray();
			ulong[] distances = Regex.Matches(input[1], @"(\d+)").Cast<Match>().Select(match => ulong.Parse(match.Value)).ToArray();

			for (int i = 0; i < times.Length; i++)
			{
				ulong time = times[i];
				ulong distance = distances[i];
				result *= GetWaysToBeat(time, distance);
			}

			return result;
		}

		protected override object ResolveSecondPart(string[] input)
		{
			ulong time = ulong.Parse(Regex.Match(input[0].Replace(" ", string.Empty), @"(\d+)").Value);
			ulong distance = ulong.Parse(Regex.Match(input[1].Replace(" ", string.Empty), @"(\d+)").Value);
			return GetWaysToBeat(time, distance);
		}

		private ulong GetWaysToBeat(ulong time, ulong distance)
		{
			ulong waysToBeat = 0;

			for (ulong j = 0; j < time; j++)
			{
				if (j * (time - j) > distance)
				{
					waysToBeat++;
				}
			}

			return waysToBeat;
		}
	}
}
