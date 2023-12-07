using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Year2023
{
	public class DaySeven : Day2023
	{
		private char[] firstPartCards = new[]
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
		
		private char[] secondPartCards = new[]
		{
			'J',
			'2',
			'3',
			'4',
			'5',
			'6',
			'7',
			'8',
			'9',
			'T',
			'Q',
			'K',
			'A'
		};

		private int[] values = new int[]
		{
			537824 * 6, // Five of a kind
			537824 * 5, // Four of a kind
			537824 * 4, // Full house
			537824 * 3, // Three of a kind
			537824 * 2, // Two pairs
			537824 * 1, // One paire
			0 // High card
		};

		protected override object ResolveFirstPart(string[] input)
		{
			return ResolveDay(input, false, firstPartCards);
		}

		protected override object ResolveSecondPart(string[] input)
		{
			return ResolveDay(input, true, secondPartCards);
		}

		private object ResolveDay(string[] input, bool useJocker, char[] cardReferences)
		{
			List<(int, int)> valuesOfDecks = new List<(int, int)>();

			for (int i = 0; i < input.Length; i++)
			{
				string cardsDeck = input[i].Split(' ')[0];
				int value = int.Parse(input[i].Split(' ')[1]);
				valuesOfDecks.Add((GetValueOfType(cardsDeck, useJocker) + GetValueOfCards(cardReferences, cardsDeck), value));
			}

			valuesOfDecks = valuesOfDecks.OrderBy(item => item.Item1).ToList();

			ulong finalValue = 0;

			for (int i = 0; i < valuesOfDecks.Count; i++)
			{
				finalValue += (ulong)valuesOfDecks[i].Item2 * (ulong)(i + 1);
			}

			return finalValue;
		}

		// 253840749 too high 
		// 253216559 too low
		// 253296336 too low 
		// 253609148 not right
		public int GetValueOfType(string cardsDeck, bool useJocker)
		{
			int jockerCount = 0;
			Dictionary<char, int> charCount = new Dictionary<char, int>();

			for (int i = 0; i < cardsDeck.Length; i++)
			{
				if (useJocker && cardsDeck[i] == 'J')
				{
					jockerCount++;
				}
				else
				{
					if (charCount.ContainsKey(cardsDeck[i]))
						charCount[cardsDeck[i]]++;
					else
						charCount.Add(cardsDeck[i], 1);
				}
			}

			if (useJocker && jockerCount != 0)
			{
				if (jockerCount == 5 || jockerCount == 4)
				{
					return values[0];
				}

				if (jockerCount == 3)
				{
					if (charCount.Where(pair => pair.Value == 2).Any())
					{
						return values[0];
					}
					return values[1];
				}

				if (jockerCount == 2)
				{
					if (charCount.Where(pair => pair.Value == 3).Any())
					{
						return values[0];
					}
					if (charCount.Where(pair => pair.Value == 2).Any())
					{
						return values[1];
					}

					return values[3];
				}

				if (jockerCount == 1)
				{
					if (charCount.Where(pair => pair.Value == 4).Any())
					{
						return values[0];
					}
					if (charCount.Where(pair => pair.Value == 3).Any())
					{
						return values[1];
					}
					if (charCount.Where(pair => pair.Value == 2).Count() == 2)
					{
						return values[2];
					}
					if (charCount.Where(pair => pair.Value == 2).Count() == 1)
					{
						return values[3];
					}

					return values[5];
				}
			}

			if (charCount.Where(pair => pair.Value == 5).Any())
			{
				return values[0];
			}
			if (charCount.Where(pair => pair.Value == 4).Any())
			{
				return values[1];
			}
			if (charCount.Where(pair => pair.Value == 3).Count() == 1 && charCount.Where(pair => pair.Value == 2).Any())
			{
				return values[2];
			}
			if (charCount.Where(pair => pair.Value == 3).Count() == 1 && !charCount.Where(pair => pair.Value == 2).Any())
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

		public int GetValueOfCards(char[] cardReferences, string cardsDeck)
		{
			int value = 0;

			for (int i = 0; i < cardsDeck.Length; i++)
			{
				int pow = (int)Math.Pow(14, cardsDeck.Length - 1 - i);
				value += Array.IndexOf(cardReferences, cardsDeck[i]) * pow;
			}

			return value;
		}
	}
}
