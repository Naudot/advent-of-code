using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode.Year2020
{
	public class DayTwentyFour : Day2020
	{
		public class Tile
		{
			public int East;
			public int South;
			public bool IsWhite = true;
		}

		protected override object ResolveFirstPart()
		{
			string[] input = File.ReadAllLines(GetResourcesPath());

			List<Tile> tiles = new List<Tile>();

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

				Tile alreadyUsed = tiles.FirstOrDefault(tile =>
					tile.East == newTile.East
					&& tile.South == newTile.South
				);

				if (alreadyUsed == null)
				{
					tiles.Add(newTile);
					newTile.IsWhite = !newTile.IsWhite;
				}
				else
				{
					alreadyUsed.IsWhite = !alreadyUsed.IsWhite;
				}
			}

			return tiles.Where(tile => !tile.IsWhite).Count();
		}

		protected override object ResolveSecondPart()
		{
			return string.Empty;
		}
	}
}
