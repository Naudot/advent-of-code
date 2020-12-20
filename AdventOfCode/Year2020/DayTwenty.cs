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

		public int PuzzleIndex = -1;
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
				return TopTile == other;
			}

			for (int i = 0; i < DayTwenty.SQUARE_SIZE; i++)
			{
				if (TopBorder[i] != other.BottomBorder[i])
				{
					return false;
				}
			}

			//Console.WriteLine("Assigning " + other.ID + " to " + ID + " top");
			TopTile = other;
			TopTile.BottomTile = this;
			return true;
		}

		public bool AssignRight(Tile other)
		{
			if (RightTile != null)
			{
				return RightTile == other;
			}

			for (int i = 0; i < DayTwenty.SQUARE_SIZE; i++)
			{
				if (RightBorder[i] != other.LeftBorder[i])
				{
					return false;
				}
			}

			//Console.WriteLine("Assigning " + other.ID + " to " + ID + " right");
			RightTile = other;
			RightTile.LeftTile = this;
			return true;
		}

		public bool AssignBottom(Tile other)
		{
			if (BottomTile != null)
			{
				return BottomTile == other;
			}

			for (int i = 0; i < DayTwenty.SQUARE_SIZE; i++)
			{
				if (BottomBorder[i] != other.TopBorder[i])
				{
					return false;
				}
			}

			//Console.WriteLine("Assigning " + other.ID + " to " + ID + " bottom");
			BottomTile = other;
			BottomTile.TopTile = this;
			return true;
		}

		public bool AssignLeft(Tile other)
		{
			if (LeftTile != null)
			{
				return LeftTile == other;
			}

			for (int i = 0; i < DayTwenty.SQUARE_SIZE; i++)
			{
				if (LeftBorder[i] != other.RightBorder[i])
				{
					return false;
				}
			}

			//Console.WriteLine("Assigning " + other.ID + " to " + ID + " left");
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

			Console.WriteLine("Testing " + mTiles[55].ID + " at state 6");
			mTiles[55].SetState(6);
			ProcessHighIQAlgorithm(mTiles[55]);
			mTiles[55].Clear();

			// 0 = 1117 Index 55
			// 11 = 1543
			// 143 = 1213
			// 132 = 1291

			Tile topLeft = mTiles.FirstOrDefault(tile => tile.PuzzleIndex == 0);
			if (topLeft == null)
			{
				Console.WriteLine("topLeft not found");
				return -1;
			}
			Console.WriteLine("topLeft : " + topLeft.ID);
			Tile topRight = mTiles.FirstOrDefault(tile => tile.PuzzleIndex == PUZZLE_SIZE - 1);
			if (topRight == null)
			{
				Console.WriteLine("topRight not found");
				return -1;
			}
			Console.WriteLine("topRight: " + topRight.ID);
			Tile bottomRight = mTiles.FirstOrDefault(tile => tile.PuzzleIndex == (PUZZLE_SIZE - 1) * PUZZLE_SIZE);
			if (bottomRight == null)
			{
				Console.WriteLine("bottomRight not found");
				return -1;
			}
			Console.WriteLine("bottomRight: " + bottomRight.ID);
			Tile bottomLeft = mTiles.FirstOrDefault(tile => tile.PuzzleIndex == (PUZZLE_SIZE - 1) * PUZZLE_SIZE + (PUZZLE_SIZE - 1));
			if (bottomLeft == null)
			{
				Console.WriteLine("bottomLeft not found");
				return -1;
			}
			Console.WriteLine("bottomLeft: " + bottomLeft.ID);

			return topLeft.ID * topRight.ID * bottomRight.ID * bottomLeft.ID;
		}

		private bool mGoingRight = true;

		private bool ProcessHighIQAlgorithm(Tile currentTile)
		{
			//Console.WriteLine("Processing " + currentTile.ID);

			mLookedTiles.Add(currentTile.ID, currentTile);
			bool foundRight = false;
			bool foundBottom = false;
			bool foundLeft = false;

			for (int i = 0; i < mTiles.Count; i++)
			{
				if (mLookedTiles.ContainsKey(mTiles[i].ID) && mLookedTiles.Count != mTiles.Count)
				{
					continue;
				}

				for (int state = 0; state < 8; state++)
				{
					mTiles[i].SetState(state);

					bool lookForRight = mCurrentColumn != PUZZLE_SIZE - 1 && mGoingRight;
					bool lookForLeft = mCurrentColumn != 0 && !mGoingRight;
					bool lookForBottom = mCurrentRow != PUZZLE_SIZE - 1 && 
						((mGoingRight && mCurrentColumn == PUZZLE_SIZE - 1) || (!mGoingRight && mCurrentColumn == 0));

					if (lookForRight && currentTile.AssignRight(mTiles[i]))
					{
						foundRight = true;
					}
					else if (lookForBottom && currentTile.AssignBottom(mTiles[i]))
					{
						foundBottom = true;
					}
					else if (lookForLeft && currentTile.AssignLeft(mTiles[i]))
					{
						foundLeft = true;
					}

					if ((mGoingRight && !lookForRight && !lookForBottom)
						|| (!mGoingRight && !lookForLeft && !lookForBottom))
					{
						currentTile.PuzzleIndex = PUZZLE_SIZE * mCurrentRow + mCurrentColumn;
						return true;
					}

					// Looking for right case
					if (lookForRight && foundRight)
					{
						mCurrentColumn++;
						if (ProcessHighIQAlgorithm(currentTile.RightTile))
						{
							mCurrentColumn--;
							currentTile.PuzzleIndex = PUZZLE_SIZE * mCurrentRow + mCurrentColumn;
							return true;
						}
						mCurrentColumn--;
					}
					else if (lookForBottom && foundBottom)
					{
						mCurrentRow++;
						mGoingRight = !mGoingRight;
						if (ProcessHighIQAlgorithm(currentTile.BottomTile))
						{
							mCurrentRow--;
							currentTile.PuzzleIndex = PUZZLE_SIZE * mCurrentRow + mCurrentColumn;
							return true;
						}
						mCurrentRow--;
					}
					else if (lookForLeft && foundLeft)
					{
						mCurrentColumn--;
						if (ProcessHighIQAlgorithm(currentTile.LeftTile))
						{
							mCurrentColumn++;
							currentTile.PuzzleIndex = PUZZLE_SIZE * mCurrentRow + mCurrentColumn;
							return true;
						}
						mCurrentColumn++;
					}

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
