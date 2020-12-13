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
			string[] input = File.ReadAllLines(GetResourcesPath());
			string[] buses = input[1].Split(',').ToArray();

			Dictionary<int, int> busesIndexes = new Dictionary<int, int>(); // We keep each bus with his departure value
			Dictionary<int, int> steps = new Dictionary<int, int>(); // I want to iterate over a value with the higher possible step

			for (int i = 0; i < buses.Length; i++)
			{
				int busID;
				if (int.TryParse(buses[i], out busID)) // We do not process the "x"
				{
					busesIndexes.Add(busID, i);
					int sameDepartureBusID = i - busID;
					int newBusID;
					// Check if a bus BusID has the same departure has another bus
					// On my input, it happens to always be later buses which depart after the 523 bus so I check by using i - busID
					if (sameDepartureBusID >= 0 && int.TryParse(buses[sameDepartureBusID], out newBusID))
					{
						// Console.WriteLine("Bus " + busID + " is with " + newBusID);
						if (!steps.ContainsKey(sameDepartureBusID))
						{
							steps.Add(sameDepartureBusID, newBusID);
						}
						steps[sameDepartureBusID] *= busID;
					}
				}
			}

			// The step is the multiplier between same bus id departuring at the same time
			// E.g. : 523 * 17 * 29 * 37 because 17 29 and 37 are respectively departuring at 17 29 and 37 minutes from the 523
			KeyValuePair<int, int> chosenStep = new KeyValuePair<int, int>(0, 0);

			foreach (KeyValuePair<int, int> item in steps)
			{
				if (item.Value > chosenStep.Value)
				{
					chosenStep = item;
				}
			}

			double resultTimestamp = 0;
			bool isFound = false;

			while (!isFound)
			{
				isFound = true;
				foreach (KeyValuePair<int, int> bus in busesIndexes)
				{
					// We must remove the chosen step key to properly check the timestamp for each bus
					// E.g. : If my chosen step is 523 hence the 19th bus, I check each bus by removing 19 minutes
					if ((resultTimestamp + bus.Value - chosenStep.Key) % bus.Key != 0)
					{
						isFound = false;
						break;
					}
				}

				if (!isFound)
				{
					resultTimestamp += chosenStep.Value;
				}
			}

			return resultTimestamp - chosenStep.Key;
		}
	}
}
