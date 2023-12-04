using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;

namespace AdventOfCode.Year2023
{
	public class DayFour : Day2023
	{
		protected override object ResolveFirstPart(string[] input)
		{
			double sum = 0;

			for (int i = 0; i < input.Length; i++)
			{
				string card = input[i].Split(':')[1];
				MatchCollection numbersIHave = Regex.Matches(card.Split('|')[0], @"(\d+)");
				MatchCollection winningNumbers = Regex.Matches(card.Split('|')[1], @"(\d+)");

				List<int> rightNumbers = new List<int>();
				for (int j = 0; j < winningNumbers.Count; j++)
				{
					rightNumbers.Add(int.Parse(winningNumbers[j].Groups[1].Value));
				}

				int count = 0;
				for (int j = 0; j < numbersIHave.Count; j++)
				{
					int value = int.Parse(numbersIHave[j].Groups[1].Value);
					count += rightNumbers.Where(numb => numb == value).Count();
				}

				if (count > 0)
				{
					sum += Math.Pow(2, count - 1);
				}
			}

			return sum;
		}

		protected override object ResolveSecondPart(string[] input)
		{
			double scratchCardCount = input.Length;
			int[] cardsCount = new int[input.Length];

			for (int i = 0; i < input.Length; i++)
			{
				cardsCount[i] = 1;
			}

			for (int i = 0; i < input.Length; i++)
			{
				Console.WriteLine("Processing card " + (i + 1));

				string card = input[i].Split(':')[1];
				MatchCollection numbersIHave = Regex.Matches(card.Split('|')[0], @"(\d+)");
				MatchCollection winningNumbers = Regex.Matches(card.Split('|')[1], @"(\d+)");

				List<int> rightNumbers = new List<int>();
				for (int j = 0; j < winningNumbers.Count; j++)
				{
					rightNumbers.Add(int.Parse(winningNumbers[j].Groups[1].Value));
				}

				int count = 0;
				for (int j = 0; j < numbersIHave.Count; j++)
				{
					int value = int.Parse(numbersIHave[j].Groups[1].Value);
					count += rightNumbers.Where(numb => numb == value).Count();
				}

				int processCount = cardsCount[i];
				for (int j = 0; j < count; j++)
				{
					cardsCount[i + 1 + j] += processCount;
				}
				scratchCardCount += count * processCount;
			}

			return scratchCardCount;
		}
	}
}
