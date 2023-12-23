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

		private int[] typeValues = new int[]
		{
			(int)Math.Pow(14, 5) * 6, // Five of a kind
			(int)Math.Pow(14, 5) * 5, // Four of a kind
			(int)Math.Pow(14, 5) * 4, // Full house
			(int)Math.Pow(14, 5) * 3, // Three of a kind
			(int)Math.Pow(14, 5) * 2, // Two pairs
			(int)Math.Pow(14, 5) * 1, // One paire
			(int)Math.Pow(14, 5) * 0 // High card
		};

		protected override object ResolveFirstPart(string[] input)
		{
			return ResolveDay(input, firstPartCards, false);
		}

		protected override object ResolveSecondPart(string[] input)
		{
			return ResolveDay(input, secondPartCards, true);
		}

		private object ResolveDay(string[] input, char[] cardReferences, bool useJocker)
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

		public int GetValueOfType(string cardsDeck, bool useJocker)
		{
			int jockerCount = 0;
			Dictionary<char, int> charCount = new Dictionary<char, int>();

			for (int i = 0; i < cardsDeck.Length; i++)
			{
				if (useJocker && cardsDeck[i] == 'J')
				{
					jockerCount++;
					continue;
				}

				if (charCount.ContainsKey(cardsDeck[i]))
					charCount[cardsDeck[i]]++;
				else
					charCount.Add(cardsDeck[i], 1);
			}

			if (useJocker && jockerCount != 0)
			{
				if (jockerCount == 5 || jockerCount == 4)
				{
					return typeValues[0];
				}

				if (jockerCount == 3)
				{
					if (charCount.Where(pair => pair.Value == 2).Any())
					{
						return typeValues[0];
					}
					return typeValues[1];
				}

				if (jockerCount == 2)
				{
					if (charCount.Where(pair => pair.Value == 3).Any())
					{
						return typeValues[0];
					}
					if (charCount.Where(pair => pair.Value == 2).Any())
					{
						return typeValues[1];
					}

					return typeValues[3];
				}

				if (jockerCount == 1)
				{
					if (charCount.Where(pair => pair.Value == 4).Any())
					{
						return typeValues[0];
					}
					if (charCount.Where(pair => pair.Value == 3).Any())
					{
						return typeValues[1];
					}
					if (charCount.Where(pair => pair.Value == 2).Count() == 2)
					{
						return typeValues[2];
					}
					if (charCount.Where(pair => pair.Value == 2).Count() == 1)
					{
						return typeValues[3];
					}

					return typeValues[5];
				}
			}

			if (charCount.Where(pair => pair.Value == 5).Any())
			{
				return typeValues[0];
			}
			if (charCount.Where(pair => pair.Value == 4).Any())
			{
				return typeValues[1];
			}
			if (charCount.Where(pair => pair.Value == 3).Count() == 1 && charCount.Where(pair => pair.Value == 2).Any())
			{
				return typeValues[2];
			}
			if (charCount.Where(pair => pair.Value == 3).Count() == 1 && !charCount.Where(pair => pair.Value == 2).Any())
			{
				return typeValues[3];
			}
			if (charCount.Where(pair => pair.Value == 2).Count() == 2)
			{
				return typeValues[4];
			}
			if (charCount.Where(pair => pair.Value == 2).Count() == 1)
			{
				return typeValues[5];
			}

			return typeValues[6];
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
