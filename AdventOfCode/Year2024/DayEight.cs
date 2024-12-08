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
						GetAntinodesOfAntennas(firstAntenna, secondAntenna, isSecondPart, input.Length).ForEach(val => antinodes.Add(val));
						GetAntinodesOfAntennas(secondAntenna, firstAntenna, isSecondPart, input.Length).ForEach(val => antinodes.Add(val));
					}
				}
			}

			return antinodes;
		}

		private static List<(int, int)> GetAntinodesOfAntennas((int x, int y) startAntenna, (int x, int y) endAntenna, bool isSecondPart, int mapSize)
		{
			List<(int, int)> antinodes = new();

			(int x, int y) delta = ((endAntenna.x - startAntenna.x), (endAntenna.y - startAntenna.y));
			(int x, int y) departure = isSecondPart ? startAntenna : endAntenna;
			(int x, int y) antinode = (departure.x + delta.x, departure.y + delta.y);

			while (antinode.x >= 0 && antinode.x < mapSize && antinode.y >= 0 && antinode.y < mapSize)
			{
				antinodes.Add(antinode);
				antinode = (antinode.x + delta.x, antinode.y + delta.y);

				if (!isSecondPart)
					break;
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
