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
				string[] values = input[i].Split(':');
				double sum = double.Parse(values[0]);
				double[] operators = values[1].Split(' ').Where(val => val != string.Empty).Select(val => double.Parse(val)).ToArray();
				Ope[] operations = new Ope[operators.Length - 1];
				result += IsReaching(sum, operators, operations, 0, isSecondPart) ? sum : 0;
			}

			return result;
		}

		private bool IsReaching(double sum, double[] operators, Ope[] operations, int depthIndex, bool secondPart)
		{
			if (depthIndex >= operations.Length)
				return false;

			for (int i = 0; i < (secondPart ? 3 : 2); i++)
			{
				double result = operators[0];

				Ope[] newOperations = new Ope[operations.Length];
				Array.Copy(operations, newOperations, operations.Length);
				newOperations[depthIndex] = (Ope)i;

				for (int j = 0; j < newOperations.Length; j++)
				{
					Ope ope = newOperations[j];

					if (ope == Ope.MULT)
						result *= operators[j + 1];
					if (ope == Ope.ADD)
						result += operators[j + 1];
					if (secondPart && ope == Ope.CONCAT)
					{
						int digit = CountDigits(operators[j + 1]);
						result *= Math.Pow(10, digit);
						result += operators[j + 1];
					}
					if (result > sum)
						break;
				}

				if (result == sum || IsReaching(sum, operators, newOperations, depthIndex + 1, secondPart))
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
