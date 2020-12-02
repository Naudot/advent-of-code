using System.IO;
using System.Linq;

namespace AdventOfCode.Year2019
{
	public class DayOne : Day2019
	{
		protected override object ResolveFirstPart()
		{
			int[] input = File.ReadAllLines(GetResourcesPath()).Select(int.Parse).ToArray();

			int result = 0;

			for (int i = 0; i < input.Length; i++)
			{
				result += input[i] / 3 - 2;
			}

			return result;
		}

		protected override object ResolveSecondPart()
		{
			int[] input = File.ReadAllLines(GetResourcesPath()).Select(int.Parse).ToArray();

			int result = 0;

			for (int i = 0; i < input.Length; i++)
			{
				int resultingFuel = input[i] / 3 - 2;

				do
				{
					result += resultingFuel;
					resultingFuel = resultingFuel / 3 - 2;
				} while (resultingFuel > 0);
			}

			return result;
		}
	}
}
