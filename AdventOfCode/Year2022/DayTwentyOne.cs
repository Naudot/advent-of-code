using System.Collections.Generic;

namespace AdventOfCode.Year2022
{
	public class DayTwentyOne : Day2022
	{
		public class Operation
		{
			public bool IsResolved;

			public long Value;

			public char OpType;
			public string LeftElement;
			public string RightElement;
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
			return string.Empty;
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

			operation.IsResolved = true;
			return left.Value * right.Value;
		}
	}
}
