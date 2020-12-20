using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Year2020
{
	public class Tile
	{
		#region Fields

		public int CurrentState = 0;

		public ulong ID;
		public bool[] TopBorder = new bool[DayTwenty.SQUARE_SIZE];
		public bool[] RightBorder = new bool[DayTwenty.SQUARE_SIZE];
		public bool[] BottomBorder = new bool[DayTwenty.SQUARE_SIZE];
		public bool[] LeftBorder = new bool[DayTwenty.SQUARE_SIZE];

		public Tile TopTile;
		public Tile RightTile;
		public Tile BottomTile;
		public Tile LeftTile;

		// Allow array manipulations in flip and rotate methods without reallocating arrays
		private bool[] mTmp = new bool[10];
		private bool mIsSwapped = false;
		private bool mVerticalFlip = false;
		private bool mHorizontalFlip = false;

		#endregion

		#region Methods

		/// <summary>
		/// Please use this method with state from 0 to 15
		/// </summary>
		public void SetState(int state)
		{
			CurrentState = state;

			bool isSwapped = (state & 1) == 1;
			bool mustVerticalFlip = (state & 2) == 2;
			bool mustHorizonalFlip = (state & 4) == 4;

			if (isSwapped != mIsSwapped)
			{
				Swap(isSwapped);
				mIsSwapped = isSwapped;
			}

			if (mVerticalFlip != mustVerticalFlip)
			{
				Flip(true);
				mVerticalFlip = mustVerticalFlip;
			}

			if (mHorizontalFlip != mustHorizonalFlip)
			{
				Flip(false);
				mHorizontalFlip = mustHorizonalFlip;
			}
		}

		public void Draw()
		{
			Console.WriteLine("Tile " + ID + ": Etat " + CurrentState);
			for (int i = 0; i < DayTwenty.SQUARE_SIZE; i++)
			{
				for (int j = 0; j < DayTwenty.SQUARE_SIZE; j++)
				{
					if (i == 0) // TopBorder
					{
						Console.Write(TopBorder[j] ? '#' : '.');
					}
					else if (i == DayTwenty.SQUARE_SIZE - 1) // BottomBorder
					{
						Console.Write(BottomBorder[j] ? '#' : '.');
					}
					else if (j == 0) // LeftBorder
					{
						Console.Write(LeftBorder[i] ? '#' : '.');
					}
					else if (j == DayTwenty.SQUARE_SIZE - 1) // RightBorder
					{
						Console.Write(RightBorder[i] ? '#' : '.');
					}
					else
					{
						Console.Write(' ');
					}
				}
				Console.WriteLine();
			}
			Console.WriteLine();
		}

		public bool AssignRight(Tile other)
		{
			for (int i = 0; i < DayTwenty.SQUARE_SIZE; i++)
			{
				if (RightBorder[i] != other.RightBorder[i])
				{
					return false;
				}
			}

			RightTile = other;
			return true;
		}

		public bool AssignBottom(Tile other)
		{
			for (int i = 0; i < DayTwenty.SQUARE_SIZE; i++)
			{
				if (BottomBorder[i] != other.BottomBorder[i])
				{
					return false;
				}
			}

			BottomTile = other;
			return true;
		}

		public bool AssignLeft(Tile other)
		{
			for (int i = 0; i < DayTwenty.SQUARE_SIZE; i++)
			{
				if (LeftBorder[i] != other.LeftBorder[i])
				{
					return false;
				}
			}

			LeftTile = other;
			return true;
		}

		public bool AssignTop(Tile other)
		{
			for (int i = 0; i < DayTwenty.SQUARE_SIZE; i++)
			{
				if (TopBorder[i] != other.TopBorder[i])
				{
					return false;
				}
			}

			TopTile = other;
			return true;
		}

		#endregion

		#region Implementation

		/// <summary>
		/// Rotate the tile clockwise.
		/// </summary>
		private void Swap(bool back)
		{
			for (int i = 0; i < (back ? 3 : 1); i++)
			{
				for (int j = 0; j < DayTwenty.SQUARE_SIZE; j++)
				{
					mTmp[j] = LeftBorder[j];
				}

				for (int j = 0; j < DayTwenty.SQUARE_SIZE; j++)
				{
					LeftBorder[j] = BottomBorder[j];
				}

				for (int j = 0; j < DayTwenty.SQUARE_SIZE; j++)
				{
					BottomBorder[j] = RightBorder[DayTwenty.SQUARE_SIZE - 1 - j];
				}

				for (int j = 0; j < DayTwenty.SQUARE_SIZE; j++)
				{
					RightBorder[j] = TopBorder[j];
				}

				for (int j = 0; j < DayTwenty.SQUARE_SIZE; j++)
				{
					TopBorder[j] = mTmp[DayTwenty.SQUARE_SIZE - 1 - j];
				}
			}
		}

		/// <summary>
		/// Flip the tile.
		/// </summary>
		private void Flip(bool vertical)
		{
			if (vertical)
			{
				for (int i = 0; i < DayTwenty.SQUARE_SIZE; i++)
				{
					mTmp[i] = TopBorder[i];
				}
				for (int i = 0; i < DayTwenty.SQUARE_SIZE; i++)
				{
					TopBorder[i] = BottomBorder[i];
				}
				for (int i = 0; i < DayTwenty.SQUARE_SIZE; i++)
				{
					BottomBorder[i] = mTmp[i];
				}

				for (int i = 0; i < DayTwenty.SQUARE_SIZE; i++)
				{
					mTmp[i] = LeftBorder[i];
				}
				for (int i = 0; i < DayTwenty.SQUARE_SIZE; i++)
				{
					LeftBorder[i] = mTmp[DayTwenty.SQUARE_SIZE - 1 - i];
				}

				for (int i = 0; i < DayTwenty.SQUARE_SIZE; i++)
				{
					mTmp[i] = RightBorder[i];
				}
				for (int i = 0; i < DayTwenty.SQUARE_SIZE; i++)
				{
					RightBorder[i] = mTmp[DayTwenty.SQUARE_SIZE - 1 - i];
				}
			}
			else
			{
				for (int i = 0; i < DayTwenty.SQUARE_SIZE; i++)
				{
					mTmp[i] = RightBorder[i];
				}
				for (int i = 0; i < DayTwenty.SQUARE_SIZE; i++)
				{
					RightBorder[i] = LeftBorder[i];
				}
				for (int i = 0; i < DayTwenty.SQUARE_SIZE; i++)
				{
					LeftBorder[i] = mTmp[i];
				}

				for (int i = 0; i < DayTwenty.SQUARE_SIZE; i++)
				{
					mTmp[i] = TopBorder[i];
				}
				for (int i = 0; i < DayTwenty.SQUARE_SIZE; i++)
				{
					TopBorder[i] = mTmp[DayTwenty.SQUARE_SIZE - 1 - i];
				}

				for (int i = 0; i < DayTwenty.SQUARE_SIZE; i++)
				{
					mTmp[i] = BottomBorder[i];
				}
				for (int i = 0; i < DayTwenty.SQUARE_SIZE; i++)
				{
					BottomBorder[i] = mTmp[DayTwenty.SQUARE_SIZE - 1 - i];
				}
			}
		}

		public void Clear()
		{
			SetState(0);
			TopTile = null;
			RightTile = null;
			BottomTile = null;
			LeftTile = null;
		}

		#endregion
	}

	public class DayTwenty : Day2020
	{
		public const int SQUARE_SIZE = 10;
		public const int PUZZLE_SIZE = 12;

		private List<Tile> mTiles = new List<Tile>();
		private Dictionary<ulong, Tile> mLookedTiles = new Dictionary<ulong, Tile>();
		private int mCurrentColumn = 0;
		private int mCurrentRow = 0;

		protected override object ResolveFirstPart()
		{
			mCurrentColumn = 0;
			mCurrentRow = 0;
			mLookedTiles.Clear();
			mTiles.Clear();

			MatchCollection tiles = Regex.Matches(File.ReadAllText(GetResourcesPath()), @"Tile (\d*):\n([.#\n]*)");

			for (int i = 0; i < tiles.Count; i++)
			{
				Match match = tiles[i];
				Tile tile = new Tile();
				tile.ID = ulong.Parse(match.Groups[1].Value);
				string content = match.Groups[2].Value.Replace("\n", string.Empty);

				for (int j = 0; j < content.Length; j++)
				{
					if (j < 10) // TopBorder
					{
						tile.TopBorder[j] = content[j] == '#';
					}
					if (j >= 90) // BottomBorder
					{
						tile.BottomBorder[j - 90] = content[j] == '#';
					}
					if (j % 10 == 0) // LeftBorder
					{
						tile.LeftBorder[j / 10] = content[j] == '#';
					}
					if (j % 10 == 9) // RightBorder
					{
						tile.RightBorder[(j - 9) / 10] = content[j] == '#';
					}
				}

				mTiles.Add(tile);
			}

			// Tests all states of the first tile for debug purpose
			//mTiles[0].Draw();
			//mTiles[0].SetState(0);
			//mTiles[0].Draw();
			//mTiles[0].SetState(1);
			//mTiles[0].Draw();
			//mTiles[0].SetState(2);
			//mTiles[0].Draw();
			//mTiles[0].SetState(3);
			//mTiles[0].Draw();
			//mTiles[0].SetState(4);
			//mTiles[0].Draw();
			//mTiles[0].SetState(5);
			//mTiles[0].Draw();
			//mTiles[0].SetState(6);
			//mTiles[0].Draw();
			//mTiles[0].SetState(7);
			//mTiles[0].Draw();

			for (int i = 0; i < mTiles.Count; i++)
			{
				if (ProcessHighIQAlgorithm(mTiles[i]))
				{
					break;
				}
			}

			Tile topLeft = mTiles.FirstOrDefault(tile => tile.TopTile == null && tile.LeftTile == null);
			if (topLeft == null)
			{
				Console.WriteLine("topLeft not found");
				return -1;
			}
			Tile topRight = mTiles.FirstOrDefault(tile => tile.TopTile == null && tile.RightTile == null);
			if (topRight == null)
			{
				Console.WriteLine("topRight not found");
				return -1;
			}
			Tile bottomRight = mTiles.FirstOrDefault(tile => tile.RightTile == null && tile.BottomTile == null);
			if (bottomRight == null)
			{
				Console.WriteLine("bottomRight not found");
				return -1;
			}
			Tile bottomLeft = mTiles.FirstOrDefault(tile => tile.LeftTile == null && tile.BottomTile == null);
			if (bottomLeft == null)
			{
				Console.WriteLine("bottomLeft not found");
				return -1;
			}

			return topLeft.ID * topRight.ID * bottomRight.ID * bottomLeft.ID;
		}

		private bool ProcessHighIQAlgorithm(Tile currentTile)
		{
			mLookedTiles.Add(currentTile.ID, currentTile);
			int state = 0;
			bool foundRight = false;
			bool foundBottom = false;
			bool foundLeft = false;
			bool foundTop = false;

			if (mCurrentColumn == 0 && mCurrentRow == 0) // We want right and bottom tiles
			{
				for (int j = 0; j < 8; j++)
				{
					state = j;
					for (int i = 0; i < mTiles.Count; i++)
					{
						if (mLookedTiles.ContainsKey(mTiles[i].ID))
						{
							continue;
						}

						mTiles[i].SetState(state);

						if (currentTile.AssignRight(mTiles[i]))
						{
							foundRight = true;
						}
						if (currentTile.AssignBottom(mTiles[i]))
						{
							foundBottom = true;
						}

						if (foundRight && foundBottom)
						{
							mCurrentColumn++;
							if (ProcessHighIQAlgorithm(currentTile.RightTile))
							{
								return true;
							}
							mCurrentColumn--;
							foundRight = false;
							foundBottom = false;
							mLookedTiles.Remove(mTiles[i].ID);
							mTiles[i].Clear();
						}
					}
				}
			}
			else if (mCurrentColumn > 0 && mCurrentColumn < PUZZLE_SIZE - 1)
			{
				if (mCurrentRow == 0) // We want left, bottom, right
				{

				}
				else if (mCurrentRow == PUZZLE_SIZE - 1) // We want left, top, right
				{

				}
				else if (mCurrentRow > 0 && mCurrentRow < PUZZLE_SIZE - 1) // We want top, right, bottom, left
				{

				}
			}
			else if (mCurrentRow > 0 && mCurrentRow < PUZZLE_SIZE - 1)
			{
				if (mCurrentColumn == 0) // We want top, bottom, right
				{

				}
				else if (mCurrentColumn == PUZZLE_SIZE - 1) // We want top, bottom, left
				{

				}
				else if (mCurrentColumn > 0 && mCurrentColumn < PUZZLE_SIZE - 1) // We want top, right, bottom, left
				{

				}
			}
			else if (mCurrentColumn == PUZZLE_SIZE - 1 && mCurrentRow == PUZZLE_SIZE - 1) // We want top and left tiles
			{
				// TODO : tout
				return true;
			}

			return false;
			/*
			 Il faut conserver l'état du carré
			 Pour chaque tile, on vérifie si une des autres tiles correspond dans un de ses 16 positions (4 rotate et 4 flips):
				Notre condition dépend de notre column et row actuels
					Voir conditions
					si on est à l'intérieur on veut les quatre
				Si aucune des conditions ne match, cela veut dire que notre tile est mal placée
				On revient process la tile d'avant en tournant les tiles subséquentes dans leurs états non faits
			Si une tile correspond à notre condition, selon notre condition on avance et on rappelle l'algo pour la tile que l'on a trouvé
			Je les cherche de gauche à droite et de haut en bas
			 */
		}

		protected override object ResolveSecondPart()
		{
			return string.Empty;
		}
	}
}
