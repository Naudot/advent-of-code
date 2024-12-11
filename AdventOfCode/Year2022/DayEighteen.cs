namespace AdventOfCode.Year2022
{
	public class DayEighteen : Day2022
	{
		public class Cube
		{
			public (int, int, int) Coords;

			public Cube LeftCube; // -X
			public Cube RightCube; // +X
			public Cube BottomCube; // -Y
			public Cube TopCube; // +Y
			public Cube BackCube; // -Z
			public Cube FrontCube; // +Z

			public int GetFreeSidesCount()
			{
				int count = 0;

				count += LeftCube != null ? 0 : 1;
				count += RightCube != null ? 0 : 1;
				count += BottomCube != null ? 0 : 1;
				count += TopCube != null ? 0 : 1;
				count += BackCube != null ? 0 : 1;
				count += FrontCube != null ? 0 : 1;

				return count;
			}
		}

		private static Dictionary<(int, int, int), Cube> Cubes = new Dictionary<(int, int, int), Cube>();

		protected override object ResolveFirstPart(string[] input)
		{
			Cubes.Clear();

			for (int i = 0; i < input.Length; i++)
			{
				string[] strCoord = input[i].Split(',');
				(int, int, int) coord = (int.Parse(strCoord[0]), int.Parse(strCoord[1]), int.Parse(strCoord[2]));

				Cube newCube = new Cube() { Coords = coord };
				Cubes.Add(coord, newCube);

				(int, int, int) leftCoord = coord;
				leftCoord.Item1--;
				if (Cubes.ContainsKey(leftCoord))
				{
					Cubes[leftCoord].RightCube = newCube;
					newCube.LeftCube = Cubes[leftCoord];
				}

				(int, int, int) rightCoord = coord;
				rightCoord.Item1++;
				if (Cubes.ContainsKey(rightCoord))
				{
					Cubes[rightCoord].LeftCube = newCube;
					newCube.RightCube = Cubes[rightCoord];
				}

				(int, int, int) bottomCoord = coord;
				bottomCoord.Item2--;
				if (Cubes.ContainsKey(bottomCoord))
				{
					Cubes[bottomCoord].TopCube = newCube;
					newCube.BottomCube = Cubes[bottomCoord];
				}

				(int, int, int) topCoord = coord;
				topCoord.Item2++;
				if (Cubes.ContainsKey(topCoord))
				{
					Cubes[topCoord].BottomCube = newCube;
					newCube.TopCube = Cubes[topCoord];
				}

				(int, int, int) backCoord = coord;
				backCoord.Item3--;
				if (Cubes.ContainsKey(backCoord))
				{
					Cubes[backCoord].FrontCube = newCube;
					newCube.BackCube = Cubes[backCoord];
				}

				(int, int, int) frontCoord = coord;
				frontCoord.Item3++;
				if (Cubes.ContainsKey(frontCoord))
				{
					Cubes[frontCoord].BackCube = newCube;
					newCube.FrontCube = Cubes[frontCoord];
				}
			}

			return Cubes.Select(val => val.Value.GetFreeSidesCount()).Sum();
		}

		protected override object ResolveSecondPart(string[] input)
		{
			Cubes.Clear();

			for (int i = 0; i < input.Length; i++)
			{
				string[] strCoord = input[i].Split(',');
				(int, int, int) coord = (int.Parse(strCoord[0]), int.Parse(strCoord[1]), int.Parse(strCoord[2]));

				Cube newCube = new Cube() { Coords = coord };
				Cubes.Add(coord, newCube);

				(int, int, int) leftCoord = coord;
				leftCoord.Item1--;
				if (Cubes.ContainsKey(leftCoord))
				{
					Cubes[leftCoord].RightCube = newCube;
					newCube.LeftCube = Cubes[leftCoord];
				}

				(int, int, int) rightCoord = coord;
				rightCoord.Item1++;
				if (Cubes.ContainsKey(rightCoord))
				{
					Cubes[rightCoord].LeftCube = newCube;
					newCube.RightCube = Cubes[rightCoord];
				}

				(int, int, int) bottomCoord = coord;
				bottomCoord.Item2--;
				if (Cubes.ContainsKey(bottomCoord))
				{
					Cubes[bottomCoord].TopCube = newCube;
					newCube.BottomCube = Cubes[bottomCoord];
				}

				(int, int, int) topCoord = coord;
				topCoord.Item2++;
				if (Cubes.ContainsKey(topCoord))
				{
					Cubes[topCoord].BottomCube = newCube;
					newCube.TopCube = Cubes[topCoord];
				}

				(int, int, int) backCoord = coord;
				backCoord.Item3--;
				if (Cubes.ContainsKey(backCoord))
				{
					Cubes[backCoord].FrontCube = newCube;
					newCube.BackCube = Cubes[backCoord];
				}

				(int, int, int) frontCoord = coord;
				frontCoord.Item3++;
				if (Cubes.ContainsKey(frontCoord))
				{
					Cubes[frontCoord].BackCube = newCube;
					newCube.FrontCube = Cubes[frontCoord];
				}
			}

			// 2100 -> Alors que c'est 2102 :(
			return Cubes.Select(val => GetFreeSidesThatTouchAirCount(val.Value)).Sum();
		}

		private int GetFreeSidesThatTouchAirCount(Cube cube)
		{
			HashSet<(int, int, int)> previousLookedAt = new HashSet<(int, int, int)>();
			previousLookedAt.Add(cube.Coords);

			bool doLeftTouchesAir = false;
			(int, int, int) leftCoord = cube.Coords;
			leftCoord.Item1--;
			if (cube.LeftCube == null)
			{
				doLeftTouchesAir = DoCoordTouchesAir(leftCoord, previousLookedAt);
			}

			bool doRightTouchesAir = false;
			(int, int, int) rightCoord = cube.Coords;
			rightCoord.Item1++;
			if (cube.RightCube == null)
			{
				doRightTouchesAir = DoCoordTouchesAir(rightCoord, previousLookedAt);
			}

			bool doBottomTouchesAir = false;
			(int, int, int) bottomCoord = cube.Coords;
			bottomCoord.Item2--;
			if (cube.BottomCube == null)
			{
				doBottomTouchesAir = DoCoordTouchesAir(bottomCoord, previousLookedAt);
			}

			bool doTopTouchesAir = false;
			(int, int, int) topCoord = cube.Coords;
			topCoord.Item2++;
			if (cube.TopCube == null)
			{
				doTopTouchesAir = DoCoordTouchesAir(topCoord, previousLookedAt);
			}

			bool doBackTouchesAir = false;
			(int, int, int) backCoord = cube.Coords;
			backCoord.Item3--;
			if (cube.BackCube == null)
			{
				doBackTouchesAir = DoCoordTouchesAir(backCoord, previousLookedAt);
			}

			bool doFrontTouchesAir = false;
			(int, int, int) frontCoord = cube.Coords;
			frontCoord.Item3++;
			if (cube.FrontCube == null)
			{
				doFrontTouchesAir = DoCoordTouchesAir(frontCoord, previousLookedAt);
			}

			int count = 0;

			count += doLeftTouchesAir ? 1 : 0;
			count += doRightTouchesAir ? 1 : 0;
			count += doBottomTouchesAir ? 1 : 0;
			count += doTopTouchesAir ? 1 : 0;
			count += doBackTouchesAir ? 1 : 0;
			count += doFrontTouchesAir ? 1 : 0;

			return count;
		}

		private bool DoCoordTouchesAir((int, int, int) coords, HashSet<(int, int, int)> previous)
		{
			previous.Add(coords);

			(int, int, int) leftCoord = coords;
			leftCoord.Item1--;
			if (leftCoord.Item1 < 0 || (!previous.Contains(leftCoord) && (!Cubes.ContainsKey(leftCoord) && DoCoordTouchesAir(leftCoord, previous))))
			{
				return true;
			}
			(int, int, int) rightCoord = coords;
			rightCoord.Item1++;
			if (rightCoord.Item1 > 19 || (!previous.Contains(rightCoord) && (!Cubes.ContainsKey(rightCoord) && DoCoordTouchesAir(rightCoord, previous))))
			{
				return true;
			}

			(int, int, int) bottomCoord = coords;
			bottomCoord.Item2--;
			if (bottomCoord.Item2 < 0 || (!previous.Contains(bottomCoord) && (!Cubes.ContainsKey(bottomCoord) && DoCoordTouchesAir(bottomCoord, previous))))
			{
				return true;
			}
			(int, int, int) topCoord = coords;
			topCoord.Item2++;
			if (topCoord.Item2 > 19 || (!previous.Contains(topCoord) && (!Cubes.ContainsKey(topCoord) && DoCoordTouchesAir(topCoord, previous))))
			{
				return true;
			}

			(int, int, int) backCoord = coords;
			backCoord.Item3--;
			if (backCoord.Item3 < 0 || (!previous.Contains(backCoord) && (!Cubes.ContainsKey(backCoord) && DoCoordTouchesAir(backCoord, previous))))
			{
				return true;
			}
			(int, int, int) frontCoord = coords;
			frontCoord.Item3++;
			if (frontCoord.Item3 > 19 || (!previous.Contains(frontCoord) && (!Cubes.ContainsKey(frontCoord) && DoCoordTouchesAir(frontCoord, previous))))
			{
				return true;
			}

			return false;
		}
	}
}
