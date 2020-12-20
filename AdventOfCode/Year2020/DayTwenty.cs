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

		public bool AssignTop(Tile other)
		{
			if (TopTile != null)
			{
				return true;
			}

			for (int i = 0; i < DayTwenty.SQUARE_SIZE; i++)
			{
				if (TopBorder[i] != other.TopBorder[i])
				{
					return false;
				}
			}

			TopTile = other;
			TopTile.BottomTile = this;
			return true;
		}

		public bool AssignRight(Tile other)
		{
			if (RightTile != null)
			{
				return true;
			}

			for (int i = 0; i < DayTwenty.SQUARE_SIZE; i++)
			{
				if (RightBorder[i] != other.RightBorder[i])
				{
					return false;
				}
			}

			RightTile = other;
			RightTile.LeftTile = this;
			return true;
		}

		public bool AssignBottom(Tile other)
		{
			if (BottomTile != null)
			{
				return true;
			}

			for (int i = 0; i < DayTwenty.SQUARE_SIZE; i++)
			{
				if (BottomBorder[i] != other.BottomBorder[i])
				{
					return false;
				}
			}

			BottomTile = other;
			BottomTile.TopTile = this;
			return true;
		}

		public bool AssignLeft(Tile other)
		{
			if (LeftTile != null)
			{
				return true;
			}

			for (int i = 0; i < DayTwenty.SQUARE_SIZE; i++)
			{
				if (LeftBorder[i] != other.LeftBorder[i])
				{
					return false;
				}
			}

			LeftTile = other;
			LeftTile.RightTile = this;
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
			if (TopTile != null)
			{
				TopTile.BottomTile = null;
				TopTile = null;
			}
			if (RightTile != null)
			{
				RightTile.LeftTile = null;
				RightTile = null;
			}
			if (BottomTile != null)
			{
				BottomTile.TopTile = null;
				BottomTile = null;
			}
			if (LeftTile != null)
			{
				LeftTile.RightTile = null;
				LeftTile = null;
			}
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
			bool foundTop = false;
			bool foundRight = false;
			bool foundBottom = false;
			bool foundLeft = false;

			for (int state = 0; state < 8; state++)
			{
				for (int i = 0; i < mTiles.Count; i++)
				{
					if (mLookedTiles.ContainsKey(mTiles[i].ID))
					{
						continue;
					}

					mTiles[i].SetState(state);

					bool lookForTop = mCurrentRow != 0;
					bool lookForRight = mCurrentColumn != PUZZLE_SIZE - 1;
					bool lookForBottom = mCurrentRow != PUZZLE_SIZE - 1;
					bool lookForLeft = mCurrentColumn != 0;

					if (lookForTop && currentTile.AssignTop(mTiles[i]))
					{
						foundTop = true;
					}
					if (lookForRight && currentTile.AssignRight(mTiles[i]))
					{
						foundRight = true;
					}
					if (lookForBottom && currentTile.AssignBottom(mTiles[i]))
					{
						foundBottom = true;
					}
					if (lookForLeft && currentTile.AssignLeft(mTiles[i]))
					{
						foundLeft = true;
					}

					// End case
					if (lookForTop && lookForLeft && !lookForBottom && !lookForRight 
						&& foundTop && foundLeft)
					{
						return true;
					}

					// Top left case (begin case)
					if (!lookForTop && lookForRight && lookForBottom && !lookForLeft)
					{
						if (foundRight && foundBottom)
						{
							mCurrentColumn++;
							if (ProcessHighIQAlgorithm(currentTile.RightTile))
							{
								return true;
							}
							mCurrentColumn--;
						}
					}
					// Top row case
					if (!lookForTop && lookForRight && lookForBottom && lookForLeft)
					{
						if (foundRight && foundBottom && foundLeft)
						{
							mCurrentColumn++;
							if (ProcessHighIQAlgorithm(currentTile.BottomTile))
							{
								return true;
							}
							mCurrentColumn--;
						}
					}
					// Top right case
					if (!lookForTop && !lookForRight && lookForBottom && lookForLeft)
					{
						if (foundBottom && foundLeft)
						{
							mCurrentRow++;
							if (ProcessHighIQAlgorithm(currentTile.BottomTile))
							{
								return true;
							}
							mCurrentRow--;
						}
					}

					foundTop = false;
					foundRight = false;
					foundBottom = false;
					foundLeft = false;
				}
			}

			currentTile.Clear();
			mLookedTiles.Remove(currentTile.ID);
			return false;
		}

		protected override object ResolveSecondPart()
		{
			return string.Empty;
		}
	}
}
