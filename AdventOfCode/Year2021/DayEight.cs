using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Year2021
{
	public class DayEight : Day2021
	{
		// 10 first entries are 10 unique numbers, 4 last entries are just what is displayed

		public class WireLinkedToGoodOutput // 'bad' -> 'good possibilities'
		{
			public List<char> Possibilities = new List<char>() { 'a', 'b', 'c', 'd', 'e', 'f', 'g' };
			public bool IsDecoded = false;
		}

		public class WireLinkedToBadOutput // 'good' -> 'bad possibilities'
		{
			public List<char> Possibilities = new List<char>();
			public bool IsDecoded = false;
		}

		public class Entry
		{
			public List<int> NumberPossibility = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
		}

		protected override object ResolveFirstPart(string[] input)
		{
			int simpleNumbersCount = 0;
			Dictionary<int, List<char>> numbers = GetNumbers();

			for (int i = 0; i < input.Length; i++)
			{
				string[] line =  input[i].Split('|');
				string[] displayed = line[1].Split(' ');

				for (int j = 0; j < displayed.Length; j++)
				{
					if (displayed[j].Length == numbers[1].Count 
						|| displayed[j].Length == numbers[4].Count 
						|| displayed[j].Length == numbers[7].Count 
						|| displayed[j].Length == numbers[8].Count)
					{
						simpleNumbersCount++;
					}
				}
			}

			return simpleNumbersCount;
		}

		protected override object ResolveSecondPart(string[] input)
		{
			int accumulator = 0;
			Dictionary<int, List<char>> numbers = GetNumbers();
			Dictionary<char, WireLinkedToGoodOutput> linkedToGood = GetWireOutputs();
			Dictionary<char, WireLinkedToBadOutput> linkedToBad = GetLinkedToBadWireOutputs();
			for (int i = 0; i < input.Length; i++)
			{
				int lineResult = 0;
				string[] line =  input[i].Split('|');
				string[] rawEntries = line[0].TrimEnd(' ').Split(' ');
				string[] displayed = line[1].Split(' ');

				// Process unique numbers
				for (int j = 0; j < rawEntries.Length; j++)
				{
					/*
					 * E.g. : 1 = 'cf'
					 * we found 'de'
					 * d is either c or f
					 * e is either c or f
					 * or
					 * c is either d or e
					 * f is either d or e
					 */

					string entry = rawEntries[j];
					Test(entry, numbers[1], linkedToGood, linkedToBad);
					Test(entry, numbers[4], linkedToGood, linkedToBad);
					Test(entry, numbers[7], linkedToGood, linkedToBad);
					Test(entry, numbers[8], linkedToGood, linkedToBad);
				}

				Dictionary<string, Entry> entries = GetEntries(rawEntries);


				// Si seulement deux char peuvent être c ou f alors on retire c et f de partout
				// Si seulement X char peuvent être Y char alors on retire ces Y char de partout

				// TODO : While tout n'est pas décodé
				//while (!AreWiresDecoded(linkedToGood))
				//{
					List<WireLinkedToGoodOutput> test = linkedToGood.Values.Where(item => item.Possibilities.Count == 2).ToList();
					if (test.Count == 2)
					{
						for (int j = 0; j < test.Count; j++)
						{
							foreach (KeyValuePair<char, WireLinkedToGoodOutput> item in linkedToGood)
							{
								if (!test.Contains(item.Value))
								{
									for (int k = 0; k < test[j].Possibilities.Count; k++)
									{
										item.Value.Possibilities.Remove(test[j].Possibilities[k]);
									}
								}
							}
						}
					}

					List<WireLinkedToGoodOutput> test1 = linkedToGood.Values.Where(item => item.Possibilities.Count == 1).ToList();
					if (test1.Count == 1)
					{
						for (int j = 0; j < test1.Count; j++)
						{
							foreach (KeyValuePair<char, WireLinkedToGoodOutput> item in linkedToGood)
							{
								if (!test1.Contains(item.Value))
								{
									for (int k = 0; k < test1[j].Possibilities.Count; k++)
									{
										item.Value.Possibilities.Remove(test1[j].Possibilities[k]);
									}
								}
							}
						}
					}

					List<WireLinkedToGoodOutput> test3 = linkedToGood.Values.Except(test).Where(item => item.Possibilities.Count == 2).ToList();
					if (test3.Count == 2)
					{
						for (int j = 0; j < test3.Count; j++)
						{
							foreach (KeyValuePair<char, WireLinkedToGoodOutput> item in linkedToGood)
							{
								if (!test3.Contains(item.Value))
								{
									for (int k = 0; k < test3[j].Possibilities.Count; k++)
									{
										item.Value.Possibilities.Remove(test3[j].Possibilities[k]);
									}
								}
							}
						}
					}

				// Si j'arrive à trouver 6 je peux en déduire si le a (mauvais) est le c (bon) ou le f (bon) car 6 exclu un des deux
				// avec le 7 on a 
				// le 'd' mauvais lié au 'a' bon
				// avec ce 'a' bon on peut exclure de toutes les entries les nombres qui possèdent un 'a' bon mais que l'entry no possède pas un 'd' mauvais

				var seven = linkedToGood.FirstOrDefault(item => item.Value.Possibilities.Count == 1);
				char aBadChar = seven.Key;
				char aGoodChar = seven.Value.Possibilities[0];
				foreach (var item in entries)
				{
					foreach (var itemNumber in numbers.Where(num => num.Value.Contains(aGoodChar)))
					{
						if (!item.Key.Contains(aBadChar))
						{
							item.Value.NumberPossibility.Remove(itemNumber.Key);
						}
					}
				}

				foreach (KeyValuePair<char, WireLinkedToGoodOutput> linkedToGo in linkedToGood)
					{
						WireLinkedToGoodOutput wire = linkedToGo.Value;
						foreach (KeyValuePair<string, Entry> entry in entries)
						{
							// If our entry has the characters
							bool entryHasEveryCharacter = true;
							for (int j = 0; j < wire.Possibilities.Count; j++)
							{
								if (!entry.Key.Contains(wire.Possibilities[j]))
								{
									entryHasEveryCharacter = false;
									break;
								}
							}

							if (entryHasEveryCharacter)
							{
								foreach (KeyValuePair<int, List<char>> number in numbers)
								{
									// If our entry has the characters
									bool numberHasEveryCharacter = true;
									for (int j = 0; j < wire.Possibilities.Count; j++)
									{
										if (!number.Value.Contains(wire.Possibilities[j]))
										{
											numberHasEveryCharacter = false;
											break;
										}
									}

									if (numberHasEveryCharacter && !entry.Value.NumberPossibility.Contains(number.Key) && entry.Key.Length == number.Value.Count)
									{
										entry.Value.NumberPossibility.Add(number.Key);
									}
								}
							}
						}
					}
				//}

				// TODO calcule des displayed
				for (int j = 0; j < displayed.Length; j++)
				{
				}

				accumulator += lineResult;
			}

			return accumulator;
		}

		public Dictionary<int, List<char>> GetNumbers()
		{
			Dictionary<int, List<char>> numbers = new Dictionary<int, List<char>>();
			numbers[0] = new List<char>() { 'a', 'b', 'c', 'e', 'f', 'g' };
			numbers[1] = new List<char>() { 'c', 'f' };
			numbers[2] = new List<char>() { 'a', 'c', 'd', 'e', 'g' };
			numbers[3] = new List<char>() { 'a', 'c', 'd', 'f', 'g' };
			numbers[4] = new List<char>() { 'b', 'c', 'd', 'f' };
			numbers[5] = new List<char>() { 'a', 'b', 'd', 'f', 'g' };
			numbers[6] = new List<char>() { 'a', 'b', 'd', 'e', 'f', 'g' };
			numbers[7] = new List<char>() { 'a', 'c', 'f' };
			numbers[8] = new List<char>() { 'a', 'b', 'c', 'd', 'e', 'f', 'g' };
			numbers[9] = new List<char>() { 'a', 'b', 'c', 'd', 'f', 'g' };
			return numbers;
		}

		public Dictionary<char, WireLinkedToGoodOutput> GetWireOutputs()
		{
			Dictionary<char, WireLinkedToGoodOutput> wires = new Dictionary<char, WireLinkedToGoodOutput>();
			wires['a'] = new WireLinkedToGoodOutput();
			wires['b'] = new WireLinkedToGoodOutput();
			wires['c'] = new WireLinkedToGoodOutput();
			wires['d'] = new WireLinkedToGoodOutput();
			wires['e'] = new WireLinkedToGoodOutput();
			wires['f'] = new WireLinkedToGoodOutput();
			wires['g'] = new WireLinkedToGoodOutput();
			return wires;
		}

		public Dictionary<char, WireLinkedToBadOutput> GetLinkedToBadWireOutputs()
		{
			Dictionary<char, WireLinkedToBadOutput> wires = new Dictionary<char, WireLinkedToBadOutput>();
			wires['a'] = new WireLinkedToBadOutput();
			wires['b'] = new WireLinkedToBadOutput();
			wires['c'] = new WireLinkedToBadOutput();
			wires['d'] = new WireLinkedToBadOutput();
			wires['e'] = new WireLinkedToBadOutput();
			wires['f'] = new WireLinkedToBadOutput();
			wires['g'] = new WireLinkedToBadOutput();
			return wires;
		}

		public Dictionary<string, Entry> GetEntries(string[] rawEntries)
		{
			Dictionary<string, Entry> entries = new Dictionary<string, Entry>();
			for (int i = 0; i < rawEntries.Length; i++)
			{
				entries[rawEntries[i]] = new Entry();
			}
			return entries;
		}

		public bool AreWiresDecoded(Dictionary<char, WireLinkedToGoodOutput> wires)
		{
			foreach (KeyValuePair<char, WireLinkedToGoodOutput> wire in wires)
			{
				if (!wire.Value.IsDecoded)
				{
					return false;
				}
			}

			return true;
		}

		public void Test(string entry, List<char> number, Dictionary<char, WireLinkedToGoodOutput> linkedToGood, Dictionary<char, WireLinkedToBadOutput> linkedToBad)
		{
			if (entry.Length == number.Count)
			{
				for (int k = 0; k < entry.Length; k++)
				{
					WireLinkedToGoodOutput goodToBad = linkedToGood[entry[k]];
					// d -> c or f
					goodToBad.Possibilities = goodToBad.Possibilities.Except(goodToBad.Possibilities.Except(number).ToList()).ToList();
				}

				for (int k = 0; k < number.Count; k++)
				{
					for (int l = 0; l < entry.Length; l++)
					{
						WireLinkedToBadOutput badToGood = linkedToBad[number[k]];
						if (!badToGood.Possibilities.Contains(entry[l]))
						{
							badToGood.Possibilities.Add(entry[l]);
						}
					}
				}
			}
		}
	}
}
