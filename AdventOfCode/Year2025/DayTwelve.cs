namespace AdventOfCode.Year2025
{
	public class DayTwelve : Day2025
	{
		protected override object ResolveFirstPart(string[] input)
		{
			int fitCount = 0;
			int[] piecesSize = new int[] { 6, 6, 7, 7, 7, 7 };

			for (int i = 30; i < input.Length; i++)
			{
				string[] sizes = input[i].Split(':')[0].Split('x');
				string[] values = input[i].Split(':')[1].TrimStart().Split(' ');

				long size = long.Parse(sizes[0]) * long.Parse(sizes[1]);
				long wantedSize = 0;
				for (int j = 0; j < values.Length; j++)
					wantedSize += piecesSize[j] * long.Parse(values[j]);

				fitCount += size >= wantedSize ? 1 : 0;
			}

			return fitCount;
		}

		protected override object ResolveSecondPart(string[] input)
		{
			return "lol";
		}
	}
}
