namespace AdventOfCode.Year2024
{
	public class DayEight : Day2024
	{
		protected override bool DeactivateJIT
		{
			get
			{
				return true;
			}
		}

		protected override object ResolveFirstPart(string[] input)
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

						// Exemple
						// T'as une antenne en 4 3 et une autre en 5 5
						// La première en 4 3 rajoute (5 - 4) 1 en x et (5 - 3) 2 en y par rapport à la deuxième
						// La deuxième en 5 5 rajoute (4 - 5) -1 en x et (3 - 5) -2 en y par rapport à la première

						(int x, int y) firstAntinode = (secondAntenna.x + (secondAntenna.x - firstAntenna.x), secondAntenna.y + (secondAntenna.y - firstAntenna.y));
						(int x, int y) secondAntinode = (firstAntenna.x + (firstAntenna.x - secondAntenna.x), firstAntenna.y + (firstAntenna.y - secondAntenna.y));

						if (firstAntinode.x >= 0 && firstAntinode.x < input[0].Length && firstAntinode.y >= 0 && firstAntinode.y < input.Length)
							antinodes.Add(firstAntinode);

						if (secondAntinode.x >= 0 && secondAntinode.x < input[0].Length && secondAntinode.y >= 0 && secondAntinode.y < input.Length)
							antinodes.Add(secondAntinode);
					}
				}
			}

			for (int y = 0; y < input.Length; y++)
			{
				for (int x = 0; x < input[y].Length; x++)
				{
					Console.Write(antinodes.Contains((x, y)) ? '#' : '.');
				}
				Console.WriteLine();
			}

			return antinodes.Count;
		}

		protected override object ResolveSecondPart(string[] input)
		{
			return 0;
		}
	}
}
