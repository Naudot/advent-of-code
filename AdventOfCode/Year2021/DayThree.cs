using System.Text;

namespace AdventOfCode.Year2021
{
	public class DayThree : Day2021
	{
		protected override object ResolveFirstPart(string[] input)
		{
			int length = input[0].Length;

			StringBuilder gammaRateString = new StringBuilder(input[0]);
			StringBuilder epsilonRateString = new StringBuilder(input[0]);

			for (int i = 0; i < length; i++)
			{
				int zeroCount = 0;
				int oneCount = 0;

				for (int j = 0; j < input.Length; j++)
				{
					char bit = input[j][i];

					if (bit == '0')
					{
						zeroCount++;
					}
					else
					{
						oneCount++;
					}
				}

				gammaRateString[i] = zeroCount > oneCount ? '0' : '1';
				epsilonRateString[i] = zeroCount > oneCount ? '1' : '0';
			}

			long epsilonRate = Convert.ToInt64(gammaRateString.ToString(), 2);
			long gammaRate = Convert.ToInt64(epsilonRateString.ToString(), 2);

			return epsilonRate * gammaRate;
		}

		protected override object ResolveSecondPart(string[] input)
		{
			return GetValue(input, true) * GetValue(input, false);
		}

		private int GetValue(string[] input, bool greatest)
		{
			List<string> values = input.ToList();

			int index = 0;
			while (values.Count != 1)
			{
				int zeroCount = values.Where(str => str[index] == '0').Count();
				int oneCount = values.Count - zeroCount;
				char notUsedChar = greatest ? (oneCount >= zeroCount ? '0' : '1') : (oneCount < zeroCount ? '0' : '1');
				values.RemoveAll(str => str[index] == notUsedChar);
				index++;
			}

			return Convert.ToInt32(values[0], 2);
		}
	}
}
