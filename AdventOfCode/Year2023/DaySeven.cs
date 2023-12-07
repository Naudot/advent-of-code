using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Year2023
{
	public class DaySeven : Day2023
	{
		private char[] cards = new[]
		{
			'2',
			'3',
			'4',
			'5',
			'6',
			'7',
			'8',
			'9',
			'T',
			'J',
			'Q',
			'K',
			'A'
		};

		private int[] values = new int[]
		{
			537824 * 7, // Five of a kind
			537824 * 6, // Four of a kind
			537824 * 5, // Full house
			537824 * 4, // Three of a kind
			537824 * 3, // Two pairs
			537824 * 2, // One paire
			0 // High card
		};

		protected override object ResolveFirstPart(string[] input)
		{
			List<(int, int, string)> valuesOfDecks = new List<(int, int, string)>();

			for (int i = 0; i < input.Length; i++)
			{
				string cardsDeck = input[i].Split(' ')[0];
				int value = int.Parse(input[i].Split(' ')[1]);

				int cardsTypeValue = GetValueOfType(cardsDeck);
				int cardsValue = GetValueOfCards(cardsDeck);

				valuesOfDecks.Add((cardsTypeValue + cardsValue, value, cardsDeck));
			}

			valuesOfDecks = valuesOfDecks.OrderBy(item => item.Item1).ToList();

			ulong finalValue = 0;

			for (int i = 0; i < valuesOfDecks.Count; i++)
			{
				finalValue += (ulong)valuesOfDecks[i].Item2 * (ulong)(i + 1);
			}

			return finalValue;
		}

		protected override object ResolveSecondPart(string[] input)
		{
			return string.Empty;
		}

		public int GetValueOfType(string cardsDeck)
		{
			Dictionary<char, int> charCount = new Dictionary<char, int>();

			for (int i = 0; i < cardsDeck.Length; i++)
			{
				if (charCount.ContainsKey(cardsDeck[i]))
					charCount[cardsDeck[i]]++;
				else
					charCount.Add(cardsDeck[i], 1);
			}

			if (charCount.Where(pair => pair.Value == 5).Any())
			{
				return values[0];
			}
			if (charCount.Where(pair => pair.Value == 4).Any())
			{
				return values[1];
			}
			if (charCount.Where(pair => pair.Value == 3).Any() && charCount.Where(pair => pair.Value == 2).Any())
			{
				return values[2];
			}
			if (charCount.Where(pair => pair.Value == 3).Any() && !charCount.Where(pair => pair.Value == 2).Any())
			{
				return values[3];
			}
			if (charCount.Where(pair => pair.Value == 2).Count() == 2)
			{
				return values[4];
			}
			if (charCount.Where(pair => pair.Value == 2).Count() == 1)
			{
				return values[5];
			}
			return values[6];
		}

		public int GetValueOfCards(string cardsDeck)
		{
			int value = 0;

			for (int i = 0; i < cardsDeck.Length; i++)
			{
				int pow = (int)Math.Pow(14, cardsDeck.Length - 1 - i);
				value += Array.IndexOf(cards, cardsDeck[i]) * pow;
			}

			return value;
		}
	}
}
