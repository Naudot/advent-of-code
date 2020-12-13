using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode.Year2020
{
	public class DayThirteen : Day2020
	{
		protected override object ResolveFirstPart()
		{
			string[] input = File.ReadAllLines(GetResourcesPath());
			int[] buses = input[1].Split(',').Where(value => value != "x").Select(int.Parse).ToArray();
			int departure = int.Parse(input[0]);

			int resultBusID = 0;
			int minutes = 0;
			bool isFound = false;

			while (!isFound)
			{
				for (int i = 0; i < buses.Length; i++)
				{
					if ((departure + minutes) % buses[i] == 0)
					{
						resultBusID = buses[i];
						isFound = true;
						break;
					}
				}
				if (!isFound)
				{
					minutes++;
				}
			}

			return resultBusID * minutes;
		}

		protected override object ResolveSecondPart()
		{
			// 19 c'est l'index de 523
			// je prends tous les bus qui partent en même temps que 523 et je les multiplie entre eux
			string[] input = File.ReadAllLines(GetResourcesPath());
			string[] buses = input[1].Split(',').ToArray();
			Dictionary<int, int> busesIndexes = new Dictionary<int, int>();

			Dictionary<int, int> steps = new Dictionary<int, int>();

			for (int i = 0; i < buses.Length; i++)
			{
				int busID;
				if (int.TryParse(buses[i], out busID))
				{
					busesIndexes.Add(busID, i);
					if (i - busID >= 0)
					{
						Console.WriteLine("Bus " + busID + " is with " + buses[i - busID]);
					}
				}
			}

			int step = 9540043; // Le 523 part, 17 minutes après le 17 part, pareil pour le 29 et le 37

			double resultTimestamp = 0;
			bool isFound = false;

			while (!isFound)
			{
				isFound = true;
				foreach (KeyValuePair<int, int> bus in busesIndexes)
				{
					if ((resultTimestamp + bus.Value - 19) % bus.Key != 0)
					{
						isFound = false;
						break;
					}
				}

				if (!isFound)
				{
					resultTimestamp += step;
				}
			}

			return resultTimestamp - 19;
		}
	}
}
