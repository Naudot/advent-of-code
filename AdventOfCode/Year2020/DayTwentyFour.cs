using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode.Year2020
{
	public class DayTwentyFour : Day2020
	{
		private List<Tile> mTiles = new List<Tile>();
		private List<Tile> mNewTilesToProcess = new List<Tile>();

		public class Tile
		{
			public int East;
			public int South;
			public bool IsWhite = true;
		}

		protected override object ResolveFirstPart()
		{
			mTiles.Clear();

			string[] input = File.ReadAllLines(GetResourcesPath());

			for (int i = 0; i < input.Length; i++)
			{
				Tile newTile = new Tile();
				string moves = input[i];

				for (int j = 0; j < moves.Length; j++)
				{
					char firstChar = moves[j];

					if (firstChar == 'e')
					{
						newTile.East += 2;
					}
					if (firstChar == 'w')
					{
						newTile.East -= 2;
					}

					if (j == moves.Length - 1)
					{
						continue;
					}

					char secondChar = moves[j + 1];
					bool isSecondCharUsed = false;

					if (firstChar == 's')
					{
						isSecondCharUsed = true;
						if (secondChar == 'e')
						{
							newTile.South += 1;
							newTile.East += 1;
						}
						else if (secondChar == 'w')
						{
							newTile.South += 1;
							newTile.East -= 1;
						}
					}
					if (firstChar == 'n')
					{
						isSecondCharUsed = true;
						if (secondChar == 'e')
						{
							newTile.South -= 1;
							newTile.East += 1;
						}
						else if (secondChar == 'w')
						{
							newTile.South -= 1;
							newTile.East -= 1;
						}
					}

					if (isSecondCharUsed)
					{
						j++;
					}
				}

				Tile alreadyUsed = mTiles.FirstOrDefault(tile =>
					tile.East == newTile.East
					&& tile.South == newTile.South
				);

				if (alreadyUsed == null)
				{
					mTiles.Add(newTile);
					newTile.IsWhite = !newTile.IsWhite;
				}
				else
				{
					alreadyUsed.IsWhite = !alreadyUsed.IsWhite;
				}
			}

			return mTiles.Where(tile => !tile.IsWhite).Count();
		}

		protected override object ResolveSecondPart()
		{
			ResolveFirstPart();

			List<Tile> tilesToReplace = new List<Tile>();

			for (int i = 0; i < 100; i++)
			{
				mNewTilesToProcess.Clear();
				tilesToReplace.Clear();

				for (int j = 0; j < mTiles.Count; j++)
				{
					Tile tile = mTiles[j];
					int blackAdjacentTiles = CountBlackAdjacentTiles(tile);

					if (tile.IsWhite)
					{
						if (blackAdjacentTiles == 2)
						{
							tilesToReplace.Add(new Tile() { East = tile.East, South = tile.South, IsWhite = false });
						}
					}
					else
					{
						if (blackAdjacentTiles == 0 || blackAdjacentTiles > 2)
						{
							tilesToReplace.Add(new Tile() { East = tile.East, South = tile.South, IsWhite = true });
						}
					}
				}

				for (int j = 0; j < mNewTilesToProcess.Count; j++)
				{
					Tile tile = mNewTilesToProcess[j];
					int blackAdjacentTiles = CountBlackAdjacentTiles(tile);

					if (tile.IsWhite)
					{
						if (blackAdjacentTiles == 2)
						{
							tilesToReplace.Add(new Tile() { East = tile.East, South = tile.South, IsWhite = false });
						}
					}
					else
					{
						if (blackAdjacentTiles == 0 || blackAdjacentTiles > 2)
						{
							tilesToReplace.Add(new Tile() { East = tile.East, South = tile.South, IsWhite = true });
						}
					}

					mTiles.Add(tile);
				}

				for (int j = 0; j < tilesToReplace.Count; j++)
				{
					Tile toUpdate = mTiles.FirstOrDefault(savedTile => savedTile.East == tilesToReplace[j].East && savedTile.South == tilesToReplace[j].South);
					toUpdate.IsWhite = tilesToReplace[j].IsWhite;
				}

				Console.WriteLine("Day " + (i + 1) + ": " + mTiles.Where(tile => !tile.IsWhite).Count());
			}

			return mTiles.Where(tile => !tile.IsWhite).Count();
		}

		private int CountBlackAdjacentTiles(Tile tile)
		{
			int blackAdjacentTile = 0;

			blackAdjacentTile += ProcessTile(tile, 2, 0) ? 0 : 1; // East
			blackAdjacentTile += ProcessTile(tile, 1, 1) ? 0 : 1; // SouthEast
			blackAdjacentTile += ProcessTile(tile, -1, 1) ? 0 : 1; // SouthWest
			blackAdjacentTile += ProcessTile(tile, -2, 0) ? 0 : 1; // West
			blackAdjacentTile += ProcessTile(tile, -1, -1) ? 0 : 1; // NorthWest
			blackAdjacentTile += ProcessTile(tile, 1, -1) ? 0 : 1; // NorthEast

			return blackAdjacentTile;
		}

		private bool ProcessTile(Tile existingTile, int eastOffset, int southOffset)
		{
			// Look for a known tile
			Tile tile = mTiles.FirstOrDefault(savedTile => savedTile.East == existingTile.East + eastOffset && savedTile.South == existingTile.South + southOffset);
			
			// If the tile is unknown
			if (tile == null)
			{
				// We create it
				tile = new Tile() { East = existingTile.East + eastOffset, South = existingTile.South + southOffset, IsWhite = true };

				// If our existing tile is black
				if (!existingTile.IsWhite)
				{
					if (mNewTilesToProcess.FirstOrDefault(createdTile => createdTile.East == tile.East && createdTile.South == tile.South) == null)
					{
						mNewTilesToProcess.Add(tile);
					}
				}
			}

			// Return the check
			return tile.IsWhite;
		}
	}
}
