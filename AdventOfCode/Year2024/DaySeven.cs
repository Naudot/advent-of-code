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

		private long GetResult(string[] input, bool isSecondPart)
		{
			long result = 0;

			for (int i = 0; i < input.Length; i++)
			{
				string[] values = input[i].Split(':');
				long sum = long.Parse(values[0]);
				long[] operators = values[1].Split(' ').Where(val => val != string.Empty).Select(val => long.Parse(val)).ToArray();
				Ope[] operations = new Ope[operators.Length - 1];

				result += IsReaching(sum, operators, operations, 0, isSecondPart) ? sum : 0;
			}

			return result;
		}

		private bool IsReaching(long sum, long[] operators, Ope[] operations, int depthIndex, bool secondPart)
		{
			if (depthIndex >= operations.Length)
				return false;

			for (int i = 0; i < (secondPart ? 3 : 2); i++)
			{
				long result = operators[0];

				operations[depthIndex] = (Ope)i;

				for (int j = 0; j < operations.Length; j++)
				{
					Ope ope = operations[j];

					if (ope == Ope.MULT)
						result *= operators[j + 1];
					else if (ope == Ope.ADD)
						result += operators[j + 1];
					else if (secondPart && ope == Ope.CONCAT)
					{
						result *= (long)Math.Pow(10, operators[j + 1].CountDigits());
						result += operators[j + 1];
					}
					if (result > sum)
						break;
				}

				if (result == sum || IsReaching(sum, operators, operations, depthIndex + 1, secondPart))
					return true;
			}

			return false;
		}
	}
}
