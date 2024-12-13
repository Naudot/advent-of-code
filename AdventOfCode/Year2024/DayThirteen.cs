using System.Text.RegularExpressions;

namespace AdventOfCode.Year2024
{
	public class DayThirteen : Day2024
	{
		protected override bool DeactivateJIT
		{
			get
			{
				return true;
			}
		}

		protected override object ResolveFirstPart(string[] input)
		{
			long sum = 0;

			for (long i = 0; i < input.Length; i += 4)
			{
				if (i + 2 >= input.Length)
					break;

				Match matchA = Regex.Match(input[i], @"Button A: X\+(\d+), Y\+(\d+)");
				Match matchB = Regex.Match(input[i + 1], @"Button B: X\+(\d+), Y\+(\d+)");
				Match matchPrize = Regex.Match(input[i + 2], @"Prize: X=(\d+), Y=(\d+)");

				(long x, long y) a = (long.Parse(matchA.Groups[1].Value), long.Parse(matchA.Groups[2].Value));
				(long x, long y) b = (long.Parse(matchB.Groups[1].Value), long.Parse(matchB.Groups[2].Value));
				(long x, long y) prize = (long.Parse(matchPrize.Groups[1].Value), long.Parse(matchPrize.Groups[2].Value));

				sum += GetPerfectInputValue(a, b, prize, false);
			}

			return sum;
		}

		protected override object ResolveSecondPart(string[] input)
		{
			long sum = 0;

			for (long i = 0; i < input.Length; i += 4)
			{
				if (i + 2 >= input.Length)
					break;

				Match matchA = Regex.Match(input[i], @"Button A: X\+(\d+), Y\+(\d+)");
				Match matchB = Regex.Match(input[i + 1], @"Button B: X\+(\d+), Y\+(\d+)");
				Match matchPrize = Regex.Match(input[i + 2], @"Prize: X=(\d+), Y=(\d+)");

				(long x, long y) a = (long.Parse(matchA.Groups[1].Value), long.Parse(matchA.Groups[2].Value));
				(long x, long y) b = (long.Parse(matchB.Groups[1].Value), long.Parse(matchB.Groups[2].Value));
				(long x, long y) prize = (long.Parse(matchPrize.Groups[1].Value), long.Parse(matchPrize.Groups[2].Value));

				sum += GetPerfectInputValue(a, b, prize, true);
			}

			return sum;
		}

		private long GetPerfectInputValue((long x, long y) a, (long x, long y) b, (long x, long y) prize, bool isSecondPart)
		{
			//if (isSecondPart)
			//	prize = (prize.x + 10000000000000, prize.y + 10000000000000);

			long xMin = Math.Min(a.x, b.x);
			long xMax = Math.Max(a.x, b.x);
			long xLoopMax = prize.x / xMin;
			long xLoopMin = prize.x / xMax;
			bool xMinIsA = xMin == a.x;

			long yMin = Math.Min(a.y, b.y);
			long yMax = Math.Max(a.y, b.y);
			long yLoopMax = prize.y / yMin;
			long yLoopMin = prize.y / yMax;
			bool yMinIsA = yMin == a.y;

			HashSet<(long pushA, long pushB)> solutions = new();

			for (long pushA = 0; pushA < 100; pushA++)
			{
				for (long pushB = 0; pushB < 100; pushB++)
				{
					long finalX = a.x * pushA + b.x * pushB;
					long finalY = a.y * pushA + b.y * pushB;

					if (prize == (finalX, finalY))
						solutions.Add((pushA, pushB));
				}
			}

			long tokenCount = long.MaxValue;

			foreach ((long pushA, long pushB) solution in solutions)
				tokenCount = Math.Min(tokenCount, solution.pushA * 3 + solution.pushB);

			return tokenCount == long.MaxValue ? 0 : tokenCount;
		}
	}
}
