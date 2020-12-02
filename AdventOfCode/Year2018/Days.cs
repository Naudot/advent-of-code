using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode.Year2018
{
	public class DayOne : Day2018
	{
		protected override object ResolveFirstPart()
		{
			return File.ReadAllLines(GetResourcesPath()).Select(int.Parse).Aggregate((val1, val2) => val1 + val2);
		}

		protected override object ResolveSecondPart()
		{
			string[] input = File.ReadAllLines(GetResourcesPath());

			int result = 0;

			HashSet<int> frequencies = new HashSet<int>();

			while (true)
			{
				for (int i = 0; i < input.Length; i++)
				{
					if (input[i].Contains("+"))
					{
						result += int.Parse(input[i].Split('+')[1]);
					}
					else
					{
						result -= int.Parse(input[i].Split('-')[1]);
					}

					if (frequencies.Contains(result))
					{
						return result;
					}
					else
					{
						frequencies.Add(result);
					}
				}
			}
		}
	}
}
