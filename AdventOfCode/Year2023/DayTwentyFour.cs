namespace AdventOfCode.Year2023
{
	public class Hailstone
	{
		public (decimal X, decimal Y, decimal Z) StartPosition;
		public (decimal X, decimal Y, decimal Z) KnownPosition;
		public (decimal X, decimal Y, decimal Z) EndPosition;
		public (decimal X, decimal Y, decimal Z) Velocity;
	}

	public class DayTwentyFour : Day2023
	{
		//private const decimal LOW_BOUND = 200000000000000;
		//private const decimal HIGH_BOUND = 400000000000000;
		private const decimal LOW_BOUND = 7;
		private const decimal HIGH_BOUND = 27;

		protected override bool DeactivateJIT => true;

		// 200 000 000 000 000
		// 400 000 000 000 000
		protected override object ResolveFirstPart(string[] input)
		{
			List<Hailstone> hailstones = new();

			for (int i = 0; i < input.Length; i++)
			{
				string[] vals = input[i].Replace(" ", string.Empty).Split('@');
				decimal[] pos = vals[0].Split(',').Select(val => decimal.Parse(val)).ToArray();
				decimal[] vel = vals[1].Split(',').Select(val => decimal.Parse(val)).ToArray();

				Hailstone hailstone = new()
				{
					KnownPosition = (pos[0], pos[1], pos[2]),
					Velocity = (vel[0], vel[1], vel[2])
				};

				if (IsInBoundaries(hailstone.KnownPosition.X, hailstone.KnownPosition.Y))
				{
					CalculateInBoundariesPosition(hailstone, false);
					CalculateInBoundariesPosition(hailstone, true);
				}
				else
				{
					Console.WriteLine(input[i]);
				}

				hailstones.Add(hailstone);
			}

			long intersections = 0;

			for (int i = 0; i < hailstones.Count - 1; i++)
				for (int j = i + 1; j < hailstones.Count; j++)
					intersections += DoHailstonesIntersect(hailstones[i], hailstones[j]) ? 1 : 0;

			return intersections;
		}

		protected override object ResolveSecondPart(string[] input)
		{
			return string.Empty;
		}

		private bool DoHailstonesIntersect(Hailstone first, Hailstone second)
		{
			return true;
		}

		// Returns true if an entry point has been found
		// PastMode true is inverted velocity to find EndPoint
		private bool CalculateOutOfBoundaries(Hailstone hs, bool pastMode)
		{
			bool isXInBoundaries = IsInBoundaries(hs.KnownPosition.X);
			bool isYInBoundaries = IsInBoundaries(hs.KnownPosition.Y);

			// Exemple 1 : Cas où les deux points sont à l'extérieur
			// Bounds 20 40
			// X 13 Y 15
			// VelX 1 VelY 2
			// 20 - 13 = 7 / 1 = 7 Cycles pour atteindre l'aire
			// 20 - 15 = 5 / 2 = 2.5 Cycles pour atteindre l'aire
			// Je dois check que 15 + 7 * 2 < HIGH BOUND = 29 donc c'est < 40 et > 20 donc la droite traverse bien
			// Et son start point est x20 y29

			// Exemple 2 : cas où un des points est à l'intérieur
			// Bounds 20 40
			// X 22 Y 15
			// VelX 1 VelY 2
			// Je ne calcule le cycle que pour le point en dehors
			// 20 - 15 = 5 / 2 = 2.5 Cycles
			// 22 + 2.5 * 1 = 24.5 donc X est toujours dans l'aire au moment où Y nous rejoint
			// Le start point est donc 24.5 20

			// C'est son start car j'ai trouvé avec sa vélocité normale et non inversée
			// X 13 Y 15 en mode passé
			// VelX -1 VelY -2
			// X < 20 et X-1 < X < 20 donc on s'éloigne
			// Y < 20 et Y-2 < Y < 20 donc on s'éloigne
			// Vu qu'on s'éloigne des deux côtés on ne trouvera aucun résultat ici

			// Pour trouver le end point si le start point n'est pas trouvé on utilise juste les formules au dessus
			// En inversant la vélocité

			//if (!isXInBoundaries && !isYInBoundaries)
			//{
			//	bool isXGettingAway = 
			//}
			//else if (isXInBoundaries)
			//{

			//}
			//else if (isYInBoundaries)
			//{

			//}
			//else
			//{
			//	Console.WriteLine("Error: Should not be here");
			//}

			return false;
		}

		private void CalculateInBoundariesPosition(Hailstone hailstone, bool pastMode)
		{
			// E.g. : Point à l'intérieur : Future and past mode
			// 20 40 boundaries
			// 25 28 position de base

			// Future : -1 5 Velocity
			/*
			 * 20 (low bound because negative velocity) - 25 = -5 / -1 = 5 cycles
			 * 40 (high bound because positive velocity) - 28 = 12 / 5 = 2.4 cycles
			 * 2,4 cycles authorized so end position is
			 * 25 + 2.4 * -1 = 22.6
			 * 28 + 2.4 * 5 = 40
			 */
			// Past : 1 -5 Velocity
			/*
			 * 40 (high bound because positive velocity) - 25 = 15 / 1 = 15 cycles
			 * 20 (low bound because negative velocity) - 28 = -8 / -5 = 1,4 cycles
			 * 1,4 cycles authorized so start position is
			 * 25 + 1.4 * 1  = 26.4
			 * 28 + 1.4 * -5 = 20
			 */
			decimal xVelocityUsed = pastMode ? -hailstone.Velocity.X : hailstone.Velocity.X;
			decimal yVelocityUsed = pastMode ? -hailstone.Velocity.Y : hailstone.Velocity.Y;

			decimal xUsed = xVelocityUsed < 0 ? LOW_BOUND : HIGH_BOUND;
			decimal xDiff = (xUsed - hailstone.KnownPosition.X);
			decimal xCycles = xDiff / xVelocityUsed;

			decimal yUsed = yVelocityUsed < 0 ? LOW_BOUND : HIGH_BOUND;
			decimal yDiff = (yUsed - hailstone.KnownPosition.Y);
			decimal yCycles = yDiff / yVelocityUsed;

			decimal minCycles = Math.Min(xCycles, yCycles);
			if (!pastMode)
			{
				hailstone.EndPosition =
					(hailstone.KnownPosition.X + xVelocityUsed * minCycles,
					hailstone.KnownPosition.Y + yVelocityUsed * minCycles,
					hailstone.KnownPosition.Z);
			}
			else
			{
				hailstone.StartPosition =
					(hailstone.KnownPosition.X + xVelocityUsed * minCycles,
					hailstone.KnownPosition.Y + yVelocityUsed * minCycles,
					hailstone.KnownPosition.Z);
			}
		}

		private bool IsInBoundaries(decimal x, decimal y)
		{
			return IsInBoundaries(x) && IsInBoundaries(y);
		}

		private bool IsInBoundaries(decimal val)
		{
			return val >= LOW_BOUND && val <= HIGH_BOUND && val >= LOW_BOUND && val <= HIGH_BOUND;
		}
	}
}
