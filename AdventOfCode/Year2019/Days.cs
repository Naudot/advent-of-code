using System;
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

	public class DayTwo : Day2019
	{
		protected override object ResolveFirstPart()
		{
			int[] input = File.ReadAllText(GetResourcesPath()).Split(',').Select(int.Parse).ToArray();
			return Calculate(input, 12, 2);
		}

		protected override object ResolveSecondPart()
		{
			int[] input = File.ReadAllText(GetResourcesPath()).Split(',').Select(int.Parse).ToArray();
			int[] temp = new int[input.Length];

			int wantedValue = 19690720;
			int currentNounVerb = 0;

			for (int i = 0; i < 10000; i++)
			{
				Array.Copy(input, temp, input.Length); // Reinit the input array
				if (Calculate(temp, currentNounVerb / 100, currentNounVerb % 100) == wantedValue)
				{
					return currentNounVerb;
				}
				currentNounVerb++;
			}

			return 0;
		}

		private int Calculate(int[] input, int noun, int verb)
		{
			input[1] = noun;
			input[2] = verb;

			int i = 0;
			do
			{
				int firstIndex = input[i + 1];
				int secondIndex = input[i + 2];
				int thirdIndex = input[i + 3];

				if (input[i] == 1)
				{
					input[thirdIndex] = input[firstIndex] + input[secondIndex];
				}
				else if (input[i] == 2)
				{
					input[thirdIndex] = input[firstIndex] * input[secondIndex];
				}
				else if (input[i] == 99)
				{
					break;
				}

				i += 4;
			} while (i < input.Length - 1); // Remove backspace, please put an input with a backspace at the end :x

			return input[0];
		}
	}
}
