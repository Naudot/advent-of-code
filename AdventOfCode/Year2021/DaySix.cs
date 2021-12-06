using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Year2021
{
	public class DaySix : Day2021
	{
		protected override object ResolveFirstPart(string[] input)
		{
			return CountFishesFast(input, 80);
		}

		protected override object ResolveSecondPart(string[] input)
		{
			return CountFishesFast(input, 256); // 434 max value
		}

		public int CountFishesSlow(string[] input, int days)
		{
			List<Lanternfish> fishes = input[0].Split(',').Select((val) => new Lanternfish() { DaysBeforeAnotherLanternfish = int.Parse(val) }).ToList();
			List<Lanternfish> newFishes = new List<Lanternfish>();
			for (int i = 0; i < days; i++)
			{
				newFishes.Clear();

				for (int j = 0; j < fishes.Count; j++)
				{
					Lanternfish fish = fishes[j];
					fish.DaysBeforeAnotherLanternfish--;
					if (fish.DaysBeforeAnotherLanternfish == -1)
					{
						fish.ResetTimer();
						newFishes.Add(fish.Reproduce());
					}
				}

				fishes.AddRange(newFishes);
			}

			return fishes.Count;
		}

		public long CountFishesFast(string[] input, int days)
		{
			// Days before reproducing, number of fishes reproducing that day
			Dictionary<long, long> fishesFast = new Dictionary<long, long>();

			for (int j = 0; j < 9; j++)
			{
				fishesFast.Add(j, 0);
			}

			List<Lanternfish> fishes = input[0].Split(',').Select((val) => new Lanternfish() { DaysBeforeAnotherLanternfish = int.Parse(val) }).ToList();

			for (int i = 0; i < fishes.Count; i++)
			{
				fishesFast[fishes[i].DaysBeforeAnotherLanternfish]++;
			}

			for (int i = 0; i < days; i++)
			{
				long zero = fishesFast[0]; // Will reproduce
				long one = fishesFast[1];
				long two = fishesFast[2];
				long three = fishesFast[3];
				long four = fishesFast[4];
				long five = fishesFast[5];
				long six = fishesFast[6];
				long seven = fishesFast[7];
				long eight = fishesFast[8]; // Previous newborns

				fishesFast[8] += zero;
				fishesFast[8] -= eight;

				fishesFast[7] += eight;
				fishesFast[7] -= seven;

				fishesFast[6] += (seven + zero);
				fishesFast[6] -= six;

				fishesFast[5] += six;
				fishesFast[5] -= five;

				fishesFast[4] += five;
				fishesFast[4] -= four;

				fishesFast[3] += four;
				fishesFast[3] -= three;

				fishesFast[2] += three;
				fishesFast[2] -= two;

				fishesFast[1] += two;
				fishesFast[1] -= one;

				fishesFast[0] += one;
				fishesFast[0] -= zero;
			}

			long fishCount = 0;
			foreach (KeyValuePair<long, long> item in fishesFast)
			{
				fishCount += item.Value;
			}

			return fishCount;
		}
	}

	public class Lanternfish
	{
		public int DaysBeforeAnotherLanternfish { get; set; }

		public void ResetTimer()
		{
			DaysBeforeAnotherLanternfish = 6; // 7 Days
		}

		public Lanternfish Reproduce()
		{
			return new Lanternfish() { DaysBeforeAnotherLanternfish = 8 };
		}
	}
}
