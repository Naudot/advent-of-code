namespace AdventOfCode.Year2024
{
	public class DaySeven : Day2024
	{
		private enum Ope
		{
			MULT,
			ADD,
			CONCAT
		}

		protected override bool DeactivateJIT
		{
			get
			{
				return true;
			}
		}

		protected override object ResolveFirstPart(string[] input)
		{
			return GetResult(input, false);
		}

		protected override object ResolveSecondPart(string[] input)
		{
			return GetResult(input, true);
		}

		private double GetResult(string[] input, bool isSecondPart)
		{
			double result = 0;

			for (int i = 0; i < input.Length; i++)
			{
				bool isReaching = false;
				string line = input[i];
				string[] values = line.Split(':');
				double sum = double.Parse(values[0]);
				double[] operators = values[1].Split(' ').Where(val => val != string.Empty).Select(val => double.Parse(val)).ToArray();

				Ope[] operationsMult = new Ope[operators.Length - 1];
				isReaching |= IsReaching(sum, operators, operationsMult, 0, isSecondPart);
				Ope[] operationsAdd = new Ope[operators.Length - 1];
				operationsAdd[0] = Ope.ADD;
				isReaching |= IsReaching(sum, operators, operationsAdd, 0, isSecondPart);

				if (isSecondPart)
				{
					Ope[] operationsConcat = new Ope[operators.Length - 1];
					operationsConcat[0] = Ope.CONCAT;
					isReaching |= IsReaching(sum, operators, operationsConcat, 0, isSecondPart);
				}

				result += isReaching ? sum : 0;
			}

			return result;
		}

		private bool IsReaching(double sum, double[] operators, Ope[] operations, int depthIndex, bool secondPart)
		{
			double result = operators[0];

			for (int i = 0; i < operations.Length; i++)
			{
				Ope ope = operations[i];

				if (ope == Ope.MULT)
					result *= operators[i + 1];
				if (ope == Ope.ADD)
					result += operators[i + 1];
				if (ope == Ope.CONCAT && secondPart)
				{
					int digit = CountDigits(operators[i + 1]);
					result *= Math.Pow(10, digit);
					result += operators[i + 1];
				}
				if (result > sum)
					break;

				if (!secondPart && result == sum)
					return true;
			}

			// Wait for the end of calculations when using concatenation
			if (secondPart && result == sum)
				return true;

			int newDepth = depthIndex + 1;
			if (newDepth >= operations.Length)
				return false;

			// We have two Ops
			for (int i = 0; i < (secondPart ? 3 : 2); i++)
			{
				Ope[] newOperations = new Ope[operations.Length];
				Array.Copy(operations, newOperations, operations.Length);
				newOperations[newDepth] = (Ope)i;

				if (IsReaching(sum, operators, newOperations, newDepth, secondPart))
					return true;
			}

			return false;
		}

		private static int CountDigits(double num)
		{
			int digits = 0;
			while (num >= 1)
			{
				digits++;
				num /= 10;
			}
			return digits;
		}
	}
}
