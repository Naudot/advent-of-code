using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode.Year2020
{
	public class DayTen : Day2020
	{
		protected override object ResolveFirstPart()
		{
			List<int> input = File.ReadAllLines(GetResourcesPath()).Select(int.Parse).ToList();
			input.Add(0);
			input.Add(input.Max() + 3);
			input.Sort();

			int differenceOne = 0;
			int differenceThree = 0;

			for (int i = 0; i < input.Count - 1; i++)
			{
				if (input[i + 1] - input[i] == 1)
				{
					differenceOne++;
				}
				if (input[i + 1] - input[i] == 3)
				{
					differenceThree++;
				}
			}

			return differenceOne * differenceThree;
		}

		protected override object ResolveSecondPart()
		{
			List<int> input = File.ReadAllLines(GetResourcesPath()).Select(int.Parse).ToList();
			input.Add(0);
			input.Add(input.Max() + 3);
			input.Sort();

			double result = 1;

			int beginIndex = 0;

			for (int i = 0; i < input.Count - 1; i++)
			{
				int value = input[i];
				int nextValue = input[i + 1];
				int diffValue = nextValue - value;

				if (diffValue == 3)
				{
					int indexDifference = i - beginIndex;
					if (indexDifference > 1)
					{
						if (indexDifference == 3)
						{
							result *= 4;
						}
						else
						{
							result *= (1 + Fact(indexDifference - 1));
						}
					}

					beginIndex = i + 1;
				}
			}

			return result;
		}
	
		public double Fact(int number)
		{
			double result = 1;
			while (number != 1)
			{
				result *= number;
				number -= 1;
			}
			return result;
		}
	}
}
