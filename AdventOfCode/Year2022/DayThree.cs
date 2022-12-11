using System.Linq;

namespace AdventOfCode.Year2022
{
	public class DayThree : Day2022
	{
		protected override object ResolveFirstPart(string[] input)
		{
			int totalPriorities = 0;

			for (int i = 0; i < input.Length; i++)
			{
				string rucksack = input[i];
				int compartimentSize = rucksack.Length / 2;
				char result = rucksack.Take(compartimentSize).Intersect(rucksack.Skip(compartimentSize).Take(compartimentSize)).First();

				if (result >= 'A' && result <= 'Z')
				{
					totalPriorities += ((int)result) - 38;
				}
				else
				{
					totalPriorities += ((int)result) - 96;
				}
			}

			return totalPriorities;
		}

		protected override object ResolveSecondPart(string[] input)
		{
			int totalPriorities = 0;

			for (int i = 0; i < input.Length; i += 3)
			{
				string rucksack1 = input[i];
				string rucksack2 = input[i + 1];
				string rucksack3 = input[i + 2];

				char result = rucksack1.Intersect(rucksack2).Intersect(rucksack3).First();

				if (result >= 'A' && result <= 'Z')
				{
					totalPriorities += ((int)result) - 38;
				}
				else
				{
					totalPriorities += ((int)result) - 96;
				}
			}

			return totalPriorities;
		}
	}
}
