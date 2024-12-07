namespace AdventOfCode.Year2024
{
	public class DaySeven : Day2024
	{
		private enum Ope
		{
			MULT,
			ADD
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
				isReaching |= IsReaching(sum, operators, operationsRight, 0);
				Ope[] operationsLeft = new Ope[operators.Length - 1];
				operationsLeft[0] = Ope.ADD;
				isReaching |= IsReaching(sum, operators, operationsLeft, 0);

				result += isReaching ? sum : 0;

				Console.WriteLine($"Found {isReaching} for {sum} at {i}");
			}

			return result;
		}

		private bool IsReaching(double sum, double[] operators, Ope[] operations, int depthIndex)
		{
			Log(operations, depthIndex);

			double result = operators[0];

			for (int i = 0; i < operations.Length; i++)
			{
				Ope ope = operations[i];

				if (ope == Ope.MULT)
					result *= operators[i + 1];
				if (ope == Ope.ADD)
					result += operators[i + 1];
				if (result == sum)
					return true;

				// Trouver d'autres cas d'arrêts ?
				if (result > sum)
					break;
			}

			int newDepth = depthIndex + 1;
			if (newDepth >= operations.Length)
				return false;

			// We have two Ops
			for (int i = 0; i < 2; i++)
			{
				Ope[] newOperations = new Ope[operations.Length];
				Array.Copy(operations, newOperations, operations.Length);
				newOperations[newDepth] = (Ope)i;

				if (IsReaching(sum, operators, newOperations, newDepth))
					return true;
			}

			return false;
		}

		protected override object ResolveSecondPart(string[] input)
		{
			return 0;
		}

		private void Log(Ope[] operations, int depthIndex)
		{
			string log = string.Empty;
			for (int i = 0; i < operations.Length; i++)
			{
				log += operations[i] == Ope.MULT ? "*" : "+";
				log += " ";
			}

			Console.WriteLine($"Processing {log} at DI {depthIndex}");
		}
	}
}
