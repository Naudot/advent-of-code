﻿namespace AdventOfCode.Year2023
{
	public class DaySixteen : Day2023
	{
		protected override bool DeactivateJIT
		{
			get
			{
				return true;
			}
		}

		public class Beam
		{
			public int X;
			public int Y;
			public Direction Direction;

			public Beam(int x, int y, Direction direction)
			{
				X = x;
				Y = y;
				Direction = direction;
			}
		}

		public class Tile
		{
			public HashSet<Direction> Directions = new HashSet<Direction>();
			public Type TileType;
		}

		public enum Direction
		{
			RIGHT,
			DOWN,
			LEFT,
			UP
		}

		public enum Type
		{
			EMPTY,
			MIRROR_RIGHT_UP_LEFT_DOWN,
			MIRROR_RIGHT_DOWN_LEFT_UP,
			SPLITTER_VERTICAL,
			SPLITTER_HORIZONTAL
		}

		protected override object ResolveFirstPart(string[] input)
		{
			return GetEnergy(input, 0, 0, Direction.RIGHT);
		}

		protected override object ResolveSecondPart(string[] input)
		{
			int maxEnergy = 0;

			for (int y = 0; y < input.Length; y++)
			{
				for (int x = 0; x < input[0].Length; x++)
				{
					if (y > 0 && y < input.Length - 1 && x > 0 && x < input[0].Length - 1)
						continue;

					if (y == 0 && x == 0)
					{
						maxEnergy = Math.Max(maxEnergy, GetEnergy(input, x, y, Direction.RIGHT));
						maxEnergy = Math.Max(maxEnergy, GetEnergy(input, x, y, Direction.DOWN));
					}
					else if (y == 0 && x == input[0].Length - 1)
					{
						maxEnergy = Math.Max(maxEnergy, GetEnergy(input, x, y, Direction.LEFT));
						maxEnergy = Math.Max(maxEnergy, GetEnergy(input, x, y, Direction.DOWN));
					}
					else if (y == input.Length - 1 && x == 0)
					{
						maxEnergy = Math.Max(maxEnergy, GetEnergy(input, x, y, Direction.RIGHT));
						maxEnergy = Math.Max(maxEnergy, GetEnergy(input, x, y, Direction.UP));
					}
					else if (y == input.Length - 1 && x == input[0].Length - 1)
					{
						maxEnergy = Math.Max(maxEnergy, GetEnergy(input, x, y, Direction.LEFT));
						maxEnergy = Math.Max(maxEnergy, GetEnergy(input, x, y, Direction.UP));
					}
					else if (y == 0)
					{
						maxEnergy = Math.Max(maxEnergy, GetEnergy(input, x, y, Direction.DOWN));
					}
					else if (x == 0)
					{
						maxEnergy = Math.Max(maxEnergy, GetEnergy(input, x, y, Direction.RIGHT));
					}
					else if (y == input.Length - 1)
					{
						maxEnergy = Math.Max(maxEnergy, GetEnergy(input, x, y, Direction.UP));
					}
					else if (x == input[0].Length - 1)
					{
						maxEnergy = Math.Max(maxEnergy, GetEnergy(input, x, y, Direction.LEFT));
					}
				}
			}

			return maxEnergy;
		}

		private int GetEnergy(string[] input, int startX, int startY, Direction direction)
		{
			int yLength = input.Length;
			int xLength = input[0].Length;

			Beam firstBeam = new Beam(startX, startY, direction);
			List<Beam> beams = new List<Beam>();
			beams.Add(firstBeam);

			Tile[,] tiles = new Tile[input.Length, input[0].Length];
			for (int y = 0; y < yLength; y++)
			{
				for (int x = 0; x < xLength; x++)
				{
					tiles[y, x] = new Tile();
					tiles[y, x].TileType = GetType(input[y][x]);
				}
			}

			List<Beam> toAdd = new List<Beam>();
			List<Beam> toRemove = new List<Beam>();

			while (beams.Any())
			{
				for (int i = 0; i < beams.Count; i++)
				{
					Beam beam = beams[i];

					if (beam.X < 0 || beam.X >= xLength || beam.Y < 0 || beam.Y >= yLength)
					{
						toRemove.Add(beam);
						continue;
					}

					Tile currentTile = tiles[beam.Y, beam.X];

					if (currentTile.Directions.Contains(beam.Direction))
					{
						toRemove.Add(beam);
						continue;
					}

					currentTile.Directions.Add(beam.Direction);

					if (currentTile.TileType == Type.EMPTY)
					{
						if (beam.Direction == Direction.RIGHT)
							beam.X++;
						if (beam.Direction == Direction.DOWN)
							beam.Y++;
						if (beam.Direction == Direction.LEFT)
							beam.X--;
						if (beam.Direction == Direction.UP)
							beam.Y--;
					}
					else if (currentTile.TileType == Type.MIRROR_RIGHT_UP_LEFT_DOWN)
					{
						if (beam.Direction == Direction.RIGHT)
						{
							beam.Direction = Direction.UP;
							beam.Y--;
						}
						else if (beam.Direction == Direction.DOWN)
						{
							beam.Direction = Direction.LEFT;
							beam.X--;
						}
						else if (beam.Direction == Direction.LEFT)
						{
							beam.Direction = Direction.DOWN;
							beam.Y++;
						}
						else if (beam.Direction == Direction.UP)
						{
							beam.Direction = Direction.RIGHT;
							beam.X++;
						}
					}
					else if (currentTile.TileType == Type.MIRROR_RIGHT_DOWN_LEFT_UP)
					{
						if (beam.Direction == Direction.RIGHT)
						{
							beam.Direction = Direction.DOWN;
							beam.Y++;
						}
						else if (beam.Direction == Direction.DOWN)
						{
							beam.Direction = Direction.RIGHT;
							beam.X++;
						}
						else if (beam.Direction == Direction.LEFT)
						{
							beam.Direction = Direction.UP;
							beam.Y--;
						}
						else if (beam.Direction == Direction.UP)
						{
							beam.Direction = Direction.LEFT;
							beam.X--;
						}
					}
					else if (currentTile.TileType == Type.SPLITTER_VERTICAL)
					{
						if (beam.Direction == Direction.DOWN)
							beam.Y++;
						else if (beam.Direction == Direction.UP)
							beam.Y--;
						else
						{
							Beam newUpBeam = new Beam(beam.X, beam.Y, Direction.UP);
							newUpBeam.Y--;
							Beam newDownBeam = new Beam(beam.X, beam.Y, Direction.DOWN);
							newDownBeam.Y++;

							toAdd.Add(newUpBeam);
							toAdd.Add(newDownBeam);
							toRemove.Add(beam);
						}
					}
					else if (currentTile.TileType == Type.SPLITTER_HORIZONTAL)
					{
						if (beam.Direction == Direction.RIGHT)
							beam.X++;
						else if (beam.Direction == Direction.LEFT)
							beam.X--;
						else
						{
							Beam newLeftBeam = new Beam(beam.X, beam.Y, Direction.LEFT);
							newLeftBeam.X--;
							Beam newRightBeam = new Beam(beam.X, beam.Y, Direction.RIGHT);
							newRightBeam.X++;

							toAdd.Add(newLeftBeam);
							toAdd.Add(newRightBeam);
							toRemove.Add(beam);
						}
					}
				}

				for (int i = 0; i < toRemove.Count; i++)
				{
					beams.Remove(toRemove[i]);
				}
				toRemove.Clear();
				for (int i = 0; i < toAdd.Count; i++)
				{
					beams.Add(toAdd[i]);
				}
				toAdd.Clear();
			}

			//WriteTiles(tiles);

			int energizedSum = 0;
			for (int y = 0; y < yLength; y++)
				for (int x = 0; x < xLength; x++)
					energizedSum += tiles[y, x].Directions.Count() != 0 ? 1 : 0;
			return energizedSum;
		}

		private void WriteTiles(Tile[,] tiles)
		{
			int yLength = tiles.GetLength(0);
			int xLength = tiles.GetLength(1);

			for (int y = 0; y < yLength; y++)
			{
				Console.WriteLine();
				for (int x = 0; x < xLength; x++)
				{
					int count = tiles[y, x].Directions.Count();

					if (tiles[y, x].TileType != Type.EMPTY)
					{
						if (count > 0)
						{
							Console.Write("#");
						}
						else
						{
							Console.Write(GetChar(tiles[y, x].TileType));
						}
					}
					else
					{
						Console.Write(count > 0 ? (count > 1 ? count.ToString() : "#") : ".");
					}
				}
			}
			Console.WriteLine();
		}

		private Type GetType(char c)
		{
			if (c == '\\')
				return Type.MIRROR_RIGHT_DOWN_LEFT_UP;
			if (c == '/')
				return Type.MIRROR_RIGHT_UP_LEFT_DOWN;
			if (c == '|')
				return Type.SPLITTER_VERTICAL;
			if (c == '-')
				return Type.SPLITTER_HORIZONTAL;

			return Type.EMPTY;
		}

		private char GetChar(Type type)
		{
			if (type == Type.MIRROR_RIGHT_DOWN_LEFT_UP)
				return '\\';
			if (type == Type.MIRROR_RIGHT_UP_LEFT_DOWN)
				return '/';
			if (type == Type.SPLITTER_VERTICAL)
				return '|';
			if (type == Type.SPLITTER_HORIZONTAL)
				return '-';

			return '.';
		}
	}
}
