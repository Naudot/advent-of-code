using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AdventOfCode.Year2021
{
	public class DayFour : Day2021
	{
		protected override object ResolveFirstPart(string[] input)
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

			MatchCollection draws = Regex.Matches(input[0], @"(\d+)");
			int lastWinningBoard = 0;

			for (int i = 0; i < draws.Count; i++)
			{
				int value = int.Parse(draws[i].Value);
				boardsContainer.MarkBoards(value);
				List<Check[,]> winningBoards = boardsContainer.GetWinningBoards();

				for (int j = 0; j < winningBoards.Count; j++)
				{
					lastWinningBoard = boardsContainer.GetSumOfAllMarkedValues(winningBoards[j]) * value;
					boardsContainer.Boards.Remove(winningBoards[j]);
				}
			}

			return lastWinningBoard;
		}
	}

	public class BoardsContainer
	{
		public List<Check[,]> Boards = new List<Check[,]>();

		public void MarkBoards(int value)
		{
			for (int k = 0; k < Boards.Count; k++)
			{
				Check[,] board = Boards[k];

				// For each row
				for (int i = 0; i < board.GetLength(0); i++)
				{
					// Checking all columns
					for (int j = 0; j < board.GetLength(1); j++)
					{
						if (board[i, j].Value == value)
						{
							board[i, j].IsChecked = true;
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
			// For each row
			for (int i = 0; i < board.GetLength(0); i++)
			{
				bool isValid = true;

				// Checking all columns
				for (int j = 0; j < board.GetLength(1); j++)
				{
					if (!board[i, j].IsChecked)
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
			// For each column
			for (int i = 0; i < board.GetLength(1); i++)
			{
				bool isValid = true;

				// Checking all rows
				for (int j = 0; j < board.GetLength(0); j++)
				{
					if (!board[j , i].IsChecked)
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

			// For each row
			for (int i = 0; i < board.GetLength(0); i++)
			{
				// Checking all columns
				for (int j = 0; j < board.GetLength(1); j++)
				{
					if (!board[i, j].IsChecked)
					{
						sum += board[i, j].Value;
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
