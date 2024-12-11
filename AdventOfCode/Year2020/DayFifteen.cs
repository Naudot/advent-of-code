namespace AdventOfCode.Year2020
{
	public class DayFifteen : Day2020
	{
		protected override object ResolveFirstPart()
		{
			return ProcessLastNumber(2020);
		}

		protected override object ResolveSecondPart()
		{
			return ProcessLastNumber(30000000);
		}

		private int ProcessLastNumber(int treshold)
		{
			int[] input = "19,0,5,1,10,13".Split(',').Select(int.Parse).ToArray();
			int lastNumber = 0;
			Dictionary<int, int> values = new Dictionary<int, int>();

			for (int i = 0; i < treshold - 1; i++)
			{
				int usedNumber = i < input.Length ? input[i] : lastNumber;

				if (values.ContainsKey(usedNumber))
				{
					int lastEncountered = values[usedNumber];
					values[usedNumber] = i + 1;
					lastNumber = (i + 1) - lastEncountered;
				}
				else
				{
					values.Add(usedNumber, i + 1);
					lastNumber = 0;
				}
			}

			return lastNumber;
		}
	}
}
