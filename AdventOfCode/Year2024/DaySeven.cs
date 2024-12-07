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
			double result = 0;

			for (int i = 0; i < input.Length; i++)
			{
				bool isReaching = false;
				string line = input[i];
				string[] values = line.Split(':');
				double sum = double.Parse(values[0]);
				double[] operators = values[1].Split(' ').Where(val => val != string.Empty).Select(val => double.Parse(val)).ToArray();

				Ope[] operationsRight = new Ope[operators.Length - 1];
				isReaching |= IsReaching(sum, operators, operationsRight, 0, false);
				Ope[] operationsLeft = new Ope[operators.Length - 1];
				operationsLeft[0] = Ope.ADD;
				isReaching |= IsReaching(sum, operators, operationsLeft, 0, false);

				result += isReaching ? sum : 0;
			}

			return result;
		}

		protected override object ResolveSecondPart(string[] input)
		{
			double result = 0;

			for (int i = 0; i < input.Length; i++)
			{
				bool isReaching = false;
				string line = input[i];
				string[] values = line.Split(':');
				double sum = double.Parse(values[0]);
				double[] operators = values[1].Split(' ').Where(val => val != string.Empty).Select(val => double.Parse(val)).ToArray();

				Ope[] operationsRight = new Ope[operators.Length - 1];
				isReaching |= IsReaching(sum, operators, operationsRight, 0, true);

				Ope[] operationsMid = new Ope[operators.Length - 1];
				operationsMid[0] = Ope.ADD;
				isReaching |= IsReaching(sum, operators, operationsMid, 0, true);

				Ope[] operationsLeft = new Ope[operators.Length - 1];
				operationsLeft[0] = Ope.CONCAT;
				isReaching |= IsReaching(sum, operators, operationsLeft, 0, true);

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

				if (result == sum && secondPart)
				{
					Log(sum, operators, operations);
					return true;
				}

				if (result > sum)
					break;
			}

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

		public static int CountDigits(double num)
		{
			int digits = 0;
			while (num >= 1)
			{
				digits++;
				num /= 10;
			}
			return digits;
		}

		private static void Log(double sum, double[] operators, Ope[] operations)
		{
			string log = string.Empty;

			for (int i = 0; i < operators.Length; i++)
			{
				log += operators[i].ToString();

				if (i < operations.Length)
				{
					log += " ";
					log += operations[i] == Ope.MULT ? "*" : (operations[i] == Ope.ADD ? "+" : "||");
					log += " ";
				}
			}

			Console.WriteLine($"Processing {sum} \t \t {log}");
		}
	}
}
