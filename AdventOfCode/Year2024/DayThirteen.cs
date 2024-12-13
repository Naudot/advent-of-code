using System.Text.RegularExpressions;

namespace AdventOfCode.Year2024
{
	public class DayThirteen : Day2024
	{
		protected override object ResolveFirstPart(string[] input)
		{
			return GetSum(input, false);
		}

		protected override object ResolveSecondPart(string[] input)
		{
			return GetSum(input, true);
		}

		private double GetSum(string[] input, bool isSecondPart)
		{
			double sum = 0;

			for (int i = 0; i < input.Length; i += 4)
			{
				if (i + 2 >= input.Length)
					break;

				Match matchA = Regex.Match(input[i], @"Button A: X\+(\d+), Y\+(\d+)");
				Match matchB = Regex.Match(input[i + 1], @"Button B: X\+(\d+), Y\+(\d+)");
				Match matchPrize = Regex.Match(input[i + 2], @"Prize: X=(\d+), Y=(\d+)");

				(double x, double y) a = (double.Parse(matchA.Groups[1].Value), double.Parse(matchA.Groups[2].Value));
				(double x, double y) b = (double.Parse(matchB.Groups[1].Value), double.Parse(matchB.Groups[2].Value));
				(double x, double y) prize = (double.Parse(matchPrize.Groups[1].Value), double.Parse(matchPrize.Groups[2].Value));

				sum += GetPerfectInputValue(a, b, prize, isSecondPart);
			}

			return sum;
		}

		private double GetPerfectInputValue((double x, double y) a, (double x, double y) b, (double x, double y) prize, bool isSecondPart)
		{
			/*
				Second part equation solving

				Button A: X+94, Y+34
				Button B: X+22, Y+67
				Prize: X=8400, Y=5400

				8400 = 94x + 22y avec x le nombre de push de A et y le nombre de push de B
				5400 = 34x + 67y
			 
				y = (8400 - 94x) / 22

				5400 = 34x + 67/22*(8400 - 94x)
				5400 = 34x + 67/22*8400 - 67/22*94x
				-67/22*8400 + 5400 = x * (34 - 67/22*94) 
				-67/22*8400 + 5400 / (34 - 64/22*94) = x
			 */

			if (isSecondPart)
				prize = (prize.x + 10000000000000, prize.y + 10000000000000);

			double pushACount = ((-1 * (b.y / b.x) * prize.x) + prize.y) / (a.y - (b.y / b.x * a.x));
			double pushBCount = (prize.x - a.x * pushACount) / b.x;

			double roundedA = Math.Round(pushACount);
			double roundedB = Math.Round(pushBCount);

			if (prize.x != (roundedA * a.x + roundedB * b.x) || prize.y != (roundedA * a.y + roundedB * b.y))
				return 0;
			
			return pushACount * 3 + pushBCount;
		}
	}
}
