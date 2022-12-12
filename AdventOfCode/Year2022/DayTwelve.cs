using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Year2022
{
	public class Tile
	{
		public int X;
		public int Y;
		public int Elevation;
		public int Cost;
		public int Distance;
		public int CostDistance => Cost + Distance;
		public Tile Parent;
		
		//The distance is essentially the estimated distance, ignoring walls to our target. 
		//So how many tiles left and right, up and down, ignoring walls, to get there. 
		public void SetDistance(int targetX, int targetY)
		{
			this.Distance = Math.Abs(targetX - X) + Math.Abs(targetY - Y);
		}
	}

	public class DayTwelve : Day2022
	{
		protected override bool DeactivateJIT { get { return true; } }

		protected override object ResolveFirstPart(string[] input)
		{
			Tile start = new Tile();
			Tile end = new Tile();

			// Height : Y
			for (int i = 0; i < input.Length; i++)
			{
				// Width : X
				for (int j = 0; j < input[i].Length; j++)
				{
					if (input[i][j] == 'S')
					{
						start.X = j;
						start.Y = i;
						start.Elevation = 'a';
					}
					else if (input[i][j] == 'E')
					{
						end.X = j;
						end.Y = i;
						end.Elevation = 'z';
					}
				}
			}

			return GetStepCount(input, start, end);
		}

		protected override object ResolveSecondPart(string[] input)
		{
			List<Tile> starts = new List<Tile>();

			Tile end = new Tile();

			// Height : Y
			for (int i = 0; i < input.Length; i++)
			{
				// Width : X
				for (int j = 0; j < input[i].Length; j++)
				{
					if (input[i][j] == 'a' || input[i][j] == 'S')
					{
						Tile newStart = new Tile()
						{
							X = j,
							Y = i,
							Elevation = 'a'
						};
						starts.Add(newStart);
					}
					else if (input[i][j] == 'E')
					{
						end.X = j;
						end.Y = i;
						end.Elevation = 'z';
					}
				}
			}

			int bestPath = int.MaxValue;

			for (int i = 0; i < starts.Count; i++)
			{
				int stepCount = GetStepCount(input, starts[i], end);
				bestPath = stepCount < bestPath && stepCount != -1 ? stepCount : bestPath;
			}

			return bestPath;
		}

		private static List<Tile> GetWalkableTiles(string[] input, Tile currentTile, Tile targetTile)
		{
			int maxX = input[0].Length - 1;
			int maxY = input.Length - 1;

			List<Tile> possibleTiles = new List<Tile>()
			{
				// Down tile
				new Tile { X = currentTile.X, Y = currentTile.Y - 1, Parent = currentTile, Cost = currentTile.Cost + 1 },
				// Up tile
				new Tile { X = currentTile.X, Y = currentTile.Y + 1, Parent = currentTile, Cost = currentTile.Cost + 1 },
				// Left tile
				new Tile { X = currentTile.X - 1, Y = currentTile.Y, Parent = currentTile, Cost = currentTile.Cost + 1 },
				// Right tile
				new Tile { X = currentTile.X + 1, Y = currentTile.Y, Parent = currentTile, Cost = currentTile.Cost + 1 },
			};

			for (int i = 0; i < possibleTiles.Count; i++)
			{
				Tile tile = possibleTiles[i];
				if (tile.X >= 0 && tile.X <= maxX && tile.Y >= 0 && tile.Y <= maxY)
				{
					if (input[tile.Y][tile.X] == 'E')
					{
						tile.Elevation = 'z';
					}
					else if (input[tile.Y][tile.X] == 'S')
					{
						tile.Elevation = 'a';
					}
					else
					{
						tile.Elevation = input[tile.Y][tile.X];
					}
				}
			}

			possibleTiles.ForEach(tile => tile.SetDistance(targetTile.X, targetTile.Y));

			int currentElevation = currentTile.Elevation;

			return possibleTiles
					.Where(tile => tile.X >= 0 && tile.X <= maxX)
					.Where(tile => tile.Y >= 0 && tile.Y <= maxY)
					.Where(tile => tile.Elevation - currentElevation <= 1)
					.ToList();
		}

		private static int GetStepCount(string[] input, Tile start, Tile end)
		{
			start.SetDistance(end.X, end.Y);

			List<Tile> activeTiles = new List<Tile>();
			activeTiles.Add(start);

			List<Tile> visitedTiles = new List<Tile>();

			int stepCount = -1;
			while (activeTiles.Any())
			{
				Tile nextTile = activeTiles.OrderBy(tile => tile.CostDistance).First();

				if (nextTile.X == end.X && nextTile.Y == end.Y)
				{
					Tile test = nextTile;
					while (test != null)
					{
						stepCount++;
						test = test.Parent;
					}

					Console.WriteLine("Path found for start " + start.X + " " + start.Y);
					break;
				}

				visitedTiles.Add(nextTile);
				activeTiles.Remove(nextTile);

				List<Tile> walkableTiles = GetWalkableTiles(input, nextTile, end);

				foreach (Tile walkableTile in walkableTiles)
				{
					if (visitedTiles.Any(tile => tile.X == walkableTile.X && tile.Y == walkableTile.Y))
					{
						continue;
					}

					if (activeTiles.Any(tile => tile.X == walkableTile.X && tile.Y == walkableTile.Y))
					{
						Tile existingTile = activeTiles.First(tile => tile.X == walkableTile.X && tile.Y == walkableTile.Y);
						if (existingTile.CostDistance > nextTile.CostDistance)
						{
							activeTiles.Remove(existingTile);
							activeTiles.Add(walkableTile);
						}
					}
					else
					{
						activeTiles.Add(walkableTile);
					}
				}
			}

			Console.WriteLine("Can not find path for start " + start.X + " " + start.Y);
			return stepCount;
		}
	}
}
