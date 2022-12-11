using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Year2022
{
	public class Item
	{
		public long WorryValue;
	}

	public class Monkey
	{
		public int ID;
		public List<Item> Items = new List<Item>();
		public bool IsOperationMult;
		public bool IsOperationSelfMult;
		public int OperationValue;
		public int TestValue;
		public int TrueMonkeyID;
		public int FalseMonkeyID;
		public long InspectedItems;
	}

	public class DayEleven : Day2022
	{
		protected override object ResolveFirstPart(string[] input)
		{
			return GetValue(input, false);
		}

		protected override object ResolveSecondPart(string[] input)
		{
			return GetValue(input, true);
		}

		public long GetValue(string[] input, bool isSecondPart)
		{
			List<Monkey> monkeys = new List<Monkey>();
			int monkeyNumber = 8;
			int monkeyInfo = 7;
			int commonWorryValue = 1;

			for (int i = 0; i < monkeyNumber; i++)
			{
				Monkey monkey = new Monkey();
				monkey.ID = i;

				for (int j = 0; j < monkeyInfo; j++)
				{
					int lineIndex = i * monkeyInfo + j;
					if (lineIndex >= input.Length)
					{
						continue;
					}

					string line = input[lineIndex];

					if (j == 1)
					{
						line = line.Replace("Starting items: ", string.Empty).Trim();
						monkey.Items = line.Split(',').Select(number => new Item() { WorryValue = int.Parse(number) }).ToList();
					}
					if (j == 2)
					{
						if (line.Contains("old * old"))
						{
							monkey.IsOperationSelfMult = true;
						}
						else
						{
							monkey.IsOperationSelfMult = false;
							monkey.IsOperationMult = line.Contains("*");
							string toParse = line.Replace("Operation: new = old ", string.Empty).Replace("*", string.Empty).Replace("+", string.Empty).Trim();
							monkey.OperationValue = int.Parse(toParse);
						}
					}
					if (j == 3)
					{
						monkey.TestValue = int.Parse(line.Replace("Test: divisible by ", string.Empty).Trim());
						if (isSecondPart)
						{
							commonWorryValue *= monkey.TestValue;
						}
					}
					if (j == 4)
					{
						monkey.TrueMonkeyID = int.Parse(line.Replace("If true: throw to monkey ", string.Empty).Trim());
					}
					if (j == 5)
					{
						monkey.FalseMonkeyID = int.Parse(line.Replace("If false: throw to monkey ", string.Empty).Trim());
					}
				}

				monkeys.Add(monkey);
			}

			int turnCount = isSecondPart ? 10000 : 20;

			for (int i = 0; i < turnCount; i++)
			{
				for (int j = 0; j < monkeys.Count; j++)
				{
					Monkey monkey = monkeys[j];

					for (int k = 0; k < monkey.Items.Count; k++)
					{
						monkey.InspectedItems++;

						Item item = monkey.Items[k];

						if (monkey.IsOperationSelfMult)
						{
							item.WorryValue = item.WorryValue * item.WorryValue;
						}
						else if (monkey.IsOperationMult)
						{
							item.WorryValue = item.WorryValue * monkey.OperationValue;
						}
						else
						{
							item.WorryValue = item.WorryValue + monkey.OperationValue;
						}

						if (!isSecondPart)
						{
							item.WorryValue /= 3;
						}
						else
						{
							item.WorryValue %= commonWorryValue;
						}

						if (item.WorryValue % monkey.TestValue == 0)
						{
							monkeys.FirstOrDefault(newMonkey => newMonkey.ID == monkey.TrueMonkeyID).Items.Add(item);
						}
						else
						{
							monkeys.FirstOrDefault(newMonkey => newMonkey.ID == monkey.FalseMonkeyID).Items.Add(item);
						}
					}

					monkey.Items.Clear();
				}
			}

			monkeys = monkeys.OrderByDescending(monkey => monkey.InspectedItems).ToList();

			return monkeys[0].InspectedItems * monkeys[1].InspectedItems;
		}
	}
}
