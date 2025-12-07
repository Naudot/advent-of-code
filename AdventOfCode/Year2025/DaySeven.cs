using System.Collections.Generic;

namespace AdventOfCode.Year2025
{
	public class DaySeven : Day2025
	{
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
			Dictionary<int, long> beams = new() { { input[0].IndexOf('S'), 1 } };

			for (int y = 0; y < input.Length - 1; y++)
			{
				Dictionary<int, long> newBeams = new();
				
				foreach (KeyValuePair<int, long> beamPair in beams)
				{
					int beamX = beamPair.Key;

					if (input[y + 1][beamX] == '^')
					{
						if (beamX - 1 >= 0)
							newBeams.AddUp(beamX - 1, beamPair.Value);
						if (beamX + 1 < input[0].Length)
							newBeams.AddUp(beamX + 1, beamPair.Value);
					}
					else
						newBeams.AddUp(beamX, beamPair.Value);

					beams.Remove(beamX);
				}

				beams = newBeams;
			}

			return beams.Sum(beam => beam.Value);
		}
	}
}
