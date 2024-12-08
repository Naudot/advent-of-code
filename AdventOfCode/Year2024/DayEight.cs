namespace AdventOfCode.Year2024
{
	public class DayEight : Day2024
	{
		protected override object ResolveFirstPart(string[] input)
		{
			return GetAntinodes(input, GetAntennas(input), false).Count;
		}

		protected override object ResolveSecondPart(string[] input)
		{
			return GetAntinodes(input, GetAntennas(input), true).Count;
		}

		private static HashSet<(int, int)> GetAntinodes(string[] input, Dictionary<char, HashSet<(int x, int y)>> antennas, bool isSecondPart)
		{
			HashSet<(int, int)> antinodes = new();

			foreach (KeyValuePair<char, HashSet<(int x, int y)>> antenna in antennas)
			{
				// For each position excepted the last
				for (int i = 0; i < antenna.Value.Count - 1; i++)
				{
					(int x, int y) firstAntenna = antenna.Value.ElementAt(i);

					// I look the other positions
					for (int j = i + 1; j < antenna.Value.Count; j++)
					{
						(int x, int y) secondAntenna = antenna.Value.ElementAt(j);

						(int x, int y) firstDelta = ((secondAntenna.x - firstAntenna.x), (secondAntenna.y - firstAntenna.y));
						(int x, int y) secondDelta = ((firstAntenna.x - secondAntenna.x), (firstAntenna.y - secondAntenna.y));

						if (!isSecondPart)
						{
							(int x, int y) firstAntinode = (secondAntenna.x + firstDelta.x, secondAntenna.y + firstDelta.y);
							if (firstAntinode.x >= 0 && firstAntinode.x < input[0].Length && firstAntinode.y >= 0 && firstAntinode.y < input.Length)
								antinodes.Add(firstAntinode);

							(int x, int y) secondAntinode = (firstAntenna.x + secondDelta.x, firstAntenna.y + secondDelta.y);
							if (secondAntinode.x >= 0 && secondAntinode.x < input[0].Length && secondAntinode.y >= 0 && secondAntinode.y < input.Length)
								antinodes.Add(secondAntinode);
						}
						else
						{
							(int x, int y) firstAntinode = (firstAntenna.x + firstDelta.x, firstAntenna.y + firstDelta.y);
							while (firstAntinode.x >= 0 && firstAntinode.x < input[0].Length && firstAntinode.y >= 0 && firstAntinode.y < input.Length)
							{
								antinodes.Add(firstAntinode);
								firstAntinode = (firstAntinode.x + firstDelta.x, firstAntinode.y + firstDelta.y);
							}

							(int x, int y) secondAntinode = (secondAntenna.x + secondDelta.x, secondAntenna.y + secondDelta.y);
							while (secondAntinode.x >= 0 && secondAntinode.x < input[0].Length && secondAntinode.y >= 0 && secondAntinode.y < input.Length)
							{
								antinodes.Add(secondAntinode);
								secondAntinode = (secondAntinode.x + secondDelta.x, secondAntinode.y + secondDelta.y);
							}
						}
					}
				}
			}

			return antinodes;
		}

		private static Dictionary<char, HashSet<(int x, int y)>> GetAntennas(string[] input)
		{
			Dictionary<char, HashSet<(int x, int y)>> antennas = new();

			for (int y = 0; y < input.Length; y++)
			{
				for (int x = 0; x < input[y].Length; x++)
				{
					char symbol = input[y][x];

					if (symbol != '.')
					{
						if (!antennas.ContainsKey(symbol))
							antennas.Add(symbol, new());
						antennas[symbol].Add((x, y));
					}
				}
			}

			return antennas;
		}
	}
}
