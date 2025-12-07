using System.Linq;

namespace AdventOfCode.Year2025
{
	public class DaySeven : Day2025
	{
		protected override bool DeactivateJIT => true;

		protected override object ResolveFirstPart(string[] input)
		{
			int splitCount = 0;
			int height = input.Length;
			int width = input[0].Length;

			HashSet<(int x, int y)> beams = new() { (input[0].IndexOf('S'), 1) };

			bool atLeastOneBeamMoved = true;
			while (atLeastOneBeamMoved)
			{
				atLeastOneBeamMoved = false;

				HashSet<(int x, int y)> copy = new(beams);
				int beamCount = copy.Count;
				for (int i = 0; i < beamCount; i++)
				{
					(int x, int y) beam = copy.ElementAt(i);

					// The beam reached the bottom of the map
					if (beam.y + 1 == height)
						break;

					char downwardChar = input[beam.y + 1][beam.x];
					if (downwardChar == '.' && !beams.Contains((beam.x, beam.y + 1)))
					{
						beams.Add((beam.x, beam.y + 1));
						atLeastOneBeamMoved = true;
					}
					else if (downwardChar == '^')
					{
						bool hasBeenSplitted = false;
						if (beam.x - 1 >= 0)
						{
							if (!beams.Contains((beam.x - 1, beam.y + 1)) && !hasBeenSplitted)
							{
								hasBeenSplitted = true;
								atLeastOneBeamMoved = true;
								splitCount++;
							}
							beams.Add((beam.x - 1, beam.y + 1));
						}
						if (beam.x + 1 < width)
						{
							if (!beams.Contains((beam.x + 1, beam.y + 1)) && !hasBeenSplitted)
							{
								hasBeenSplitted = true;
								atLeastOneBeamMoved = true;
								splitCount++;
							}
							beams.Add((beam.x + 1, beam.y + 1));
						}
					}
				}
			}

			return splitCount;
		}

		protected override object ResolveSecondPart(string[] input)
		{
			int height = input.Length;
			int width = input[0].Length;

			// Number of timeline on this beam coordinate
			Dictionary<(int x, int y), long> beams = new();
			beams.Add((input[0].IndexOf('S'), 1), 1);

			// When a beam join another beam, they merge and the beam count augments
			for (int y = 0; y < height; y++)
			{
				Dictionary<(int x, int y), long> newBeams = new();
				foreach (KeyValuePair<(int x, int y), long> beamPair in beams.Where(beam => beam.Key.y == y))
				{
					(int x, int y) beam = beamPair.Key;

					// The beam reached the bottom of the map
					if (beam.y + 1 == height)
						break;

					char downwardChar = input[beam.y + 1][beam.x];
					if (downwardChar == '.')
					{
						// Should not happen ?
						if (newBeams.ContainsKey((beam.x, beam.y + 1)))
							newBeams[(beam.x, beam.y + 1)] += beamPair.Value;
						else
							newBeams.Add((beam.x, beam.y + 1), beamPair.Value);

					}
					else if (downwardChar == '^')
					{
						if (beam.x - 1 >= 0)
						{
							if (newBeams.ContainsKey((beam.x - 1, beam.y + 1)))
								newBeams[(beam.x - 1, beam.y + 1)] += beamPair.Value;
							else
								newBeams.Add((beam.x - 1, beam.y + 1), beamPair.Value);
						}
						if (beam.x + 1 < width)
						{
							if (newBeams.ContainsKey((beam.x + 1, beam.y + 1)))
								newBeams[(beam.x + 1, beam.y + 1)] += beamPair.Value;
							else
								newBeams.Add((beam.x + 1, beam.y + 1), beamPair.Value);
						}
					}

					beams.Remove(beam);
				}

				if (newBeams.Count != 0)
					beams = newBeams;
			}

			return beams.Sum(beam => beam.Value);
		}
	}
}
