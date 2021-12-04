using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AdventOfCode.Year2021
{
	public class DayFour : Day2021
	{
		protected override object ResolveFirstPart(string[] input)
		{
			BoardsContainer boardsContainer = GetBoards(input);
			MatchCollection draws = Regex.Matches(input[0], @"(\d+)");

			for (int i = 0; i < draws.Count; i++)
			{
				int value = int.Parse(draws[i].Value);
				boardsContainer.MarkBoards(value);
				List<Check[,]> winningBoards = boardsContainer.GetWinningBoards();

				if (winningBoards.Count != 0)
				{
					return boardsContainer.GetSumOfAllMarkedValues(winningBoards[0]) * value;
				}
			}

			return -1;
		}

		protected override object ResolveSecondPart(string[] input)
		{
			BoardsContainer boardsContainer = GetBoards(input);
			MatchCollection draws = Regex.Matches(input[0], @"(\d+)");
			int lastWinningBoardValue = 0;

			for (int i = 0; i < draws.Count; i++)
			{
				int value = int.Parse(draws[i].Value);
				boardsContainer.MarkBoards(value);
				List<Check[,]> winningBoards = boardsContainer.GetWinningBoards();

				for (int j = 0; j < winningBoards.Count; j++)
				{
					lastWinningBoardValue = boardsContainer.GetSumOfAllMarkedValues(winningBoards[j]) * value;
					boardsContainer.Boards.Remove(winningBoards[j]);
				}
			}

			return lastWinningBoardValue;
		}

		private BoardsContainer GetBoards(string[] input)
		{
			BoardsContainer boardsContainer = new BoardsContainer();

			// Begin at second line
			for (int i = 1; i < input.Length; i += 6)
			{
				// First line is a jump
				Check[,] board = new Check[5, 5];

				// Check the next 5 lines
				for (int j = 1; j < 6; j++)
				{
					MatchCollection values = Regex.Matches(input[i + j], @"(\d+)");
					for (int k = 0; k < values.Count; k++)
					{
						board[j - 1, k] = new Check() { Value = int.Parse(values[k].Value) };
					}
				}

				boardsContainer.Boards.Add(board);
			}

			return boardsContainer;
		}
	}

	public class BoardsContainer
	{
		public List<Check[,]> Boards = new List<Check[,]>();

		public void MarkBoards(int value)
		{
			for (int i = 0; i < Boards.Count; i++)
			{
				Check[,] board = Boards[i];

				// For each row
				for (int row = 0; row < board.GetLength(0); row++)
				{
					// Checking all columns
					for (int column = 0; column < board.GetLength(1); column++)
					{
						if (board[row, column].Value == value)
						{
							board[row, column].IsChecked = true;
						}
					}
				}
			}
		}

		public List<Check[,]> GetWinningBoards()
		{
			List<Check[,]> winningBoards = new List<Check[,]>();

			for (int i = 0; i < Boards.Count; i++)
			{
				if (HasRowCompleted(Boards[i]) || HasColumndCompleted(Boards[i]))
				{
					winningBoards.Add(Boards[i]);
				}
			}

			return winningBoards;
		}

		public bool HasRowCompleted(Check[,] board)
		{
			for (int row = 0; row < board.GetLength(0); row++)
			{
				bool isValid = true;

				for (int column = 0; column < board.GetLength(1); column++)
				{
					if (!board[row, column].IsChecked)
					{
						isValid = false;
					}
				}

				if (isValid)
				{
					return true;
				}
			}

			return false;
		}

		public bool HasColumndCompleted(Check[,] board)
		{
			for (int column = 0; column < board.GetLength(1); column++)
			{
				bool isValid = true;

				for (int row = 0; row < board.GetLength(0); row++)
				{
					if (!board[row , column].IsChecked)
					{
						isValid = false;
					}
				}

				if (isValid)
				{
					return true;
				}
			}

			return false;
		}

		public int GetSumOfAllMarkedValues(Check[,] board)
		{
			int sum = 0;

			for (int row = 0; row < board.GetLength(0); row++)
			{
				for (int column = 0; column < board.GetLength(1); column++)
				{
					if (!board[row, column].IsChecked)
					{
						sum += board[row, column].Value;
					}
				}
			}

			return sum;
		}
	}

	public class Check
	{
		public int Value;
		public bool IsChecked;
	}
}
