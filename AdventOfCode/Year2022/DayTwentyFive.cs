namespace AdventOfCode.Year2022
{
	public class DayTwentyFive : Day2022
	{
		protected override object ResolveFirstPart(string[] input)
		{
			long result = 0;

			for (int i = 0; i < input.Length; i++)
			{
				result += GetValue(input[i]);
			}

			return GetSnafu(result, new char[] {'0','1','2','3','4' });
		}

		private long GetValue(string snafu)
		{
			double value = 0;

			for (int i = 0; i < snafu.Length; i++)
			{
				char snafuChar = snafu[i];

				double pow = Math.Pow(5, (snafu.Length - 1 - i));

				value += pow * (snafuChar == '1' ? 1 : (snafuChar == '2' ? 2 : (snafuChar == '=' ? -2 : (snafuChar == '-' ? -1 : 0))));
			}

			return (long)value;
		}

		private string GetSnafu(long value, char[] baseChars)
		{
			string result = string.Empty;

			int targetBase = 5;

			do
			{
				result = baseChars[value % targetBase] + result;
				value /= targetBase;
			}
			while (value > 0);

			int deduction = 0;
			string trueResult = string.Empty;
			int index = result.Length - 1;
			while (index > -1)
			{
				int val = int.Parse(result[index].ToString()) + deduction;
				deduction = 0;
				
				if (val == 5)
				{
					deduction = 1;
					trueResult = "0" + trueResult;
				}
				else if (val == 4)
				{
					deduction = 1;
					trueResult = "-" + trueResult;
				}
				else if (val == 3)
				{
					deduction = 1;
					trueResult = "=" + trueResult;
				}
				else
				{
					trueResult = val.ToString() + trueResult;
				}

				index--;
			}

			return trueResult;
		}

		protected override object ResolveSecondPart(string[] input)
		{
			return 0;
		}
	}
}
