using System.Text.RegularExpressions;

namespace AdventOfCode.Year2015
{
	public class DaySix : Day2015
	{
		protected override object ResolveFirstPart(string[] input)
		{
			bool[,] lights = new bool[1000, 1000];

			int result = 0;

			for (int i = 0; i < input.Length; i++)
			{
				MatchCollection matches = Regex.Matches(input[i], @"(\d*),(\d*)");

				bool isToggle = input[i].Contains("toggle");
				bool isOn = input[i].Contains("on");

				int beginX = int.Parse(matches[0].Groups[1].Value);
				int beginY = int.Parse(matches[0].Groups[2].Value);
				int endX = int.Parse(matches[1].Groups[1].Value);
				int endY = int.Parse(matches[1].Groups[2].Value);

				for (int j = 0; j <= endX - beginX; j++)
				{
					for (int k = 0; k <= endY - beginY; k++)
					{
						int xIndex = beginX + j;
						int yIndex = beginY + k;

						bool oldValue = lights[xIndex, yIndex];
						if (isToggle)
						{
							lights[xIndex, yIndex] = !lights[xIndex, yIndex];
						}
						else
						{
							lights[xIndex, yIndex] = isOn;
						}
						bool newValue = lights[xIndex, yIndex];
						if (oldValue != newValue)
						{
							result += newValue ? 1 : -1;
						}
					}
				}
			}

			return result;
		}

		protected override object ResolveSecondPart(string[] input)
		{
			int[,] lights = new int[1000, 1000];

			int result = 0;

			for (int i = 0; i < input.Length; i++)
			{
				MatchCollection matches = Regex.Matches(input[i], @"(\d*),(\d*)");

				bool isToggle = input[i].Contains("toggle");
				bool isOn = input[i].Contains("on");

				int beginX = int.Parse(matches[0].Groups[1].Value);
				int beginY = int.Parse(matches[0].Groups[2].Value);
				int endX = int.Parse(matches[1].Groups[1].Value);
				int endY = int.Parse(matches[1].Groups[2].Value);

				for (int j = 0; j <= endX - beginX; j++)
				{
					for (int k = 0; k <= endY - beginY; k++)
					{
						int xIndex = beginX + j;
						int yIndex = beginY + k;

						if (isToggle)
						{
							lights[xIndex, yIndex] += 2;
							result += 2;
						}
						else if (isOn)
						{
							lights[xIndex, yIndex] += 1;
							result += 1;
						}
						else if (lights[xIndex, yIndex] > 0)
						{
							lights[xIndex, yIndex] -= 1;
							result -= 1;
						}
					}
				}
			}

			return result;
		}
	}
}
