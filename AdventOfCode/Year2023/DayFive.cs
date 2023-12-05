using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Year2023
{
	public class Map
	{
		public string Begin;
		public string End;
		public List<Data> Dataset = new List<Data>();
	}

	public class Data
	{
		public ulong DestRangStart;
		public ulong SourceRangStart;
		public ulong RangeLength;
	}

	public class DayFive : Day2023
	{
		protected override object ResolveFirstPart(string[] input)
		{
			ulong[] seeds = input[0].Replace("seeds: ", string.Empty).Split(' ').Select(seed => ulong.Parse(seed)).ToArray();
			List<Map> maps = GetMapping(input);

			// the section that starts with seed-to-soil map: describes how to convert a seed number (the source) to a soil number (the destination)
			// Each line within a map contains three numbers: the destination range start, the source range start, and the range length.
			// E.g : 50 98 2 you know that seed number 98 corresponds to soil number 50 and that seed number 99 corresponds to soil number 51.
			// 52 50 48 So, seed number 53 corresponds to soil number 55.
			// Any source numbers that aren't mapped correspond to the same destination number. So, seed number 10 corresponds to soil number 10.

			// the lowest location number that corresponds to any of the initial seeds

			ulong lowestLocationNumber = ulong.MaxValue;

			for (int i = 0; i < seeds.Length; i++)
			{
				ulong seed = seeds[i];
				ulong usedValue = seed;

				for (int j = 0; j < maps.Count; j++)
				{
					Map map = maps[j];

					for (int k = 0; k < map.Dataset.Count; k++)
					{
						Data data = map.Dataset[k];

						// Not <= because we do not include the last number : E.g. Range 2 from 50 is 50 and 51
						if (usedValue >= data.SourceRangStart && usedValue < data.SourceRangStart + data.RangeLength)
						{
							usedValue += (data.DestRangStart - data.SourceRangStart);
							break;
						}
					}
				}

				if (usedValue < lowestLocationNumber)
				{
					lowestLocationNumber = usedValue;
				}
			}

			return lowestLocationNumber;
		}

		protected override object ResolveSecondPart(string[] input)
		{
			ulong[] seeds = input[0].Replace("seeds: ", string.Empty).Split(' ').Select(seed => ulong.Parse(seed)).ToArray();
			List<Map> map = GetMapping(input);

			return string.Empty;
		}

		private List<Map> GetMapping(string[] input)
		{
			List<Map> maps = new List<Map>();

			Map currentMap = new Map();
			for (int i = 2; i < input.Length; i++)
			{
				string line = input[i];

				if (line == string.Empty)
				{
					continue;
				}

				if (line.Contains(':'))
				{
					string[] mapString = line.Replace(" map:", string.Empty).Replace("-to-", ",").Split(',').Select(title => title).ToArray();
					Map map = new Map() { Begin = mapString[0], End = mapString[1] };
					maps.Add(map);

					currentMap = map;
				}
				else
				{
					ulong[] dataValues = line.Split(' ').Select(seed => ulong.Parse(seed)).ToArray();
					Data data = new Data() { DestRangStart = dataValues[0], SourceRangStart = dataValues[1], RangeLength = dataValues[2] };
					currentMap.Dataset.Add(data);
				}
			}

			return maps;
		}
	}
}
