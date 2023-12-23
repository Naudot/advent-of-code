namespace AdventOfCode.Year2023
{
	public class DayTwentyTwo : Day2023
	{
		public class Brick
		{
			public (int X, int Y, int Z) Start;
			public (int X, int Y, int Z) End;

			public HashSet<(int, int, int)> GetCubes(bool fullZ, bool onlyTopZ)
			{
				HashSet<(int, int, int)> cubes = new();

				if (Start.X != End.X)
					for (int i = 0; i < End.X - Start.X + 1; i++)
						cubes.Add((Start.X + i, Start.Y, Start.Z));
				else if (Start.Y != End.Y)
					for (int i = 0; i < End.Y - Start.Y + 1; i++)
						cubes.Add((Start.X, Start.Y + i, Start.Z));
				else if (Start.Z != End.Z)
					if (fullZ)
						for (int i = 0; i < End.Z - Start.Z + 1; i++)
							cubes.Add((Start.X, Start.Y, Start.Z + i));
					else
						cubes.Add((Start.X, Start.Y, onlyTopZ ? End.Z : Start.Z));
				else
					cubes.Add((Start.X, Start.Y, Start.Z));

				return cubes;
			}
		}

		protected override bool DeactivateJIT => true;

		protected override object ResolveFirstPart(string[] input)
		{
			SortedDictionary<int, List<Brick>> bricks = new();
			HashSet<(int, int, int)> cubes = new();
			int maxZ = 0;

			// Bricks parsing
			for (int i = 0; i < input.Length; i++)
			{
				string[] coords = input[i].Split('~');
				string[] startCoords = coords[0].Split(',');
				string[] endCoords = coords[1].Split(',');

				Brick brick = new()
				{
					Start = (int.Parse(startCoords[0]), int.Parse(startCoords[1]), int.Parse(startCoords[2]) - 1),
					End = (int.Parse(endCoords[0]), int.Parse(endCoords[1]), int.Parse(endCoords[2]) - 1)
				};

				// Get full cubes
				cubes.UnionWith(brick.GetCubes(true, false));

				if (bricks.ContainsKey(brick.Start.Z))
					bricks[brick.Start.Z].Add(brick);
				else
					bricks.Add(brick.Start.Z, new() { brick });

				if (brick.End.Z > maxZ)
					maxZ = brick.End.Z;
			}

			// Filling missing Z to easily use them later
			for (int i = 0; i < maxZ; i++)
				if (!bricks.ContainsKey(i))
					bricks.Add(i, new());

			// Falling algorithm
			foreach (KeyValuePair<int, List<Brick>> item in bricks)
			{
				for (int i = 0; i < item.Value.Count; i++)
				{
					Brick brick = item.Value[i];
					bool hasRemovedOneInCount = false;
					bool hasFallen = true;

					// As long as the brick keeps falling, we continue
					while (hasFallen)
					{
						hasFallen = false;

						if (brick.Start.Z == 0)
							break;

						// Check if the brick can fall
						List<(int, int, int)> fallingCubes = brick.GetCubes(false, false).ToList();
						bool isFallable = true;
						for (int j = 0; j < fallingCubes.Count; j++)
							if (cubes.Contains((fallingCubes[j].Item1, fallingCubes[j].Item2, fallingCubes[j].Item3 - 1)))
							{
								isFallable = false;
								break;
							}
						if (!isFallable) continue;

						// If the brick has falled at least once, the loop index needs to be reajusted
						if (!hasRemovedOneInCount)
						{
							i--;
							hasRemovedOneInCount = true;
						}

						// We make the brick go down a level
						bricks[brick.Start.Z].Remove(brick);
						bricks[brick.Start.Z - 1].Add(brick);

						// We replace old cubes by new ones
						List<(int, int, int)> oldCubes = brick.GetCubes(true, false).ToList();
						for (int j = 0; j < oldCubes.Count; j++)
							cubes.Remove(oldCubes[j]);
						brick.Start = (brick.Start.X, brick.Start.Y, brick.Start.Z - 1);
						brick.End = (brick.End.X, brick.End.Y, brick.End.Z - 1);
						cubes.UnionWith(brick.GetCubes(true, false));

						hasFallen = true;
					}
				}
			}

			int safeDisintegratableBrick = 0;

			// For each Z
			foreach (KeyValuePair<int, List<Brick>> item in bricks)
			{
				// For each brick at this Z
				for (int i = 0; i < item.Value.Count; i++)
				{
					Brick brick = item.Value[i];
					List<Brick> topBricks = bricks[brick.End.Z + 1];
					HashSet<(int, int, int)> supportingCubes = brick.GetCubes(false, true);
					bool isDisintegretable = true;

					// For each top brick, if the brick fall, we can not disintegrate our current brick
					for (int j = 0; j < topBricks.Count; j++)
					{
						bool isBrickFalling = true;
						List<(int, int, int)> topCubes = topBricks[j].GetCubes(false, false).ToList();

						for (int k = 0; k < topCubes.Count; k++)
						{
							(int, int, int) cube = topCubes[k];
							(int, int, int) downCubeCoord = (cube.Item1, cube.Item2, cube.Item3 - 1);

							bool isEmpty = !cubes.Contains(downCubeCoord);
							bool isSupportingCube = supportingCubes.Contains(downCubeCoord);

							if (!isEmpty && !isSupportingCube)
							{
								isBrickFalling = false;
								break;
							}
						}

						if (isBrickFalling)
						{
							isDisintegretable = false;
							break;
						}
					}

					if (isDisintegretable)
						safeDisintegratableBrick++;
				}
			}

			return safeDisintegratableBrick;
		}

		protected override object ResolveSecondPart(string[] input)
		{
			SortedDictionary<int, List<Brick>> bricks = new();
			HashSet<(int, int, int)> cubes = new();
			int maxZ = 0;

			// Bricks parsing
			for (int i = 0; i < input.Length; i++)
			{
				string[] coords = input[i].Split('~');
				string[] startCoords = coords[0].Split(',');
				string[] endCoords = coords[1].Split(',');

				Brick brick = new()
				{
					Start = (int.Parse(startCoords[0]), int.Parse(startCoords[1]), int.Parse(startCoords[2]) - 1),
					End = (int.Parse(endCoords[0]), int.Parse(endCoords[1]), int.Parse(endCoords[2]) - 1)
				};

				// Get full cubes
				cubes.UnionWith(brick.GetCubes(true, false));

				if (bricks.ContainsKey(brick.Start.Z))
					bricks[brick.Start.Z].Add(brick);
				else
					bricks.Add(brick.Start.Z, new() { brick });

				if (brick.End.Z > maxZ)
					maxZ = brick.End.Z;
			}

			// Filling missing Z to easily use them later
			for (int i = 0; i < maxZ; i++)
				if (!bricks.ContainsKey(i))
					bricks.Add(i, new());

			// Falling algorithm
			foreach (KeyValuePair<int, List<Brick>> item in bricks)
			{
				for (int i = 0; i < item.Value.Count; i++)
				{
					Brick brick = item.Value[i];
					bool hasRemovedOneInCount = false;
					bool hasFallen = true;

					// As long as the brick keeps falling, we continue
					while (hasFallen)
					{
						hasFallen = false;

						if (brick.Start.Z == 0)
							break;

						// Check if the brick can fall
						List<(int, int, int)> fallingCubes = brick.GetCubes(false, false).ToList();
						bool isFallable = true;
						for (int j = 0; j < fallingCubes.Count; j++)
							if (cubes.Contains((fallingCubes[j].Item1, fallingCubes[j].Item2, fallingCubes[j].Item3 - 1)))
							{
								isFallable = false;
								break;
							}
						if (!isFallable) continue;

						// If the brick has falled at least once, the loop index needs to be reajusted
						if (!hasRemovedOneInCount)
						{
							i--;
							hasRemovedOneInCount = true;
						}

						// We make the brick go down a level
						bricks[brick.Start.Z].Remove(brick);
						bricks[brick.Start.Z - 1].Add(brick);

						// We replace old cubes by new ones
						List<(int, int, int)> oldCubes = brick.GetCubes(true, false).ToList();
						for (int j = 0; j < oldCubes.Count; j++)
							cubes.Remove(oldCubes[j]);
						brick.Start = (brick.Start.X, brick.Start.Y, brick.Start.Z - 1);
						brick.End = (brick.End.X, brick.End.Y, brick.End.Z - 1);
						cubes.UnionWith(brick.GetCubes(true, false));

						hasFallen = true;
					}
				}
			}

			int sum = 0;
			// For each Z
			foreach (KeyValuePair<int, List<Brick>> item in bricks)
			{
				// For each brick at this Z
				for (int i = 0; i < item.Value.Count; i++)
				{
					Brick brick = item.Value[i];
					sum += CountFallingBricksWithout(brick, bricks, cubes);
				}
			}

			return sum;
		}

		private int CountFallingBricksWithout(Brick brickToExlude, SortedDictionary<int, List<Brick>> bricks, HashSet<(int, int, int)> cubes)
		{
			int hasFallenCount = 0;

			// TODO : Hard copy de bricks et de cubes

			HashSet<(int, int, int)> newCubes = new();
			SortedDictionary<int, List<Brick>> newBricks = new();
			foreach (KeyValuePair<int, List<Brick>> pair in bricks)
			{
				List<Brick> newBrickList = new();

				for (int i = 0; i < pair.Value.Count; i++)
				{
					if (brickToExlude.Start.X == pair.Value[i].Start.X 
						&& brickToExlude.Start.Y == pair.Value[i].Start.Y
						&& brickToExlude.Start.Z == pair.Value[i].Start.Z
						&& brickToExlude.End.X == pair.Value[i].End.X
						&& brickToExlude.End.Y == pair.Value[i].End.Y
						&& brickToExlude.End.Z == pair.Value[i].End.Z)
					{
						continue;
					}

					Brick newBrick = new() { End = pair.Value[i].End, Start = pair.Value[i].Start };
					newCubes.UnionWith(newBrick.GetCubes(true, false));
					newBrickList.Add(newBrick);
				}
				newBricks.Add(pair.Key, newBrickList);
			}

			// Falling algorithm
			foreach (KeyValuePair<int, List<Brick>> item in newBricks)
			{
				for (int i = 0; i < item.Value.Count; i++)
				{
					Brick brick = item.Value[i];
					bool hasRemovedOneInCount = false;
					bool hasFallen = true;

					// As long as the brick keeps falling, we continue
					while (hasFallen)
					{
						hasFallen = false;

						if (brick.Start.Z == 0)
							break;

						// Check if the brick can fall
						List<(int, int, int)> fallingCubes = brick.GetCubes(false, false).ToList();
						bool isFallable = true;
						for (int j = 0; j < fallingCubes.Count; j++)
							if (newCubes.Contains((fallingCubes[j].Item1, fallingCubes[j].Item2, fallingCubes[j].Item3 - 1)))
							{
								isFallable = false;
								break;
							}
						if (!isFallable) continue;

						// If the brick has falled at least once, the loop index needs to be reajusted
						if (!hasRemovedOneInCount)
						{
							i--;
							hasFallenCount++;
							hasRemovedOneInCount = true;
						}

						// We make the brick go down a level
						newBricks[brick.Start.Z].Remove(brick);
						newBricks[brick.Start.Z - 1].Add(brick);

						// We replace old cubes by new ones
						List<(int, int, int)> oldCubes = brick.GetCubes(true, false).ToList();
						for (int j = 0; j < oldCubes.Count; j++)
							newCubes.Remove(oldCubes[j]);
						brick.Start = (brick.Start.X, brick.Start.Y, brick.Start.Z - 1);
						brick.End = (brick.End.X, brick.End.Y, brick.End.Z - 1);
						newCubes.UnionWith(brick.GetCubes(true, false));

						hasFallen = true;
					}
				}
			}

			return hasFallenCount;
		}
	}
}
