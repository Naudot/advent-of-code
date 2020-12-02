using System;
using System.IO;

namespace AdventOfCode.Year2017
{
	public class DayOne : Day2017
	{
		protected override object ResolveFirstPart()
		{
			char[] digits = File.ReadAllText(GetResourcesPath()).ToCharArray();

			int firstDigit = Convert.ToInt32(digits[0]) - 48;
			int secondDigit = Convert.ToInt32(digits[1]) - 48;
			int result = firstDigit;

			for (int i = 2; i < digits.Length; i++)
			{
				if (firstDigit == secondDigit)
				{
					result += firstDigit;
				}
				firstDigit = secondDigit;
				secondDigit = Convert.ToInt32(digits[i]) - 48;
			}

			return result;
		}

		protected override object ResolveSecondPart()
		{
			char[] digits = File.ReadAllText(GetResourcesPath()).ToCharArray();

			int result = 0;

			for (int i = 0; i < digits.Length; i++)
			{
				int firstDigit = Convert.ToInt32(digits[i]) - 48;
				int secondDigit = Convert.ToInt32(digits[(i + digits.Length / 2) % digits.Length]) - 48;
				if (firstDigit == secondDigit)
				{
					result += firstDigit;
				}
			}

			return result;
		}
	}
}
