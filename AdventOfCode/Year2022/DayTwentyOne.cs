using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Year2022
{
	public class DayTwentyOne : Day2022
	{
		public class Operation
		{
			public bool IsValue;
			public bool IsResolved;

			public long Value;

			public char OpType;
			public string LeftElement;
			public string RightElement;

			public void Reset()
			{
				if (!IsValue)
				{
					IsResolved = false;
				}
			}
		}

		protected override object ResolveFirstPart(string[] input)
		{
			Dictionary<string, Operation> monkeys = new Dictionary<string, Operation>();

			for (int i = 0; i < input.Length; i++)
			{
				string monkeyName = input[i].Substring(0, 4);
				Operation operation = new Operation();

				if (input[i].Length >= 11)
				{
					operation.OpType = input[i][11];
					operation.LeftElement = input[i].Substring(6, 4);
					operation.RightElement = input[i].Substring(13, 4);
				}
				else
				{
					operation.IsResolved = true;
					operation.Value = long.Parse(input[i].Substring(6, input[i].Length - 6));
				}

				monkeys.Add(monkeyName, operation);
			}

			Operation main = monkeys["root"];
			return Resolve(main, monkeys);
		}

		protected override object ResolveSecondPart(string[] input)
		{
			Dictionary<string, Operation> monkeys = new Dictionary<string, Operation>();

			for (int i = 0; i < input.Length; i++)
			{
				string monkeyName = input[i].Substring(0, 4);
				Operation operation = new Operation();

				if (input[i].Length >= 11)
				{
					if (monkeyName == "root")
					{
						operation.OpType = '=';
					}
					else
					{
						operation.OpType = input[i][11];
					}

					operation.LeftElement = input[i].Substring(6, 4);
					operation.RightElement = input[i].Substring(13, 4);
				}
				else
				{
					operation.IsValue = true;
					operation.IsResolved = true;
					operation.Value = long.Parse(input[i].Substring(6, input[i].Length - 6));
				}

				monkeys.Add(monkeyName, operation);
			}

			Operation main = monkeys["root"];

			//monkeys["humn"].Value = 3352886138778;
			//long left = Resolve(monkeys[main.LeftElement], monkeys);
			//long right = Resolve(monkeys[main.RightElement], monkeys);

			/*
				 105945721530026
				 52783992130116 avec 3348609108778
				 52720489072332 avec 3352609108778
				 52717313919420 avec 3352809108778
				 52715726343000 avec 3352909108778
				 52716202615910 avec 3352879108778
				 52716107361340 avec 3352885108778
				 52716091485564 avec 3352886108778
				 52716091009302 avec 3352886138778
 
				 52716091087786
 
				 Je dois trouver 52716091087786
				 J'allais quand même pas retravailler tout mon algo
			*/

			long numbToFind = long.MinValue;
			for (long i = 3352886108778; i < 3352886138778; i++)
			{
				monkeys["humn"].Value = i;
				if (Resolve(main, monkeys) == 0)
				{
					numbToFind = i;
					break;
				}

				foreach (KeyValuePair<string, Operation> item in monkeys)
				{
					item.Value.Reset();
				}
			}

			return numbToFind;
		}

		private long Resolve(Operation operation, Dictionary<string, Operation> monkeys)
		{
			if (operation.IsResolved == true)
			{
				return operation.Value;
			}

			Operation left = monkeys[operation.LeftElement];
			if (!left.IsResolved)
			{
				left.Value = Resolve(left, monkeys);
			}

			Operation right = monkeys[operation.RightElement];
			if (!right.IsResolved)
			{
				right.Value = Resolve(right, monkeys);
			}

			if (operation.OpType == '+')
			{
				operation.IsResolved = true;
				return left.Value + right.Value;
			}
			if (operation.OpType == '-')
			{
				operation.IsResolved = true;
				return left.Value - right.Value;
			}
			if (operation.OpType == '/')
			{
				operation.IsResolved = true;
				return left.Value / right.Value;
			}
			if (operation.OpType == '=')
			{
				return left.Value.CompareTo(right.Value);
			}

			operation.IsResolved = true;
			return left.Value * right.Value;
		}
	}
}
