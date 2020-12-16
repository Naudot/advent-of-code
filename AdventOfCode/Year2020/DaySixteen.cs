using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Year2020
{
	public class DaySixteen : Day2020
	{
		protected override object ResolveFirstPart()
		{
			string input = File.ReadAllText(GetResourcesPath());

			MatchCollection ranges = Regex.Matches(input, @"\D*: (\d*-\d*) or (\d*-\d*)"); // Todo : split group 1 et 2 avec - pour avoir deux chiffres
			Match nearbyTickets = Regex.Match(input, @"nearby tickets:\n([$\n[\d*,]*]*)"); // Todo split group 1 par /n et encore split les strings par ,

			int errorRate = 0;

			List<Tuple<int, int>> rangesCalculated = new List<Tuple<int, int>>();

			for (int i = 0; i < ranges.Count; i++)
			{
				Match range = ranges[i];
				string[] lowerRange = range.Groups[1].Value.Split('-');
				string[] higherRange = range.Groups[2].Value.Split('-');
				int lowerMin = int.Parse(lowerRange[0]);
				int lowerMax = int.Parse(lowerRange[1]);
				int higherMin = int.Parse(higherRange[0]);
				int higherMax = int.Parse(higherRange[1]);
				rangesCalculated.Add(new Tuple<int, int>(lowerMin, lowerMax));
				rangesCalculated.Add(new Tuple<int, int>(higherMin, higherMax));
			}

			string[] tickets = nearbyTickets.Groups[1].Value.Split('\n');

			for (int i = 0; i < tickets.Length; i++)
			{
				if (string.IsNullOrEmpty(tickets[i]))
				{
					continue;
				}

				errorRate += GetValidityRateOf(rangesCalculated, tickets[i]);
			}

			return errorRate;
		}

		protected override object ResolveSecondPart()
		{
			string input = File.ReadAllText(GetResourcesPath());

			MatchCollection ranges = Regex.Matches(input, @"(\D*): (\d*-\d*) or (\d*-\d*)"); // Todo : split group 1 et 2 avec - pour avoir deux chiffres
			Match nearbyTickets = Regex.Match(input, @"nearby tickets:\n([$\n[\d*,]*]*)"); // Todo split group 1 par /n et encore split les strings par ,

			List<Tuple<string, int, int>> rangesCalculated = new List<Tuple<string, int, int>>();
			Dictionary<string, int[]> zones = new Dictionary<string, int[]>();

			for (int i = 0; i < ranges.Count; i++)
			{
				Match range = ranges[i];
				string zone = range.Groups[1].Value;

				if (!zones.ContainsKey(zone))
				{
					zones.Add(zone, new int[20]);
				}

				string[] lowerRange = range.Groups[2].Value.Split('-');
				string[] higherRange = range.Groups[3].Value.Split('-');
				int lowerMin = int.Parse(lowerRange[0]);
				int lowerMax = int.Parse(lowerRange[1]);
				int higherMin = int.Parse(higherRange[0]);
				int higherMax = int.Parse(higherRange[1]);
				rangesCalculated.Add(new Tuple<string, int, int>(zone, lowerMin, lowerMax));
				rangesCalculated.Add(new Tuple<string, int, int>(zone, higherMin, higherMax));
			}

			int validTicketCount = 0;
			string[] tickets = nearbyTickets.Groups[1].Value.Split('\n');

			for (int i = 0; i < tickets.Length; i++)
			{
				if (string.IsNullOrEmpty(tickets[i]))
				{
					continue;
				}

				validTicketCount += CalculateExclusiveValues(zones, rangesCalculated, tickets[i]) ? 1 : 0;
			}

			foreach (KeyValuePair<string, int[]> zone in zones)
			{
				Console.Write(zone.Key + ":\t");
				for (int i = 0; i < zone.Value.Length; i++)
				{
					Console.Write(zone.Value[i] - validTicketCount + 1);
				}
				Console.WriteLine();
			}

			/*
			departure time:			01111001101011111111 Index 10 (par rapport à departure date)
			departure date:			01111001100011111111 Index 3 (par rapport à departure platform)
			departure platform:     01101001100011111111 Index 18 (par rapport à departure location)
			departure location:     01101001100011111101 Index 12 (par rapport à departure station)
			departure station:      01101001100001111101 Index 8 (par rapport à departure track)
			departure track:        01101001000001111101 Index 13 (par rapport à arrival track)
			arrival track:			01101001000000111101
			*/

			return 59ul * 67 * 151 * 107 * 167 * 179;
		}

		private int GetValidityRateOf(List<Tuple<int, int>> rangesCalculated, string ticket)
		{
			int result = 0;

			string[] values = ticket.Split(',');

			for (int i = 0; i < values.Length; i++)
			{
				int value = int.Parse(values[i]);
				if (rangesCalculated.Where(tuple => tuple.Item1 <= value && tuple.Item2 >= value).FirstOrDefault() == null)
				{
					result += value;
				}
			}

			return result;
		}

		private bool CalculateExclusiveValues(Dictionary<string, int[]> zones, List<Tuple<string, int, int>> rangesCalculated, string ticket)
		{
			string[] values = ticket.Split(',');

			for (int i = 0; i < values.Length; i++)
			{
				int value = int.Parse(values[i]);
				if (rangesCalculated.Where(tuple => tuple.Item2 <= value && tuple.Item3 >= value).FirstOrDefault() == null)
				{
					return false;
				}
			}

			for (int i = 0; i < values.Length; i++) // Test sur seulement la première valeur
			{
				int value = int.Parse(values[i]);
				List<Tuple<string, int, int>> matchedRanges = rangesCalculated.Where(tuple => tuple.Item2 <= value && tuple.Item3 >= value).ToList();
				for (int j = 0; j < matchedRanges.Count; j++)
				{
					string zone = matchedRanges[j].Item1;
					if (zones.ContainsKey(zone))
					{
						zones[zone][i] += 1;
					}
				}
			}

			return true;
		}
	}
}
